// Decompiled with JetBrains decompiler
// Type: TutorialArmoryEnterTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class TutorialArmoryEnterTrigger : MonoBehaviour
{
  public Transform ArmoryDoor;
  public Transform ArmoryDesk;
  public SplineController ArmoryCameraPath;
  private bool _entered;
  private Vector3 _velocity;

  private void OnTriggerEnter()
  {
    if (this._entered)
      return;
    this._entered = true;
    GameState.LocalPlayer.IsWalkingEnabled = false;
    AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = false;
    LevelCamera.SetBobMode(BobMode.None);
    LevelCamera.Instance.SetMode(LevelCamera.CameraMode.None);
    LevelTutorial.Instance.ArmoryWaypoint.CanShow = false;
    Object.Destroy((Object) LevelTutorial.Instance.ArmoryWaypoint);
    this.StartCoroutine(this.StartEnterArmory());
  }

  [DebuggerHidden]
  private IEnumerator StartEnterArmory() => (IEnumerator) new TutorialArmoryEnterTrigger.\u003CStartEnterArmory\u003Ec__Iterator3D()
  {
    \u003C\u003Ef__this = this
  };

  public void Reset() => this._entered = false;
}
