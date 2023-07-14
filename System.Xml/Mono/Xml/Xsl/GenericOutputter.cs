// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.GenericOutputter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace Mono.Xml.Xsl
{
  internal class GenericOutputter : Outputter
  {
    private Hashtable _outputs;
    private XslOutput _currentOutput;
    private Emitter _emitter;
    private TextWriter pendingTextWriter;
    private StringBuilder pendingFirstSpaces;
    private WriteState _state;
    private Attribute[] pendingAttributes = new Attribute[10];
    private int pendingAttributesPos;
    private XmlNamespaceManager _nsManager;
    private ListDictionary _currentNamespaceDecls;
    private ArrayList newNamespaces = new ArrayList();
    private NameTable _nt;
    private Encoding _encoding;
    private bool _canProcessAttributes;
    private bool _insideCData;
    private bool _omitXmlDeclaration;
    private int _xpCount;

    private GenericOutputter(Hashtable outputs, Encoding encoding)
    {
      this._encoding = encoding;
      this._outputs = outputs;
      this._currentOutput = (XslOutput) outputs[(object) string.Empty];
      this._state = WriteState.Prolog;
      this._nt = new NameTable();
      this._nsManager = new XmlNamespaceManager((XmlNameTable) this._nt);
      this._currentNamespaceDecls = new ListDictionary();
      this._omitXmlDeclaration = false;
    }

    public GenericOutputter(XmlWriter writer, Hashtable outputs, Encoding encoding)
      : this(writer, outputs, encoding, false)
    {
    }

    internal GenericOutputter(
      XmlWriter writer,
      Hashtable outputs,
      Encoding encoding,
      bool isVariable)
      : this(outputs, encoding)
    {
      this._emitter = (Emitter) new XmlWriterEmitter(writer);
      this._state = writer.WriteState;
      this._omitXmlDeclaration = true;
    }

    public GenericOutputter(TextWriter writer, Hashtable outputs, Encoding encoding)
      : this(outputs, encoding)
    {
      this.pendingTextWriter = writer;
    }

    internal GenericOutputter(TextWriter writer, Hashtable outputs)
      : this(writer, outputs, (Encoding) null)
    {
    }

    internal GenericOutputter(XmlWriter writer, Hashtable outputs)
      : this(writer, outputs, (Encoding) null)
    {
    }

    private Emitter Emitter
    {
      get
      {
        if (this._emitter == null)
          this.DetermineOutputMethod((string) null, (string) null);
        return this._emitter;
      }
    }

    private void DetermineOutputMethod(string localName, string ns)
    {
      XslOutput output = (XslOutput) this._outputs[(object) string.Empty];
      switch (output.Method)
      {
        case OutputMethod.XML:
          XmlTextWriter writer = new XmlTextWriter(this.pendingTextWriter);
          if (output.Indent == "yes")
            writer.Formatting = Formatting.Indented;
          this._emitter = (Emitter) new XmlWriterEmitter((XmlWriter) writer);
          if (!this._omitXmlDeclaration && !output.OmitXmlDeclaration)
          {
            this._emitter.WriteStartDocument(this._encoding == null ? output.Encoding : this._encoding, output.Standalone);
            break;
          }
          break;
        case OutputMethod.HTML:
          this._emitter = (Emitter) new HtmlEmitter(this.pendingTextWriter, output);
          break;
        case OutputMethod.Text:
          this._emitter = (Emitter) new TextEmitter(this.pendingTextWriter);
          break;
        default:
          if (localName == null || string.Compare(localName, "html", true, CultureInfo.InvariantCulture) != 0 || !(ns == string.Empty))
            goto case OutputMethod.XML;
          else
            goto case OutputMethod.HTML;
      }
      this.pendingTextWriter = (TextWriter) null;
    }

    private void CheckState()
    {
      if (this._state == WriteState.Element)
      {
        this._nsManager.PushScope();
        foreach (string key in (IEnumerable) this._currentNamespaceDecls.Keys)
        {
          string currentNamespaceDecl = this._currentNamespaceDecls[(object) key] as string;
          if (!(this._nsManager.LookupNamespace(key, false) == currentNamespaceDecl))
          {
            this.newNamespaces.Add((object) key);
            this._nsManager.AddNamespace(key, currentNamespaceDecl);
          }
        }
        for (int index = 0; index < this.pendingAttributesPos; ++index)
        {
          Attribute pendingAttribute = this.pendingAttributes[index];
          string str1 = pendingAttribute.Prefix;
          if (str1 == "xml" && pendingAttribute.Namespace != "http://www.w3.org/XML/1998/namespace")
            str1 = string.Empty;
          string str2 = this._nsManager.LookupPrefix(pendingAttribute.Namespace, false);
          if (str1.Length == 0 && pendingAttribute.Namespace.Length > 0)
            str1 = str2;
          if (pendingAttribute.Namespace.Length > 0 && (str1 == null || str1 == string.Empty))
          {
            str1 = "xp_" + (object) this._xpCount++;
            while (this._nsManager.LookupNamespace(str1) != null)
              str1 = "xp_" + (object) this._xpCount++;
            this.newNamespaces.Add((object) str1);
            this._currentNamespaceDecls.Add((object) str1, (object) pendingAttribute.Namespace);
            this._nsManager.AddNamespace(str1, pendingAttribute.Namespace);
          }
          this.Emitter.WriteAttributeString(str1, pendingAttribute.LocalName, pendingAttribute.Namespace, pendingAttribute.Value);
        }
        for (int index = 0; index < this.newNamespaces.Count; ++index)
        {
          string newNamespace = (string) this.newNamespaces[index];
          string currentNamespaceDecl = this._currentNamespaceDecls[(object) newNamespace] as string;
          if (newNamespace != string.Empty)
            this.Emitter.WriteAttributeString("xmlns", newNamespace, "http://www.w3.org/2000/xmlns/", currentNamespaceDecl);
          else
            this.Emitter.WriteAttributeString(string.Empty, "xmlns", "http://www.w3.org/2000/xmlns/", currentNamespaceDecl);
        }
        this._currentNamespaceDecls.Clear();
        this._state = WriteState.Content;
        this.newNamespaces.Clear();
      }
      this._canProcessAttributes = false;
    }

    public override void WriteStartElement(string prefix, string localName, string nsURI)
    {
      if (this._emitter == null)
      {
        this.DetermineOutputMethod(localName, nsURI);
        if (this.pendingFirstSpaces != null)
        {
          this.WriteWhitespace(this.pendingFirstSpaces.ToString());
          this.pendingFirstSpaces = (StringBuilder) null;
        }
      }
      if (this._state == WriteState.Prolog && (this._currentOutput.DoctypePublic != null || this._currentOutput.DoctypeSystem != null))
        this.Emitter.WriteDocType(prefix + (prefix != null ? string.Empty : ":") + localName, this._currentOutput.DoctypePublic, this._currentOutput.DoctypeSystem);
      this.CheckState();
      if (nsURI == string.Empty)
        prefix = string.Empty;
      this.Emitter.WriteStartElement(prefix, localName, nsURI);
      this._state = WriteState.Element;
      if (this._nsManager.LookupNamespace(prefix, false) != nsURI)
        this._currentNamespaceDecls[(object) prefix] = (object) nsURI;
      this.pendingAttributesPos = 0;
      this._canProcessAttributes = true;
    }

    public override void WriteEndElement() => this.WriteEndElementInternal(false);

    public override void WriteFullEndElement() => this.WriteEndElementInternal(true);

    private void WriteEndElementInternal(bool fullEndElement)
    {
      this.CheckState();
      if (fullEndElement)
        this.Emitter.WriteFullEndElement();
      else
        this.Emitter.WriteEndElement();
      this._state = WriteState.Content;
      this._nsManager.PopScope();
    }

    public override void WriteAttributeString(
      string prefix,
      string localName,
      string nsURI,
      string value)
    {
      for (int index = 0; index < this.pendingAttributesPos; ++index)
      {
        Attribute pendingAttribute = this.pendingAttributes[index];
        if (pendingAttribute.LocalName == localName && pendingAttribute.Namespace == nsURI)
        {
          this.pendingAttributes[index].Value = value;
          this.pendingAttributes[index].Prefix = prefix;
          return;
        }
      }
      if (this.pendingAttributesPos == this.pendingAttributes.Length)
      {
        Attribute[] pendingAttributes = this.pendingAttributes;
        this.pendingAttributes = new Attribute[this.pendingAttributesPos * 2 + 1];
        if (this.pendingAttributesPos > 0)
          Array.Copy((Array) pendingAttributes, 0, (Array) this.pendingAttributes, 0, this.pendingAttributesPos);
      }
      this.pendingAttributes[this.pendingAttributesPos].Prefix = prefix;
      this.pendingAttributes[this.pendingAttributesPos].Namespace = nsURI;
      this.pendingAttributes[this.pendingAttributesPos].LocalName = localName;
      this.pendingAttributes[this.pendingAttributesPos].Value = value;
      ++this.pendingAttributesPos;
    }

    public override void WriteNamespaceDecl(string prefix, string nsUri)
    {
      if (this._nsManager.LookupNamespace(prefix, false) == nsUri)
        return;
      for (int index = 0; index < this.pendingAttributesPos; ++index)
      {
        Attribute pendingAttribute = this.pendingAttributes[index];
        if (pendingAttribute.Prefix == prefix || pendingAttribute.Namespace == nsUri)
          return;
      }
      if (!(this._currentNamespaceDecls[(object) prefix] as string != nsUri))
        return;
      this._currentNamespaceDecls[(object) prefix] = (object) nsUri;
    }

    public override void WriteComment(string text)
    {
      this.CheckState();
      this.Emitter.WriteComment(text);
    }

    public override void WriteProcessingInstruction(string name, string text)
    {
      this.CheckState();
      this.Emitter.WriteProcessingInstruction(name, text);
    }

    public override void WriteString(string text)
    {
      this.CheckState();
      if (this._insideCData)
        this.Emitter.WriteCDataSection(text);
      else if (this._state != WriteState.Content && text.Length > 0 && XmlChar.IsWhitespace(text))
        this.Emitter.WriteWhitespace(text);
      else
        this.Emitter.WriteString(text);
    }

    public override void WriteRaw(string data)
    {
      this.CheckState();
      this.Emitter.WriteRaw(data);
    }

    public override void WriteWhitespace(string text)
    {
      if (this._emitter == null)
      {
        if (this.pendingFirstSpaces == null)
          this.pendingFirstSpaces = new StringBuilder();
        this.pendingFirstSpaces.Append(text);
        if (this._state != WriteState.Start)
          return;
        this._state = WriteState.Prolog;
      }
      else
      {
        this.CheckState();
        this.Emitter.WriteWhitespace(text);
      }
    }

    public override void Done()
    {
      this.Emitter.Done();
      this._state = WriteState.Closed;
    }

    public override bool CanProcessAttributes => this._canProcessAttributes;

    public override bool InsideCDataSection
    {
      get => this._insideCData;
      set => this._insideCData = value;
    }
  }
}
