// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdInvalidValidationState
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace Mono.Xml.Schema
{
  internal class XsdInvalidValidationState : XsdValidationState
  {
    internal XsdInvalidValidationState(XsdParticleStateManager manager)
      : base(manager)
    {
    }

    public override void GetExpectedParticles(ArrayList al)
    {
    }

    public override XsdValidationState EvaluateStartElement(string name, string ns) => (XsdValidationState) this;

    public override bool EvaluateEndElement() => false;

    internal override bool EvaluateIsEmptiable() => false;
  }
}
