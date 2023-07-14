// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdSequenceValidationState
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdSequenceValidationState : XsdValidationState
  {
    private readonly XmlSchemaSequence seq;
    private int current;
    private XsdValidationState currentAutomata;
    private bool emptiable;

    public XsdSequenceValidationState(XmlSchemaSequence sequence, XsdParticleStateManager manager)
      : base(manager)
    {
      this.seq = sequence;
      this.current = -1;
    }

    public override void GetExpectedParticles(ArrayList al)
    {
      if (this.currentAutomata == null)
      {
        foreach (XmlSchemaParticle compiledItem in this.seq.CompiledItems)
        {
          al.Add((object) compiledItem);
          if (!compiledItem.ValidateIsEmptiable())
            break;
        }
      }
      else
      {
        if (this.currentAutomata != null)
        {
          this.currentAutomata.GetExpectedParticles(al);
          if (!this.currentAutomata.EvaluateIsEmptiable())
            return;
          for (int index = this.current + 1; index < this.seq.CompiledItems.Count; ++index)
          {
            XmlSchemaParticle compiledItem = this.seq.CompiledItems[index] as XmlSchemaParticle;
            al.Add((object) compiledItem);
            if (!compiledItem.ValidateIsEmptiable())
              break;
          }
        }
        if ((Decimal) (this.Occured + 1) == this.seq.ValidatedMaxOccurs)
          return;
        for (int index = 0; index <= this.current; ++index)
          al.Add((object) this.seq.CompiledItems[index]);
      }
    }

    public override XsdValidationState EvaluateStartElement(string name, string ns)
    {
      if (this.seq.CompiledItems.Count == 0)
        return (XsdValidationState) XsdValidationState.Invalid;
      int index = this.current >= 0 ? this.current : 0;
      XsdValidationState xsdValidationState = this.currentAutomata;
      bool flag = false;
      XsdValidationState startElement;
      while (true)
      {
        if (xsdValidationState == null)
        {
          xsdValidationState = this.Manager.Create((XmlSchemaObject) (this.seq.CompiledItems[index] as XmlSchemaParticle));
          flag = true;
        }
        if (!(xsdValidationState is XsdEmptyValidationState) || this.seq.CompiledItems.Count != index + 1 || !((Decimal) this.Occured == this.seq.ValidatedMaxOccurs))
        {
          startElement = xsdValidationState.EvaluateStartElement(name, ns);
          if (startElement == XsdValidationState.Invalid)
          {
            if (xsdValidationState.EvaluateIsEmptiable())
            {
              ++index;
              if (index <= this.current || !flag || this.current < 0)
              {
                if (this.seq.CompiledItems.Count > index)
                  xsdValidationState = this.Manager.Create((XmlSchemaObject) (this.seq.CompiledItems[index] as XmlSchemaParticle));
                else if (this.current >= 0)
                {
                  index = 0;
                  xsdValidationState = (XsdValidationState) null;
                }
                else
                  goto label_19;
              }
              else
                goto label_15;
            }
            else
              goto label_9;
          }
          else
            goto label_10;
        }
        else
          break;
      }
      return (XsdValidationState) XsdValidationState.Invalid;
label_9:
      this.emptiable = false;
      return (XsdValidationState) XsdValidationState.Invalid;
label_10:
      this.current = index;
      this.currentAutomata = startElement;
      if (flag)
      {
        ++this.OccuredInternal;
        if ((Decimal) this.Occured > this.seq.ValidatedMaxOccurs)
          return (XsdValidationState) XsdValidationState.Invalid;
      }
      return (XsdValidationState) this;
label_15:
      return (XsdValidationState) XsdValidationState.Invalid;
label_19:
      return (XsdValidationState) XsdValidationState.Invalid;
    }

    public override bool EvaluateEndElement()
    {
      if (this.seq.ValidatedMinOccurs > (Decimal) (this.Occured + 1))
        return false;
      if (this.seq.CompiledItems.Count == 0 || this.currentAutomata == null && this.seq.ValidatedMinOccurs <= (Decimal) this.Occured)
        return true;
      int current = this.current >= 0 ? this.current : 0;
      for (XsdValidationState xsdValidationState = this.currentAutomata ?? this.Manager.Create((XmlSchemaObject) (this.seq.CompiledItems[current] as XmlSchemaParticle)); xsdValidationState != null; xsdValidationState = this.seq.CompiledItems.Count <= current ? (XsdValidationState) null : this.Manager.Create((XmlSchemaObject) (this.seq.CompiledItems[current] as XmlSchemaParticle)))
      {
        if (!xsdValidationState.EvaluateEndElement() && !xsdValidationState.EvaluateIsEmptiable())
          return false;
        ++current;
      }
      if (this.current < 0)
        ++this.OccuredInternal;
      return this.seq.ValidatedMinOccurs <= (Decimal) this.Occured && this.seq.ValidatedMaxOccurs >= (Decimal) this.Occured;
    }

    internal override bool EvaluateIsEmptiable()
    {
      if (this.seq.ValidatedMinOccurs > (Decimal) (this.Occured + 1))
        return false;
      if (this.seq.ValidatedMinOccurs == 0M && this.currentAutomata == null || this.emptiable || this.seq.CompiledItems.Count == 0)
        return true;
      int current = this.current >= 0 ? this.current : 0;
      for (XsdValidationState xsdValidationState = this.currentAutomata ?? this.Manager.Create((XmlSchemaObject) (this.seq.CompiledItems[current] as XmlSchemaParticle)); xsdValidationState != null; xsdValidationState = this.seq.CompiledItems.Count <= current ? (XsdValidationState) null : this.Manager.Create((XmlSchemaObject) (this.seq.CompiledItems[current] as XmlSchemaParticle)))
      {
        if (!xsdValidationState.EvaluateIsEmptiable())
          return false;
        ++current;
      }
      this.emptiable = true;
      return true;
    }
  }
}
