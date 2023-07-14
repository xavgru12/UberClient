// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlTextWriter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace System.Xml
{
  public class XmlTextWriter : XmlWriter
  {
    private const string XmlNamespace = "http://www.w3.org/XML/1998/namespace";
    private const string XmlnsNamespace = "http://www.w3.org/2000/xmlns/";
    private static readonly Encoding unmarked_utf8encoding = (Encoding) new UTF8Encoding(false, false);
    private static char[] escaped_text_chars;
    private static char[] escaped_attr_chars;
    private Stream base_stream;
    private TextWriter source;
    private TextWriter writer;
    private StringWriter preserver;
    private string preserved_name;
    private bool is_preserved_xmlns;
    private bool allow_doc_fragment;
    private bool close_output_stream = true;
    private bool ignore_encoding;
    private bool namespaces = true;
    private XmlTextWriter.XmlDeclState xmldecl_state;
    private bool check_character_validity;
    private NewLineHandling newline_handling = NewLineHandling.None;
    private bool is_document_entity;
    private WriteState state;
    private XmlNodeType node_state;
    private XmlNamespaceManager nsmanager;
    private int open_count;
    private XmlTextWriter.XmlNodeInfo[] elements = new XmlTextWriter.XmlNodeInfo[10];
    private Stack new_local_namespaces = new Stack();
    private ArrayList explicit_nsdecls = new ArrayList();
    private NamespaceHandling namespace_handling;
    private bool indent;
    private int indent_count = 2;
    private char indent_char = ' ';
    private string indent_string = "  ";
    private string newline;
    private bool indent_attributes;
    private char quote_char = '"';
    private bool v2;

    public XmlTextWriter(string filename, Encoding encoding)
      : this((Stream) new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None), encoding)
    {
    }

    public XmlTextWriter(Stream stream, Encoding encoding)
      : this((TextWriter) new StreamWriter(stream, encoding != null ? encoding : XmlTextWriter.unmarked_utf8encoding))
    {
      this.ignore_encoding = encoding == null;
      this.Initialize(this.writer);
      this.allow_doc_fragment = true;
    }

    public XmlTextWriter(TextWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      this.ignore_encoding = writer.Encoding == null;
      this.Initialize(writer);
      this.allow_doc_fragment = true;
    }

    internal XmlTextWriter(TextWriter writer, XmlWriterSettings settings, bool closeOutput)
    {
      this.v2 = true;
      if (settings == null)
        settings = new XmlWriterSettings();
      this.Initialize(writer);
      this.close_output_stream = closeOutput;
      this.allow_doc_fragment = settings.ConformanceLevel != ConformanceLevel.Document;
      switch (settings.ConformanceLevel)
      {
        case ConformanceLevel.Auto:
          this.xmldecl_state = !settings.OmitXmlDeclaration ? XmlTextWriter.XmlDeclState.Allow : XmlTextWriter.XmlDeclState.Ignore;
          break;
        case ConformanceLevel.Fragment:
          this.xmldecl_state = XmlTextWriter.XmlDeclState.Prohibit;
          break;
        case ConformanceLevel.Document:
          this.xmldecl_state = !settings.OmitXmlDeclaration ? XmlTextWriter.XmlDeclState.Auto : XmlTextWriter.XmlDeclState.Ignore;
          break;
      }
      if (settings.Indent)
        this.Formatting = Formatting.Indented;
      this.indent_string = settings.IndentChars != null ? settings.IndentChars : string.Empty;
      if (settings.NewLineChars != null)
        this.newline = settings.NewLineChars;
      this.indent_attributes = settings.NewLineOnAttributes;
      this.check_character_validity = settings.CheckCharacters;
      this.newline_handling = settings.NewLineHandling;
      this.namespace_handling = settings.NamespaceHandling;
    }

    private void Initialize(TextWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      XmlNameTable nameTable = (XmlNameTable) new NameTable();
      this.writer = writer;
      if (writer is StreamWriter)
        this.base_stream = ((StreamWriter) writer).BaseStream;
      this.source = writer;
      this.nsmanager = new XmlNamespaceManager(nameTable);
      this.newline = writer.NewLine;
      char[] chArray;
      if (this.newline_handling != NewLineHandling.None)
        chArray = new char[5]{ '&', '<', '>', '\r', '\n' };
      else
        chArray = new char[3]{ '&', '<', '>' };
      XmlTextWriter.escaped_text_chars = chArray;
      XmlTextWriter.escaped_attr_chars = new char[6]
      {
        '"',
        '&',
        '<',
        '>',
        '\r',
        '\n'
      };
    }

    public Formatting Formatting
    {
      get => this.indent ? Formatting.Indented : Formatting.None;
      set => this.indent = value == Formatting.Indented;
    }

    public int Indentation
    {
      get => this.indent_count;
      set
      {
        this.indent_count = value >= 0 ? value : throw this.ArgumentError("Indentation must be non-negative integer.");
        this.indent_string = value != 0 ? new string(this.indent_char, this.indent_count) : string.Empty;
      }
    }

    public char IndentChar
    {
      get => this.indent_char;
      set
      {
        this.indent_char = value;
        this.indent_string = new string(this.indent_char, this.indent_count);
      }
    }

    public char QuoteChar
    {
      get => this.quote_char;
      set
      {
        if (this.state == WriteState.Attribute)
          throw this.InvalidOperation("QuoteChar must not be changed inside attribute value.");
        this.quote_char = value == '\'' || value == '"' ? value : throw this.ArgumentError("Only ' and \" are allowed as an attribute quote character.");
        XmlTextWriter.escaped_attr_chars[0] = this.quote_char;
      }
    }

    public override string XmlLang => this.open_count == 0 ? (string) null : this.elements[this.open_count - 1].XmlLang;

    public override XmlSpace XmlSpace => this.open_count == 0 ? XmlSpace.None : this.elements[this.open_count - 1].XmlSpace;

    public override WriteState WriteState => this.state;

    public override string LookupPrefix(string namespaceUri)
    {
      if (namespaceUri == null || namespaceUri == string.Empty)
        throw this.ArgumentError("The Namespace cannot be empty.");
      return namespaceUri == this.nsmanager.DefaultNamespace ? string.Empty : this.nsmanager.LookupPrefixExclusive(namespaceUri, false);
    }

    public Stream BaseStream => this.base_stream;

    public override void Close()
    {
      if (this.state != WriteState.Error)
      {
        if (this.state == WriteState.Attribute)
          this.WriteEndAttribute();
        while (this.open_count > 0)
          this.WriteEndElement();
      }
      if (this.close_output_stream)
        this.writer.Close();
      else
        this.writer.Flush();
      this.state = WriteState.Closed;
    }

    public override void Flush() => this.writer.Flush();

    public bool Namespaces
    {
      get => this.namespaces;
      set
      {
        if (this.state != WriteState.Start)
          throw this.InvalidOperation("This property must be set before writing output.");
        this.namespaces = value;
      }
    }

    public override void WriteStartDocument()
    {
      this.WriteStartDocumentCore(false, false);
      this.is_document_entity = true;
    }

    public override void WriteStartDocument(bool standalone)
    {
      this.WriteStartDocumentCore(true, standalone);
      this.is_document_entity = true;
    }

    private void WriteStartDocumentCore(bool outputStd, bool standalone)
    {
      if (this.state != WriteState.Start)
        throw this.StateError("XmlDeclaration");
      switch (this.xmldecl_state)
      {
        case XmlTextWriter.XmlDeclState.Ignore:
          break;
        case XmlTextWriter.XmlDeclState.Prohibit:
          throw this.InvalidOperation("WriteStartDocument cannot be called when ConformanceLevel is Fragment.");
        default:
          this.state = WriteState.Prolog;
          this.writer.Write("<?xml version=");
          this.writer.Write(this.quote_char);
          this.writer.Write("1.0");
          this.writer.Write(this.quote_char);
          if (!this.ignore_encoding)
          {
            this.writer.Write(" encoding=");
            this.writer.Write(this.quote_char);
            this.writer.Write(this.writer.Encoding.WebName);
            this.writer.Write(this.quote_char);
          }
          if (outputStd)
          {
            this.writer.Write(" standalone=");
            this.writer.Write(this.quote_char);
            this.writer.Write(!standalone ? "no" : "yes");
            this.writer.Write(this.quote_char);
          }
          this.writer.Write("?>");
          this.xmldecl_state = XmlTextWriter.XmlDeclState.Ignore;
          break;
      }
    }

    public override void WriteEndDocument()
    {
      switch (this.state)
      {
        case WriteState.Start:
        case WriteState.Closed:
        case WriteState.Error:
          throw this.StateError("EndDocument");
        default:
          if (this.state == WriteState.Attribute)
            this.WriteEndAttribute();
          while (this.open_count > 0)
            this.WriteEndElement();
          this.state = WriteState.Start;
          this.is_document_entity = false;
          break;
      }
    }

    public override void WriteDocType(string name, string pubid, string sysid, string subset)
    {
      if (name == null)
        throw this.ArgumentError(nameof (name));
      if (!XmlChar.IsName(name))
        throw this.ArgumentError(nameof (name));
      this.node_state = this.node_state == XmlNodeType.None ? XmlNodeType.DocumentType : throw this.StateError("DocType");
      if (this.xmldecl_state == XmlTextWriter.XmlDeclState.Auto)
        this.OutputAutoStartDocument();
      this.WriteIndent();
      this.writer.Write("<!DOCTYPE ");
      this.writer.Write(name);
      if (pubid != null)
      {
        this.writer.Write(" PUBLIC ");
        this.writer.Write(this.quote_char);
        this.writer.Write(pubid);
        this.writer.Write(this.quote_char);
        this.writer.Write(' ');
        this.writer.Write(this.quote_char);
        if (sysid != null)
          this.writer.Write(sysid);
        this.writer.Write(this.quote_char);
      }
      else if (sysid != null)
      {
        this.writer.Write(" SYSTEM ");
        this.writer.Write(this.quote_char);
        this.writer.Write(sysid);
        this.writer.Write(this.quote_char);
      }
      if (subset != null)
      {
        this.writer.Write("[");
        this.writer.Write(subset);
        this.writer.Write("]");
      }
      this.writer.Write('>');
      this.state = WriteState.Prolog;
    }

    public override void WriteStartElement(string prefix, string localName, string namespaceUri)
    {
      if (this.state == WriteState.Error || this.state == WriteState.Closed)
        throw this.StateError("StartTag");
      this.node_state = XmlNodeType.Element;
      bool flag = prefix == null;
      if (prefix == null)
        prefix = string.Empty;
      if (!this.namespaces && namespaceUri != null && namespaceUri.Length > 0)
        throw this.ArgumentError("Namespace is disabled in this XmlTextWriter.");
      if (!this.namespaces && prefix.Length > 0)
        throw this.ArgumentError("Namespace prefix is disabled in this XmlTextWriter.");
      if (prefix.Length > 0 && namespaceUri == null)
      {
        namespaceUri = this.nsmanager.LookupNamespace(prefix, false);
        if (namespaceUri == null || namespaceUri.Length == 0)
          throw this.ArgumentError("Namespace URI must not be null when prefix is not an empty string.");
      }
      if (this.namespaces && prefix != null && prefix.Length == 3 && namespaceUri != "http://www.w3.org/XML/1998/namespace" && (prefix[0] == 'x' || prefix[0] == 'X') && (prefix[1] == 'm' || prefix[1] == 'M') && (prefix[2] == 'l' || prefix[2] == 'L'))
        throw new ArgumentException("A prefix cannot be equivalent to \"xml\" in case-insensitive match.");
      if (this.xmldecl_state == XmlTextWriter.XmlDeclState.Auto)
        this.OutputAutoStartDocument();
      if (this.state == WriteState.Element)
        this.CloseStartElement();
      if (this.open_count > 0)
        this.elements[this.open_count - 1].HasElements = true;
      this.nsmanager.PushScope();
      if (this.namespaces && namespaceUri != null)
      {
        if (flag && namespaceUri.Length > 0)
          prefix = this.LookupPrefix(namespaceUri);
        if (prefix == null || namespaceUri.Length == 0)
          prefix = string.Empty;
      }
      this.WriteIndent();
      this.writer.Write("<");
      if (prefix.Length > 0)
      {
        this.writer.Write(prefix);
        this.writer.Write(':');
      }
      this.writer.Write(localName);
      if (this.elements.Length == this.open_count)
      {
        XmlTextWriter.XmlNodeInfo[] destinationArray = new XmlTextWriter.XmlNodeInfo[this.open_count << 1];
        Array.Copy((Array) this.elements, (Array) destinationArray, this.open_count);
        this.elements = destinationArray;
      }
      if (this.elements[this.open_count] == null)
        this.elements[this.open_count] = new XmlTextWriter.XmlNodeInfo();
      XmlTextWriter.XmlNodeInfo element = this.elements[this.open_count];
      element.Prefix = prefix;
      element.LocalName = localName;
      element.NS = namespaceUri;
      element.HasSimple = false;
      element.HasElements = false;
      element.XmlLang = this.XmlLang;
      element.XmlSpace = this.XmlSpace;
      ++this.open_count;
      if (this.namespaces && namespaceUri != null && this.nsmanager.LookupNamespace(prefix, false) != namespaceUri)
      {
        this.nsmanager.AddNamespace(prefix, namespaceUri);
        this.new_local_namespaces.Push((object) prefix);
      }
      this.state = WriteState.Element;
    }

    private void CloseStartElement()
    {
      this.CloseStartElementCore();
      if (this.state == WriteState.Element)
        this.writer.Write('>');
      this.state = WriteState.Content;
    }

    private void CloseStartElementCore()
    {
      if (this.state == WriteState.Attribute)
        this.WriteEndAttribute();
      if (this.new_local_namespaces.Count == 0)
      {
        if (this.explicit_nsdecls.Count <= 0)
          return;
        this.explicit_nsdecls.Clear();
      }
      else
      {
        int count = this.explicit_nsdecls.Count;
        while (this.new_local_namespaces.Count > 0)
        {
          string str = (string) this.new_local_namespaces.Pop();
          bool flag = false;
          for (int index = 0; index < this.explicit_nsdecls.Count; ++index)
          {
            if ((string) this.explicit_nsdecls[index] == str)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            this.explicit_nsdecls.Add((object) str);
        }
        for (int index = count; index < this.explicit_nsdecls.Count; ++index)
        {
          string explicitNsdecl = (string) this.explicit_nsdecls[index];
          string text = this.nsmanager.LookupNamespace(explicitNsdecl, false);
          if (text != null)
          {
            if (explicitNsdecl.Length > 0)
            {
              this.writer.Write(" xmlns:");
              this.writer.Write(explicitNsdecl);
            }
            else
              this.writer.Write(" xmlns");
            this.writer.Write('=');
            this.writer.Write(this.quote_char);
            this.WriteEscapedString(text, true);
            this.writer.Write(this.quote_char);
          }
        }
        this.explicit_nsdecls.Clear();
      }
    }

    public override void WriteEndElement() => this.WriteEndElementCore(false);

    public override void WriteFullEndElement() => this.WriteEndElementCore(true);

    private void WriteEndElementCore(bool full)
    {
      if (this.state == WriteState.Error || this.state == WriteState.Closed)
        throw this.StateError("EndElement");
      if (this.open_count == 0)
        throw this.InvalidOperation("There is no more open element.");
      this.CloseStartElementCore();
      this.nsmanager.PopScope();
      if (this.state == WriteState.Element)
      {
        if (full)
          this.writer.Write('>');
        else
          this.writer.Write(" />");
      }
      if (full || this.state == WriteState.Content)
        this.WriteIndentEndElement();
      XmlTextWriter.XmlNodeInfo element = this.elements[--this.open_count];
      if (full || this.state == WriteState.Content)
      {
        this.writer.Write("</");
        if (element.Prefix.Length > 0)
        {
          this.writer.Write(element.Prefix);
          this.writer.Write(':');
        }
        this.writer.Write(element.LocalName);
        this.writer.Write('>');
      }
      this.state = WriteState.Content;
      if (this.open_count != 0)
        return;
      this.node_state = XmlNodeType.EndElement;
    }

    public override void WriteStartAttribute(string prefix, string localName, string namespaceUri)
    {
      if (this.state == WriteState.Attribute)
        this.WriteEndAttribute();
      if (this.state != WriteState.Element && this.state != WriteState.Start)
        throw this.StateError("Attribute");
      if (prefix == null)
        prefix = string.Empty;
      bool flag;
      if (namespaceUri == "http://www.w3.org/2000/xmlns/")
      {
        flag = true;
        if (prefix.Length == 0 && localName != "xmlns")
          prefix = "xmlns";
      }
      else
        flag = prefix == "xmlns" || localName == "xmlns" && prefix.Length == 0;
      if (this.namespaces)
      {
        if (prefix == "xml")
          namespaceUri = "http://www.w3.org/XML/1998/namespace";
        else if (namespaceUri == null)
          namespaceUri = !flag ? string.Empty : "http://www.w3.org/2000/xmlns/";
        if (flag && namespaceUri != "http://www.w3.org/2000/xmlns/")
          throw this.ArgumentError(string.Format("The 'xmlns' attribute is bound to the reserved namespace '{0}'", (object) "http://www.w3.org/2000/xmlns/"));
        if (prefix.Length > 0 && namespaceUri.Length == 0)
        {
          namespaceUri = this.nsmanager.LookupNamespace(prefix, false);
          if (namespaceUri == null || namespaceUri.Length == 0)
            throw this.ArgumentError("Namespace URI must not be null when prefix is not an empty string.");
        }
        if (!flag && namespaceUri.Length > 0)
          prefix = this.DetermineAttributePrefix(prefix, localName, namespaceUri);
      }
      if (this.indent_attributes)
        this.WriteIndentAttribute();
      else if (this.state != WriteState.Start)
        this.writer.Write(' ');
      if (prefix.Length > 0)
      {
        this.writer.Write(prefix);
        this.writer.Write(':');
      }
      this.writer.Write(localName);
      this.writer.Write('=');
      this.writer.Write(this.quote_char);
      if (flag || prefix == "xml")
      {
        if (this.preserver == null)
          this.preserver = new StringWriter();
        else
          this.preserver.GetStringBuilder().Length = 0;
        this.writer = (TextWriter) this.preserver;
        if (!flag)
        {
          this.is_preserved_xmlns = false;
          this.preserved_name = localName;
        }
        else
        {
          this.is_preserved_xmlns = true;
          this.preserved_name = !(localName == "xmlns") ? localName : string.Empty;
        }
      }
      this.state = WriteState.Attribute;
    }

    private string DetermineAttributePrefix(string prefix, string local, string ns)
    {
      bool flag = false;
      if (prefix.Length == 0)
      {
        prefix = this.LookupPrefix(ns);
        if (prefix != null && prefix.Length > 0)
          return prefix;
        flag = true;
      }
      else
      {
        prefix = this.nsmanager.NameTable.Add(prefix);
        string uri = this.nsmanager.LookupNamespace(prefix, true);
        if (uri == ns)
          return prefix;
        if (uri != null)
        {
          this.nsmanager.RemoveNamespace(prefix, uri);
          if (this.nsmanager.LookupNamespace(prefix, true) != uri)
          {
            flag = true;
            this.nsmanager.AddNamespace(prefix, uri);
          }
        }
      }
      if (flag)
        prefix = this.MockupPrefix(ns, true);
      this.new_local_namespaces.Push((object) prefix);
      this.nsmanager.AddNamespace(prefix, ns);
      return prefix;
    }

    private string MockupPrefix(string ns, bool skipLookup)
    {
      string str1 = !skipLookup ? this.LookupPrefix(ns) : (string) null;
      if (str1 != null && str1.Length > 0)
        return str1;
      int num = 1;
      string str2;
      while (true)
      {
        str2 = XmlTextWriter.StringUtil.Format("d{0}p{1}", (object) this.open_count, (object) num);
        if (this.new_local_namespaces.Contains((object) str2) || this.nsmanager.LookupNamespace(this.nsmanager.NameTable.Get(str2)) != null)
          ++num;
        else
          break;
      }
      this.nsmanager.AddNamespace(str2, ns);
      this.new_local_namespaces.Push((object) str2);
      return str2;
    }

    public override void WriteEndAttribute()
    {
      if (this.state != WriteState.Attribute)
        throw this.StateError("End of attribute");
      if (this.writer == this.preserver)
      {
        this.writer = this.source;
        string uri = this.preserver.ToString();
        if (this.is_preserved_xmlns)
        {
          if (this.preserved_name.Length > 0 && uri.Length == 0)
            throw this.ArgumentError("Non-empty prefix must be mapped to non-empty namespace URI.");
          string str = this.nsmanager.LookupNamespace(this.preserved_name, false);
          if ((this.namespace_handling & NamespaceHandling.OmitDuplicates) == NamespaceHandling.Default || str != uri)
            this.explicit_nsdecls.Add((object) this.preserved_name);
          if (this.open_count > 0)
          {
            if (this.v2 && this.elements[this.open_count - 1].Prefix == this.preserved_name && this.elements[this.open_count - 1].NS != uri)
              throw new XmlException(string.Format("Cannot redefine the namespace for prefix '{0}' used at current element", (object) this.preserved_name));
            if ((!(this.elements[this.open_count - 1].NS == string.Empty) || !(this.elements[this.open_count - 1].Prefix == this.preserved_name)) && str != uri)
              this.nsmanager.AddNamespace(this.preserved_name, uri);
          }
        }
        else
        {
          string preservedName = this.preserved_name;
          if (preservedName != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (XmlTextWriter.\u003C\u003Ef__switch\u0024map54 == null)
            {
              // ISSUE: reference to a compiler-generated field
              XmlTextWriter.\u003C\u003Ef__switch\u0024map54 = new Dictionary<string, int>(2)
              {
                {
                  "lang",
                  0
                },
                {
                  "space",
                  1
                }
              };
            }
            int num1;
            // ISSUE: reference to a compiler-generated field
            if (XmlTextWriter.\u003C\u003Ef__switch\u0024map54.TryGetValue(preservedName, out num1))
            {
              switch (num1)
              {
                case 0:
                  if (this.open_count > 0)
                  {
                    this.elements[this.open_count - 1].XmlLang = uri;
                    break;
                  }
                  break;
                case 1:
                  string key = uri;
                  if (key != null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (XmlTextWriter.\u003C\u003Ef__switch\u0024map53 == null)
                    {
                      // ISSUE: reference to a compiler-generated field
                      XmlTextWriter.\u003C\u003Ef__switch\u0024map53 = new Dictionary<string, int>(2)
                      {
                        {
                          "default",
                          0
                        },
                        {
                          "preserve",
                          1
                        }
                      };
                    }
                    int num2;
                    // ISSUE: reference to a compiler-generated field
                    if (XmlTextWriter.\u003C\u003Ef__switch\u0024map53.TryGetValue(key, out num2))
                    {
                      if (num2 != 0)
                      {
                        if (num2 == 1)
                        {
                          if (this.open_count > 0)
                          {
                            this.elements[this.open_count - 1].XmlSpace = XmlSpace.Preserve;
                            break;
                          }
                          break;
                        }
                      }
                      else
                      {
                        if (this.open_count > 0)
                        {
                          this.elements[this.open_count - 1].XmlSpace = XmlSpace.Default;
                          break;
                        }
                        break;
                      }
                    }
                  }
                  throw this.ArgumentError("Invalid value for xml:space.");
              }
            }
          }
        }
        this.writer.Write(uri);
      }
      this.writer.Write(this.quote_char);
      this.state = WriteState.Element;
    }

    public override void WriteComment(string text)
    {
      if (text == null)
        throw this.ArgumentError(nameof (text));
      if (text.Length > 0 && text[text.Length - 1] == '-')
        throw this.ArgumentError("An input string to WriteComment method must not end with '-'. Escape it with '&#2D;'.");
      if (XmlTextWriter.StringUtil.IndexOf(text, "--") > 0)
        throw this.ArgumentError("An XML comment cannot end with \"-\".");
      if (this.state == WriteState.Attribute || this.state == WriteState.Element)
        this.CloseStartElement();
      this.WriteIndent();
      this.ShiftStateTopLevel("Comment", false, false, false);
      this.writer.Write("<!--");
      this.writer.Write(text);
      this.writer.Write("-->");
    }

    public override void WriteProcessingInstruction(string name, string text)
    {
      if (name == null)
        throw this.ArgumentError(nameof (name));
      if (text == null)
        throw this.ArgumentError(nameof (text));
      this.WriteIndent();
      if (!XmlChar.IsName(name))
        throw this.ArgumentError("A processing instruction name must be a valid XML name.");
      if (XmlTextWriter.StringUtil.IndexOf(text, "?>") > 0)
        throw this.ArgumentError("Processing instruction cannot contain \"?>\" as its value.");
      this.ShiftStateTopLevel("ProcessingInstruction", false, name == "xml", false);
      this.writer.Write("<?");
      this.writer.Write(name);
      this.writer.Write(' ');
      this.writer.Write(text);
      this.writer.Write("?>");
      if (this.state != WriteState.Start)
        return;
      this.state = WriteState.Prolog;
    }

    public override void WriteWhitespace(string text)
    {
      switch (text)
      {
        case null:
          throw this.ArgumentError(nameof (text));
        case "":
          throw this.ArgumentError("WriteWhitespace method accepts only whitespaces.");
        default:
          if (XmlChar.IndexOfNonWhitespace(text) < 0)
          {
            this.ShiftStateTopLevel("Whitespace", true, false, true);
            this.writer.Write(text);
            break;
          }
          goto case "";
      }
    }

    public override void WriteCData(string text)
    {
      if (text == null)
        text = string.Empty;
      this.ShiftStateContent("CData", false);
      if (XmlTextWriter.StringUtil.IndexOf(text, "]]>") >= 0)
        throw this.ArgumentError("CDATA section must not contain ']]>'.");
      this.writer.Write("<![CDATA[");
      this.WriteCheckedString(text);
      this.writer.Write("]]>");
    }

    public override void WriteString(string text)
    {
      switch (text)
      {
        case null:
          return;
        case "":
          if (!this.v2)
            return;
          break;
      }
      this.ShiftStateContent("Text", true);
      this.WriteEscapedString(text, this.state == WriteState.Attribute);
    }

    public override void WriteRaw(string raw)
    {
      if (raw == null)
        return;
      this.ShiftStateTopLevel("Raw string", true, true, true);
      this.writer.Write(raw);
    }

    public override void WriteCharEntity(char ch) => this.WriteCharacterEntity(ch, char.MinValue, false);

    public override void WriteSurrogateCharEntity(char low, char high) => this.WriteCharacterEntity(low, high, true);

    private void WriteCharacterEntity(char ch, char high, bool surrogate)
    {
      if (surrogate && ('\uD800' > high || high > '\uDC00' || '\uDC00' > ch || ch > '\uDFFF'))
        throw this.ArgumentError(string.Format("Invalid surrogate pair was found. Low: &#x{0:X}; High: &#x{0:X};", (object) (int) ch, (object) (int) high));
      if (this.check_character_validity && XmlChar.IsInvalid((int) ch))
        throw this.ArgumentError(string.Format("Invalid character &#x{0:X};", (object) (int) ch));
      this.ShiftStateContent("Character", true);
      int num = !surrogate ? (int) ch : ((int) high - 55296) * 1024 + (int) ch - 56320 + 65536;
      this.writer.Write("&#x");
      this.writer.Write(num.ToString("X", (IFormatProvider) CultureInfo.InvariantCulture));
      this.writer.Write(';');
    }

    public override void WriteEntityRef(string name)
    {
      if (name == null)
        throw this.ArgumentError(nameof (name));
      if (!XmlChar.IsName(name))
        throw this.ArgumentError("Argument name must be a valid XML name.");
      this.ShiftStateContent("Entity reference", true);
      this.writer.Write('&');
      this.writer.Write(name);
      this.writer.Write(';');
    }

    public override void WriteName(string name)
    {
      if (name == null)
        throw this.ArgumentError(nameof (name));
      if (!XmlChar.IsName(name))
        throw this.ArgumentError("Not a valid name string.");
      this.WriteString(name);
    }

    public override void WriteNmToken(string nmtoken)
    {
      if (nmtoken == null)
        throw this.ArgumentError(nameof (nmtoken));
      if (!XmlChar.IsNmToken(nmtoken))
        throw this.ArgumentError("Not a valid NMTOKEN string.");
      this.WriteString(nmtoken);
    }

    public override void WriteQualifiedName(string localName, string ns)
    {
      if (localName == null)
        throw this.ArgumentError(nameof (localName));
      if (ns == null)
        ns = string.Empty;
      if (ns == "http://www.w3.org/2000/xmlns/")
        throw this.ArgumentError("Prefix 'xmlns' is reserved and cannot be overriden.");
      if (!XmlChar.IsNCName(localName))
        throw this.ArgumentError("localName must be a valid NCName.");
      this.ShiftStateContent("QName", true);
      string str = ns.Length <= 0 ? string.Empty : this.LookupPrefix(ns);
      if (str == null)
      {
        if (this.state != WriteState.Attribute)
          throw this.ArgumentError(string.Format("Namespace '{0}' is not declared.", (object) ns));
        str = this.MockupPrefix(ns, false);
      }
      if (str != string.Empty)
      {
        this.writer.Write(str);
        this.writer.Write(":");
      }
      this.writer.Write(localName);
    }

    private void CheckChunkRange(Array buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (index < 0 || buffer.Length < index)
        throw this.ArgumentOutOfRangeError(nameof (index));
      if (count < 0 || buffer.Length < index + count)
        throw this.ArgumentOutOfRangeError(nameof (count));
    }

    public override void WriteBase64(byte[] buffer, int index, int count)
    {
      this.CheckChunkRange((Array) buffer, index, count);
      this.WriteString(Convert.ToBase64String(buffer, index, count));
    }

    public override void WriteBinHex(byte[] buffer, int index, int count)
    {
      this.CheckChunkRange((Array) buffer, index, count);
      this.ShiftStateContent("BinHex", true);
      XmlConvert.WriteBinHex(buffer, index, count, this.writer);
    }

    public override void WriteChars(char[] buffer, int index, int count)
    {
      this.CheckChunkRange((Array) buffer, index, count);
      this.ShiftStateContent("Chars", true);
      this.WriteEscapedBuffer(buffer, index, count, this.state == WriteState.Attribute);
    }

    public override void WriteRaw(char[] buffer, int index, int count)
    {
      this.CheckChunkRange((Array) buffer, index, count);
      this.ShiftStateContent("Raw text", false);
      this.writer.Write(buffer, index, count);
    }

    private void WriteIndent() => this.WriteIndentCore(0, false);

    private void WriteIndentEndElement() => this.WriteIndentCore(-1, false);

    private void WriteIndentAttribute()
    {
      if (this.WriteIndentCore(0, true))
        return;
      this.writer.Write(' ');
    }

    private bool WriteIndentCore(int nestFix, bool attribute)
    {
      if (!this.indent)
        return false;
      for (int index = this.open_count - 1; index >= 0; --index)
      {
        if (!attribute && this.elements[index].HasSimple)
          return false;
      }
      if (this.state != WriteState.Start)
        this.writer.Write(this.newline);
      for (int index = 0; index < this.open_count + nestFix; ++index)
        this.writer.Write(this.indent_string);
      return true;
    }

    private void OutputAutoStartDocument()
    {
      if (this.state != WriteState.Start)
        return;
      this.WriteStartDocumentCore(false, false);
    }

    private void ShiftStateTopLevel(
      string occured,
      bool allowAttribute,
      bool dontCheckXmlDecl,
      bool isCharacter)
    {
      switch (this.state)
      {
        case WriteState.Start:
          if (isCharacter)
            this.CheckMixedContentState();
          if (this.xmldecl_state == XmlTextWriter.XmlDeclState.Auto && !dontCheckXmlDecl)
            this.OutputAutoStartDocument();
          this.state = WriteState.Prolog;
          break;
        case WriteState.Element:
          if (isCharacter)
            this.CheckMixedContentState();
          this.CloseStartElement();
          break;
        case WriteState.Attribute:
          if (allowAttribute)
            break;
          goto case WriteState.Closed;
        case WriteState.Content:
          if (!isCharacter)
            break;
          this.CheckMixedContentState();
          break;
        case WriteState.Closed:
        case WriteState.Error:
          throw this.StateError(occured);
      }
    }

    private void CheckMixedContentState()
    {
      if (this.open_count <= 0)
        return;
      this.elements[this.open_count - 1].HasSimple = true;
    }

    private void ShiftStateContent(string occured, bool allowAttribute)
    {
      switch (this.state)
      {
        case WriteState.Start:
        case WriteState.Prolog:
          if (this.allow_doc_fragment && !this.is_document_entity)
          {
            if (this.xmldecl_state == XmlTextWriter.XmlDeclState.Auto)
              this.OutputAutoStartDocument();
            this.CheckMixedContentState();
            this.state = WriteState.Content;
            break;
          }
          goto case WriteState.Closed;
        case WriteState.Element:
          this.CloseStartElement();
          this.CheckMixedContentState();
          break;
        case WriteState.Attribute:
          if (allowAttribute)
            break;
          goto case WriteState.Closed;
        case WriteState.Content:
          this.CheckMixedContentState();
          break;
        case WriteState.Closed:
        case WriteState.Error:
          throw this.StateError(occured);
      }
    }

    private void WriteEscapedString(string text, bool isAttribute)
    {
      char[] anyOf = !isAttribute ? XmlTextWriter.escaped_text_chars : XmlTextWriter.escaped_attr_chars;
      int num = text.IndexOfAny(anyOf);
      if (num >= 0)
      {
        char[] charArray = text.ToCharArray();
        this.WriteCheckedBuffer(charArray, 0, num);
        this.WriteEscapedBuffer(charArray, num, charArray.Length - num, isAttribute);
      }
      else
        this.WriteCheckedString(text);
    }

    private void WriteCheckedString(string s)
    {
      int num = XmlChar.IndexOfInvalid(s, true);
      if (num >= 0)
      {
        char[] charArray = s.ToCharArray();
        this.writer.Write(charArray, 0, num);
        this.WriteCheckedBuffer(charArray, num, charArray.Length - num);
      }
      else
        this.writer.Write(s);
    }

    private void WriteCheckedBuffer(char[] text, int idx, int length)
    {
      int num1 = idx;
      int num2 = idx + length;
      for (; (idx = XmlChar.IndexOfInvalid(text, num1, length, true)) >= 0; num1 = idx + 1)
      {
        if (this.check_character_validity)
          throw this.ArgumentError(string.Format("Input contains invalid character at {0} : &#x{1:X};", (object) idx, (object) (int) text[idx]));
        if (num1 < idx)
          this.writer.Write(text, num1, idx - num1);
        this.writer.Write("&#x");
        this.writer.Write(((int) text[idx]).ToString("X", (IFormatProvider) CultureInfo.InvariantCulture));
        this.writer.Write(';');
        length -= idx - num1 + 1;
      }
      if (num1 >= num2)
        return;
      this.writer.Write(text, num1, num2 - num1);
    }

    private void WriteEscapedBuffer(char[] text, int index, int length, bool isAttribute)
    {
      int idx = index;
      int num = index + length;
      for (int index1 = idx; index1 < num; ++index1)
      {
        char ch1 = text[index1];
        switch (ch1)
        {
          case '"':
          case '\'':
            if (!isAttribute || (int) text[index1] != (int) this.quote_char)
              continue;
            goto case '&';
          case '&':
label_4:
            if (idx < index1)
              this.WriteCheckedBuffer(text, idx, index1 - idx);
            this.writer.Write('&');
            char ch2 = text[index1];
            switch (ch2)
            {
              case '"':
                this.writer.Write("quot;");
                break;
              case '&':
                this.writer.Write("amp;");
                break;
              case '\'':
                this.writer.Write("apos;");
                break;
              default:
                switch (ch2)
                {
                  case '<':
                    this.writer.Write("lt;");
                    break;
                  case '>':
                    this.writer.Write("gt;");
                    break;
                }
                break;
            }
            break;
          default:
            switch (ch1)
            {
              case '\n':
                if (idx < index1)
                  this.WriteCheckedBuffer(text, idx, index1 - idx);
                if (isAttribute)
                {
                  this.writer.Write(text[index1] != '\r' ? "&#xA;" : "&#xD;");
                  break;
                }
                switch (this.newline_handling)
                {
                  case NewLineHandling.Replace:
                    this.writer.Write(this.newline);
                    break;
                  case NewLineHandling.Entitize:
                    this.writer.Write(text[index1] != '\r' ? "&#xA;" : "&#xD;");
                    break;
                  default:
                    this.writer.Write(text[index1]);
                    break;
                }
                break;
              case '\r':
                if (index1 + 1 < num && text[index1] == '\n')
                {
                  ++index1;
                  goto case '\n';
                }
                else
                  goto case '\n';
              default:
                switch (ch1)
                {
                  case '<':
                  case '>':
                    goto label_4;
                  default:
                    continue;
                }
            }
            break;
        }
        idx = index1 + 1;
      }
      if (idx >= num)
        return;
      this.WriteCheckedBuffer(text, idx, num - idx);
    }

    private Exception ArgumentOutOfRangeError(string name)
    {
      this.state = WriteState.Error;
      return (Exception) new ArgumentOutOfRangeException(name);
    }

    private Exception ArgumentError(string msg)
    {
      this.state = WriteState.Error;
      return (Exception) new ArgumentException(msg);
    }

    private Exception InvalidOperation(string msg)
    {
      this.state = WriteState.Error;
      return (Exception) new InvalidOperationException(msg);
    }

    private Exception StateError(string occured) => this.InvalidOperation(string.Format("This XmlWriter does not accept {0} at this state {1}.", (object) occured, (object) this.state));

    private class XmlNodeInfo
    {
      public string Prefix;
      public string LocalName;
      public string NS;
      public bool HasSimple;
      public bool HasElements;
      public string XmlLang;
      public XmlSpace XmlSpace;
    }

    internal class StringUtil
    {
      private static CultureInfo cul = CultureInfo.InvariantCulture;
      private static CompareInfo cmp = CultureInfo.InvariantCulture.CompareInfo;

      public static int IndexOf(string src, string target) => XmlTextWriter.StringUtil.cmp.IndexOf(src, target);

      public static int Compare(string s1, string s2) => XmlTextWriter.StringUtil.cmp.Compare(s1, s2);

      public static string Format(string format, params object[] args) => string.Format((IFormatProvider) XmlTextWriter.StringUtil.cul, format, args);
    }

    private enum XmlDeclState
    {
      Allow,
      Ignore,
      Auto,
      Prohibit,
    }
  }
}
