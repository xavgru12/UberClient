// Decompiled with JetBrains decompiler
// Type: TrafficMonitor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class TrafficMonitor
{
  private string lastEvent;
  private Dictionary<int, string> peerOperationNames = new Dictionary<int, string>();
  private Dictionary<int, string> roomOperationNames = new Dictionary<int, string>();
  private Dictionary<int, string> eventNames = new Dictionary<int, string>();

  public LinkedList<string> AllEvents { get; private set; }

  public bool IsEnabled { get; internal set; }

  internal TrafficMonitor(bool enable = true)
  {
    this.AllEvents = new LinkedList<string>();
    this.IsEnabled = enable;
  }

  public void AddEvent(string ev)
  {
    if (this.lastEvent == ev)
      this.AllEvents.Last.Value += ".";
    else
      this.AllEvents.AddLast(Time.frameCount.ToString() + ": " + ev);
    while (this.AllEvents.Count >= 200)
      this.AllEvents.RemoveFirst();
    this.lastEvent = ev;
  }

  internal bool SendOperation(
    byte operationCode,
    Dictionary<byte, object> customOpParameters,
    bool sendReliable,
    byte channelId,
    bool encrypted)
  {
    if (customOpParameters.ContainsKey((byte) 0))
      this.AddEvent("Room Operation<" + operationCode.ToString() + ">: " + (!this.roomOperationNames.ContainsKey((int) operationCode) ? operationCode.ToString() : this.roomOperationNames[(int) operationCode]));
    else if (customOpParameters.ContainsKey((byte) 1))
      this.AddEvent("Peer Operation<" + operationCode.ToString() + ">: " + (!this.peerOperationNames.ContainsKey((int) operationCode) ? operationCode.ToString() : this.peerOperationNames[(int) operationCode]));
    else
      this.AddEvent("Operation<" + operationCode.ToString() + ">");
    return true;
  }

  internal void OnEvent(byte eventCode, byte[] data) => this.AddEvent("OnEvent<" + eventCode.ToString() + ">: " + (!this.eventNames.ContainsKey((int) eventCode) ? eventCode.ToString() : this.eventNames[(int) eventCode]));

  public void AddNamesForPeerOperations(System.Type enumType)
  {
    if (!enumType.IsEnum)
      throw new ArgumentException("AddNamesForPeerOperations failed because argument must be an enumerated type");
    foreach (object key in Enum.GetValues(enumType))
      this.peerOperationNames[(int) key] = key.ToString();
  }

  public void AddNamesForRoomOperations(System.Type enumType)
  {
    if (!enumType.IsEnum)
      throw new ArgumentException("AddNamesForPeerOperations failed because argument must be an enumerated type");
    foreach (object key in Enum.GetValues(enumType))
      this.roomOperationNames[(int) key] = key.ToString();
  }

  public void AddNamesForEvents(System.Type enumType)
  {
    if (!enumType.IsEnum)
      throw new ArgumentException("AddNamesForPeerOperations failed because argument must be an enumerated type");
    foreach (object key in Enum.GetValues(enumType))
      this.eventNames[(int) key] = key.ToString();
  }
}
