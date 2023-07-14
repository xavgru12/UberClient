// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlDeclaration
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections.Generic;
using System.Globalization;

namespace System.Xml
{
  public class XmlDeclaration : XmlLinkedNode
  {
    private string encoding = "UTF-8";
    private string standalone;
    private string version;

    protected internal XmlDeclaration(
      string version,
      string encoding,
      string standalone,
      XmlDocument doc)
      : base(doc)
    {
      if (encoding == null)
        encoding = string.Empty;
      if (standalone == null)
        standalone = string.Empty;
      this.version = version;
      this.encoding = encoding;
      this.standalone = standalone;
    }

    public string Encoding
    {
      get => this.encoding;
      set => this.encoding = value != null ? value : string.Empty;
    }

    public override string InnerText
    {
      get => this.Value;
      set => this.ParseInput(value);
    }

    public override string LocalName => "xml";

    public override string Name => "xml";

    public override XmlNodeType NodeType => XmlNodeType.XmlDeclaration;

    public string Standalone
    {
      get => this.standalone;
      set
      {
        if (value != null)
        {
          if (string.Compare(value, "YES", true, CultureInfo.InvariantCulture) == 0)
            this.standalone = "yes";
          if (string.Compare(value, "NO", true, CultureInfo.InvariantCulture) != 0)
            return;
          this.standalone = "no";
        }
        else
          this.standalone = string.Empty;
      }
    }

    public override string Value
    {
      get
      {
        string str1 = string.Empty;
        string str2 = string.Empty;
        if (this.encoding != string.Empty)
          str1 = string.Format(" encoding=\"{0}\"", (object) this.encoding);
        if (this.standalone != string.Empty)
          str2 = string.Format(" standalone=\"{0}\"", (object) this.standalone);
        return string.Format("version=\"{0}\"{1}{2}", (object) this.Version, (object) str1, (object) str2);
      }
      set => this.ParseInput(value);
    }

    public string Version => this.version;

    public override XmlNode CloneNode(bool deep) => (XmlNode) new XmlDeclaration(this.Version, this.Encoding, this.standalone, this.OwnerDocument);

    public override void WriteContentTo(XmlWriter w)
    {
    }

    public override void WriteTo(XmlWriter w) => w.WriteRaw(string.Format("<?xml {0}?>", (object) this.Value));

    private int SkipWhitespace(string input, int index)
    {
      while (index < input.Length && XmlChar.IsWhitespace((int) input[index]))
        ++index;
      return index;
    }

    private void ParseInput(string input)
    {
      int startIndex1 = this.SkipWhitespace(input, 0);
      if (startIndex1 + 7 > input.Length || input.IndexOf("version", startIndex1, 7) != startIndex1)
        throw new XmlException("Missing 'version' specification.");
      int index1 = this.SkipWhitespace(input, startIndex1 + 7);
      if (input[index1] != '=')
        throw new XmlException("Invalid 'version' specification.");
      int index2 = index1 + 1;
      int index3 = this.SkipWhitespace(input, index2);
      char ch1 = input[index3];
      switch (ch1)
      {
        case '"':
        case '\'':
          int startIndex2 = index3 + 1;
          if (input.IndexOf(ch1, startIndex2) < 0 || input.IndexOf("1.0", startIndex2, 3) != startIndex2)
            throw new XmlException("Invalid 'version' specification.");
          int index4 = startIndex2 + 4;
          if (index4 == input.Length)
            break;
          int startIndex3 = XmlChar.IsWhitespace((int) input[index4]) ? this.SkipWhitespace(input, index4 + 1) : throw new XmlException("Invalid XML declaration.");
          if (startIndex3 == input.Length)
            break;
          if (input.Length > startIndex3 + 8 && input.IndexOf("encoding", startIndex3, 8) > 0)
          {
            int index5 = this.SkipWhitespace(input, startIndex3 + 8);
            if (input[index5] != '=')
              throw new XmlException("Invalid 'version' specification.");
            int index6 = index5 + 1;
            int index7 = this.SkipWhitespace(input, index6);
            char ch2 = input[index7];
            switch (ch2)
            {
              case '"':
              case '\'':
                int num = input.IndexOf(ch2, index7 + 1);
                if (num < 0)
                  throw new XmlException("Invalid 'encoding' specification.");
                this.Encoding = input.Substring(index7 + 1, num - index7 - 1);
                int index8 = num + 1;
                if (index8 == input.Length)
                  return;
                startIndex3 = XmlChar.IsWhitespace((int) input[index8]) ? this.SkipWhitespace(input, index8 + 1) : throw new XmlException("Invalid XML declaration.");
                break;
              default:
                throw new XmlException("Invalid 'encoding' specification.");
            }
          }
          if (input.Length > startIndex3 + 10 && input.IndexOf("standalone", startIndex3, 10) > 0)
          {
            int index9 = this.SkipWhitespace(input, startIndex3 + 10);
            if (input[index9] != '=')
              throw new XmlException("Invalid 'version' specification.");
            int index10 = index9 + 1;
            int index11 = this.SkipWhitespace(input, index10);
            char ch3 = input[index11];
            switch (ch3)
            {
              case '"':
              case '\'':
                int num1 = input.IndexOf(ch3, index11 + 1);
                if (num1 < 0)
                  throw new XmlException("Invalid 'standalone' specification.");
                string str = input.Substring(index11 + 1, num1 - index11 - 1);
                string key = str;
                if (key != null)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (XmlDeclaration.\u003C\u003Ef__switch\u0024map4A == null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    XmlDeclaration.\u003C\u003Ef__switch\u0024map4A = new Dictionary<string, int>(2)
                    {
                      {
                        "yes",
                        0
                      },
                      {
                        "no",
                        0
                      }
                    };
                  }
                  int num2;
                  // ISSUE: reference to a compiler-generated field
                  if (XmlDeclaration.\u003C\u003Ef__switch\u0024map4A.TryGetValue(key, out num2) && num2 == 0)
                  {
                    this.Standalone = str;
                    int index12 = num1 + 1;
                    startIndex3 = this.SkipWhitespace(input, index12);
                    break;
                  }
                }
                throw new XmlException("Invalid standalone specification.");
              default:
                throw new XmlException("Invalid 'standalone' specification.");
            }
          }
          if (startIndex3 == input.Length)
            break;
          throw new XmlException("Invalid XML declaration.");
        default:
          throw new XmlException("Invalid 'version' specification.");
      }
    }
  }
}
