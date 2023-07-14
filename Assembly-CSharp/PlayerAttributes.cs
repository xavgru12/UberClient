// Decompiled with JetBrains decompiler
// Type: PlayerAttributes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;

[Serializable]
public class PlayerAttributes
{
  public const float HEIGHT_NORMAL = 1.6f;
  public const float HEIGHT_DUCKED = 0.9f;
  public const float CENTER_OFFSET_DUCKED = -0.4f;
  public const float CENTER_OFFSET_NORMAL = -0.1f;

  public float Speed => 7f;

  public float JumpForce => 15f;
}
