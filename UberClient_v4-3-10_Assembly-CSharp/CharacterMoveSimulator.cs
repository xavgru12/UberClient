// Decompiled with JetBrains decompiler
// Type: CharacterMoveSimulator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CharacterMoveSimulator
{
  private Transform _transform;
  private IObserver _positionObserver;

  public CharacterMoveSimulator(Transform transform) => this._transform = transform;

  public void Update(UberStrike.Realtime.UnitySdk.CharacterInfo state)
  {
    if (state == null)
      return;
    this._transform.localPosition = state.Position;
    this._transform.localRotation = Quaternion.Lerp(this._transform.rotation, state.HorizontalRotation, Time.deltaTime * 5f);
    if (this._positionObserver == null)
      return;
    this._positionObserver.Notify();
  }

  public void AddPositionObserver(IObserver observer) => this._positionObserver = observer;

  public void RemovePositionObserver() => this._positionObserver = (IObserver) null;
}
