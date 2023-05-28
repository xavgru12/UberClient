// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.MonoInstance
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace UberStrike.WebService.Unity
{
  internal class MonoInstance : MonoBehaviour
  {
    private static MonoBehaviour mono;

    public static MonoBehaviour Mono
    {
      get
      {
        if ((Object) MonoInstance.mono == (Object) null)
        {
          GameObject target = GameObject.Find("AutoMonoBehaviours");
          if ((Object) target == (Object) null)
            target = new GameObject("AutoMonoBehaviours");
          Object.DontDestroyOnLoad((Object) target);
          MonoInstance.mono = (MonoBehaviour) target.AddComponent<MonoInstance>();
        }
        return MonoInstance.mono;
      }
    }
  }
}
