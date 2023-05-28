// Decompiled with JetBrains decompiler
// Type: LevelBoundary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using System.Text;
using UnityEngine;

[RequireComponent(typeof (Collider))]
public class LevelBoundary : MonoBehaviour
{
  private static float _checkTime;
  private static LevelBoundary _currentLevelBoundary;

  private void Awake()
  {
    if ((bool) (Object) this.renderer)
      this.renderer.enabled = false;
    this.StartCoroutine(this.StartCheckingPlayerInBounds(this.collider));
  }

  private void OnDisable()
  {
    LevelBoundary._checkTime = 0.0f;
    LevelBoundary._currentLevelBoundary = (LevelBoundary) null;
  }

  private void OnTriggerExit(Collider c)
  {
    if (!(c.tag == "Player") || !GameState.HasCurrentGame)
      return;
    if ((Object) LevelBoundary._currentLevelBoundary == (Object) this)
      LevelBoundary._currentLevelBoundary = (LevelBoundary) null;
    this.StartCoroutine(this.StartCheckingPlayer());
  }

  [DebuggerHidden]
  private IEnumerator StartCheckingPlayer()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    LevelBoundary.\u003CStartCheckingPlayer\u003Ec__Iterator59 playerCIterator59 = new LevelBoundary.\u003CStartCheckingPlayer\u003Ec__Iterator59();
    return (IEnumerator) playerCIterator59;
  }

  [DebuggerHidden]
  private IEnumerator StartCheckingPlayerInBounds(Collider c) => (IEnumerator) new LevelBoundary.\u003CStartCheckingPlayerInBounds\u003Ec__Iterator5A()
  {
    c = c,
    \u003C\u0024\u003Ec = c
  };

  public static void KillPlayer()
  {
    if (GameState.HasCurrentGame && GameState.CurrentGame.IsWaitingForPlayers)
    {
      GameState.CurrentGame.RespawnPlayer();
    }
    else
    {
      if (GameState.LocalPlayer.IsDead || !((Object) GameState.LocalPlayer.Character != (Object) null) || GameState.LocalPlayer.IsDead)
        return;
      GameState.LocalPlayer.Character.ApplyDamage(new DamageInfo((short) 999));
    }
  }

  private void OnTriggerEnter(Collider c)
  {
    if (!(c.tag == "Player") || !GameState.HasCurrentGame)
      return;
    LevelBoundary._currentLevelBoundary = this;
  }

  private string PrintHierarchy(Transform t)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(t.name);
    for (Transform parent = t.parent; (bool) (Object) parent; parent = parent.parent)
      stringBuilder.Insert(0, parent.name + "/");
    return stringBuilder.ToString();
  }

  private string PrintVector(Vector3 v) => string.Format("({0:N6},{1:N6},{2:N6})", (object) v.x, (object) v.y, (object) v.z);
}
