// Decompiled with JetBrains decompiler
// Type: LotteryPopupTask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;

public class LotteryPopupTask
{
  private const float MinWaitingTime = 2f;
  private LotteryPopupTask.State _state;
  private LotteryPopupDialog _popup;

  public LotteryPopupTask(LotteryPopupDialog dialog)
  {
    this._state = LotteryPopupTask.State.None;
    this._popup = dialog;
    this._popup.SetRollCallback(new Action(this.OnLotteryRolled));
    MonoRoutine.Start(this.StartTask());
  }

  [DebuggerHidden]
  private IEnumerator StartTask() => (IEnumerator) new LotteryPopupTask.\u003CStartTask\u003Ec__Iterator55()
  {
    \u003C\u003Ef__this = this
  };

  private void OnLotteryRolled() => this._state = LotteryPopupTask.State.Rolled;

  private enum State
  {
    None,
    Rolled,
  }
}
