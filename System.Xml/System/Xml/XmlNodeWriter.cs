// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlNodeWriter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections.Generic;

namespace System.Xml
{
  internal class XmlNodeWriter : XmlWriter
  {
    private XmlDocument doc;
    private bool isClosed;
    private XmlNode current;
    private XmlAttribute attribute;
    private bool isDocumentEntity;
    private XmlDocumentFragment fragment;
    private XmlNodeType state;

    public XmlNodeWriter()
      : this(true)
    {
    }

    public XmlNodeWriter(bool isDocumentEntity)
    {
      this.doc = new XmlDocument();
      this.state = XmlNodeType.None;
      this.isDocumentEntity = isDocumentEntity;
      if (isDocumentEntity)
        return;
      this.current = (XmlNode) (this.fragment = this.doc.CreateDocumentFragment());
    }

    public XmlNode Document => this.isDocumentEntity ? (XmlNode) this.doc : (XmlNode) this.fragment;

    public override WriteState WriteState
    {
      get
      {
        if (this.isClosed)
          return WriteState.Closed;
        if (this.attribute != null)
          return WriteState.Attribute;
        switch (this.state)
        {
          case XmlNodeType.None:
            return WriteState.Start;
          case XmlNodeType.DocumentType:
            return WriteState.Element;
          case XmlNodeType.XmlDeclaration:
            return WriteState.Prolog;
          default:
            return WriteState.Content;
        }
      }
    }

    public override string XmlLang
    {
      get
      {
        for (XmlElement xmlElement = this.current as XmlElement; xmlElement != null; xmlElement = xmlElement.ParentNode as XmlElement)
        {
          if (xmlElement.HasAttribute("xml:lang"))
            return xmlElement.GetAttribute("xml:lang");
        }
        return string.Empty;
      }
    }

    public override XmlSpace XmlSpace
    {
      get
      {
        for (XmlElement xmlElement = this.current as XmlElement; xmlElement != null; xmlElement = xmlElement.ParentNode as XmlElement)
        {
          string attribute = xmlElement.GetAttribute("xml:space");
          string key = attribute;
          if (key != null)
          {
            if (XmlNodeWriter.\u003C\u003Ef__switch\u0024map29 == null)
              XmlNodeWriter.\u003C\u003Ef__switch\u0024map29 = new Dictionary<string, int>(3)
              {
                {
                  "preserve",
                  0
                },
                {
                  "default",
                  1
                },
                {
                  string.Empty,
                  2
                }
              };
            int num;
            if (XmlNodeWriter.\u003C\u003Ef__switch\u0024map29.TryGetValue(key, out num))
            {
              switch (num)
              {
                case 0:
                  return XmlSpace.Preserve;
                case 1:
                  return XmlSpace.Default;
                case 2:
                  continue;
              }
            }
          }
          throw new InvalidOperationException(string.Format("Invalid xml:space {0}.", (object) attribute));
        }
        return XmlSpace.None;
      }
    }

    private void CheckState()
    {
      if (this.isClosed)
        throw new InvalidOperationException();
    }

    private void WritePossiblyTopLevelNode(XmlNode n, bool possiblyAttribute)
    {
      this.CheckState();
      if (!possiblyAttribute && this.attribute != null)
        throw new InvalidOperationException(string.Format("Current state is not acceptable for {0}.", (object) n.NodeType));
      if (this.state != XmlNodeType.Element)
        this.Document.AppendChild(n);
      else if (this.attribute != null)
        this.attribute.AppendChild(n);
      else
        this.current.AppendChild(n);
      if (this.state != XmlNodeType.None)
        return;
      this.state = XmlNodeType.XmlDeclaration;
    }

    public override void Close()
    {
      this.CheckState();
      this.isClosed = true;
    }

    public override void Flush()
    {
    }

    public override string LookupPrefix(string ns)
    {
      this.CheckState();
      return this.current != null ? this.current.GetPrefixOfNamespace(ns) : throw new InvalidOperationException();
    }

    public override void WriteStartDocument() => this.WriteStartDocument((string) null);

    public override void WriteStartDocument(bool standalone) => this.WriteStartDocument(!standalone ? "no" : "yes");

    private void WriteStartDocument(string sddecl)
    {
      this.CheckState();
      if (this.state != XmlNodeType.None)
        throw new InvalidOperationException("Current state is not acceptable for xmldecl.");
      this.doc.AppendChild((XmlNode) this.doc.CreateXmlDeclaration("1.0", (string) null, sddecl));
      this.state = XmlNodeType.XmlDeclaration;
    }

    public override void WriteEndDocument()
    {
      this.CheckState();
      this.isClosed = true;
    }

    public override void WriteDocType(
      string name,
      string publicId,
      string systemId,
      string internalSubset)
    {
      this.CheckState();
      switch (this.state)
      {
        case XmlNodeType.None:
        case XmlNodeType.XmlDeclaration:
          this.doc.AppendChild((XmlNode) this.doc.CreateDocumentType(name, publicId, systemId, internalSubset));
          this.state = XmlNodeType.DocumentType;
          break;
        default:
          throw new InvalidOperationException("Current state is not acceptable for doctype.");
      }
    }

    public override void WriteStartElement(string prefix, string name, string ns)
    {
      this.CheckState();
      if (this.isDocumentEntity && this.state == XmlNodeType.EndElement && this.doc.DocumentElement != null)
        throw new InvalidOperationException("Current state is not acceptable for startElement.");
      XmlElement element = this.doc.CreateElement(prefix, name, ns);
      if (this.current == null)
      {
        this.Document.AppendChild((XmlNode) element);
        this.state = XmlNodeType.Element;
      }
      else
      {
        this.current.AppendChild((XmlNode) element);
        this.state = XmlNodeType.Element;
      }
      this.current = (XmlNode) element;
    }

    public override void WriteEndElement() => this.WriteEndElementInternal(false);

    public override void WriteFullEndElement() => this.WriteEndElementInternal(true);

    private void WriteEndElementInternal(bool forceFull)
    {
      this.CheckState();
      if (this.current == null)
        throw new InvalidOperationException("Current state is not acceptable for endElement.");
      if (!forceFull && this.current.FirstChild == null)
        ((XmlElement) this.current).IsEmpty = true;
      if (this.isDocumentEntity && this.current.ParentNode == this.doc)
        this.state = XmlNodeType.EndElement;
      else
        this.current = this.current.ParentNode;
    }

    public override void WriteStartAttribute(string prefix, string name, string ns)
    {
      this.CheckState();
      if (this.attribute != null)
        throw new InvalidOperationException("There is an open attribute.");
      if (!(this.current is XmlElement))
        throw new InvalidOperationException("Current state is not acceptable for startAttribute.");
      this.attribute = this.doc.CreateAttribute(prefix, name, ns);
      ((XmlElement) this.current).SetAttributeNode(this.attribute);
    }

    public override void WriteEndAttribute()
    {
      this.CheckState();
      this.attribute = this.attribute != null ? (XmlAttribute) null : throw new InvalidOperationException("Current state is not acceptable for startAttribute.");
    }

    public override void WriteCData(string data)
    {
      this.CheckState();
      if (this.current == null)
        throw new InvalidOperationException("Current state is not acceptable for CDATAsection.");
      this.current.AppendChild((XmlNode) this.doc.CreateCDataSection(data));
    }

    public override void WriteComment(string comment) => this.WritePossiblyTopLevelNode((XmlNode) this.doc.CreateComment(comment), false);

    public override void WriteProcessingInstruction(string name, string value) => this.WritePossiblyTopLevelNode((XmlNode) this.doc.CreateProcessingInstruction(name, value), false);

    public override void WriteEntityRef(string name) => this.WritePossiblyTopLevelNode((XmlNode) this.doc.CreateEntityReference(name), true);

    public override void WriteCharEntity(char c) => this.WritePossiblyTopLevelNode((XmlNode) this.doc.CreateTextNode(new string(new char[1]
    {
      c
    }, 0, 1)), true);

    public override void WriteWhitespace(string ws) => this.WritePossiblyTopLevelNode((XmlNode) this.doc.CreateWhitespace(ws), true);

    public override void WriteString(string data)
    {
      this.CheckState();
      if (this.current == null)
        throw new InvalidOperationException("Current state is not acceptable for Text.");
      if (this.attribute != null)
        this.attribute.AppendChild((XmlNode) this.doc.CreateTextNode(data));
      else if (!(this.current.LastChild is XmlText lastChild))
        this.current.AppendChild((XmlNode) this.doc.CreateTextNode(data));
      else
        lastChild.AppendData(data);
    }

    public override void WriteName(string name) => this.WriteString(name);

    public override void WriteNmToken(string nmtoken) => this.WriteString(nmtoken);

    public override void WriteQualifiedName(string name, string ns)
    {
      string str = this.LookupPrefix(ns);
      if (str == null)
        throw new ArgumentException(string.Format("Invalid namespace {0}", (object) ns));
      if (str != string.Empty)
        this.WriteString(name);
      else
        this.WriteString(str + ":" + name);
    }

    public override void WriteChars(char[] chars, int start, int len) => this.WriteString(new string(chars, start, len));

    public override void WriteRaw(string data) => this.WriteString(data);

    public override void WriteRaw(char[] chars, int start, int len) => this.WriteChars(chars, start, len);

    public override void WriteBase64(byte[] data, int start, int len) => this.WriteString(Convert.ToBase64String(data, start, len));

    public override void WriteBinHex(byte[] data, int start, int len) => throw new NotImplementedException();

    public override void WriteSurrogateCharEntity(char c1, char c2) => throw new NotImplementedException();
  }
}
