// Decompiled with JetBrains decompiler
// Type: AutoMonoBehaviour`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class AutoMonoBehaviour<T> : MonoBehaviour where T : class
{
  public const string GameObjectName = "AutoMonoBehaviours";
  private static T _instance;
  private static GameObject _parent;
  private static bool _isRunning = true;
  private static bool _isInstantiating;

  private static GameObject Parent
  {
    get
    {
      AutoMonoBehaviour<T>._parent = GameObject.Find("AutoMonoBehaviours");
      if ((UnityEngine.Object) AutoMonoBehaviour<T>._parent == (UnityEngine.Object) null)
      {
        AutoMonoBehaviour<T>._parent = new GameObject("AutoMonoBehaviours");
        UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) AutoMonoBehaviour<T>._parent);
      }
      return AutoMonoBehaviour<T>._parent;
    }
  }

  private void OnApplicationQuit() => AutoMonoBehaviour<T>._isRunning = false;

  private void Start()
  {
    if ((object) AutoMonoBehaviour<T>._instance == null)
      throw new Exception("The script " + typeof (T).Name + " is self instantiating and shouldn't be attached manually to a GameObject.");
  }

  public static T Instance
  {
    get
    {
      if ((object) AutoMonoBehaviour<T>._instance == null && AutoMonoBehaviour<T>._isRunning)
      {
        AutoMonoBehaviour<T>._isInstantiating = !AutoMonoBehaviour<T>._isInstantiating ? true : throw new Exception("Recursive calls to Constuctor of AutoMonoBehaviour! Check your " + (object) typeof (T) + ":Awake() function for calls to " + (object) typeof (T) + ".Instance");
        AutoMonoBehaviour<T>._instance = (object) AutoMonoBehaviour<T>.Parent.AddComponent(typeof (T)) as T;
      }
      return AutoMonoBehaviour<T>._instance;
    }
  }
}
