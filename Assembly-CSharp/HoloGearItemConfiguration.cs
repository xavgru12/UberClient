// Decompiled with JetBrains decompiler
// Type: HoloGearItemConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Core.Models.Views;
using UnityEngine;

[Serializable]
public class HoloGearItemConfiguration : UberStrikeItemGearView
{
  [SerializeField]
  private AvatarDecorator _avatar;
  [SerializeField]
  private AvatarDecoratorConfig _ragdoll;

  public AvatarDecorator Avatar => this._avatar;

  public AvatarDecoratorConfig Ragdoll => this._ragdoll;
}
