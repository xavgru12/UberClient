// Decompiled with JetBrains decompiler
// Type: AnimationInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;

[Serializable]
public class AnimationInfo
{
  public UnityEngine.AnimationState State;
  public string Name;
  public AnimationIndex Index;
  public float EndTime;
  public float CurrentTimePlayed;
  public float Speed = 1f;

  public AnimationInfo(AnimationIndex idx, UnityEngine.AnimationState state)
  {
    this.State = state;
    this.Name = Enum.GetName(typeof (AnimationIndex), (object) idx);
    this.Index = idx;
  }
}
