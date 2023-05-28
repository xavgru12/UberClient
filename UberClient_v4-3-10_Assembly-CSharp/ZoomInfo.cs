// Decompiled with JetBrains decompiler
// Type: ZoomInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;

[Serializable]
public class ZoomInfo
{
  public float MinMultiplier;
  public float MaxMultiplier;
  public float DefaultMultiplier;
  public float CurrentMultiplier;

  public ZoomInfo()
  {
    this.MinMultiplier = 1f;
    this.MaxMultiplier = 1f;
    this.DefaultMultiplier = 1f;
    this.CurrentMultiplier = 1f;
  }

  public ZoomInfo(float defaultMultiplier, float minMultiplier, float maxMultiplier)
  {
    this.MinMultiplier = minMultiplier;
    this.MaxMultiplier = maxMultiplier;
    this.DefaultMultiplier = defaultMultiplier;
    this.CurrentMultiplier = this.DefaultMultiplier;
  }
}
