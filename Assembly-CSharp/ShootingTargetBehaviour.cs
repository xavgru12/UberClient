// Decompiled with JetBrains decompiler
// Type: ShootingTargetBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ShootingTargetBehaviour : MonoBehaviour
{
  private List<TutorialShootingTarget> _targets = new List<TutorialShootingTarget>(6);
  [SerializeField]
  private Transform[] _targetPositons;
  [SerializeField]
  private TutorialShootingTarget _targetPrefab;

  private void Awake()
  {
    foreach (Transform targetPositon in this._targetPositons)
    {
      TutorialShootingTarget tutorialShootingTarget = Object.Instantiate((Object) this._targetPrefab, targetPositon.position, targetPositon.rotation) as TutorialShootingTarget;
      tutorialShootingTarget.transform.parent = this.transform;
      this._targets.Add(tutorialShootingTarget);
    }
  }

  private void OnEnable() => this.StartCoroutine(this.StartShootingRange());

  [DebuggerHidden]
  private IEnumerator StartShootingRange() => (IEnumerator) new ShootingTargetBehaviour.\u003CStartShootingRange\u003Ec__Iterator5B()
  {
    \u003C\u003Ef__this = this
  };
}
