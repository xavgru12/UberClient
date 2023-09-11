// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.Utils.PerformanceStats
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.Collections.Generic;

namespace Cmune.Realtime.Common.Utils
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
