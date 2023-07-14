// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XslOutput
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class XslOutput
  {
    private string uri;
    private XmlQualifiedName customMethod;
    private OutputMethod method = OutputMethod.Unknown;
    private string version;
    private Encoding encoding = Encoding.UTF8;
    private bool omitXmlDeclaration;
    private StandaloneType standalone;
    private string doctypePublic;
    private string doctypeSystem;
    private XmlQualifiedName[] cdataSectionElements;
    private string indent;
    private string mediaType;
    private string stylesheetVersion;
    private ArrayList cdSectsList = new ArrayList();

    public XslOutput(string uri, string stylesheetVersion)
    {
      this.uri = uri;
      this.stylesheetVersion = stylesheetVersion;
    }

    public OutputMethod Method => this.method;

    public XmlQualifiedName CustomMethod => this.customMethod;

    public string Version => this.version;

    public Encoding Encoding => this.encoding;

    public string Uri => this.uri;

    public bool OmitXmlDeclaration => this.omitXmlDeclaration;

    public StandaloneType Standalone => this.standalone;

    public string DoctypePublic => this.doctypePublic;

    public string DoctypeSystem => this.doctypeSystem;

    public XmlQualifiedName[] CDataSectionElements
    {
      get
      {
        if (this.cdataSectionElements == null)
          this.cdataSectionElements = this.cdSectsList.ToArray(typeof (XmlQualifiedName)) as XmlQualifiedName[];
        return this.cdataSectionElements;
      }
    }

    public string Indent => this.indent;

    public string MediaType => this.mediaType;

    public void Fill(XPathNavigator nav)
    {
      if (!nav.MoveToFirstAttribute())
        return;
      this.ProcessAttribute(nav);
      while (nav.MoveToNextAttribute())
        this.ProcessAttribute(nav);
      nav.MoveToParent();
    }

    private void ProcessAttribute(XPathNavigator nav)
    {
      if (nav.NamespaceURI != string.Empty)
        return;
      string str = nav.Value;
      string localName = nav.LocalName;
      if (localName != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XslOutput.\u003C\u003Ef__switch\u0024map23 == null)
        {
          // ISSUE: reference to a compiler-generated field
          XslOutput.\u003C\u003Ef__switch\u0024map23 = new Dictionary<string, int>(10)
          {
            {
              "cdata-section-elements",
              0
            },
            {
              "method",
              1
            },
            {
              "version",
              2
            },
            {
              "encoding",
              3
            },
            {
              "standalone",
              4
            },
            {
              "doctype-public",
              5
            },
            {
              "doctype-system",
              6
            },
            {
              "media-type",
              7
            },
            {
              "omit-xml-declaration",
              8
            },
            {
              "indent",
              9
            }
          };
        }
        int num1;
        // ISSUE: reference to a compiler-generated field
        if (XslOutput.\u003C\u003Ef__switch\u0024map23.TryGetValue(localName, out num1))
        {
          switch (num1)
          {
            case 0:
              if (str.Length <= 0)
                return;
              this.cdSectsList.AddRange((ICollection) XslNameUtil.FromListString(str, nav));
              return;
            case 1:
              if (str.Length == 0)
                return;
              string key1 = str;
              if (key1 != null)
              {
                // ISSUE: reference to a compiler-generated field
                if (XslOutput.\u003C\u003Ef__switch\u0024map1F == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  XslOutput.\u003C\u003Ef__switch\u0024map1F = new Dictionary<string, int>(3)
                  {
                    {
                      "xml",
                      0
                    },
                    {
                      "html",
                      1
                    },
                    {
                      "text",
                      2
                    }
                  };
                }
                int num2;
                // ISSUE: reference to a compiler-generated field
                if (XslOutput.\u003C\u003Ef__switch\u0024map1F.TryGetValue(key1, out num2))
                {
                  switch (num2)
                  {
                    case 0:
                      this.method = OutputMethod.XML;
                      return;
                    case 1:
                      this.omitXmlDeclaration = true;
                      this.method = OutputMethod.HTML;
                      return;
                    case 2:
                      this.omitXmlDeclaration = true;
                      this.method = OutputMethod.Text;
                      return;
                  }
                }
              }
              this.method = OutputMethod.Custom;
              this.customMethod = XslNameUtil.FromString(str, nav);
              if (!(this.customMethod.Namespace == string.Empty))
                return;
              IXmlLineInfo xmlLineInfo1 = nav as IXmlLineInfo;
              throw new XsltCompileException((Exception) new ArgumentException("Invalid output method value: '" + str + "'. It must be either 'xml' or 'html' or 'text' or QName."), nav.BaseURI, xmlLineInfo1 == null ? 0 : xmlLineInfo1.LineNumber, xmlLineInfo1 == null ? 0 : xmlLineInfo1.LinePosition);
            case 2:
              if (str.Length <= 0)
                return;
              this.version = str;
              return;
            case 3:
              if (str.Length <= 0)
                return;
              try
              {
                this.encoding = Encoding.GetEncoding(str);
                return;
              }
              catch (ArgumentException ex)
              {
                return;
              }
              catch (NotSupportedException ex)
              {
                return;
              }
            case 4:
              string key2 = str;
              if (key2 != null)
              {
                // ISSUE: reference to a compiler-generated field
                if (XslOutput.\u003C\u003Ef__switch\u0024map20 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  XslOutput.\u003C\u003Ef__switch\u0024map20 = new Dictionary<string, int>(2)
                  {
                    {
                      "yes",
                      0
                    },
                    {
                      "no",
                      1
                    }
                  };
                }
                int num3;
                // ISSUE: reference to a compiler-generated field
                if (XslOutput.\u003C\u003Ef__switch\u0024map20.TryGetValue(key2, out num3))
                {
                  if (num3 != 0)
                  {
                    if (num3 == 1)
                    {
                      this.standalone = StandaloneType.NO;
                      return;
                    }
                  }
                  else
                  {
                    this.standalone = StandaloneType.YES;
                    return;
                  }
                }
              }
              if (this.stylesheetVersion != "1.0")
                return;
              IXmlLineInfo xmlLineInfo2 = nav as IXmlLineInfo;
              throw new XsltCompileException((Exception) new XsltException("'" + str + "' is an invalid value for 'standalone' attribute.", (Exception) null), nav.BaseURI, xmlLineInfo2 == null ? 0 : xmlLineInfo2.LineNumber, xmlLineInfo2 == null ? 0 : xmlLineInfo2.LinePosition);
            case 5:
              this.doctypePublic = str;
              return;
            case 6:
              this.doctypeSystem = str;
              return;
            case 7:
              if (str.Length <= 0)
                return;
              this.mediaType = str;
              return;
            case 8:
              string key3 = str;
              if (key3 != null)
              {
                // ISSUE: reference to a compiler-generated field
                if (XslOutput.\u003C\u003Ef__switch\u0024map21 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  XslOutput.\u003C\u003Ef__switch\u0024map21 = new Dictionary<string, int>(2)
                  {
                    {
                      "yes",
                      0
                    },
                    {
                      "no",
                      1
                    }
                  };
                }
                int num4;
                // ISSUE: reference to a compiler-generated field
                if (XslOutput.\u003C\u003Ef__switch\u0024map21.TryGetValue(key3, out num4))
                {
                  if (num4 != 0)
                  {
                    if (num4 == 1)
                    {
                      this.omitXmlDeclaration = false;
                      return;
                    }
                  }
                  else
                  {
                    this.omitXmlDeclaration = true;
                    return;
                  }
                }
              }
              if (this.stylesheetVersion != "1.0")
                return;
              IXmlLineInfo xmlLineInfo3 = nav as IXmlLineInfo;
              throw new XsltCompileException((Exception) new XsltException("'" + str + "' is an invalid value for 'omit-xml-declaration' attribute.", (Exception) null), nav.BaseURI, xmlLineInfo3 == null ? 0 : xmlLineInfo3.LineNumber, xmlLineInfo3 == null ? 0 : xmlLineInfo3.LinePosition);
            case 9:
              this.indent = str;
              if (this.stylesheetVersion != "1.0")
                return;
              string key4 = str;
              if (key4 != null)
              {
                // ISSUE: reference to a compiler-generated field
                if (XslOutput.\u003C\u003Ef__switch\u0024map22 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  XslOutput.\u003C\u003Ef__switch\u0024map22 = new Dictionary<string, int>(2)
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
                int num5;
                // ISSUE: reference to a compiler-generated field
                if (XslOutput.\u003C\u003Ef__switch\u0024map22.TryGetValue(key4, out num5) && num5 == 0)
                  return;
              }
              if (this.method == OutputMethod.Custom)
                return;
              throw new XsltCompileException(string.Format("Unexpected 'indent' attribute value in 'output' element: '{0}'", (object) str), (Exception) null, nav);
          }
        }
      }
      if (!(this.stylesheetVersion != "1.0"))
      {
        IXmlLineInfo xmlLineInfo = nav as IXmlLineInfo;
        throw new XsltCompileException((Exception) new XsltException("'" + nav.LocalName + "' is an invalid attribute for 'output' element.", (Exception) null), nav.BaseURI, xmlLineInfo == null ? 0 : xmlLineInfo.LineNumber, xmlLineInfo == null ? 0 : xmlLineInfo.LinePosition);
      }
    }
  }
}
