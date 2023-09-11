// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Photon.Client.Utils.UnityDebug
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Util;
using System;
using UnityEngine;

namespace Cmune.Realtime.Photon.Client.Utils
{
  public class UnityDebug : ICmuneDebug
  {
    public void Log(int level, string message)
    {
      string message1 = string.Format("[{0} {1}] {2}", (object) DateTime.Now.ToShortDateString(), (object) DateTime.Now.ToShortTimeString(), (object) message);
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
