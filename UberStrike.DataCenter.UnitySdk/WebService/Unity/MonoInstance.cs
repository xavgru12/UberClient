// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.MonoInstance
// Assembly: UberStrike.DataCenter.UnitySdk, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 1925C691-C9DE-44B0-95F4-3171E7957DDD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.DataCenter.UnitySdk.dll

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
          GameObject gameObject = GameObject.Find("AutoMonoBehaviours");
          if ((Object) gameObject == (Object) null)
            gameObject = new GameObject("AutoMonoBehaviours");
          MonoInstance.mono = (MonoBehaviour) gameObject.AddComponent<MonoInstance>();
        }
        return MonoInstance.mono;
      }
    }
  }
}
