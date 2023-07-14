// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlParserContext
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml;
using System.Collections;
using System.IO;
using System.Text;

namespace System.Xml
{
  public class XmlParserContext
  {
    private string baseURI = string.Empty;
    private string docTypeName = string.Empty;
    private Encoding encoding;
    private string internalSubset = string.Empty;
    private XmlNamespaceManager namespaceManager;
    private XmlNameTable nameTable;
    private string publicID = string.Empty;
    private string systemID = string.Empty;
    private string xmlLang = string.Empty;
    private XmlSpace xmlSpace;
    private ArrayList contextItems;
    private int contextItemCount;
    private DTDObjectModel dtd;

    public XmlParserContext(
      XmlNameTable nt,
      XmlNamespaceManager nsMgr,
      string xmlLang,
      XmlSpace xmlSpace)
      : this(nt, nsMgr, (string) null, (string) null, (string) null, (string) null, (string) null, xmlLang, xmlSpace, (Encoding) null)
    {
    }

    public XmlParserContext(
      XmlNameTable nt,
      XmlNamespaceManager nsMgr,
      string xmlLang,
      XmlSpace xmlSpace,
      Encoding enc)
      : this(nt, nsMgr, (string) null, (string) null, (string) null, (string) null, (string) null, xmlLang, xmlSpace, enc)
    {
    }

    public XmlParserContext(
      XmlNameTable nt,
      XmlNamespaceManager nsMgr,
      string docTypeName,
      string pubId,
      string sysId,
      string internalSubset,
      string baseURI,
      string xmlLang,
      XmlSpace xmlSpace)
      : this(nt, nsMgr, docTypeName, pubId, sysId, internalSubset, baseURI, xmlLang, xmlSpace, (Encoding) null)
    {
    }

    public XmlParserContext(
      XmlNameTable nt,
      XmlNamespaceManager nsMgr,
      string docTypeName,
      string pubId,
      string sysId,
      string internalSubset,
      string baseURI,
      string xmlLang,
      XmlSpace xmlSpace,
      Encoding enc)
      : this(nt, nsMgr, docTypeName == null || !(docTypeName != string.Empty) ? (DTDObjectModel) null : new Mono.Xml2.XmlTextReader(TextReader.Null, nt).GenerateDTDObjectModel(docTypeName, pubId, sysId, internalSubset), baseURI, xmlLang, xmlSpace, enc)
    {
    }

    internal XmlParserContext(
      XmlNameTable nt,
      XmlNamespaceManager nsMgr,
      DTDObjectModel dtd,
      string baseURI,
      string xmlLang,
      XmlSpace xmlSpace,
      Encoding enc)
    {
      this.namespaceManager = nsMgr;
      this.nameTable = nt == null ? (nsMgr == null ? (XmlNameTable) null : nsMgr.NameTable) : nt;
      if (dtd != null)
      {
        this.DocTypeName = dtd.Name;
        this.PublicId = dtd.PublicId;
        this.SystemId = dtd.SystemId;
        this.InternalSubset = dtd.InternalSubset;
        this.dtd = dtd;
      }
      this.encoding = enc;
      this.BaseURI = baseURI;
      this.XmlLang = xmlLang;
      this.xmlSpace = xmlSpace;
      this.contextItems = new ArrayList();
    }

    public string BaseURI
    {
      get => this.baseURI;
      set => this.baseURI = value == null ? string.Empty : value;
    }

    public string DocTypeName
    {
      get
      {
        if (this.docTypeName != null)
          return this.docTypeName;
        return this.dtd != null ? this.dtd.Name : (string) null;
      }
      set => this.docTypeName = value == null ? string.Empty : value;
    }

    internal DTDObjectModel Dtd
    {
      get => this.dtd;
      set => this.dtd = value;
    }

    public Encoding Encoding
    {
      get => this.encoding;
      set => this.encoding = value;
    }

    public string InternalSubset
    {
      get
      {
        if (this.internalSubset != null)
          return this.internalSubset;
        return this.dtd != null ? this.dtd.InternalSubset : (string) null;
      }
      set => this.internalSubset = value == null ? string.Empty : value;
    }

    public XmlNamespaceManager NamespaceManager
    {
      get => this.namespaceManager;
      set => this.namespaceManager = value;
    }

    public XmlNameTable NameTable
    {
      get => this.nameTable;
      set => this.nameTable = value;
    }

    public string PublicId
    {
      get
      {
        if (this.publicID != null)
          return this.publicID;
        return this.dtd != null ? this.dtd.PublicId : (string) null;
      }
      set => this.publicID = value == null ? string.Empty : value;
    }

    public string SystemId
    {
      get
      {
        if (this.systemID != null)
          return this.systemID;
        return this.dtd != null ? this.dtd.SystemId : (string) null;
      }
      set => this.systemID = value == null ? string.Empty : value;
    }

    public string XmlLang
    {
      get => this.xmlLang;
      set => this.xmlLang = value == null ? string.Empty : value;
    }

    public XmlSpace XmlSpace
    {
      get => this.xmlSpace;
      set => this.xmlSpace = value;
    }

    internal void PushScope()
    {
      XmlParserContext.ContextItem contextItem;
      if (this.contextItems.Count == this.contextItemCount)
      {
        contextItem = new XmlParserContext.ContextItem();
        this.contextItems.Add((object) contextItem);
      }
      else
        contextItem = (XmlParserContext.ContextItem) this.contextItems[this.contextItemCount];
      contextItem.BaseURI = this.BaseURI;
      contextItem.XmlLang = this.XmlLang;
      contextItem.XmlSpace = this.XmlSpace;
      ++this.contextItemCount;
    }

    internal void PopScope()
    {
      if (this.contextItemCount == 0)
        throw new XmlException("Unexpected end of element scope.");
      --this.contextItemCount;
      XmlParserContext.ContextItem contextItem = (XmlParserContext.ContextItem) this.contextItems[this.contextItemCount];
      this.baseURI = contextItem.BaseURI;
      this.xmlLang = contextItem.XmlLang;
      this.xmlSpace = contextItem.XmlSpace;
    }

    private class ContextItem
    {
      public string BaseURI;
      public string XmlLang;
      public XmlSpace XmlSpace;
    }
  }
}
