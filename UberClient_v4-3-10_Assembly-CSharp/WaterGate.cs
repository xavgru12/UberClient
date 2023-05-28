// Decompiled with JetBrains decompiler
// Type: WaterGate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

[RequireComponent(typeof (BoxCollider))]
public class WaterGate : SecretDoor
{
  [SerializeField]
  private float _maxTime = 1f;
  [SerializeField]
  private WaterGate.DoorElement[] _elements;
  private WaterGate.DoorState _state;
  private float _currentTime;
  private float _timeToClose;
  private int _doorID;

  private void Awake()
  {
    this._state = WaterGate.DoorState.Closed;
    foreach (WaterGate.DoorElement element in this._elements)
      element.ClosedPosition = element.Element.transform.localPosition;
    this._doorID = this.transform.position.GetHashCode();
  }

  public override void Open()
  {
    if (GameState.HasCurrentGame)
      GameState.CurrentGame.OpenDoor(this.DoorID);
    this.OpenDoor();
  }

  private void OpenDoor()
  {
    switch (this._state)
    {
      case WaterGate.DoorState.Closed:
        this._state = WaterGate.DoorState.Opening;
        this._currentTime = 0.0f;
        break;
      case WaterGate.DoorState.Open:
        this._timeToClose = Time.time + 2f;
        break;
      case WaterGate.DoorState.Closing:
        this._state = WaterGate.DoorState.Opening;
        this._currentTime = this._maxTime - this._currentTime;
        break;
    }
    if (!(bool) (UnityEngine.Object) this.audio)
      return;
    this.audio.Play();
  }

  private void OnEnable() => CmuneEventHandler.AddListener<DoorOpenedEvent>(new Action<DoorOpenedEvent>(this.OnDoorOpenedEvent));

  private void OnDisable() => CmuneEventHandler.RemoveListener<DoorOpenedEvent>(new Action<DoorOpenedEvent>(this.OnDoorOpenedEvent));

  private void OnDoorOpenedEvent(DoorOpenedEvent ev)
  {
    if (this.DoorID != ev.DoorID)
      return;
    this.OpenDoor();
  }

  private void OnTriggerEnter(Collider c)
  {
    if (!(c.tag == "Player"))
      return;
    this.Open();
  }

  private void OnTriggerStay(Collider c)
  {
    if (!(c.tag == "Player"))
      return;
    this._timeToClose = Time.time + 2f;
  }

  private void Update()
  {
    if (this._state == WaterGate.DoorState.Opening)
    {
      this._currentTime += Time.deltaTime;
      foreach (WaterGate.DoorElement element in this._elements)
        element.Element.transform.localPosition = Vector3.Lerp(element.ClosedPosition, element.OpenPosition, this._currentTime / this._maxTime);
      if ((double) this._currentTime < (double) this._maxTime)
        return;
      this._state = WaterGate.DoorState.Open;
      this._timeToClose = Time.time + 2f;
      if (!(bool) (UnityEngine.Object) this.audio)
        return;
      this.audio.Stop();
    }
    else if (this._state == WaterGate.DoorState.Open)
    {
      if ((double) this._timeToClose >= (double) Time.time)
        return;
      this._state = WaterGate.DoorState.Closing;
      this._currentTime = 0.0f;
      if (!(bool) (UnityEngine.Object) this.audio)
        return;
      this.audio.Play();
    }
    else
    {
      if (this._state != WaterGate.DoorState.Closing)
        return;
      this._currentTime += Time.deltaTime;
      foreach (WaterGate.DoorElement element in this._elements)
        element.Element.transform.localPosition = Vector3.Lerp(element.OpenPosition, element.ClosedPosition, this._currentTime / this._maxTime);
      if ((double) this._currentTime < (double) this._maxTime)
        return;
      this._state = WaterGate.DoorState.Closed;
      if (!(bool) (UnityEngine.Object) this.audio)
        return;
      this.audio.Stop();
    }
  }

  public int DoorID => this._doorID;

  private enum DoorState
  {
    Closed,
    Opening,
    Open,
    Closing,
  }

  [Serializable]
  public class DoorElement
  {
    [HideInInspector]
    public Vector3 ClosedPosition;
    [HideInInspector]
    public Quaternion ClosedRotation;
    public GameObject Element;
    public Vector3 OpenPosition;
  }
}
