// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlInputStream
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.IO;
using System.Text;

namespace System.Xml
{
  internal class XmlInputStream : Stream
  {
    public static readonly Encoding StrictUTF8;
    private Encoding enc;
    private Stream stream;
    private byte[] buffer;
    private int bufLength;
    private int bufPos;
    private static XmlException encodingException = new XmlException("invalid encoding specification.");

    public XmlInputStream(Stream stream) => this.Initialize(stream);

    static XmlInputStream() => XmlInputStream.StrictUTF8 = (Encoding) new UTF8Encoding(false, true);

    private static string GetStringFromBytes(byte[] bytes, int index, int count) => Encoding.ASCII.GetString(bytes, index, count);

    private void Initialize(Stream stream)
    {
      this.buffer = new byte[64];
      this.stream = stream;
      this.enc = XmlInputStream.StrictUTF8;
      this.bufLength = stream.Read(this.buffer, 0, this.buffer.Length);
      if (this.bufLength == -1 || this.bufLength == 0)
        return;
      switch (this.ReadByteSpecial())
      {
        case 60:
          if (this.bufLength >= 5 && XmlInputStream.GetStringFromBytes(this.buffer, 1, 4) == "?xml")
          {
            this.bufPos += 4;
            int num1 = this.SkipWhitespace();
            if (num1 == 118)
            {
              while (num1 >= 0)
              {
                num1 = this.ReadByteSpecial();
                if (num1 == 48)
                {
                  this.ReadByteSpecial();
                  break;
                }
              }
              num1 = this.SkipWhitespace();
            }
            if (num1 == 101 && this.bufLength - this.bufPos >= 7 && XmlInputStream.GetStringFromBytes(this.buffer, this.bufPos, 7) == "ncoding")
            {
              this.bufPos += 7;
              if (this.SkipWhitespace() != 61)
                throw XmlInputStream.encodingException;
              int num2 = this.SkipWhitespace();
              StringBuilder stringBuilder = new StringBuilder();
              while (true)
              {
                int num3 = this.ReadByteSpecial();
                if (num3 != num2)
                {
                  if (num3 >= 0)
                    stringBuilder.Append((char) num3);
                  else
                    break;
                }
                else
                  goto label_27;
              }
              throw XmlInputStream.encodingException;
label_27:
              string str = stringBuilder.ToString();
              this.enc = XmlChar.IsValidIANAEncoding(str) ? Encoding.GetEncoding(str) : throw XmlInputStream.encodingException;
            }
          }
          this.bufPos = 0;
          break;
        case 239:
          if (this.ReadByteSpecial() == 187)
          {
            if (this.ReadByteSpecial() == 191)
              break;
            this.bufPos = 0;
            break;
          }
          this.buffer[--this.bufPos] = (byte) 239;
          break;
        case 254:
          if (this.ReadByteSpecial() == (int) byte.MaxValue)
          {
            this.enc = Encoding.BigEndianUnicode;
            break;
          }
          this.bufPos = 0;
          break;
        case (int) byte.MaxValue:
          if (this.ReadByteSpecial() == 254)
          {
            this.enc = Encoding.Unicode;
            break;
          }
          this.bufPos = 0;
          break;
        default:
          this.bufPos = 0;
          break;
      }
    }

    private int ReadByteSpecial()
    {
      if (this.bufLength > this.bufPos)
        return (int) this.buffer[this.bufPos++];
      byte[] numArray = new byte[this.buffer.Length * 2];
      Buffer.BlockCopy((Array) this.buffer, 0, (Array) numArray, 0, this.bufLength);
      int num = this.stream.Read(numArray, this.bufLength, this.buffer.Length);
      switch (num)
      {
        case -1:
        case 0:
          return -1;
        default:
          this.bufLength += num;
          this.buffer = numArray;
          return (int) this.buffer[this.bufPos++];
      }
    }

    private int SkipWhitespace()
    {
      int num;
      char ch;
      do
      {
        num = this.ReadByteSpecial();
        ch = (char) num;
        switch ((int) ch - 9)
        {
          case 0:
          case 1:
          case 4:
            continue;
          default:
            continue;
        }
      }
      while (ch == ' ');
      return num;
    }

    public Encoding ActualEncoding => this.enc;

    public override bool CanRead => this.bufLength > this.bufPos || this.stream.CanRead;

    public override bool CanSeek => false;

    public override bool CanWrite => false;

    public override long Length => this.stream.Length;

    public override long Position
    {
      get => this.stream.Position - (long) this.bufLength + (long) this.bufPos;
      set
      {
        if (value < (long) this.bufLength)
          this.bufPos = (int) value;
        else
          this.stream.Position = value - (long) this.bufLength;
      }
    }

    public override void Close() => this.stream.Close();

    public override void Flush() => this.stream.Flush();

    public override int Read(byte[] buffer, int offset, int count)
    {
      int num;
      if (count <= this.bufLength - this.bufPos)
      {
        Buffer.BlockCopy((Array) this.buffer, this.bufPos, (Array) buffer, offset, count);
        this.bufPos += count;
        num = count;
      }
      else
      {
        int count1 = this.bufLength - this.bufPos;
        if (this.bufLength > this.bufPos)
        {
          Buffer.BlockCopy((Array) this.buffer, this.bufPos, (Array) buffer, offset, count1);
          this.bufPos += count1;
        }
        num = count1 + this.stream.Read(buffer, offset + count1, count - count1);
      }
      return num;
    }

    public override int ReadByte() => this.bufLength > this.bufPos ? (int) this.buffer[this.bufPos++] : this.stream.ReadByte();

    public override long Seek(long offset, SeekOrigin origin)
    {
      int num = this.bufLength - this.bufPos;
      if (origin != SeekOrigin.Current)
        return this.stream.Seek(offset, origin);
      return offset < (long) num ? (long) this.buffer[(long) this.bufPos + offset] : this.stream.Seek(offset - (long) num, origin);
    }

    public override void SetLength(long value) => this.stream.SetLength(value);

    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
  }
}
