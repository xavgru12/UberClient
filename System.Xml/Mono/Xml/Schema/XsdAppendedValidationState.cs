// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdAppendedValidationState
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace Mono.Xml.Schema
{
  internal class XsdAppendedValidationState : XsdValidationState
  {
    private XsdValidationState head;
    private XsdValidationState rest;

    public XsdAppendedValidationState(
      XsdParticleStateManager manager,
      XsdValidationState head,
      XsdValidationState rest)
      : base(manager)
    {
      this.head = head;
      this.rest = rest;
    }

    public override void GetExpectedParticles(ArrayList al)
    {
      this.head.GetExpectedParticles(al);
      this.rest.GetExpectedParticles(al);
    }

    public override XsdValidationState EvaluateStartElement(string name, string ns)
    {
      XsdValidationState startElement = this.head.EvaluateStartElement(name, ns);
      if (startElement != XsdValidationState.Invalid)
      {
        this.head = startElement;
        return startElement is XsdEmptyValidationState ? this.rest : (XsdValidationState) this;
      }
      return !this.head.EvaluateIsEmptiable() ? (XsdValidationState) XsdValidationState.Invalid : this.rest.EvaluateStartElement(name, ns);
    }

    public override bool EvaluateEndElement()
    {
      if (this.head.EvaluateEndElement())
        return this.rest.EvaluateIsEmptiable();
      return this.head.EvaluateIsEmptiable() && this.rest.EvaluateEndElement();
    }

    internal override bool EvaluateIsEmptiable() => this.head.EvaluateIsEmptiable() && this.rest.EvaluateIsEmptiable();
  }
}
