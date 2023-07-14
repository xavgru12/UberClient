// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDElementDeclaration
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace Mono.Xml
{
  internal class DTDElementDeclaration : DTDNode
  {
    private DTDObjectModel root;
    private DTDContentModel contentModel;
    private string name;
    private bool isEmpty;
    private bool isAny;
    private bool isMixedContent;

    internal DTDElementDeclaration(DTDObjectModel root) => this.root = root;

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public bool IsEmpty
    {
      get => this.isEmpty;
      set => this.isEmpty = value;
    }

    public bool IsAny
    {
      get => this.isAny;
      set => this.isAny = value;
    }

    public bool IsMixedContent
    {
      get => this.isMixedContent;
      set => this.isMixedContent = value;
    }

    public DTDContentModel ContentModel
    {
      get
      {
        if (this.contentModel == null)
          this.contentModel = new DTDContentModel(this.root, this.Name);
        return this.contentModel;
      }
    }

    public DTDAttListDeclaration Attributes => this.Root.AttListDecls[this.Name];
  }
}
