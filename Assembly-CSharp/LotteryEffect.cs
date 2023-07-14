// Decompiled with JetBrains decompiler
// Type: LotteryEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LotteryEffect : MonoBehaviour
{
  private const float MAX_DURATION = 2f;
  private const float FADE_TIME = 1.5f;
  private float _time;
  private float _alpha = 1f;
  private float _cameraAlpha;
  private RenderTexture _renderTexture;
  [SerializeField]
  private Camera _renderCamera;

  private void Awake()
  {
    if (!(bool) (Object) this._renderCamera)
      return;
    this._renderTexture = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
    this._renderCamera.targetTexture = this._renderTexture;
    this._cameraAlpha = this._renderCamera.backgroundColor.a;
  }

  private void Update()
  {
    if ((double) this._time > 1.5)
    {
      this._alpha = Mathf.Clamp01(this._alpha - Time.deltaTime);
      this._renderCamera.backgroundColor.SetAlpha(Mathf.Min(this._cameraAlpha, this._alpha));
    }
    if ((double) this._time > 2.0)
    {
      Object.Destroy((Object) this.gameObject);
      if ((bool) (Object) this._renderTexture)
        this._renderTexture.Release();
    }
    this._time += Time.deltaTime;
  }

  private void OnGUI()
  {
    if (!(bool) (Object) this._renderTexture)
      return;
    GUI.depth = -1;
    GUI.color = new Color(1f, 1f, 1f, this._alpha);
    GUI.DrawTexture(new Rect((float) ((Screen.width - this._renderTexture.width) / 2), (float) ((Screen.height - this._renderTexture.height) / 2), (float) this._renderTexture.width, (float) this._renderTexture.height), (Texture) this._renderTexture);
    GUI.color = Color.white;
  }
}
