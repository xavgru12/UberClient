// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.MonoInstance
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using UnityEngine;

namespace UberStrike.Realtime.UnitySdk
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
