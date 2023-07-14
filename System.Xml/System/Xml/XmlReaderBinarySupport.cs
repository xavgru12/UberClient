// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlReaderBinarySupport
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;

namespace System.Xml
{
  internal class XmlReaderBinarySupport
  {
    private XmlReader reader;
    private XmlReaderBinarySupport.CharGetter getter;
    private byte[] base64Cache = new byte[3];
    private int base64CacheStartsAt;
    private XmlReaderBinarySupport.CommandState state;
    private StringBuilder textCache;
    private bool hasCache;
    private bool dontReset;

    public XmlReaderBinarySupport(XmlReader reader)
    {
      this.reader = reader;
      this.Reset();
    }

    public XmlReaderBinarySupport.CharGetter Getter
    {
      get => this.getter;
      set => this.getter = value;
    }

    public void Reset()
    {
      if (this.dontReset)
        return;
      this.dontReset = true;
      if (this.hasCache)
      {
        switch (this.reader.NodeType)
        {
          case XmlNodeType.Text:
          case XmlNodeType.CDATA:
          case XmlNodeType.Whitespace:
          case XmlNodeType.SignificantWhitespace:
            this.reader.Read();
            break;
        }
        switch (this.state)
        {
          case XmlReaderBinarySupport.CommandState.ReadElementContentAsBase64:
          case XmlReaderBinarySupport.CommandState.ReadElementContentAsBinHex:
            this.reader.Read();
            break;
        }
      }
      this.base64CacheStartsAt = -1;
      this.state = XmlReaderBinarySupport.CommandState.None;
      this.hasCache = false;
      this.dontReset = false;
    }

    private InvalidOperationException StateError(XmlReaderBinarySupport.CommandState action) => new InvalidOperationException(string.Format("Invalid attempt to read binary content by {0}, while once binary reading was started by {1}", (object) action, (object) this.state));

    private void CheckState(bool element, XmlReaderBinarySupport.CommandState action)
    {
      if (this.state == XmlReaderBinarySupport.CommandState.None)
      {
        if (this.textCache == null)
          this.textCache = new StringBuilder();
        else
          this.textCache.Length = 0;
        if (action != XmlReaderBinarySupport.CommandState.None && this.reader.ReadState == ReadState.Interactive)
        {
          XmlNodeType nodeType = this.reader.NodeType;
          switch (nodeType)
          {
            case XmlNodeType.Element:
              if (element)
              {
                if (!this.reader.IsEmptyElement)
                  this.reader.Read();
                this.state = action;
                return;
              }
              break;
            case XmlNodeType.Text:
            case XmlNodeType.CDATA:
              if (!element)
              {
                this.state = action;
                return;
              }
              break;
            default:
              if (nodeType == XmlNodeType.Whitespace || nodeType == XmlNodeType.SignificantWhitespace)
                goto case XmlNodeType.Text;
              else
                break;
          }
          throw new XmlException(!element ? "Reader is not positioned on a text node." : "Reader is not positioned on an element.");
        }
      }
      else if (this.state != action)
        throw this.StateError(action);
    }

    public int ReadElementContentAsBase64(byte[] buffer, int offset, int length)
    {
      this.CheckState(true, XmlReaderBinarySupport.CommandState.ReadElementContentAsBase64);
      return this.ReadBase64(buffer, offset, length);
    }

    public int ReadContentAsBase64(byte[] buffer, int offset, int length)
    {
      this.CheckState(false, XmlReaderBinarySupport.CommandState.ReadContentAsBase64);
      return this.ReadBase64(buffer, offset, length);
    }

    public int ReadElementContentAsBinHex(byte[] buffer, int offset, int length)
    {
      this.CheckState(true, XmlReaderBinarySupport.CommandState.ReadElementContentAsBinHex);
      return this.ReadBinHex(buffer, offset, length);
    }

    public int ReadContentAsBinHex(byte[] buffer, int offset, int length)
    {
      this.CheckState(false, XmlReaderBinarySupport.CommandState.ReadContentAsBinHex);
      return this.ReadBinHex(buffer, offset, length);
    }

    public int ReadBase64(byte[] buffer, int offset, int length)
    {
      if (offset < 0)
        throw XmlReaderBinarySupport.CreateArgumentOutOfRangeException(nameof (offset), (object) offset, "Offset must be non-negative integer.");
      if (length < 0)
        throw XmlReaderBinarySupport.CreateArgumentOutOfRangeException(nameof (length), (object) length, "Length must be non-negative integer.");
      if (buffer.Length < offset + length)
        throw new ArgumentOutOfRangeException("buffer length is smaller than the sum of offset and length.");
      if (this.reader.IsEmptyElement || length == 0)
        return 0;
      int index1 = offset;
      int num1 = offset + length;
      if (this.base64CacheStartsAt >= 0)
      {
        for (int base64CacheStartsAt = this.base64CacheStartsAt; base64CacheStartsAt < 3; ++base64CacheStartsAt)
        {
          buffer[index1++] = this.base64Cache[this.base64CacheStartsAt++];
          if (index1 == num1)
            return num1 - offset;
        }
      }
      for (int index2 = 0; index2 < 3; ++index2)
        this.base64Cache[index2] = (byte) 0;
      this.base64CacheStartsAt = -1;
      int length1 = (int) Math.Ceiling(4.0 / 3.0 * (double) length);
      int num2 = length1 % 4;
      if (num2 > 0)
        length1 += 4 - num2;
      char[] chArray = new char[length1];
      int charsLength = this.getter == null ? this.ReadValueChunk(chArray, 0, length1) : this.getter(chArray, 0, length1);
      int index3;
      int index4;
      for (int i1 = 0; i1 < charsLength - 3 && (index3 = this.SkipIgnorableBase64Chars(chArray, charsLength, i1)) != charsLength; i1 = index4 + 1)
      {
        byte num3 = (byte) ((uint) this.GetBase64Byte(chArray[index3]) << 2);
        if (index1 < num1)
        {
          buffer[index1] = num3;
        }
        else
        {
          if (this.base64CacheStartsAt < 0)
            this.base64CacheStartsAt = 0;
          this.base64Cache[0] = num3;
        }
        int i2;
        int index5;
        if ((i2 = index3 + 1) != charsLength && (index5 = this.SkipIgnorableBase64Chars(chArray, charsLength, i2)) != charsLength)
        {
          byte base64Byte1 = this.GetBase64Byte(chArray[index5]);
          byte num4 = (byte) ((uint) base64Byte1 >> 4);
          if (index1 < num1)
          {
            buffer[index1] += num4;
            ++index1;
          }
          else
            this.base64Cache[0] += num4;
          byte num5 = (byte) (((int) base64Byte1 & 15) << 4);
          if (index1 < num1)
          {
            buffer[index1] = num5;
          }
          else
          {
            if (this.base64CacheStartsAt < 0)
              this.base64CacheStartsAt = 1;
            this.base64Cache[1] = num5;
          }
          int i3;
          int index6;
          if ((i3 = index5 + 1) != charsLength && (index6 = this.SkipIgnorableBase64Chars(chArray, charsLength, i3)) != charsLength)
          {
            byte base64Byte2 = this.GetBase64Byte(chArray[index6]);
            byte num6 = (byte) ((uint) base64Byte2 >> 2);
            if (index1 < num1)
            {
              buffer[index1] += num6;
              ++index1;
            }
            else
              this.base64Cache[1] += num6;
            byte num7 = (byte) (((int) base64Byte2 & 3) << 6);
            if (index1 < num1)
            {
              buffer[index1] = num7;
            }
            else
            {
              if (this.base64CacheStartsAt < 0)
                this.base64CacheStartsAt = 2;
              this.base64Cache[2] = num7;
            }
            int i4;
            if ((i4 = index6 + 1) != charsLength && (index4 = this.SkipIgnorableBase64Chars(chArray, charsLength, i4)) != charsLength)
            {
              byte base64Byte3 = this.GetBase64Byte(chArray[index4]);
              if (index1 < num1)
              {
                buffer[index1] += base64Byte3;
                ++index1;
              }
              else
                this.base64Cache[2] += base64Byte3;
            }
            else
              break;
          }
          else
            break;
        }
        else
          break;
      }
      int num8 = Math.Min(num1 - offset, index1 - offset);
      return num8 < length && charsLength > 0 ? num8 + this.ReadBase64(buffer, offset + num8, length - num8) : num8;
    }

    private byte GetBase64Byte(char ch)
    {
      switch (ch)
      {
        case '+':
          return 62;
        case '/':
          return 63;
        default:
          if (ch >= 'A' && ch <= 'Z')
            return (byte) ((uint) ch - 65U);
          if (ch >= 'a' && ch <= 'z')
            return (byte) ((int) ch - 97 + 26);
          if (ch >= '0' && ch <= '9')
            return (byte) ((int) ch - 48 + 52);
          throw new XmlException("Invalid Base64 character was found.");
      }
    }

    private int SkipIgnorableBase64Chars(char[] chars, int charsLength, int i)
    {
      do
        ;
      while ((chars[i] == '=' || XmlChar.IsWhitespace((int) chars[i])) && charsLength != ++i);
      return i;
    }

    private static Exception CreateArgumentOutOfRangeException(
      string name,
      object value,
      string message)
    {
      return (Exception) new ArgumentOutOfRangeException(message);
    }

    public int ReadBinHex(byte[] buffer, int offset, int length)
    {
      if (offset < 0)
        throw XmlReaderBinarySupport.CreateArgumentOutOfRangeException(nameof (offset), (object) offset, "Offset must be non-negative integer.");
      if (length < 0)
        throw XmlReaderBinarySupport.CreateArgumentOutOfRangeException(nameof (length), (object) length, "Length must be non-negative integer.");
      if (buffer.Length < offset + length)
        throw new ArgumentOutOfRangeException("buffer length is smaller than the sum of offset and length.");
      if (length == 0)
        return 0;
      char[] chArray = new char[length * 2];
      int charLength = this.getter == null ? this.ReadValueChunk(chArray, 0, length * 2) : this.getter(chArray, 0, length * 2);
      return XmlConvert.FromBinHexString(chArray, offset, charLength, buffer);
    }

    public int ReadValueChunk(char[] buffer, int offset, int length)
    {
      XmlReaderBinarySupport.CommandState state = this.state;
      if (this.state == XmlReaderBinarySupport.CommandState.None)
        this.CheckState(false, XmlReaderBinarySupport.CommandState.None);
      if (offset < 0)
        throw XmlReaderBinarySupport.CreateArgumentOutOfRangeException(nameof (offset), (object) offset, "Offset must be non-negative integer.");
      if (length < 0)
        throw XmlReaderBinarySupport.CreateArgumentOutOfRangeException(nameof (length), (object) length, "Length must be non-negative integer.");
      if (buffer.Length < offset + length)
        throw new ArgumentOutOfRangeException("buffer length is smaller than the sum of offset and length.");
      if (length == 0 || !this.hasCache && this.reader.IsEmptyElement)
        return 0;
      bool flag = true;
      while (flag && this.textCache.Length < length)
      {
        switch (this.reader.NodeType)
        {
          case XmlNodeType.Text:
          case XmlNodeType.CDATA:
          case XmlNodeType.Whitespace:
          case XmlNodeType.SignificantWhitespace:
            if (this.hasCache)
            {
              switch (this.reader.NodeType)
              {
                case XmlNodeType.Text:
                case XmlNodeType.CDATA:
                case XmlNodeType.Whitespace:
                case XmlNodeType.SignificantWhitespace:
                  this.Read();
                  break;
                default:
                  flag = false;
                  break;
              }
            }
            this.textCache.Append(this.reader.Value);
            this.hasCache = true;
            continue;
          default:
            flag = false;
            continue;
        }
      }
      this.state = state;
      int length1 = this.textCache.Length;
      if (length1 > length)
        length1 = length;
      string str = this.textCache.ToString(0, length1);
      this.textCache.Remove(0, str.Length);
      str.CopyTo(0, buffer, offset, str.Length);
      return length1 < length && flag ? length1 + this.ReadValueChunk(buffer, offset + length1, length - length1) : length1;
    }

    private bool Read()
    {
      this.dontReset = true;
      bool flag = this.reader.Read();
      this.dontReset = false;
      return flag;
    }

    public enum CommandState
    {
      None,
      ReadElementContentAsBase64,
      ReadContentAsBase64,
      ReadElementContentAsBinHex,
      ReadContentAsBinHex,
    }

    public delegate int CharGetter(char[] buffer, int offset, int length);
  }
}
