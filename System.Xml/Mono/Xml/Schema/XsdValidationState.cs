// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdValidationState
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace Mono.Xml.Schema
{
  internal abstract class XsdValidationState
  {
    private static XsdInvalidValidationState invalid = new XsdInvalidValidationState((XsdParticleStateManager) null);
    private int occured;
    private readonly XsdParticleStateManager manager;

    public XsdValidationState(XsdParticleStateManager manager) => this.manager = manager;

    public static XsdInvalidValidationState Invalid => XsdValidationState.invalid;

    public abstract XsdValidationState EvaluateStartElement(string localName, string ns);

    public abstract bool EvaluateEndElement();

    internal abstract bool EvaluateIsEmptiable();

    public abstract void GetExpectedParticles(ArrayList al);

    public XsdParticleStateManager Manager => this.manager;

    public int Occured => this.occured;

    internal int OccuredInternal
    {
      get => this.occured;
      set => this.occured = value;
    }
  }
}
