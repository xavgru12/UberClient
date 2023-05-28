// Decompiled with JetBrains decompiler
// Type: CoroutineManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;

public static class CoroutineManager
{
  private static int _routineId = 1;
  public static Dictionary<string, int> coroutineFuncIds = new Dictionary<string, int>();

  public static void StartCoroutine(CoroutineManager.CoroutineFunction func, bool unique = true)
  {
    if (unique && CoroutineManager.IsRunning(func))
      return;
    MonoRoutine.Start(func());
  }

  public static int Begin(CoroutineManager.CoroutineFunction func)
  {
    CoroutineManager.coroutineFuncIds[CoroutineManager.GetMethodId(func)] = ++CoroutineManager._routineId;
    return CoroutineManager._routineId;
  }

  public static void End(CoroutineManager.CoroutineFunction func, int id)
  {
    string methodId = CoroutineManager.GetMethodId(func);
    if (!CoroutineManager.coroutineFuncIds.ContainsKey(methodId) || CoroutineManager.coroutineFuncIds[methodId] != id)
      return;
    CoroutineManager.coroutineFuncIds.Remove(methodId);
  }

  public static bool IsRunning(CoroutineManager.CoroutineFunction func) => CoroutineManager.coroutineFuncIds.ContainsKey(CoroutineManager.GetMethodId(func));

  public static bool IsCurrent(CoroutineManager.CoroutineFunction func, int coroutineId)
  {
    int num = 0;
    CoroutineManager.coroutineFuncIds.TryGetValue(CoroutineManager.GetMethodId(func), out num);
    return num == coroutineId;
  }

  public static void StopCoroutine(CoroutineManager.CoroutineFunction func) => CoroutineManager.coroutineFuncIds.Remove(CoroutineManager.GetMethodId(func));

  private static string GetMethodId(CoroutineManager.CoroutineFunction callback) => string.Format("{0}{1}", (object) callback.Method.DeclaringType.FullName, (object) callback.Method.Name);

  public delegate IEnumerator CoroutineFunction();
}
