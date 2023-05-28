// Decompiled with JetBrains decompiler
// Type: ICharacterState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public interface ICharacterState
{
  UberStrike.Realtime.UnitySdk.CharacterInfo Info { get; }

  Vector3 LastPosition { get; }

  void RecieveDeltaUpdate(SyncObject delta);

  void SubscribeToEvents(CharacterConfig config);

  void UnSubscribeAll();
}
