// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDNotationDeclaration
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace Mono.Xml
{
  internal class DTDNotationDeclaration : DTDNode
  {
    private string name;
    private string localName;
    private string prefix;
    private string publicId;
    private string systemId;

    internal DTDNotationDeclaration(DTDObjectModel root) => this.SetRoot(root);

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public string PublicId
    {
      get => this.publicId;
      set => this.publicId = value;
    }

    public string SystemId
    {
      get => this.systemId;
      set => this.systemId = value;
    }

    public string LocalName
    {
      get => this.localName;
      set => this.localName = value;
    }

    public string Prefix
    {
      get => this.prefix;
      set => this.prefix = value;
    }
  }
}
