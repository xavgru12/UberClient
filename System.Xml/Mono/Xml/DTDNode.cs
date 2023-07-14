// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDNode
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml;

namespace Mono.Xml
{
  internal abstract class DTDNode : IXmlLineInfo
  {
    private DTDObjectModel root;
    private bool isInternalSubset;
    private string baseURI;
    private int lineNumber;
    private int linePosition;

    public virtual string BaseURI
    {
      get => this.baseURI;
      set => this.baseURI = value;
    }

    public bool IsInternalSubset
    {
      get => this.isInternalSubset;
      set => this.isInternalSubset = value;
    }

    public int LineNumber
    {
      get => this.lineNumber;
      set => this.lineNumber = value;
    }

    public int LinePosition
    {
      get => this.linePosition;
      set => this.linePosition = value;
    }

    public bool HasLineInfo() => this.lineNumber != 0;

    internal void SetRoot(DTDObjectModel root)
    {
      this.root = root;
      if (this.baseURI != null)
        return;
      this.BaseURI = root.BaseURI;
    }

    protected DTDObjectModel Root => this.root;

    internal XmlException NotWFError(string message) => new XmlException((IXmlLineInfo) this, this.BaseURI, message);
  }
}
