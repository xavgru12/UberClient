// Decompiled with JetBrains decompiler
// Type: SceneLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class SceneLoader : Singleton<SceneLoader>
{
  private const float FadeTime = 1f;
  private const float MinLoadingTime = 1f;
  private Texture2D _blackTexture;
  private Color _color;

  private SceneLoader() => this._blackTexture = new Texture2D(1, 1, TextureFormat.RGB24, false);

  public string CurrentScene { get; private set; }

  public bool IsLoading { get; private set; }

  public Coroutine LoadLevel(string level, Action<string> onError = null, Action<float> progress = null)
  {
    if ((UnityEngine.Object) GameState.LocalDecorator != (UnityEngine.Object) null)
      GameState.LocalDecorator.transform.parent = (Transform) null;
    return MonoRoutine.Start(this.LoadLevelAsync(level, onError, progress));
  }

  [DebuggerHidden]
  private IEnumerator LoadLevelAsync(string level, Action<string> onError = null, Action<float> progress = null) => (IEnumerator) new SceneLoader.\u003CLoadLevelAsync\u003Ec__Iterator80()
  {
    level = level,
    progress = progress,
    onError = onError,
    \u003C\u0024\u003Elevel = level,
    \u003C\u0024\u003Eprogress = progress,
    \u003C\u0024\u003EonError = onError,
    \u003C\u003Ef__this = this
  };

  private void OnGUI()
  {
    GUI.depth = 8;
    GUI.color = this._color;
    GUI.DrawTexture(new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height), (Texture) this._blackTexture);
    GUI.color = Color.white;
  }
}
