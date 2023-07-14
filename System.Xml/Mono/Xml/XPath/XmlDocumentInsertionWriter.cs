// Decompiled with JetBrains decompiler
// Type: Mono.Xml.XPath.XmlDocumentInsertionWriter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml;

namespace Mono.Xml.XPath
{
  internal class XmlDocumentInsertionWriter : XmlWriter
  {
    private XmlNode parent;
    private XmlNode current;
    private XmlNode nextSibling;
    private WriteState state;
    private XmlAttribute attribute;

    public XmlDocumentInsertionWriter(XmlNode owner, XmlNode nextSibling)
    {
      this.parent = owner;
      XmlNodeType xmlNodeType = this.parent != null ? this.parent.NodeType : throw new InvalidOperationException();
      switch (xmlNodeType)
      {
        case XmlNodeType.Document:
          this.current = (XmlNode) ((XmlDocument) this.parent).CreateDocumentFragment();
          break;
        case XmlNodeType.DocumentFragment:
          this.current = (XmlNode) this.parent.OwnerDocument.CreateDocumentFragment();
          break;
        default:
          if (xmlNodeType != XmlNodeType.Element)
            throw new InvalidOperationException(string.Format("Insertion into {0} node is not allowed.", (object) this.parent.NodeType));
          goto case XmlNodeType.DocumentFragment;
      }
      this.nextSibling = nextSibling;
      this.state = WriteState.Content;
    }

    internal event XmlWriterClosedEventHandler Closed;

    public override WriteState WriteState => this.state;

    public override void Close()
    {
      while (this.current.ParentNode != null)
        this.current = this.current.ParentNode;
      this.parent.InsertBefore(this.current, this.nextSibling);
      if (this.Closed == null)
        return;
      this.Closed((XmlWriter) this);
    }

    public override void Flush()
    {
    }

    public override string LookupPrefix(string ns) => this.current.GetPrefixOfNamespace(ns);

    public override void WriteStartAttribute(string prefix, string name, string ns)
    {
      if (this.state != WriteState.Content)
        throw new InvalidOperationException("Current state is not inside element. Cannot start attribute.");
      if (prefix == null && ns != null && ns.Length > 0)
        prefix = this.LookupPrefix(ns);
      this.attribute = this.current.OwnerDocument.CreateAttribute(prefix, name, ns);
      this.state = WriteState.Attribute;
    }

    public override void WriteProcessingInstruction(string name, string value) => this.current.AppendChild((XmlNode) this.current.OwnerDocument.CreateProcessingInstruction(name, value));

    public override void WriteComment(string text) => this.current.AppendChild((XmlNode) this.current.OwnerDocument.CreateComment(text));

    public override void WriteCData(string text) => this.current.AppendChild((XmlNode) this.current.OwnerDocument.CreateCDataSection(text));

    public override void WriteStartElement(string prefix, string name, string ns)
    {
      if (prefix == null && ns != null && ns.Length > 0)
        prefix = this.LookupPrefix(ns);
      XmlElement element = this.current.OwnerDocument.CreateElement(prefix, name, ns);
      this.current.AppendChild((XmlNode) element);
      this.current = (XmlNode) element;
    }

    public override void WriteEndElement()
    {
      this.current = this.current.ParentNode;
      if (this.current == null)
        throw new InvalidOperationException("No element is opened.");
    }

    public override void WriteFullEndElement()
    {
      if (this.current is XmlElement current)
        current.IsEmpty = false;
      this.WriteEndElement();
    }

    public override void WriteDocType(
      string name,
      string pubid,
      string systemId,
      string intsubset)
    {
      throw new NotSupportedException();
    }

    public override void WriteStartDocument() => throw new NotSupportedException();

    public override void WriteStartDocument(bool standalone) => throw new NotSupportedException();

    public override void WriteEndDocument() => throw new NotSupportedException();

    public override void WriteBase64(byte[] data, int start, int length) => this.WriteString(Convert.ToBase64String(data, start, length));

    public override void WriteRaw(char[] raw, int start, int length) => throw new NotSupportedException();

    public override void WriteRaw(string raw) => throw new NotSupportedException();

    public override void WriteSurrogateCharEntity(char msb, char lsb) => throw new NotSupportedException();

    public override void WriteCharEntity(char c) => throw new NotSupportedException();

    public override void WriteEntityRef(string entname)
    {
      if (this.state != WriteState.Attribute)
        throw new InvalidOperationException("Current state is not inside attribute. Cannot write attribute value.");
      this.attribute.AppendChild((XmlNode) this.attribute.OwnerDocument.CreateEntityReference(entname));
    }

    public override void WriteChars(char[] data, int start, int length) => this.WriteString(new string(data, start, length));

    public override void WriteString(string text)
    {
      if (this.attribute != null)
        this.attribute.Value += text;
      else
        this.current.AppendChild((XmlNode) this.current.OwnerDocument.CreateTextNode(text));
    }

    public override void WriteWhitespace(string text)
    {
      if (this.state != WriteState.Attribute)
        this.current.AppendChild((XmlNode) this.current.OwnerDocument.CreateTextNode(text));
      else if (this.attribute.ChildNodes.Count == 0)
        this.attribute.AppendChild((XmlNode) this.attribute.OwnerDocument.CreateWhitespace(text));
      else
        this.attribute.Value += text;
    }

    public override void WriteEndAttribute()
    {
      if (!(this.current is XmlElement xmlElement1))
        xmlElement1 = this.nextSibling != null ? (XmlElement) null : this.parent as XmlElement;
      XmlElement xmlElement2 = xmlElement1;
      if (this.state != WriteState.Attribute || xmlElement2 == null)
        throw new InvalidOperationException("Current state is not inside attribute. Cannot close attribute.");
      xmlElement2.SetAttributeNode(this.attribute);
      this.attribute = (XmlAttribute) null;
      this.state = WriteState.Content;
    }
  }
}
