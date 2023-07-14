// Decompiled with JetBrains decompiler
// Type: TutorialAirlockFrontDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TutorialAirlockFrontDoor : MonoBehaviour
{
  private bool _playerEntered;
  private TutorialWaypoint _waypoint;

  public bool PlayerEntered => this._playerEntered;

  public TutorialWaypoint Waypoint => this._waypoint;

  private void Awake() => this._waypoint = this.GetComponent<TutorialWaypoint>();

  private void OnTriggerEnter(Collider c)
  {
    if ((bool) (Object) this._waypoint)
      this._waypoint.CanShow = false;
    this._playerEntered = true;
  }
}
