// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.PerformanceStats
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;
using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
{
  public class PerformanceStats
  {
    private Dictionary<PerfCounter, float> _counter = new Dictionary<PerfCounter, float>(20);

    public void SetCounter(PerfCounter c, float f) => this._counter[c] = f;

    public void SetCounter(int c, float f)
    {
      if (!Enum.IsDefined(typeof (PerfCounter), (object) c))
        return;
      this._counter[(PerfCounter) c] = f;
    }

    public void SetCounter(Dictionary<PerfCounter, float> counter)
    {
      foreach (KeyValuePair<PerfCounter, float> keyValuePair in counter)
        this.SetCounter(keyValuePair.Key, keyValuePair.Value);
    }

    public void SetCounter(Dictionary<int, float> counter)
    {
      foreach (KeyValuePair<int, float> keyValuePair in counter)
        this.SetCounter(keyValuePair.Key, keyValuePair.Value);
    }

    public bool HasCounter(PerfCounter counter) => this._counter.ContainsKey(counter);

    public float GetCounter(PerfCounter counter)
    {
      float counter1 = 0.0f;
      this._counter.TryGetValue(counter, out counter1);
      return counter1;
    }

    public void Reset() => this._counter.Clear();
  }
}
