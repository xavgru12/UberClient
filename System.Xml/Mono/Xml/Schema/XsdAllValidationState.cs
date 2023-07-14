// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdAllValidationState
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdAllValidationState : XsdValidationState
  {
    private readonly XmlSchemaAll all;
    private ArrayList consumed = new ArrayList();

    public XsdAllValidationState(XmlSchemaAll all, XsdParticleStateManager manager)
      : base(manager)
    {
      this.all = all;
    }

    public override void GetExpectedParticles(ArrayList al)
    {
      foreach (XmlSchemaParticle compiledItem in this.all.CompiledItems)
      {
        if (!this.consumed.Contains((object) compiledItem))
          al.Add((object) compiledItem);
      }
    }

    public override XsdValidationState EvaluateStartElement(string localName, string ns)
    {
      if (this.all.CompiledItems.Count == 0)
        return (XsdValidationState) XsdValidationState.Invalid;
      for (int index = 0; index < this.all.CompiledItems.Count; ++index)
      {
        XmlSchemaElement compiledItem = (XmlSchemaElement) this.all.CompiledItems[index];
        if (compiledItem.QualifiedName.Name == localName && compiledItem.QualifiedName.Namespace == ns)
        {
          if (this.consumed.Contains((object) compiledItem))
            return (XsdValidationState) XsdValidationState.Invalid;
          this.consumed.Add((object) compiledItem);
          this.Manager.CurrentElement = compiledItem;
          this.OccuredInternal = 1;
          return (XsdValidationState) this;
        }
      }
      return (XsdValidationState) XsdValidationState.Invalid;
    }

    public override bool EvaluateEndElement()
    {
      if (this.all.Emptiable || this.all.ValidatedMinOccurs == 0M)
        return true;
      if (this.all.ValidatedMinOccurs > 0M && this.consumed.Count == 0)
        return false;
      if (this.all.CompiledItems.Count == this.consumed.Count)
        return true;
      for (int index = 0; index < this.all.CompiledItems.Count; ++index)
      {
        XmlSchemaElement compiledItem = (XmlSchemaElement) this.all.CompiledItems[index];
        if (compiledItem.ValidatedMinOccurs > 0M && !this.consumed.Contains((object) compiledItem))
          return false;
      }
      return true;
    }

    internal override bool EvaluateIsEmptiable()
    {
      if (this.all.Emptiable || this.all.ValidatedMinOccurs == 0M)
        return true;
      for (int index = 0; index < this.all.CompiledItems.Count; ++index)
      {
        XmlSchemaElement compiledItem = (XmlSchemaElement) this.all.CompiledItems[index];
        if (compiledItem.ValidatedMinOccurs > 0M && !this.consumed.Contains((object) compiledItem))
          return false;
      }
      return true;
    }
  }
}
