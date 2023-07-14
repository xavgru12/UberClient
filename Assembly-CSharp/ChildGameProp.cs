// Decompiled with JetBrains decompiler
// Type: ChildGameProp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

public class ChildGameProp : BaseGameProp
{
  public BaseGameProp ParentProp;

  public override void ApplyDamage(DamageInfo d) => this.ParentProp.ApplyDamage(d);
}
