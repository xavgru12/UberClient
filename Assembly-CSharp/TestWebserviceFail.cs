// Decompiled with JetBrains decompiler
// Type: TestWebserviceFail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.WebService.Unity;
using UnityEngine;

public class TestWebserviceFail : MonoBehaviour
{
  private void OnGUI()
  {
    if (!GUI.Button(new Rect((float) (Screen.width - 200), (float) (Screen.height - 30), 200f, 30f), "Exception: " + (!Configuration.SimulateWebservicesFail ? "OFF" : "ON")))
      return;
    Configuration.SimulateWebservicesFail = !Configuration.SimulateWebservicesFail;
  }
}
