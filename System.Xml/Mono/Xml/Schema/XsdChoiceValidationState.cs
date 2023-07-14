// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdChoiceValidationState
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdChoiceValidationState : XsdValidationState
  {
    private readonly XmlSchemaChoice choice;
    private bool emptiable;
    private bool emptiableComputed;

    public XsdChoiceValidationState(XmlSchemaChoice choice, XsdParticleStateManager manager)
      : base(manager)
    {
      this.choice = choice;
    }

    public override void GetExpectedParticles(ArrayList al)
    {
      if (!((Decimal) this.Occured < this.choice.ValidatedMaxOccurs))
        return;
      foreach (XmlSchemaParticle compiledItem in this.choice.CompiledItems)
        al.Add((object) compiledItem);
    }

    public override XsdValidationState EvaluateStartElement(string localName, string ns)
    {
      this.emptiableComputed = false;
      bool flag = true;
      for (int index = 0; index < this.choice.CompiledItems.Count; ++index)
      {
        XsdValidationState xsdValidationState = this.Manager.Create(this.choice.CompiledItems[index]);
        XsdValidationState startElement = xsdValidationState.EvaluateStartElement(localName, ns);
        if (startElement != XsdValidationState.Invalid)
        {
          ++this.OccuredInternal;
          if ((Decimal) this.Occured > this.choice.ValidatedMaxOccurs)
            return (XsdValidationState) XsdValidationState.Invalid;
          return (Decimal) this.Occured == this.choice.ValidatedMaxOccurs ? startElement : this.Manager.MakeSequence(startElement, (XsdValidationState) this);
        }
        if (!this.emptiableComputed)
          flag &= xsdValidationState.EvaluateIsEmptiable();
      }
      if (!this.emptiableComputed)
      {
        if (flag)
          this.emptiable = true;
        if (!this.emptiable)
          this.emptiable = this.choice.ValidatedMinOccurs <= (Decimal) this.Occured;
        this.emptiableComputed = true;
      }
      return (XsdValidationState) XsdValidationState.Invalid;
    }

    public override bool EvaluateEndElement()
    {
      this.emptiableComputed = false;
      if (this.choice.ValidatedMinOccurs > (Decimal) (this.Occured + 1))
        return false;
      if (this.choice.ValidatedMinOccurs <= (Decimal) this.Occured)
        return true;
      for (int index = 0; index < this.choice.CompiledItems.Count; ++index)
      {
        if (this.Manager.Create(this.choice.CompiledItems[index]).EvaluateIsEmptiable())
          return true;
      }
      return false;
    }

    internal override bool EvaluateIsEmptiable()
    {
      if (this.emptiableComputed)
        return this.emptiable;
      if (this.choice.ValidatedMaxOccurs < (Decimal) this.Occured || this.choice.ValidatedMinOccurs > (Decimal) (this.Occured + 1))
        return false;
      for (int occured = this.Occured; (Decimal) occured < this.choice.ValidatedMinOccurs; ++occured)
      {
        bool flag = false;
        for (int index = 0; index < this.choice.CompiledItems.Count; ++index)
        {
          if (this.Manager.Create(this.choice.CompiledItems[index]).EvaluateIsEmptiable())
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return false;
      }
      return true;
    }
  }
}
