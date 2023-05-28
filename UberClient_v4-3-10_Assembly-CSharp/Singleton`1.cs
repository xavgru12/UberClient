// Decompiled with JetBrains decompiler
// Type: Singleton`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Reflection;

public class Singleton<T> : IDisposable where T : Singleton<T>
{
  private static volatile T _instance;
  private static object _lock = new object();

  public static T Instance
  {
    get
    {
      if ((object) Singleton<T>._instance == null)
      {
        lock (Singleton<T>._lock)
        {
          if ((object) Singleton<T>._instance == null)
          {
            ConstructorInfo constructor = typeof (T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, (Binder) null, new Type[0], (ParameterModifier[]) null);
            Singleton<T>._instance = constructor != null && !constructor.IsAssembly ? (T) constructor.Invoke((object[]) null) : throw new Exception(string.Format("A private or protected constructor is missing for '{0}'.", (object) typeof (T).Name));
          }
        }
      }
      return Singleton<T>._instance;
    }
  }

  public void Dispose()
  {
    this.OnDispose();
    Singleton<T>._instance = (T) null;
  }

  protected virtual void OnDispose()
  {
  }
}
