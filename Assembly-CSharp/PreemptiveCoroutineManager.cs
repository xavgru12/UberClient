// Decompiled with JetBrains decompiler
// Type: PreemptiveCoroutineManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;

public class PreemptiveCoroutineManager : Singleton<PreemptiveCoroutineManager>
{
  private Dictionary<PreemptiveCoroutineManager.CoroutineFunction, int> coroutineFuncIds;

  private PreemptiveCoroutineManager() => this.coroutineFuncIds = new Dictionary<PreemptiveCoroutineManager.CoroutineFunction, int>();

  public int IncrementId(PreemptiveCoroutineManager.CoroutineFunction func)
  {
    Dictionary<PreemptiveCoroutineManager.CoroutineFunction, int> coroutineFuncIds;
    PreemptiveCoroutineManager.CoroutineFunction key;
    return this.coroutineFuncIds.ContainsKey(func) ? ((coroutineFuncIds = this.coroutineFuncIds)[key = func] = coroutineFuncIds[key] + 1) : this.ResetCoroutineId(func);
  }

  public bool IsCurrent(PreemptiveCoroutineManager.CoroutineFunction func, int coroutineId) => this.coroutineFuncIds.ContainsKey(func) && this.coroutineFuncIds[func] == coroutineId;

  public int ResetCoroutineId(PreemptiveCoroutineManager.CoroutineFunction func)
  {
    this.coroutineFuncIds[func] = 0;
    return 0;
  }

  public delegate IEnumerator CoroutineFunction();
}
