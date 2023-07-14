// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlEntity
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml;
using System.Text;

namespace System.Xml
{
  public class XmlEntity : XmlNode, IHasXmlChildNode
  {
    private string name;
    private string NDATA;
    private string publicId;
    private string systemId;
    private string baseUri;
    private XmlLinkedNode lastLinkedChild;
    private bool contentAlreadySet;

    internal XmlEntity(
      string name,
      string NDATA,
      string publicId,
      string systemId,
      XmlDocument doc)
      : base(doc)
    {
      this.name = doc.NameTable.Add(name);
      this.NDATA = NDATA;
      this.publicId = publicId;
      this.systemId = systemId;
      this.baseUri = doc.BaseURI;
    }

    XmlLinkedNode IHasXmlChildNode.LastLinkedChild
    {
      get
      {
        if (this.lastLinkedChild != null || this.contentAlreadySet)
          return this.lastLinkedChild;
        this.contentAlreadySet = true;
        this.SetEntityContent();
        return this.lastLinkedChild;
      }
      set => this.lastLinkedChild = value;
    }

    public override string BaseURI => this.baseUri;

    public override string InnerText
    {
      get => base.InnerText;
      set => throw new InvalidOperationException("This operation is not supported.");
    }

    public override string InnerXml
    {
      get => base.InnerXml;
      set => throw new InvalidOperationException("This operation is not supported.");
    }

    public override bool IsReadOnly => true;

    public override string LocalName => this.name;

    public override string Name => this.name;

    public override XmlNodeType NodeType => XmlNodeType.Entity;

    public string NotationName => this.NDATA == null ? (string) null : this.NDATA;

    public override string OuterXml => string.Empty;

    public string PublicId => this.publicId;

    public string SystemId => this.systemId;

    public override XmlNode CloneNode(bool deep) => throw new InvalidOperationException("This operation is not supported.");

    public override void WriteContentTo(XmlWriter w)
    {
    }

    public override void WriteTo(XmlWriter w)
    {
    }

    private void SetEntityContent()
    {
      if (this.lastLinkedChild != null)
        return;
      XmlDocumentType documentType = this.OwnerDocument.DocumentType;
      if (documentType == null)
        return;
      DTDEntityDeclaration entityDecl = documentType.DTD.EntityDecls[this.name];
      if (entityDecl == null)
        return;
      XmlParserContext context = new XmlParserContext(this.OwnerDocument.NameTable, this.ConstructNamespaceManager(), documentType == null ? (DTDObjectModel) null : documentType.DTD, this.BaseURI, this.XmlLang, this.XmlSpace, (Encoding) null);
      XmlTextReader reader = new XmlTextReader(entityDecl.EntityValue, XmlNodeType.Element, context);
      reader.XmlResolver = this.OwnerDocument.Resolver;
      while (true)
      {
        XmlNode newChild = this.OwnerDocument.ReadNode((XmlReader) reader);
        if (newChild != null)
          this.InsertBefore(newChild, (XmlNode) null, false, false);
        else
          break;
      }
    }
  }
}
