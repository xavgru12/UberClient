// Decompiled with JetBrains decompiler
// Type: KillComboCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class KillComboCounter
{
  private const float MultiKillInterval = 10f;
  private float _lastKillTime;
  private int _killCounter;

  public void OnKillEnemy()
  {
    if ((double) Time.time < (double) this._lastKillTime + 10.0)
    {
      ++this._killCounter;
      this._lastKillTime = Time.time;
      Singleton<PopupHud>.Instance.PopupMultiKill(this._killCounter);
    }
    else
    {
      this._killCounter = 1;
      this._lastKillTime = Time.time;
    }
  }

  public void ResetCounter() => this._killCounter = 0;
}
