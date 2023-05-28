// Decompiled with JetBrains decompiler
// Type: TutorialShootingTargetController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TutorialShootingTargetController
{
  private TutorialGameMode _game;
  private List<TutorialShootingTarget> _targets = new List<TutorialShootingTarget>(6);

  public TutorialShootingTargetController(TutorialGameMode mode)
  {
    this._game = mode;
    List<Transform> transformList = new List<Transform>();
    transformList.AddRange((IEnumerable<Transform>) LevelTutorial.Instance.NearRangeTargetPos);
    transformList.AddRange((IEnumerable<Transform>) LevelTutorial.Instance.FarRangeTargetPos);
    foreach (Transform transform in transformList)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate((UnityEngine.Object) LevelTutorial.Instance.ShootingTargetPrefab, transform.position, transform.rotation) as GameObject;
      if ((bool) (UnityEngine.Object) gameObject)
      {
        TutorialShootingTarget component = gameObject.GetComponent<TutorialShootingTarget>();
        if ((bool) (UnityEngine.Object) component)
        {
          this._targets.Add(component);
          component.OnHitCallback = new Action(this.OnTargetHit);
        }
      }
    }
  }

  [DebuggerHidden]
  public IEnumerator StartShootingRange() => (IEnumerator) new TutorialShootingTargetController.\u003CStartShootingRange\u003Ec__Iterator41()
  {
    \u003C\u003Ef__this = this
  };

  private void OnTargetHit() => HudController.Instance.XpPtsHud.GainXp(1);

  private void EndLevelup() => this._game.OnTutorialEnd();
}
