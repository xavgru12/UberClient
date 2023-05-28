// Decompiled with JetBrains decompiler
// Type: PopupHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class PopupHud : Singleton<PopupHud>
{
  private Color _doubleKillColor;
  private Color _tripleKillColor;
  private Color _quadKillColor;
  private Color _megaKillColor;
  private Color _uberKillColor;
  private Color _defaultColor;
  private MeshGUIQuad _glowBlur;
  private MeshGUIText _popupText;
  private Animatable2DGroup _popupGroup;
  private Vector2 _spawnPosition;
  private float _scaleEnlargeFactor;
  private Vector2 _doubleKillScale;
  private Vector2 _tripleKillScale;
  private Vector2 _quadKillScale;
  private Vector2 _megaKillScale;
  private Vector2 _uberKillScale;
  private Vector2 _defaultScale;
  private Vector2 _destBlurScale;
  private Vector2 _destTextScale;
  private float _displayTime;
  private float _fadeOutTime;
  private AudioClip _sound;

  private PopupHud()
  {
    this._spawnPosition = new Vector2((float) (Screen.width / 2), 200f);
    this._doubleKillColor = new Color(0.78039217f, 0.607843161f, 0.0f);
    this._tripleKillColor = new Color(0.7529412f, 0.5882353f, 0.0f);
    this._quadKillColor = new Color(0.7529412f, 0.4627451f, 0.0f);
    this._megaKillColor = new Color(0.7529412f, 0.3372549f, 0.0f);
    this._uberKillColor = new Color(0.7529412f, 0.211764708f, 0.0f);
    this._defaultColor = Color.white;
    this._scaleEnlargeFactor = 1.1f;
    this._doubleKillScale = new Vector2(0.8f, 0.8f);
    this._tripleKillScale = this._doubleKillScale * this._scaleEnlargeFactor;
    this._quadKillScale = this._tripleKillScale * this._scaleEnlargeFactor;
    this._megaKillScale = this._quadKillScale * this._scaleEnlargeFactor;
    this._uberKillScale = this._megaKillScale * this._scaleEnlargeFactor;
    this._defaultScale = this._doubleKillScale;
    this._popupGroup = new Animatable2DGroup();
  }

  public bool Enabled
  {
    get => this._popupGroup.IsVisible;
    set
    {
      if (value)
        this._popupGroup.Show();
      else
        this._popupGroup.Hide();
    }
  }

  public void Draw() => this._popupGroup.Draw(0.0f, 0.0f);

  public void PopupMultiKill(int killCount)
  {
    PopupType type = (PopupType) killCount;
    switch (type)
    {
      case PopupType.DoubleKill:
      case PopupType.TripleKill:
      case PopupType.QuadKill:
      case PopupType.MegaKill:
      case PopupType.UberKill:
        this.DoPopup(type);
        break;
    }
  }

  public void PopupRoundStart() => this.DoPopup(PopupType.RoundStart);

  public void PopupWinTeam(TeamID teamId)
  {
    PopupType type;
    switch (teamId)
    {
      case TeamID.BLUE:
        type = PopupType.BlueTeamWins;
        SfxManager.Play2dAudioClip(GameAudio.BlueWins);
        break;
      case TeamID.RED:
        type = PopupType.RedTeamWins;
        SfxManager.Play2dAudioClip(GameAudio.RedWins);
        break;
      default:
        type = PopupType.Draw;
        SfxManager.Play2dAudioClip(GameAudio.Draw);
        break;
    }
    this.DoPopup(type);
  }

  public void PopupMatchOver() => this.DoPopup(PopupType.GameOver);

  private void DoPopup(PopupType type)
  {
    this.CreateNewPopupGroup();
    this.ResetStyleAndTransform(type);
    IAnim anim = type != PopupType.RoundStart ? (IAnim) new PopupAnim(this._popupGroup, this._glowBlur, this._popupText, this._spawnPosition, this._destBlurScale, this._destTextScale, this._displayTime, this._fadeOutTime, this._sound) : (IAnim) new RoundStartAnim(this._popupGroup, this._glowBlur, this._popupText, this._spawnPosition, this._destBlurScale, this._destTextScale, this._displayTime, this._fadeOutTime, this._sound);
    if (type >= PopupType.RoundStart)
      Singleton<InGameFeatHud>.Instance.AnimationScheduler.ClearAll();
    Singleton<InGameFeatHud>.Instance.AnimationScheduler.EnqueueAnim(anim);
  }

  private void ResetStyleAndTransform(PopupType type)
  {
    Singleton<HudStyleUtility>.Instance.SetDefaultStyle(this._popupText);
    this.UpdateScales();
    this._spawnPosition = Singleton<InGameFeatHud>.Instance.AnchorPoint;
    this._displayTime = 1f;
    this._fadeOutTime = 1f;
    this._popupText.BitmapMeshText.ShadowColor = Color.white;
    this._destTextScale = this._defaultScale;
    this._glowBlur.Color = this._defaultColor;
    this._sound = (AudioClip) null;
    switch (type)
    {
      case PopupType.DoubleKill:
        this._popupText.Text = "DOUBLE KILL";
        this._popupText.BitmapMeshText.ShadowColor = this._doubleKillColor;
        this._destTextScale = this._doubleKillScale;
        this._glowBlur.Color = this._doubleKillColor;
        this._sound = GameAudio.DoubleKill;
        break;
      case PopupType.TripleKill:
        this._popupText.Text = "TRIPLE KILL";
        this._popupText.BitmapMeshText.ShadowColor = this._tripleKillColor;
        this._destTextScale = this._tripleKillScale;
        this._glowBlur.Color = this._tripleKillColor;
        this._sound = GameAudio.TripleKill;
        break;
      case PopupType.QuadKill:
        this._popupText.Text = "QUAD KILL";
        this._popupText.BitmapMeshText.ShadowColor = this._quadKillColor;
        this._destTextScale = this._quadKillScale;
        this._glowBlur.Color = this._quadKillColor;
        this._sound = GameAudio.QuadKill;
        break;
      case PopupType.MegaKill:
        this._popupText.Text = "MEGA KILL";
        this._popupText.BitmapMeshText.ShadowColor = this._megaKillColor;
        this._destTextScale = this._megaKillScale;
        this._glowBlur.Color = this._megaKillColor;
        this._sound = GameAudio.MegaKill;
        break;
      case PopupType.UberKill:
        this._popupText.Text = "UBER KILL";
        this._popupText.BitmapMeshText.ShadowColor = this._uberKillColor;
        this._destTextScale = this._uberKillScale;
        this._glowBlur.Color = this._uberKillColor;
        this._sound = GameAudio.UberKill;
        this._fadeOutTime = 5f;
        break;
      case PopupType.RoundStart:
        this._popupText.Text = "Round Starts In";
        this._displayTime = 5f;
        break;
      case PopupType.GameOver:
        this._popupText.Text = "Game Over";
        this._displayTime = 2f;
        this._spawnPosition += new Vector2(0.0f, (float) Screen.height * 0.3f);
        break;
      case PopupType.BlueTeamWins:
        this._popupText.Text = "Blue Team Wins";
        this._popupText.BitmapMeshText.ShadowColor = HudStyleUtility.DEFAULT_BLUE_COLOR;
        this._glowBlur.Color = HudStyleUtility.DEFAULT_BLUE_COLOR;
        this._displayTime = 2f;
        this._spawnPosition += new Vector2(0.0f, (float) Screen.height * 0.3f);
        break;
      case PopupType.RedTeamWins:
        this._popupText.Text = "Red Team Wins";
        this._popupText.BitmapMeshText.ShadowColor = HudStyleUtility.DEFAULT_RED_COLOR;
        this._glowBlur.Color = HudStyleUtility.DEFAULT_RED_COLOR;
        this._displayTime = 2f;
        this._spawnPosition += new Vector2(0.0f, (float) Screen.height * 0.3f);
        break;
      case PopupType.Draw:
        this._popupText.Text = "Draw!";
        this._displayTime = 2f;
        this._spawnPosition += new Vector2(0.0f, (float) Screen.height * 0.3f);
        break;
    }
    this._popupText.Scale = this._destTextScale;
    this._destBlurScale = new Vector2(this._popupText.Size.x * HudStyleUtility.BLUR_WIDTH_SCALE_FACTOR / (float) HudTextures.WhiteBlur128.width, this._popupText.Size.y * HudStyleUtility.BLUR_HEIGHT_SCALE_FACTOR / (float) HudTextures.WhiteBlur128.height);
    this._glowBlur.Scale = Vector2.zero;
    this._glowBlur.Position = this._spawnPosition;
    this._popupText.Position = this._spawnPosition;
    this._popupText.Alpha = 0.0f;
    this._popupText.Scale = Vector2.zero;
  }

  [DebuggerHidden]
  private IEnumerator EmitPopup() => (IEnumerator) new PopupHud.\u003CEmitPopup\u003Ec__Iterator4C()
  {
    \u003C\u003Ef__this = this
  };

  private void CreateNewPopupGroup()
  {
    this._popupText = new MeshGUIText(string.Empty, HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
    this._popupText.NamePrefix = "Popup";
    this._glowBlur = new MeshGUIQuad((Texture) HudTextures.WhiteBlur128);
    this._glowBlur.Name = "PopupHudGlow";
    this._popupGroup.Group.Add((IAnimatable2D) this._popupText);
    this._popupGroup.Group.Add((IAnimatable2D) this._glowBlur);
  }

  private void UpdateScales()
  {
    float num = Singleton<InGameFeatHud>.Instance.TextHeight / this._popupText.TextBounds.y;
    this._doubleKillScale = new Vector2(num, num);
    this._tripleKillScale = this._doubleKillScale * this._scaleEnlargeFactor;
    this._quadKillScale = this._tripleKillScale * this._scaleEnlargeFactor;
    this._megaKillScale = this._quadKillScale * this._scaleEnlargeFactor;
    this._uberKillScale = this._megaKillScale * this._scaleEnlargeFactor;
  }
}
