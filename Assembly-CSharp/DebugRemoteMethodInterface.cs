// Decompiled with JetBrains decompiler
// Type: DebugRemoteMethodInterface
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class DebugRemoteMethodInterface : IDebugPage
{
  private Vector2[] _scrollers = new Vector2[3];

  public string Title => "Rmi";

  public void Draw() => this.DrawRemoteMethodInterface(GameConnectionManager.Rmi, 0);

  public void DrawRemoteMethodInterface(RemoteMethodInterface rmi, int i)
  {
    GUILayout.BeginHorizontal();
    this._scrollers[i * 3] = GUILayout.BeginScrollView(this._scrollers[i * 3], GUILayout.Width(200f));
    GUILayout.Label("WAIT REG");
    foreach (object registrationJob in (IEnumerable<RemoteMethodInterface.RegistrationJob>) rmi.RegistrationJobs)
      GUILayout.Label(string.Format("{0}", registrationJob));
    GUILayout.EndScrollView();
    this._scrollers[i * 3 + 1] = GUILayout.BeginScrollView(this._scrollers[i * 3 + 1], GUILayout.Width(400f));
    GUILayout.Label("REG CLASS");
    foreach (object registeredClass in (IEnumerable<INetworkClass>) rmi.RegisteredClasses)
      GUILayout.Label(string.Format("{0}", registeredClass));
    GUILayout.EndScrollView();
    this._scrollers[i * 3 + 2] = GUILayout.BeginScrollView(this._scrollers[i * 3 + 2], GUILayout.Width(100f));
    GUILayout.Label("NET CLASS");
    foreach (int instantiatedObject in (IEnumerable<short>) rmi.NetworkInstantiatedObjects)
      GUILayout.Label(string.Format("Nid {0}", (object) (short) instantiatedObject));
    GUILayout.EndScrollView();
    GUILayout.EndHorizontal();
  }
}
