// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDContentModel
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;

namespace Mono.Xml
{
  internal class DTDContentModel : DTDNode
  {
    private DTDObjectModel root;
    private DTDAutomata compiledAutomata;
    private string ownerElementName;
    private string elementName;
    private DTDContentOrderType orderType;
    private DTDContentModelCollection childModels = new DTDContentModelCollection();
    private DTDOccurence occurence;

    internal DTDContentModel(DTDObjectModel root, string ownerElementName)
    {
      this.root = root;
      this.ownerElementName = ownerElementName;
    }

    public DTDContentModelCollection ChildModels
    {
      get => this.childModels;
      set => this.childModels = value;
    }

    public DTDElementDeclaration ElementDecl => this.root.ElementDecls[this.ownerElementName];

    public string ElementName
    {
      get => this.elementName;
      set => this.elementName = value;
    }

    public DTDOccurence Occurence
    {
      get => this.occurence;
      set => this.occurence = value;
    }

    public DTDContentOrderType OrderType
    {
      get => this.orderType;
      set => this.orderType = value;
    }

    public DTDAutomata GetAutomata()
    {
      if (this.compiledAutomata == null)
        this.Compile();
      return this.compiledAutomata;
    }

    public DTDAutomata Compile()
    {
      this.compiledAutomata = this.CompileInternal();
      return this.compiledAutomata;
    }

    private DTDAutomata CompileInternal()
    {
      if (this.ElementDecl.IsAny)
        return (DTDAutomata) this.root.Any;
      if (this.ElementDecl.IsEmpty)
        return (DTDAutomata) this.root.Empty;
      DTDAutomata basicContentAutomata = this.GetBasicContentAutomata();
      switch (this.Occurence)
      {
        case DTDOccurence.One:
          return basicContentAutomata;
        case DTDOccurence.Optional:
          return this.Choice((DTDAutomata) this.root.Empty, basicContentAutomata);
        case DTDOccurence.ZeroOrMore:
          return this.Choice((DTDAutomata) this.root.Empty, (DTDAutomata) new DTDOneOrMoreAutomata(this.root, basicContentAutomata));
        case DTDOccurence.OneOrMore:
          return (DTDAutomata) new DTDOneOrMoreAutomata(this.root, basicContentAutomata);
        default:
          throw new InvalidOperationException();
      }
    }

    private DTDAutomata GetBasicContentAutomata()
    {
      if (this.ElementName != null)
        return (DTDAutomata) new DTDElementAutomata(this.root, this.ElementName);
      switch (this.ChildModels.Count)
      {
        case 0:
          return (DTDAutomata) this.root.Empty;
        case 1:
          return this.ChildModels[0].GetAutomata();
        default:
          int count = this.ChildModels.Count;
          switch (this.OrderType)
          {
            case DTDContentOrderType.Seq:
              DTDAutomata r1 = this.Sequence(this.ChildModels[count - 2].GetAutomata(), this.ChildModels[count - 1].GetAutomata());
              for (int index = count - 2; index > 0; --index)
                r1 = this.Sequence(this.ChildModels[index - 1].GetAutomata(), r1);
              return r1;
            case DTDContentOrderType.Or:
              DTDAutomata r2 = this.Choice(this.ChildModels[count - 2].GetAutomata(), this.ChildModels[count - 1].GetAutomata());
              for (int index = count - 2; index > 0; --index)
                r2 = this.Choice(this.ChildModels[index - 1].GetAutomata(), r2);
              return r2;
            default:
              throw new InvalidOperationException("Invalid pattern specification");
          }
      }
    }

    private DTDAutomata Sequence(DTDAutomata l, DTDAutomata r) => (DTDAutomata) this.root.Factory.Sequence(l, r);

    private DTDAutomata Choice(DTDAutomata l, DTDAutomata r) => l.MakeChoice(r);
  }
}
