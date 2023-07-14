// Decompiled with JetBrains decompiler
// Type: LevelUpHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class LevelUpHud : Singleton<LevelUpHud>
{
  private MeshGUIText _levelUpText;
  private MeshGUIQuad _glowBlur;
  private Animatable2DGroup _entireGroup;
  private Vector2 _spawnPosition;
  private Vector2 _destBlurScale;
  private Vector2 _destTextScale;
  private Color _color;

  private LevelUpHud()
  {
    this._color = new Color(0.917647064f, 0.78039217f, 0.458823532f);
    this._levelUpText = new MeshGUIText("LEVEL UP", HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
    this._glowBlur = new MeshGUIQuad((Texture) HudTextures.WhiteBlur128);
    this._glowBlur.Name = "LevelUpGlow";
    this._glowBlur.Color = this._color;
    this._glowBlur.Depth = 1f;
    this._entireGroup = new Animatable2DGroup();
    this._entireGroup.Group.Add((IAnimatable2D) this._levelUpText);
    this._entireGroup.Group.Add((IAnimatable2D) this._glowBlur);
    this.ResetHud();
    this.Enabled = true;
  }

  public bool Enabled
  {
    get => this._entireGroup.IsVisible;
    set
    {
      if (value)
        this._entireGroup.Show();
      else
        this._entireGroup.Hide();
    }
  }

  public void Draw() => this._entireGroup.Draw(0.0f, 0.0f);

  public void PopupLevelUp()
  {
    this.ResetTransform();
    MonoRoutine.Start(this.EmitLevelUp());
  }

  private void ResetHud()
  {
    this.ResetStyle();
    this.ResetTransform();
  }

  private void ResetStyle()
  {
    Singleton<HudStyleUtility>.Instance.SetDefaultStyle(this._levelUpText);
    this._levelUpText.BitmapMeshText.ShadowColor = this._color;
  }

  private void ResetTransform()
  {
    this._spawnPosition = new Vector2((float) (Screen.width / 2), (float) (Screen.height / 2) - 40f);
    this.ResetTextTransform();
    this.ResetBlurTransform();
    this._levelUpText.Scale = Vector2.zero;
  }

  private void ResetTextTransform()
  {
    this._destTextScale = new Vector2(1.5f, 1.5f);
    this._levelUpText.Position = this._spawnPosition;
    this._levelUpText.Scale = this._destTextScale;
    this._levelUpText.Alpha = 0.0f;
  }

  private void ResetBlurTransform()
  {
    this._destBlurScale = new Vector2(this._levelUpText.Size.x * HudStyleUtility.BLUR_WIDTH_SCALE_FACTOR / (float) HudTextures.WhiteBlur128.width, this._levelUpText.Size.y * HudStyleUtility.BLUR_HEIGHT_SCALE_FACTOR / (float) HudTextures.WhiteBlur128.height);
    this._glowBlur.Position = this._spawnPosition;
    this._glowBlur.Scale = Vector2.zero;
    this._glowBlur.Alpha = 0.0f;
  }

  [DebuggerHidden]
  private IEnumerator EmitLevelUp() => (IEnumerator) new LevelUpHud.\u003CEmitLevelUp\u003Ec__Iterator4A()
  {
    \u003C\u003Ef__this = this
  };
}
