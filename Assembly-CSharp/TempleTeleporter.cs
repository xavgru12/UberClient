// Decompiled with JetBrains decompiler
// Type: TempleTeleporter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

[RequireComponent(typeof (BoxCollider))]
public class TempleTeleporter : SecretDoor
{
  [SerializeField]
  private float _activationTime = 15f;
  [SerializeField]
  private Renderer[] _visuals;
  [SerializeField]
  private Transform _spawnpoint;
  [SerializeField]
  private ParticleEmitter _particles;
  private int _doorID;
  private float _timeOut;
  private AudioSource[] _audios;

  private void Awake()
  {
    this._audios = this.GetComponents<AudioSource>();
    this._particles.emit = false;
    foreach (Renderer visual in this._visuals)
      visual.enabled = false;
    this._doorID = this.transform.position.GetHashCode();
  }

  private void OnEnable() => CmuneEventHandler.AddListener<DoorOpenedEvent>(new Action<DoorOpenedEvent>(this.OnDoorOpenedEvent));

  private void OnDisable() => CmuneEventHandler.RemoveListener<DoorOpenedEvent>(new Action<DoorOpenedEvent>(this.OnDoorOpenedEvent));

  private void Update()
  {
    if ((double) this._timeOut >= (double) Time.time)
      return;
    foreach (AudioSource audio in this._audios)
      audio.Stop();
    this._particles.emit = false;
    foreach (Renderer visual in this._visuals)
      visual.enabled = false;
    this.enabled = false;
  }

  private void OnTriggerEnter(Collider c)
  {
    if (!(c.tag == "Player") || (double) this._timeOut <= (double) Time.time)
      return;
    this._timeOut = 0.0f;
    GameState.LocalPlayer.SpawnPlayerAt(this._spawnpoint.position, this._spawnpoint.rotation);
  }

  private void OnDoorOpenedEvent(DoorOpenedEvent ev)
  {
    if (this.DoorID != ev.DoorID)
      return;
    this.OpenDoor();
  }

  public override void Open()
  {
    if (GameState.HasCurrentGame)
      GameState.CurrentGame.OpenDoor(this.DoorID);
    this.OpenDoor();
  }

  private void OpenDoor()
  {
    this.enabled = true;
    this._particles.emit = true;
    foreach (Renderer visual in this._visuals)
      visual.enabled = true;
    this._timeOut = Time.time + this._activationTime;
    foreach (AudioSource audio in this._audios)
      audio.Play();
  }

  public int DoorID => this._doorID;
}
