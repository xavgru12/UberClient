// Decompiled with JetBrains decompiler
// Type: MonoRoutine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MonoRoutine : AutoMonoBehaviour<MonoRoutine>
{
  public List<MonoRoutine.Routine> Routines = new List<MonoRoutine.Routine>();
  private static readonly List<string> _runningRoutines = new List<string>();
  private static bool _isApplicationQuitting = false;

  public event Action OnUpdateEvent;

  private void OnApplicationQuit() => MonoRoutine._isApplicationQuitting = true;

  private void Update()
  {
    if (this.OnUpdateEvent == null)
      return;
    this.OnUpdateEvent();
  }

  private void OnGUI()
  {
    GUILayout.BeginArea(new Rect(0.0f, 0.0f, 300f, (float) Screen.height));
    foreach (MonoRoutine.Routine routine in AutoMonoBehaviour<MonoRoutine>.Instance.Routines)
      GUILayout.Label("Routine: " + (object) (routine.Rot != null) + " " + routine.Enu.ToString() + " " + (object) (routine.Enu.Current != null));
    GUILayout.EndArea();
  }

  public static Coroutine Start(IEnumerator routine) => !MonoRoutine._isApplicationQuitting ? AutoMonoBehaviour<MonoRoutine>.Instance.StartCoroutine(routine) : (Coroutine) null;

  private static Coroutine Start(IEnumerator routine, string code) => !MonoRoutine._isApplicationQuitting ? AutoMonoBehaviour<MonoRoutine>.Instance.StartCoroutine(AutoMonoBehaviour<MonoRoutine>.Instance.StartSafeRoutine(routine, code)) : (Coroutine) null;

  [DebuggerHidden]
  private IEnumerator StartSafeRoutine(IEnumerator routine, string code) => (IEnumerator) new MonoRoutine.\u003CStartSafeRoutine\u003Ec__Iterator84()
  {
    code = code,
    routine = routine,
    \u003C\u0024\u003Ecode = code,
    \u003C\u0024\u003Eroutine = routine
  };

  public static Coroutine Run(MonoRoutine.FunctionFloat run, float f)
  {
    string code = "Run " + (object) run.Method.GetHashCode() + " " + (object) run.Target.GetHashCode();
    return MonoRoutine.Start(run(f), code);
  }

  public static Coroutine Run(MonoRoutine.FunctionInt run, int i)
  {
    string code = "Run " + (object) run.Method.GetHashCode() + " " + (object) run.Target.GetHashCode();
    return MonoRoutine.Start(run(i), code);
  }

  public static Coroutine Run(MonoRoutine.FunctionVoid run)
  {
    string code = "Run " + (object) run.Method.GetHashCode() + " " + (object) run.Target.GetHashCode();
    return MonoRoutine.Start(run(), code);
  }

  public class Routine
  {
    public IEnumerator Enu;
    public Coroutine Rot;
  }

  public delegate IEnumerator FunctionFloat(float f);

  public delegate IEnumerator FunctionInt(int i);

  public delegate IEnumerator FunctionVoid();
}
