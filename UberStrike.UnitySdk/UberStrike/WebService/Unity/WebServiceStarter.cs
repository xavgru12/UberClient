
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
