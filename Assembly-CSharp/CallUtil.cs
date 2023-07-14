// Decompiled with JetBrains decompiler
// Type: CallUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class CallUtil : Singleton<CallUtil>, IUpdatable
{
  private Dictionary<string, CallUtil.ScheduledFunction> _timeoutGroup;
  private Dictionary<string, CallUtil.ScheduledFunction> _intervalGroup;

  private CallUtil()
  {
    this._timeoutGroup = new Dictionary<string, CallUtil.ScheduledFunction>();
    this._intervalGroup = new Dictionary<string, CallUtil.ScheduledFunction>();
  }

  public string SetTimeout(float time, EventDelegate func, params object[] args)
  {
    string key = Guid.NewGuid().ToString();
    CallUtil.ScheduledFunction scheduledFunction = new CallUtil.ScheduledFunction(new EventFunction(func, args), time);
    this._timeoutGroup.Add(key, scheduledFunction);
    return key;
  }

  public string SetInterval(EventDelegate func, float time, params object[] args)
  {
    string key = Guid.NewGuid().ToString();
    CallUtil.ScheduledFunction scheduledFunction = new CallUtil.ScheduledFunction(new EventFunction(func, args), time);
    this._intervalGroup.Add(key, scheduledFunction);
    return key;
  }

  public void ClearTimeout(string timeoutId)
  {
    if (!this._timeoutGroup.ContainsKey(timeoutId))
      throw new Exception("ClearTimeout - timeout id [" + timeoutId + "] doesn't exist.");
    this._timeoutGroup.Remove(timeoutId);
  }

  public void ClearInterval(string intervalId)
  {
    if (!this._intervalGroup.ContainsKey(intervalId))
      throw new Exception("ClearInterval - interval id [" + intervalId + "] doesn't exist.");
    this._intervalGroup.Remove(intervalId);
  }

  public void Update()
  {
    this.UpdateTimeoutGroup();
    this.UpdateIntervalGroup();
  }

  private void UpdateTimeoutGroup()
  {
    List<string> stringList = new List<string>();
    foreach (KeyValuePair<string, CallUtil.ScheduledFunction> keyValuePair in this._timeoutGroup)
    {
      CallUtil.ScheduledFunction scheduledFunction = keyValuePair.Value;
      if ((double) Time.time > (double) scheduledFunction.CallTime)
      {
        scheduledFunction.Function.Execute();
        stringList.Add(keyValuePair.Key);
      }
    }
    foreach (string key in stringList)
      this._timeoutGroup.Remove(key);
  }

  private void UpdateIntervalGroup()
  {
    foreach (CallUtil.ScheduledFunction scheduledFunction in this._intervalGroup.Values)
    {
      if ((double) Time.time > (double) scheduledFunction.CallTime)
      {
        scheduledFunction.Function.Execute();
        scheduledFunction.CallTime = Time.time + scheduledFunction.DelayedTime;
      }
    }
  }

  private class ScheduledFunction
  {
    public ScheduledFunction(EventFunction function, float time)
    {
      this.Function = function;
      this.DelayedTime = time;
      this.CallTime = Time.time + time;
    }

    public EventFunction Function { get; private set; }

    public float DelayedTime { get; private set; }

    public float CallTime { get; set; }
  }
}
