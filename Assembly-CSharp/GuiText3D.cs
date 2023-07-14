// Decompiled with JetBrains decompiler
// Type: GuiText3D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GuiText3D : MonoBehaviour
{
  public Font mFont;
  public string mText;
  public Camera mCamera;
  public Transform mTarget;
  public float mMaxDistance = 20f;
  public float mLifeTime = 5f;
  public Color mColor = Color.black;
  public bool mFadeOut = true;
  public Vector3 mFadeDirection = (Vector3) Vector2.up;
  private GUIText _guiText;
  private Transform _transform;
  private Material _material;
  private Vector3 _viewportPosition;
  private float time;
  private Vector3 fadeDir = Vector3.zero;
  private Color startColor;
  private Color finalColor;

  private void Awake() => this._transform = this.transform;

  private void Start()
  {
    this._guiText = this.gameObject.AddComponent(typeof (GUIText)) as GUIText;
    this._guiText.alignment = TextAlignment.Center;
    this._guiText.anchor = TextAnchor.MiddleCenter;
    if ((Object) this.mCamera == (Object) null || (Object) this.mTarget == (Object) null || (Object) this.mFont == (Object) null)
    {
      Object.Destroy((Object) this.gameObject);
    }
    else
    {
      this._guiText.font = this.mFont;
      this._guiText.text = this.mText;
      this._guiText.material = this.mFont.material;
      this._material = this._guiText.material;
      this.startColor = this._material.color;
      this.finalColor = this._material.color;
      if (!this.mFadeOut)
        return;
      this.finalColor.a = 0.0f;
    }
  }

  private void LateUpdate()
  {
    if ((Object) this.mCamera != (Object) null && (Object) this.mTarget != (Object) null && ((double) this.mLifeTime < 0.0 || (double) this.mLifeTime > (double) this.time))
    {
      this.time += Time.deltaTime;
      this._viewportPosition = this.mCamera.WorldToViewportPoint(this.mTarget.localPosition);
      this._material.color = !this.mFadeOut || (double) this.mLifeTime <= 0.0 ? Color.Lerp(this.startColor, this.finalColor, Mathf.Clamp01(this._viewportPosition.z / this.mMaxDistance)) : Color.Lerp(this.startColor, this.finalColor, this.time / this.mLifeTime);
      this.fadeDir += Time.deltaTime * this.mFadeDirection;
      this._transform.localPosition = this._viewportPosition + this.fadeDir;
    }
    else
      Object.Destroy((Object) this.gameObject);
  }

  [DebuggerHidden]
  private IEnumerator startShowGuiText(float mLifeTime) => (IEnumerator) new GuiText3D.\u003CstartShowGuiText\u003Ec__IteratorF()
  {
    mLifeTime = mLifeTime,
    \u003C\u0024\u003EmLifeTime = mLifeTime,
    \u003C\u003Ef__this = this
  };
}
