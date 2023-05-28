// Decompiled with JetBrains decompiler
// Type: LevelEnviroment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LevelEnviroment : MonoBehaviour
{
  public const float MovementSpeed = 1f;
  public const float Modifier = 0.035f;
  public const float PlayerWalkSpeed = 7f;
  public const float PlayerJumpSpeed = 15f;
  public EnviromentSettings Settings;

  public static LevelEnviroment Instance { get; private set; }

  public static bool Exists => (Object) LevelEnviroment.Instance != (Object) null;

  private void Awake() => LevelEnviroment.Instance = this;
}
