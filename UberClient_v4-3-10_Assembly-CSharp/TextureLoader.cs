// Decompiled with JetBrains decompiler
// Type: TextureLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TextureLoader : Singleton<TextureLoader>
{
  private Dictionary<string, Texture2D> _cache;
  private Dictionary<int, int> _state;
  private Texture2D _fallback;

  private TextureLoader()
  {
    this._cache = new Dictionary<string, Texture2D>();
    this._state = new Dictionary<int, int>();
    this._fallback = this.CreateDefaultTexture();
  }

  public Texture2D LoadImage(string url, Texture2D placeholder = null)
  {
    Texture2D texture;
    if (!string.IsNullOrEmpty(url))
    {
      if (!this._cache.TryGetValue(url, out texture))
      {
        texture = this.CreatePlaceholder(placeholder);
        this._cache[url] = texture;
        MonoRoutine.Start(this.DownloadTexture(url, texture));
      }
    }
    else
      texture = this._fallback;
    return texture;
  }

  private Texture2D CreateDefaultTexture() => new Texture2D(1, 1, TextureFormat.RGB24, false);

  public int GetState(Texture2D texture)
  {
    int num;
    return this._state.TryGetValue(texture.GetInstanceID(), out num) ? num : -1;
  }

  [DebuggerHidden]
  private IEnumerator DownloadTexture(string url, Texture2D texture) => (IEnumerator) new TextureLoader.\u003CDownloadTexture\u003Ec__Iterator87()
  {
    url = url,
    texture = texture,
    \u003C\u0024\u003Eurl = url,
    \u003C\u0024\u003Etexture = texture,
    \u003C\u003Ef__this = this
  };

  private Texture2D CreatePlaceholder(Texture2D placeholder = null) => !((Object) placeholder != (Object) null) ? this.CreateDefaultTexture() : Object.Instantiate((Object) placeholder) as Texture2D;
}
