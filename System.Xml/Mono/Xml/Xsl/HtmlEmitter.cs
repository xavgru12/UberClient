// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.HtmlEmitter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Mono.Xml.Xsl
{
  internal class HtmlEmitter : Emitter
  {
    private TextWriter writer;
    private Stack elementNameStack;
    private bool openElement;
    private bool openAttribute;
    private int nonHtmlDepth;
    private bool indent;
    private Encoding outputEncoding;
    private string mediaType;

    public HtmlEmitter(TextWriter writer, XslOutput output)
    {
      this.writer = writer;
      this.indent = output.Indent == "yes" || output.Indent == null;
      this.elementNameStack = new Stack();
      this.nonHtmlDepth = -1;
      this.outputEncoding = writer.Encoding != null ? writer.Encoding : output.Encoding;
      this.mediaType = output.MediaType;
      if (this.mediaType != null && this.mediaType.Length != 0)
        return;
      this.mediaType = "text/html";
    }

    public override void WriteStartDocument(Encoding encoding, StandaloneType standalone)
    {
    }

    public override void WriteEndDocument()
    {
    }

    public override void WriteDocType(string name, string publicId, string systemId)
    {
      this.writer.Write("<!DOCTYPE html ");
      if (publicId != null)
      {
        this.writer.Write("PUBLIC \"");
        this.writer.Write(publicId);
        this.writer.Write("\" ");
        if (systemId != null)
        {
          this.writer.Write("\"");
          this.writer.Write(systemId);
          this.writer.Write("\"");
        }
      }
      else if (systemId != null)
      {
        this.writer.Write("SYSTEM \"");
        this.writer.Write(systemId);
        this.writer.Write('"');
      }
      this.writer.Write('>');
      if (!this.indent)
        return;
      this.writer.WriteLine();
    }

    private void CloseAttribute()
    {
      this.writer.Write('"');
      this.openAttribute = false;
    }

    private void CloseStartElement()
    {
      if (this.openAttribute)
        this.CloseAttribute();
      this.writer.Write('>');
      this.openElement = false;
      if (this.outputEncoding == null || this.elementNameStack.Count <= 0)
        return;
      string upper = ((string) this.elementNameStack.Peek()).ToUpper(CultureInfo.InvariantCulture);
      if (upper == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (HtmlEmitter.\u003C\u003Ef__switch\u0024map13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        HtmlEmitter.\u003C\u003Ef__switch\u0024map13 = new Dictionary<string, int>(3)
        {
          {
            "HEAD",
            0
          },
          {
            "STYLE",
            1
          },
          {
            "SCRIPT",
            1
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!HtmlEmitter.\u003C\u003Ef__switch\u0024map13.TryGetValue(upper, out num))
        return;
      switch (num)
      {
        case 0:
          this.WriteStartElement(string.Empty, "META", string.Empty);
          this.WriteAttributeString(string.Empty, "http-equiv", string.Empty, "Content-Type");
          this.WriteAttributeString(string.Empty, "content", string.Empty, this.mediaType + "; charset=" + this.outputEncoding.WebName);
          this.WriteEndElement();
          break;
        case 1:
          this.writer.WriteLine();
          for (int index = 0; index <= this.elementNameStack.Count; ++index)
            this.writer.Write("  ");
          break;
      }
    }

    private void Indent(string elementName, bool endIndent)
    {
      if (!this.indent)
        return;
      string upper = elementName.ToUpper(CultureInfo.InvariantCulture);
      if (upper != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (HtmlEmitter.\u003C\u003Ef__switch\u0024map14 == null)
        {
          // ISSUE: reference to a compiler-generated field
          HtmlEmitter.\u003C\u003Ef__switch\u0024map14 = new Dictionary<string, int>(31)
          {
            {
              "ADDRESS",
              0
            },
            {
              "APPLET",
              0
            },
            {
              "BDO",
              0
            },
            {
              "BLOCKQUOTE",
              0
            },
            {
              "BODY",
              0
            },
            {
              "BUTTON",
              0
            },
            {
              "CAPTION",
              0
            },
            {
              "CENTER",
              0
            },
            {
              "DD",
              0
            },
            {
              "DEL",
              0
            },
            {
              "DIR",
              0
            },
            {
              "DIV",
              0
            },
            {
              "DL",
              0
            },
            {
              "DT",
              0
            },
            {
              "FIELDSET",
              0
            },
            {
              "HEAD",
              0
            },
            {
              "HTML",
              0
            },
            {
              "IFRAME",
              0
            },
            {
              "INS",
              0
            },
            {
              "LI",
              0
            },
            {
              "MAP",
              0
            },
            {
              "MENU",
              0
            },
            {
              "NOFRAMES",
              0
            },
            {
              "NOSCRIPT",
              0
            },
            {
              "OBJECT",
              0
            },
            {
              "OPTION",
              0
            },
            {
              "PRE",
              0
            },
            {
              "TABLE",
              0
            },
            {
              "TD",
              0
            },
            {
              "TH",
              0
            },
            {
              "TR",
              0
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (!HtmlEmitter.\u003C\u003Ef__switch\u0024map14.TryGetValue(upper, out num) || num != 0)
          goto label_9;
      }
      else
        goto label_9;
label_6:
      this.writer.Write(this.writer.NewLine);
      int count = this.elementNameStack.Count;
      for (int index = 0; index < count; ++index)
        this.writer.Write("  ");
      return;
label_9:
      if (elementName.Length > 0 && this.nonHtmlDepth > 0)
        goto label_6;
    }

    public override void WriteStartElement(string prefix, string localName, string nsURI)
    {
      if (this.openElement)
        this.CloseStartElement();
      this.Indent(this.elementNameStack.Count <= 0 ? string.Empty : this.elementNameStack.Peek() as string, false);
      string str = localName;
      this.writer.Write('<');
      if (nsURI != string.Empty)
      {
        if (prefix != string.Empty)
          str = prefix + ":" + localName;
        if (this.nonHtmlDepth < 0)
          this.nonHtmlDepth = this.elementNameStack.Count + 1;
      }
      this.writer.Write(str);
      this.elementNameStack.Push((object) str);
      this.openElement = true;
    }

    private bool IsHtmlElement(string localName)
    {
      string upper = localName.ToUpper(CultureInfo.InvariantCulture);
      if (upper != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (HtmlEmitter.\u003C\u003Ef__switch\u0024map15 == null)
        {
          // ISSUE: reference to a compiler-generated field
          HtmlEmitter.\u003C\u003Ef__switch\u0024map15 = new Dictionary<string, int>(91)
          {
            {
              "A",
              0
            },
            {
              "ABBR",
              0
            },
            {
              "ACRONYM",
              0
            },
            {
              "ADDRESS",
              0
            },
            {
              "APPLET",
              0
            },
            {
              "AREA",
              0
            },
            {
              "B",
              0
            },
            {
              "BASE",
              0
            },
            {
              "BASEFONT",
              0
            },
            {
              "BDO",
              0
            },
            {
              "BIG",
              0
            },
            {
              "BLOCKQUOTE",
              0
            },
            {
              "BODY",
              0
            },
            {
              "BR",
              0
            },
            {
              "BUTTON",
              0
            },
            {
              "CAPTION",
              0
            },
            {
              "CENTER",
              0
            },
            {
              "CITE",
              0
            },
            {
              "CODE",
              0
            },
            {
              "COL",
              0
            },
            {
              "COLGROUP",
              0
            },
            {
              "DD",
              0
            },
            {
              "DEL",
              0
            },
            {
              "DFN",
              0
            },
            {
              "DIR",
              0
            },
            {
              "DIV",
              0
            },
            {
              "DL",
              0
            },
            {
              "DT",
              0
            },
            {
              "EM",
              0
            },
            {
              "FIELDSET",
              0
            },
            {
              "FONT",
              0
            },
            {
              "FORM",
              0
            },
            {
              "FRAME",
              0
            },
            {
              "FRAMESET",
              0
            },
            {
              "H1",
              0
            },
            {
              "H2",
              0
            },
            {
              "H3",
              0
            },
            {
              "H4",
              0
            },
            {
              "H5",
              0
            },
            {
              "H6",
              0
            },
            {
              "HEAD",
              0
            },
            {
              "HR",
              0
            },
            {
              "HTML",
              0
            },
            {
              "I",
              0
            },
            {
              "IFRAME",
              0
            },
            {
              "IMG",
              0
            },
            {
              "INPUT",
              0
            },
            {
              "INS",
              0
            },
            {
              "ISINDEX",
              0
            },
            {
              "KBD",
              0
            },
            {
              "LABEL",
              0
            },
            {
              "LEGEND",
              0
            },
            {
              "LI",
              0
            },
            {
              "LINK",
              0
            },
            {
              "MAP",
              0
            },
            {
              "MENU",
              0
            },
            {
              "META",
              0
            },
            {
              "NOFRAMES",
              0
            },
            {
              "NOSCRIPT",
              0
            },
            {
              "OBJECT",
              0
            },
            {
              "OL",
              0
            },
            {
              "OPTGROUP",
              0
            },
            {
              "OPTION",
              0
            },
            {
              "P",
              0
            },
            {
              "PARAM",
              0
            },
            {
              "PRE",
              0
            },
            {
              "Q",
              0
            },
            {
              "S",
              0
            },
            {
              "SAMP",
              0
            },
            {
              "SCRIPT",
              0
            },
            {
              "SELECT",
              0
            },
            {
              "SMALL",
              0
            },
            {
              "SPAN",
              0
            },
            {
              "STRIKE",
              0
            },
            {
              "STRONG",
              0
            },
            {
              "STYLE",
              0
            },
            {
              "SUB",
              0
            },
            {
              "SUP",
              0
            },
            {
              "TABLE",
              0
            },
            {
              "TBODY",
              0
            },
            {
              "TD",
              0
            },
            {
              "TEXTAREA",
              0
            },
            {
              "TFOOT",
              0
            },
            {
              "TH",
              0
            },
            {
              "THEAD",
              0
            },
            {
              "TITLE",
              0
            },
            {
              "TR",
              0
            },
            {
              "TT",
              0
            },
            {
              "U",
              0
            },
            {
              "UL",
              0
            },
            {
              "VAR",
              0
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (HtmlEmitter.\u003C\u003Ef__switch\u0024map15.TryGetValue(upper, out num) && num == 0)
          return true;
      }
      return false;
    }

    public override void WriteEndElement() => this.WriteFullEndElement();

    public override void WriteFullEndElement()
    {
      string str = this.elementNameStack.Peek() as string;
      string upper = str.ToUpper(CultureInfo.InvariantCulture);
      if (upper != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (HtmlEmitter.\u003C\u003Ef__switch\u0024map16 == null)
        {
          // ISSUE: reference to a compiler-generated field
          HtmlEmitter.\u003C\u003Ef__switch\u0024map16 = new Dictionary<string, int>(13)
          {
            {
              "AREA",
              0
            },
            {
              "BASE",
              0
            },
            {
              "BASEFONT",
              0
            },
            {
              "BR",
              0
            },
            {
              "COL",
              0
            },
            {
              "FRAME",
              0
            },
            {
              "HR",
              0
            },
            {
              "IMG",
              0
            },
            {
              "INPUT",
              0
            },
            {
              "ISINDEX",
              0
            },
            {
              "LINK",
              0
            },
            {
              "META",
              0
            },
            {
              "PARAM",
              0
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (HtmlEmitter.\u003C\u003Ef__switch\u0024map16.TryGetValue(upper, out num) && num == 0)
        {
          if (this.openAttribute)
            this.CloseAttribute();
          if (this.openElement)
            this.writer.Write('>');
          this.elementNameStack.Pop();
          goto label_14;
        }
      }
      if (this.openElement)
        this.CloseStartElement();
      this.elementNameStack.Pop();
      if (this.IsHtmlElement(str))
        this.Indent(str, true);
      this.writer.Write("</");
      this.writer.Write(str);
      this.writer.Write(">");
label_14:
      if (this.nonHtmlDepth > this.elementNameStack.Count)
        this.nonHtmlDepth = -1;
      this.openElement = false;
    }

    public override void WriteAttributeString(
      string prefix,
      string localName,
      string nsURI,
      string value)
    {
      this.writer.Write(' ');
      if (prefix != null && prefix.Length != 0)
      {
        this.writer.Write(prefix);
        this.writer.Write(":");
      }
      this.writer.Write(localName);
      if (this.nonHtmlDepth >= 0)
      {
        this.writer.Write("=\"");
        this.openAttribute = true;
        this.WriteFormattedString(value);
        this.openAttribute = false;
        this.writer.Write('"');
      }
      else
      {
        string upper = localName.ToUpper(CultureInfo.InvariantCulture);
        string lower1 = ((string) this.elementNameStack.Peek()).ToLower(CultureInfo.InvariantCulture);
        if (upper == "SELECTED" && lower1 == "option" || upper == "CHECKED" && lower1 == "input")
          return;
        this.writer.Write("=\"");
        this.openAttribute = true;
        string str1 = (string) null;
        string[] strArray = (string[]) null;
        string key = lower1;
        if (key != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (HtmlEmitter.\u003C\u003Ef__switch\u0024map17 == null)
          {
            // ISSUE: reference to a compiler-generated field
            HtmlEmitter.\u003C\u003Ef__switch\u0024map17 = new Dictionary<string, int>(14)
            {
              {
                "q",
                0
              },
              {
                "blockquote",
                0
              },
              {
                "ins",
                0
              },
              {
                "del",
                0
              },
              {
                "form",
                1
              },
              {
                "a",
                2
              },
              {
                "area",
                2
              },
              {
                "link",
                2
              },
              {
                "base",
                2
              },
              {
                "head",
                3
              },
              {
                "input",
                4
              },
              {
                "img",
                5
              },
              {
                "object",
                6
              },
              {
                "script",
                7
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (HtmlEmitter.\u003C\u003Ef__switch\u0024map17.TryGetValue(key, out num))
          {
            switch (num)
            {
              case 0:
                str1 = "cite";
                break;
              case 1:
                str1 = "action";
                break;
              case 2:
                str1 = "href";
                break;
              case 3:
                str1 = "profile";
                break;
              case 4:
                strArray = new string[2]{ "src", "usemap" };
                break;
              case 5:
                strArray = new string[3]
                {
                  "src",
                  "usemap",
                  "longdesc"
                };
                break;
              case 6:
                strArray = new string[5]
                {
                  "classid",
                  "codebase",
                  "data",
                  "archive",
                  "usemap"
                };
                break;
              case 7:
                strArray = new string[2]{ "src", "for" };
                break;
            }
          }
        }
        if (strArray != null)
        {
          string lower2 = localName.ToLower(CultureInfo.InvariantCulture);
          foreach (string str2 in strArray)
          {
            if (str2 == lower2)
            {
              value = HtmlEmitter.HtmlUriEscape.EscapeUri(value);
              break;
            }
          }
        }
        else if (str1 != null && str1 == localName.ToLower(CultureInfo.InvariantCulture))
          value = HtmlEmitter.HtmlUriEscape.EscapeUri(value);
        this.WriteFormattedString(value);
        this.openAttribute = false;
        this.writer.Write('"');
      }
    }

    public override void WriteComment(string text)
    {
      if (this.openElement)
        this.CloseStartElement();
      this.writer.Write("<!--");
      this.writer.Write(text);
      this.writer.Write("-->");
    }

    public override void WriteProcessingInstruction(string name, string text)
    {
      if (text.IndexOf("?>") > 0)
        throw new ArgumentException("Processing instruction cannot contain \"?>\" as its value.");
      if (this.openElement)
        this.CloseStartElement();
      if (this.elementNameStack.Count > 0)
        this.Indent(this.elementNameStack.Peek() as string, false);
      this.writer.Write("<?");
      this.writer.Write(name);
      if (text != null && text != string.Empty)
      {
        this.writer.Write(' ');
        this.writer.Write(text);
      }
      if (this.nonHtmlDepth >= 0)
        this.writer.Write("?>");
      else
        this.writer.Write(">");
    }

    public override void WriteString(string text)
    {
      if (this.openElement)
        this.CloseStartElement();
      this.WriteFormattedString(text);
    }

    private void WriteFormattedString(string text)
    {
      if (!this.openAttribute && this.elementNameStack.Count > 0)
      {
        string upper = ((string) this.elementNameStack.Peek()).ToUpper(CultureInfo.InvariantCulture);
        if (upper != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (HtmlEmitter.\u003C\u003Ef__switch\u0024map18 == null)
          {
            // ISSUE: reference to a compiler-generated field
            HtmlEmitter.\u003C\u003Ef__switch\u0024map18 = new Dictionary<string, int>(2)
            {
              {
                "SCRIPT",
                0
              },
              {
                "STYLE",
                0
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (HtmlEmitter.\u003C\u003Ef__switch\u0024map18.TryGetValue(upper, out num) && num == 0)
          {
            this.writer.Write(text);
            return;
          }
        }
      }
      int index1 = 0;
      for (int index2 = 0; index2 < text.Length; ++index2)
      {
        char ch = text[index2];
        switch (ch)
        {
          case '<':
            if (!this.openAttribute)
            {
              this.writer.Write(text.ToCharArray(), index1, index2 - index1);
              this.writer.Write("&lt;");
              index1 = index2 + 1;
              break;
            }
            break;
          case '>':
            if (!this.openAttribute)
            {
              this.writer.Write(text.ToCharArray(), index1, index2 - index1);
              this.writer.Write("&gt;");
              index1 = index2 + 1;
              break;
            }
            break;
          default:
            if (ch != '"')
            {
              if (ch == '&' && (this.nonHtmlDepth >= 0 || index2 + 1 >= text.Length || text[index2 + 1] != '{'))
              {
                this.writer.Write(text.ToCharArray(), index1, index2 - index1);
                this.writer.Write("&amp;");
                index1 = index2 + 1;
                break;
              }
              break;
            }
            if (this.openAttribute)
            {
              this.writer.Write(text.ToCharArray(), index1, index2 - index1);
              this.writer.Write("&quot;");
              index1 = index2 + 1;
              break;
            }
            break;
        }
      }
      if (text.Length <= index1)
        return;
      this.writer.Write(text.ToCharArray(), index1, text.Length - index1);
    }

    public override void WriteRaw(string data)
    {
      if (this.openElement)
        this.CloseStartElement();
      this.writer.Write(data);
    }

    public override void WriteCDataSection(string text)
    {
      if (this.openElement)
        this.CloseStartElement();
      this.writer.Write(text);
    }

    public override void WriteWhitespace(string value)
    {
      if (this.openElement)
        this.CloseStartElement();
      this.writer.Write(value);
    }

    public override void Done() => this.writer.Flush();

    private class HtmlUriEscape : Uri
    {
      private HtmlUriEscape()
        : base("urn:foo")
      {
      }

      public static string EscapeUri(string input)
      {
        StringBuilder stringBuilder = new StringBuilder();
        int startIndex = 0;
        for (int index = 0; index < input.Length; ++index)
        {
          char ch1 = input[index];
          if (ch1 >= ' ' && ch1 <= '\u007F')
          {
            char ch2 = ch1;
            bool flag;
            switch (ch2)
            {
              case '"':
              case '&':
              case '\'':
label_4:
                flag = true;
                break;
              default:
                switch (ch2)
                {
                  case '<':
                  case '>':
                    goto label_4;
                  default:
                    flag = Uri.IsExcludedCharacter(ch1);
                    break;
                }
                break;
            }
            if (flag)
            {
              stringBuilder.Append(Uri.EscapeString(input.Substring(startIndex, index - startIndex)));
              stringBuilder.Append(ch1);
              startIndex = index + 1;
            }
          }
        }
        if (startIndex < input.Length)
          stringBuilder.Append(Uri.EscapeString(input.Substring(startIndex)));
        return stringBuilder.ToString();
      }
    }
  }
}
