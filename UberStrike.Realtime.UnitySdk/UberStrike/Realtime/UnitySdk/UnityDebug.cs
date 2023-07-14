// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.UnityDebug
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;
using UnityEngine;

namespace UberStrike.Realtime.UnitySdk
{
  public class UnityDebug : ICmuneDebug
  {
    public void Log(int level, string message)
    {
      DateTime now = DateTime.Now;
      string shortDateString = now.ToShortDateString();
      now = DateTime.Now;
      string shortTimeString = now.ToShortTimeString();
      string str = message;
      string message1 = string.Format("[{0} {1}] {2}", (object) shortDateString, (object) shortTimeString, (object) str);
      switch (level)
      {
        case 0:
          Debug.Log((object) message1);
          break;
        case 1:
          Debug.LogWarning((object) message1);
          break;
        case 2:
          Debug.LogError((object) message1);
          break;
        default:
          Debug.Log((object) message1);
          break;
      }
    }
  }
}
