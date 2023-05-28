// Decompiled with JetBrains decompiler
// Type: CheatDetection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class CheatDetection : MonoBehaviour
{
  private static int _gameTime;
  private static DateTime _dateTime;

  public static void SyncSystemTime()
  {
    CheatDetection._gameTime = SystemTime.Running;
    CheatDetection._dateTime = DateTime.Now;
  }

  public static int GameTime => SystemTime.Running - CheatDetection._gameTime;

  public static int RealTime => (int) (DateTime.Now - CheatDetection._dateTime).TotalMilliseconds;

  private void Start()
  {
    this.StartCoroutine(this.StartNewSpeedhackDetection());
    this.StartCoroutine(this.StartCheckSecureMemory());
  }

  [DebuggerHidden]
  private IEnumerator StartCheckSecureMemory()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    CheatDetection.\u003CStartCheckSecureMemory\u003Ec__Iterator81 memoryCIterator81 = new CheatDetection.\u003CStartCheckSecureMemory\u003Ec__Iterator81();
    return (IEnumerator) memoryCIterator81;
  }

  [DebuggerHidden]
  private IEnumerator StartNewSpeedhackDetection() => (IEnumerator) new CheatDetection.\u003CStartNewSpeedhackDetection\u003Ec__Iterator82()
  {
    \u003C\u003Ef__this = this
  };

  private bool IsSpeedHacking(IEnumerable<float> list)
  {
    int num1 = 0;
    float num2 = 0.0f;
    foreach (float num3 in list)
    {
      num2 += num3;
      ++num1;
    }
    float num4 = num2 / (float) num1;
    float num5 = 0.0f;
    foreach (float num6 in list)
      num5 += Mathf.Pow(num6 - num4, 2f);
    float num7 = num5 * 100f;
    return (double) num4 > 2.0 || (double) num4 > 1.1000000238418579 && (double) num7 < 0.05000000074505806;
  }
}
