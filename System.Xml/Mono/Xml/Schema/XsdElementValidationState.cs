// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdElementValidationState
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdElementValidationState : XsdValidationState
  {
    private readonly XmlSchemaElement element;

    public XsdElementValidationState(XmlSchemaElement element, XsdParticleStateManager manager)
      : base(manager)
    {
      this.element = element;
    }

    private string Name => this.element.QualifiedName.Name;

    private string NS => this.element.QualifiedName.Namespace;

    public override void GetExpectedParticles(ArrayList al)
    {
      XmlSchemaElement xmlSchemaElement = (XmlSchemaElement) this.MemberwiseClone();
      Decimal num = this.element.ValidatedMinOccurs - (Decimal) this.Occured;
      xmlSchemaElement.MinOccurs = !(num > 0M) ? 0M : num;
      if (this.element.ValidatedMaxOccurs == Decimal.MaxValue)
        xmlSchemaElement.MaxOccursString = "unbounded";
      else
        xmlSchemaElement.MaxOccurs = this.element.ValidatedMaxOccurs - (Decimal) this.Occured;
      al.Add((object) xmlSchemaElement);
    }

    public override XsdValidationState EvaluateStartElement(string name, string ns)
    {
      if (this.Name == name && this.NS == ns && !this.element.IsAbstract)
        return this.CheckOccurence(this.element);
      for (int index = 0; index < this.element.SubstitutingElements.Count; ++index)
      {
        XmlSchemaElement substitutingElement = (XmlSchemaElement) this.element.SubstitutingElements[index];
        if (substitutingElement.QualifiedName.Name == name && substitutingElement.QualifiedName.Namespace == ns)
          return this.CheckOccurence(substitutingElement);
      }
      return (XsdValidationState) XsdValidationState.Invalid;
    }

    private XsdValidationState CheckOccurence(XmlSchemaElement maybeSubstituted)
    {
      ++this.OccuredInternal;
      this.Manager.CurrentElement = maybeSubstituted;
      if ((Decimal) this.Occured > this.element.ValidatedMaxOccurs)
        return (XsdValidationState) XsdValidationState.Invalid;
      return (Decimal) this.Occured == this.element.ValidatedMaxOccurs ? this.Manager.Create((XmlSchemaObject) XmlSchemaParticle.Empty) : (XsdValidationState) this;
    }

    public override bool EvaluateEndElement() => this.EvaluateIsEmptiable();

    internal override bool EvaluateIsEmptiable() => this.element.ValidatedMinOccurs <= (Decimal) this.Occured && this.element.ValidatedMaxOccurs >= (Decimal) this.Occured;
  }
}
