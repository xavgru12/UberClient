// Decompiled with JetBrains decompiler
// Type: EventPopupManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class EventPopupManager : Singleton<EventPopupManager>
{
  private Queue<IPopupDialog> popups;

  private EventPopupManager() => this.popups = new Queue<IPopupDialog>();

  public void AddEventPopup(IPopupDialog popup) => this.popups.Enqueue(popup);

  public void ShowNextPopup(int delay = 0)
  {
    if (this.popups.Count <= 0)
      return;
    MonoRoutine.Start(this.ShowPopup(this.popups.Dequeue(), delay));
  }

  [DebuggerHidden]
  private IEnumerator ShowPopup(IPopupDialog popup, int delay) => (IEnumerator) new EventPopupManager.\u003CShowPopup\u003Ec__Iterator11()
  {
    delay = delay,
    popup = popup,
    \u003C\u0024\u003Edelay = delay,
    \u003C\u0024\u003Epopup = popup
  };
}
