// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdAnyValidationState
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdAnyValidationState : XsdValidationState
  {
    private readonly XmlSchemaAny any;

    public XsdAnyValidationState(XmlSchemaAny any, XsdParticleStateManager manager)
      : base(manager)
    {
      this.any = any;
    }

    public override void GetExpectedParticles(ArrayList al) => al.Add((object) this.any);

    public override XsdValidationState EvaluateStartElement(string name, string ns)
    {
      if (!this.MatchesNamespace(ns))
        return (XsdValidationState) XsdValidationState.Invalid;
      ++this.OccuredInternal;
      this.Manager.SetProcessContents(this.any.ResolvedProcessContents);
      if ((Decimal) this.Occured > this.any.ValidatedMaxOccurs)
        return (XsdValidationState) XsdValidationState.Invalid;
      return (Decimal) this.Occured == this.any.ValidatedMaxOccurs ? this.Manager.Create((XmlSchemaObject) XmlSchemaParticle.Empty) : (XsdValidationState) this;
    }

    private bool MatchesNamespace(string ns)
    {
      if (this.any.HasValueAny || this.any.HasValueLocal && ns == string.Empty || this.any.HasValueOther && (this.any.TargetNamespace == string.Empty || this.any.TargetNamespace != ns) || this.any.HasValueTargetNamespace && this.any.TargetNamespace == ns)
        return true;
      for (int index = 0; index < this.any.ResolvedNamespaces.Count; ++index)
      {
        if (this.any.ResolvedNamespaces[index] == ns)
          return true;
      }
      return false;
    }

    public override bool EvaluateEndElement() => this.EvaluateIsEmptiable();

    internal override bool EvaluateIsEmptiable() => this.any.ValidatedMinOccurs <= (Decimal) this.Occured && this.any.ValidatedMaxOccurs >= (Decimal) this.Occured;
  }
}
