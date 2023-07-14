// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlDocumentType
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml;
using System.IO;

namespace System.Xml
{
  public class XmlDocumentType : XmlLinkedNode
  {
    internal XmlNamedNodeMap entities;
    internal XmlNamedNodeMap notations;
    private DTDObjectModel dtd;

    protected internal XmlDocumentType(
      string name,
      string publicId,
      string systemId,
      string internalSubset,
      XmlDocument doc)
      : base(doc)
    {
      Mono.Xml2.XmlTextReader xmlTextReader = new Mono.Xml2.XmlTextReader(this.BaseURI, (TextReader) new StringReader(string.Empty), doc.NameTable);
      xmlTextReader.XmlResolver = doc.Resolver;
      xmlTextReader.GenerateDTDObjectModel(name, publicId, systemId, internalSubset);
      this.dtd = xmlTextReader.DTD;
      this.ImportFromDTD();
    }

    internal XmlDocumentType(DTDObjectModel dtd, XmlDocument doc)
      : base(doc)
    {
      this.dtd = dtd;
      this.ImportFromDTD();
    }

    private void ImportFromDTD()
    {
      this.entities = new XmlNamedNodeMap((XmlNode) this);
      this.notations = new XmlNamedNodeMap((XmlNode) this);
      foreach (DTDEntityDeclaration entityDeclaration in this.DTD.EntityDecls.Values)
        this.entities.SetNamedItem((XmlNode) new XmlEntity(entityDeclaration.Name, entityDeclaration.NotationName, entityDeclaration.PublicId, entityDeclaration.SystemId, this.OwnerDocument));
      foreach (DTDNotationDeclaration notationDeclaration in this.DTD.NotationDecls.Values)
        this.notations.SetNamedItem((XmlNode) new XmlNotation(notationDeclaration.LocalName, notationDeclaration.Prefix, notationDeclaration.PublicId, notationDeclaration.SystemId, this.OwnerDocument));
    }

    internal DTDObjectModel DTD => this.dtd;

    public XmlNamedNodeMap Entities => this.entities;

    public string InternalSubset => this.dtd.InternalSubset;

    public override bool IsReadOnly => true;

    public override string LocalName => this.dtd.Name;

    public override string Name => this.dtd.Name;

    public override XmlNodeType NodeType => XmlNodeType.DocumentType;

    public XmlNamedNodeMap Notations => this.notations;

    public string PublicId => this.dtd.PublicId;

    public string SystemId => this.dtd.SystemId;

    public override XmlNode CloneNode(bool deep) => (XmlNode) new XmlDocumentType(this.dtd, this.OwnerDocument);

    public override void WriteContentTo(XmlWriter w)
    {
    }

    public override void WriteTo(XmlWriter w) => w.WriteDocType(this.Name, this.PublicId, this.SystemId, this.InternalSubset);
  }
}
