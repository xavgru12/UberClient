// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlEntityReference
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.XPath;

namespace System.Xml
{
  public class XmlEntityReference : XmlLinkedNode, IHasXmlChildNode
  {
    private string entityName;
    private XmlLinkedNode lastLinkedChild;

    protected internal XmlEntityReference(string name, XmlDocument doc)
      : base(doc)
    {
      XmlConvert.VerifyName(name);
      this.entityName = doc.NameTable.Add(name);
    }

    XmlLinkedNode IHasXmlChildNode.LastLinkedChild
    {
      get => this.lastLinkedChild;
      set => this.lastLinkedChild = value;
    }

    public override string BaseURI => base.BaseURI;

    private XmlEntity Entity
    {
      get
      {
        XmlDocumentType documentType = this.OwnerDocument.DocumentType;
        if (documentType == null)
          return (XmlEntity) null;
        return documentType.Entities == null ? (XmlEntity) null : documentType.Entities.GetNamedItem(this.Name) as XmlEntity;
      }
    }

    internal override string ChildrenBaseURI
    {
      get
      {
        XmlEntity entity = this.Entity;
        if (entity == null)
          return string.Empty;
        if (entity.SystemId == null || entity.SystemId.Length == 0)
          return entity.BaseURI;
        if (entity.BaseURI == null || entity.BaseURI.Length == 0)
          return entity.SystemId;
        Uri baseUri = (Uri) null;
        try
        {
          baseUri = new Uri(entity.BaseURI);
        }
        catch (UriFormatException ex)
        {
        }
        XmlResolver resolver = this.OwnerDocument.Resolver;
        return resolver != null ? resolver.ResolveUri(baseUri, entity.SystemId).ToString() : new Uri(baseUri, entity.SystemId).ToString();
      }
    }

    public override bool IsReadOnly => true;

    public override string LocalName => this.entityName;

    public override string Name => this.entityName;

    public override XmlNodeType NodeType => XmlNodeType.EntityReference;

    public override string Value
    {
      get => (string) null;
      set => throw new XmlException("entity reference cannot be set value.");
    }

    internal override XPathNodeType XPathNodeType => XPathNodeType.Text;

    public override XmlNode CloneNode(bool deep) => (XmlNode) new XmlEntityReference(this.Name, this.OwnerDocument);

    public override void WriteContentTo(XmlWriter w)
    {
      for (int i = 0; i < this.ChildNodes.Count; ++i)
        this.ChildNodes[i].WriteTo(w);
    }

    public override void WriteTo(XmlWriter w)
    {
      w.WriteRaw("&");
      w.WriteName(this.Name);
      w.WriteRaw(";");
    }

    internal void SetReferencedEntityContent()
    {
      if (this.FirstChild != null || this.OwnerDocument.DocumentType == null)
        return;
      XmlEntity entity = this.Entity;
      if (entity == null)
      {
        this.InsertBefore((XmlNode) this.OwnerDocument.CreateTextNode(string.Empty), (XmlNode) null, false, true);
      }
      else
      {
        for (int i = 0; i < entity.ChildNodes.Count; ++i)
          this.InsertBefore(entity.ChildNodes[i].CloneNode(true), (XmlNode) null, false, true);
      }
    }
  }
}
