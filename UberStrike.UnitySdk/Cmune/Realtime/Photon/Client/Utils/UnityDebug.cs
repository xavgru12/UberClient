
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
