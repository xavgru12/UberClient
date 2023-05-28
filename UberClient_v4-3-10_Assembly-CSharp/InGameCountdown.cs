// Decompiled with JetBrains decompiler
// Type: InGameCountdown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

internal class InGameCountdown
{
  private int _remainingSeconds;

  public int EndTime { get; set; }

  public int RemainingSeconds
  {
    get => this._remainingSeconds;
    private set
    {
      if (this._remainingSeconds == value)
        return;
      this._remainingSeconds = value;
      this.OnUpdateRemainingSeconds();
    }
  }

  public void Stop() => this.RemainingSeconds = 0;

  public void Update()
  {
    int num = Mathf.CeilToInt((float) (this.EndTime - GameConnectionManager.Client.PeerListener.ServerTimeTicks) / 1000f);
    if (this.RemainingSeconds == num)
      return;
    Singleton<MatchStatusHud>.Instance.RemainingSeconds = num;
    this.RemainingSeconds = num;
  }

  private void OnUpdateRemainingSeconds() => Singleton<MatchStatusHud>.Instance.RemainingSeconds = this._remainingSeconds;
}
