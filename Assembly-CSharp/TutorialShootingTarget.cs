// Decompiled with JetBrains decompiler
// Type: TutorialShootingTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class TutorialShootingTarget : BaseGameProp
{
  [SerializeField]
  private GameObject Body;
  [SerializeField]
  private PlayerDamageEffect _damageEffect;
  private Vector3 _initialPos = new Vector3(0.0f, -1.72f, 0.0f);
  private bool _isHit;
  private Dictionary<Rigidbody, Vector3> _bodyPos;
  public Action OnHitCallback;

  public bool IsHit => this._isHit;

  public bool UseTimer { get; set; }

  public bool IsMoving { get; set; }

  public float MaxTime { get; set; }

  public Vector3 Direction { get; set; }

  public override void ApplyDamage(DamageInfo shot)
  {
    this._isHit = true;
    this.gameObject.layer = 1;
    if (this.OnHitCallback != null)
      this.OnHitCallback();
    base.ApplyDamage(shot);
    foreach (Rigidbody key in this._bodyPos.Keys)
    {
      key.isKinematic = false;
      key.AddExplosionForce(1000f, shot.Hitpoint, 5f);
    }
    if ((bool) (UnityEngine.Object) this._damageEffect)
    {
      PlayerDamageEffect playerDamageEffect = UnityEngine.Object.Instantiate((UnityEngine.Object) this._damageEffect, shot.Hitpoint, Quaternion.LookRotation(shot.Force)) as PlayerDamageEffect;
      if ((bool) (UnityEngine.Object) playerDamageEffect)
      {
        playerDamageEffect.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        playerDamageEffect.Show(shot);
      }
    }
    else
      UnityEngine.Debug.LogWarning((object) "No damage effect is attached!");
    SfxManager.Play2dAudioClip(GameAudio.TargetDamage);
  }

  public void Reset()
  {
    this._isHit = false;
    this.Body.transform.localPosition = this._initialPos;
    foreach (Rigidbody key in this._bodyPos.Keys)
    {
      key.isKinematic = true;
      key.transform.localPosition = this._bodyPos[key];
      key.transform.localRotation = Quaternion.identity;
    }
    this.StartCoroutine(this.StartPopup());
  }

  private void Awake()
  {
    this.Body.transform.localPosition = this._initialPos;
    this.gameObject.layer = 2;
    this._bodyPos = new Dictionary<Rigidbody, Vector3>();
    foreach (Transform componentsInChild in this.Body.GetComponentsInChildren<Transform>(true))
    {
      if (!((UnityEngine.Object) componentsInChild == (UnityEngine.Object) this.Body.transform))
      {
        MeshCollider meshCollider = componentsInChild.gameObject.AddComponent<MeshCollider>();
        if ((bool) (UnityEngine.Object) meshCollider)
          meshCollider.convex = true;
        Rigidbody key = componentsInChild.gameObject.AddComponent<Rigidbody>();
        if ((bool) (UnityEngine.Object) key)
        {
          key.isKinematic = true;
          key.gameObject.layer = 20;
          this._bodyPos.Add(key, key.transform.localPosition);
        }
      }
    }
  }

  [DebuggerHidden]
  private IEnumerator StartPopup() => (IEnumerator) new TutorialShootingTarget.\u003CStartPopup\u003Ec__Iterator3F()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator StartSelfDestroy() => (IEnumerator) new TutorialShootingTarget.\u003CStartSelfDestroy\u003Ec__Iterator40()
  {
    \u003C\u003Ef__this = this
  };
}
