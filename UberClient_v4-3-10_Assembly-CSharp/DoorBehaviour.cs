// Decompiled with JetBrains decompiler
// Type: DoorBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

[RequireComponent(typeof (Collider))]
public class DoorBehaviour : MonoBehaviour
{
  [SerializeField]
  private DoorBehaviour.DoorElement[] _elements;
  [SerializeField]
  private float _maxTime = 1f;
  private DoorBehaviour.DoorState _state;
  private float _currentTime;
  private float _timeToClose;
  private int _doorID;

  private void Awake()
  {
    foreach (DoorBehaviour.DoorElement element in this._elements)
    {
      element.Element.SetDoorLogic(this);
      element.ClosedPosition = element.Element.Transform.localPosition;
    }
    this._doorID = this.transform.position.GetHashCode();
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

  private void OpenDoor()
  {
    switch (this._state)
    {
      case DoorBehaviour.DoorState.Closed:
        this._state = DoorBehaviour.DoorState.Opening;
        this._currentTime = 0.0f;
        if (!(bool) (UnityEngine.Object) this.audio)
          break;
        this.audio.Play();
        break;
      case DoorBehaviour.DoorState.Open:
        this._timeToClose = Time.time + 2f;
        break;
      case DoorBehaviour.DoorState.Closing:
        this._state = DoorBehaviour.DoorState.Opening;
        this._currentTime = this._maxTime - this._currentTime;
        break;
    }
  }

  public void Open()
  {
    if (GameState.HasCurrentGame)
      GameState.CurrentGame.OpenDoor(this.DoorID);
    this.OpenDoor();
  }

  public void Close()
  {
    this._state = DoorBehaviour.DoorState.Closing;
    this._currentTime = 0.0f;
    if (!(bool) (UnityEngine.Object) this.audio)
      return;
    this.audio.Play();
  }

  private void Update()
  {
    float num = this._maxTime;
    if ((double) this._maxTime == 0.0)
      num = 1f;
    if (this._state == DoorBehaviour.DoorState.Opening)
    {
      this._currentTime += Time.deltaTime;
      foreach (DoorBehaviour.DoorElement element in this._elements)
        element.Element.Transform.localPosition = Vector3.Lerp(element.ClosedPosition, element.OpenPosition, this._currentTime / num);
      if ((double) this._currentTime < (double) num)
        return;
      this._state = DoorBehaviour.DoorState.Open;
      this._timeToClose = Time.time + 2f;
      if (!(bool) (UnityEngine.Object) this.audio)
        return;
      this.audio.Stop();
    }
    else if (this._state == DoorBehaviour.DoorState.Open)
    {
      if ((double) this._maxTime == 0.0 || (double) this._timeToClose >= (double) Time.time)
        return;
      this._state = DoorBehaviour.DoorState.Closing;
      this._currentTime = 0.0f;
      if (!(bool) (UnityEngine.Object) this.audio)
        return;
      this.audio.Play();
    }
    else
    {
      if (this._state != DoorBehaviour.DoorState.Closing)
        return;
      this._currentTime += Time.deltaTime;
      foreach (DoorBehaviour.DoorElement element in this._elements)
        element.Element.Transform.localPosition = Vector3.Lerp(element.OpenPosition, element.ClosedPosition, this._currentTime / num);
      if ((double) this._currentTime < (double) num)
        return;
      this._state = DoorBehaviour.DoorState.Closed;
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
    public DoorTrigger Element;
    public Vector3 OpenPosition;
  }
}
