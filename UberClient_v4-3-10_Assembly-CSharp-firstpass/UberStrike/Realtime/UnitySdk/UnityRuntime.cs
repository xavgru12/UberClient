// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.UnityRuntime
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace UberStrike.Realtime.UnitySdk
{
  internal class UnityRuntime : MonoBehaviour
  {
    [SerializeField]
    private bool showInvocationList;
    private static UnityRuntime instance;
    private Action onFixedUpdate;
    private Action onUpdate;
    private Action onShutdown;

    public static UnityRuntime Instance
    {
      get
      {
        if ((UnityEngine.Object) UnityRuntime.instance == (UnityEngine.Object) null)
        {
          GameObject gameObject = GameObject.Find("AutoMonoBehaviours");
          if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
            gameObject = new GameObject("AutoMonoBehaviours");
          UnityRuntime.instance = gameObject.AddComponent<UnityRuntime>();
        }
        return UnityRuntime.instance;
      }
    }

    public event Action OnFixedUpdate
    {
      add => this.onFixedUpdate += value;
      remove => this.onFixedUpdate -= value;
    }

    public event Action OnUpdate
    {
      add => this.onUpdate += value;
      remove => this.onUpdate -= value;
    }

    public event Action OnShutdown
    {
      add => this.onShutdown += value;
      remove => this.onShutdown -= value;
    }

    private void OnGUI()
    {
      if (!this.showInvocationList)
        return;
      GUILayout.BeginArea(new Rect(10f, 100f, 400f, (float) (Screen.height - 200)));
      if (this.onUpdate != null)
      {
        foreach (Delegate invocation in this.onUpdate.GetInvocationList())
          GUILayout.Label("Update: " + invocation.Method.DeclaringType.Name + "." + invocation.Method.Name);
      }
      if (this.onFixedUpdate != null)
      {
        foreach (Delegate invocation in this.onFixedUpdate.GetInvocationList())
          GUILayout.Label("FixedUpdate: " + invocation.Method.DeclaringType.Name + "." + invocation.Method.Name);
      }
      if (this.onShutdown != null)
      {
        foreach (Delegate invocation in this.onShutdown.GetInvocationList())
          GUILayout.Label("OnApplicationQuit: " + invocation.Method.DeclaringType.Name + "." + invocation.Method.Name);
      }
      GUILayout.EndArea();
    }

    private void Update()
    {
      if (this.onUpdate == null)
        return;
      this.onUpdate();
    }

    private void FixedUpdate()
    {
      if (this.onFixedUpdate == null)
        return;
      this.onFixedUpdate();
    }

    private void OnApplicationQuit()
    {
      if (this.onShutdown == null)
        return;
      this.onShutdown();
    }
  }
}
