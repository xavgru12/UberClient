// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.WebServiceStarter
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using UnityEngine;

namespace UberStrike.WebService.Unity
{
  internal class WebServiceStarter : MonoBehaviour
  {
    private static MonoBehaviour mono;

    public static MonoBehaviour Mono
    {
      get
      {
        if ((Object) WebServiceStarter.mono == (Object) null)
          WebServiceStarter.mono = (MonoBehaviour) new GameObject(nameof (WebServiceStarter)).AddComponent<WebServiceStarter>();
        return WebServiceStarter.mono;
      }
    }
  }
}
