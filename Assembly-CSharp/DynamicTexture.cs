// Decompiled with JetBrains decompiler
// Type: DynamicTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DynamicTexture
{
  private Texture2D _texture;
  private string _url;
  private DynamicTexture.State _state;
  private float _alpha;

  public DynamicTexture(string url, bool loadNow = false)
  {
    this._url = url;
    this._texture = new Texture2D(1, 1);
    if (!loadNow)
      return;
    this._state = DynamicTexture.State.Loading;
    this._texture = Singleton<TextureLoader>.Instance.LoadImage(this._url);
  }

  public Texture2D Texture => this._texture;

  public bool IsDone => this._state == DynamicTexture.State.Success;

  public float Aspect => (Object) this._texture != (Object) null ? (float) this._texture.height / (float) this._texture.width : 1f;

  public void Draw(Rect rect, bool forceAlpha = false)
  {
    switch (this._state)
    {
      case DynamicTexture.State.None:
        this._state = DynamicTexture.State.Loading;
        this._texture = Singleton<TextureLoader>.Instance.LoadImage(this._url);
        break;
      case DynamicTexture.State.Loading:
        switch (Singleton<TextureLoader>.Instance.GetState(this._texture) + 1)
        {
          case 0:
            this._state = DynamicTexture.State.Failed;
            break;
          case 2:
            this._state = DynamicTexture.State.Success;
            break;
          case 3:
            this._state = DynamicTexture.State.Failed;
            break;
        }
        WaitingTexture.Draw(rect.center);
        break;
      case DynamicTexture.State.Failed:
        GUI.Label(rect, "N/A", BlueStonez.label_ingamechat);
        break;
      case DynamicTexture.State.Success:
        Color color = GUI.color;
        this._alpha = Mathf.Lerp(this._alpha, 1f, Time.deltaTime);
        float a = !GUI.enabled ? Mathf.Min(this._alpha, 0.5f) : this._alpha;
        if (!forceAlpha)
          GUI.color = new Color(1f, 1f, 1f, a);
        GUI.DrawTexture(rect, (UnityEngine.Texture) this._texture);
        GUI.color = color;
        break;
    }
  }

  private enum State
  {
    None,
    Loading,
    Failed,
    Success,
  }
}
