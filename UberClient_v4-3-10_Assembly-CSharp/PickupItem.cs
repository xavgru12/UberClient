// Decompiled with JetBrains decompiler
// Type: PickupItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

[RequireComponent(typeof (BoxCollider))]
public class PickupItem : MonoBehaviour
{
  [SerializeField]
  protected int _respawnTime = 20;
  [SerializeField]
  private ParticleEmitter _emitter;
  [SerializeField]
  protected Transform _pickupItem;
  protected MeshRenderer[] _renderers;
  private bool _isAvailable;
  private int _pickupID;
  private Collider _collider;
  private static int _instanceCounter = 0;
  private static Dictionary<int, PickupItem> _instances = new Dictionary<int, PickupItem>();
  private static List<byte> _pickupRespawnDurations = new List<byte>();

  public bool IsAvailable
  {
    get => this._isAvailable;
    protected set => this._isAvailable = value;
  }

  protected virtual void Awake()
  {
    this._collider = this.GetComponent<Collider>();
    this._renderers = !(bool) (UnityEngine.Object) this._pickupItem ? new MeshRenderer[0] : this._pickupItem.GetComponentsInChildren<MeshRenderer>(true);
    this._collider.isTrigger = true;
    if ((bool) (UnityEngine.Object) this._emitter)
      this._emitter.emit = false;
    this.gameObject.layer = 2;
  }

  private void OnEnable()
  {
    this.IsAvailable = true;
    this._pickupID = PickupItem.AddInstance(this);
    foreach (Renderer renderer in this._renderers)
      renderer.enabled = true;
    CmuneEventHandler.AddListener<PickupItemEvent>(new Action<PickupItemEvent>(this.OnRemotePickupEvent));
  }

  private void OnDisable() => CmuneEventHandler.RemoveListener<PickupItemEvent>(new Action<PickupItemEvent>(this.OnRemotePickupEvent));

  private void OnRemotePickupEvent(PickupItemEvent ev)
  {
    if (this.PickupID != ev.PickupID)
      return;
    this.SetItemAvailable(ev.ShowItem);
    if (ev.ShowItem || !this.IsAvailable)
      return;
    this.OnRemotePickup();
  }

  protected virtual void OnRemotePickup()
  {
  }

  private void OnTriggerEnter(Collider c)
  {
    if (!this.IsAvailable || !(c.tag == "Player") || !GameState.HasCurrentPlayer || !GameState.LocalCharacter.IsAlive || !this.OnPlayerPickup())
      return;
    this.SetItemAvailable(false);
  }

  protected void PlayLocalPickupSound(AudioClip AudioClip) => SfxManager.Play2dAudioClip(AudioClip);

  protected void PlayRemotePickupSound(AudioClip AudioClip, Vector3 position) => SfxManager.Play3dAudioClip(AudioClip, position);

  [DebuggerHidden]
  protected IEnumerator StartHidingPickupForSeconds(int seconds) => (IEnumerator) new PickupItem.\u003CStartHidingPickupForSeconds\u003Ec__Iterator38()
  {
    seconds = seconds,
    \u003C\u0024\u003Eseconds = seconds,
    \u003C\u003Ef__this = this
  };

  public void SetItemAvailable(bool isVisible)
  {
    if (isVisible)
      ParticleEffectController.ShowPickUpEffect(this._pickupItem.position, 5);
    else if (this.IsAvailable)
      ParticleEffectController.ShowPickUpEffect(this._pickupItem.position, 100);
    foreach (Renderer renderer in this._renderers)
    {
      if ((bool) (UnityEngine.Object) renderer)
        renderer.enabled = isVisible;
    }
    this.IsAvailable = isVisible;
  }

  protected virtual bool OnPlayerPickup() => true;

  protected virtual bool CanPlayerPickup => true;

  public int PickupID
  {
    get => this._pickupID;
    set => this._pickupID = value;
  }

  public int RespawnTime => this._respawnTime;

  public static void ResetInstanceCounter()
  {
    PickupItem._instanceCounter = 0;
    PickupItem._instances.Clear();
    PickupItem._pickupRespawnDurations.Clear();
  }

  public static int GetInstanceCounter() => PickupItem._instanceCounter;

  public static List<byte> GetRespawnDurations() => PickupItem._pickupRespawnDurations;

  private static int AddInstance(PickupItem i)
  {
    int key = PickupItem._instanceCounter++;
    PickupItem._instances[key] = i;
    PickupItem._pickupRespawnDurations.Add((byte) i.RespawnTime);
    return key;
  }

  public static PickupItem GetInstance(int id)
  {
    PickupItem instance = (PickupItem) null;
    PickupItem._instances.TryGetValue(id, out instance);
    return instance;
  }
}
