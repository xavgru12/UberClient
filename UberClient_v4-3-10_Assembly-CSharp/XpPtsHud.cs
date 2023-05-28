// Decompiled with JetBrains decompiler
// Type: XpPtsHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class XpPtsHud
{
  private float _screenYOffset;
  private MeshGUIText _xpDigits;
  private MeshGUIText _ptsDigits;
  private MeshGUIText _xpText;
  private MeshGUIText _ptsText;
  private MeshGUIText _curLevelText;
  private MeshGUIText _nextLevelText;
  private MeshGUIQuad _xpBarEmptySprite;
  private MeshGUIQuad _xpBarFullSprite;
  private MeshGUIQuad _glowBlur;
  private Animatable2DGroup _xpGroup;
  private Animatable2DGroup _ptsGroup;
  private Animatable2DGroup _textGroup;
  private Animatable2DGroup _xpBarGroup;
  private Animatable2DGroup _entireGroup;
  private Vector2 _oriBlurPos;
  private Vector2 _oriBlurScale;
  private Vector2 _oriXpBarPos;
  private Vector2 _oriXpBarEmptyScale;
  private Vector2 _oriXpBarFillScale;
  private int _curLevel;
  private int _curXp;
  private int _curPts;
  private float _curXpPercentage;
  private Vector2 _groupPosition;
  public Vector2 ScreenPosition = new Vector2(0.5f, 0.97f);
  public float ScaleFactor = 0.65f;
  private Vector2 _xpPtsSpawnPoint;
  private Vector2 _xpPtsDiePoint;
  private AnimationScheduler _animScheduler;
  private bool _isOnScreen;
  private float _translationDistance;
  private float _xpBarHideTime;
  private bool _isTemporaryDisplay;
  private float _curScreenWidth;
  private float _curScreenHeight;

  public XpPtsHud()
  {
    TextAnchor textAnchor = TextAnchor.UpperLeft;
    this._xpDigits = new MeshGUIText(this._curXp.ToString(), HudAssets.Instance.InterparkBitmapFont, textAnchor);
    this._xpDigits.NamePrefix = "XP";
    this._xpText = new MeshGUIText("XP", HudAssets.Instance.HelveticaBitmapFont, textAnchor);
    this._ptsDigits = new MeshGUIText(this._curPts.ToString(), HudAssets.Instance.InterparkBitmapFont, textAnchor);
    this._ptsDigits.NamePrefix = "PTS";
    this._ptsText = new MeshGUIText("PTS", HudAssets.Instance.HelveticaBitmapFont, textAnchor);
    this._xpBarEmptySprite = new MeshGUIQuad((Texture) HudTextures.XPBarEmptyBlue);
    this._xpBarFullSprite = new MeshGUIQuad((Texture) HudTextures.XPBarFull);
    this._curLevelText = new MeshGUIText("Lvl 5", HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleRight);
    this._nextLevelText = new MeshGUIText("Lvl 6", HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleLeft);
    this._glowBlur = new MeshGUIQuad((Texture) HudTextures.WhiteBlur128);
    this._glowBlur.Color = HudStyleUtility.GLOW_BLUR_BLUE_COLOR;
    this._glowBlur.Name = "XpPtsHudGlow";
    this._glowBlur.Depth = 1f;
    this._xpGroup = new Animatable2DGroup();
    this._ptsGroup = new Animatable2DGroup();
    this._textGroup = new Animatable2DGroup();
    this._xpBarGroup = new Animatable2DGroup();
    this._entireGroup = new Animatable2DGroup();
    this._xpGroup.Group.Add((IAnimatable2D) this._xpDigits);
    this._xpGroup.Group.Add((IAnimatable2D) this._xpText);
    this._ptsGroup.Group.Add((IAnimatable2D) this._ptsDigits);
    this._ptsGroup.Group.Add((IAnimatable2D) this._ptsText);
    this._textGroup.Group.Add((IAnimatable2D) this._xpGroup);
    this._textGroup.Group.Add((IAnimatable2D) this._ptsGroup);
    this._xpBarGroup.Group.Add((IAnimatable2D) this._curLevelText);
    this._xpBarGroup.Group.Add((IAnimatable2D) this._nextLevelText);
    this._xpBarGroup.Group.Add((IAnimatable2D) this._xpBarEmptySprite);
    this._xpBarGroup.Group.Add((IAnimatable2D) this._xpBarFullSprite);
    this._entireGroup.Group.Add((IAnimatable2D) this._glowBlur);
    this._entireGroup.Group.Add((IAnimatable2D) this._textGroup);
    this._entireGroup.Group.Add((IAnimatable2D) this._xpBarGroup);
    this._animScheduler = new AnimationScheduler();
    this._translationDistance = 100f;
    this._isOnScreen = false;
    this.ResetHud();
    this.Enabled = false;
  }

  public bool Enabled
  {
    get => this._entireGroup.IsVisible;
    set
    {
      if (value)
      {
        this._entireGroup.Show();
        if (this.IsXpPtsTextVisible)
          this._textGroup.Show();
        else
          this._textGroup.Hide();
        if (this.IsNextLevelVisible)
          this._nextLevelText.Show();
        else
          this._nextLevelText.Hide();
      }
      else
        this._entireGroup.Hide();
    }
  }

  public bool IsXpPtsTextVisible { get; set; }

  public bool IsNextLevelVisible { get; set; }

  public void OnGameStart()
  {
    this.IsXpPtsTextVisible = true;
    this.PopdownOffScreen();
    this.ResetXp();
  }

  public void ResetXp()
  {
    this.SetXp(0);
    this.SetPts(0);
    this.TotalXpOnGameStart = PlayerDataManager.PlayerExperienceSecure;
    this.CurrentLevel = PlayerDataManager.PlayerLevelSecure;
  }

  public void GainXp(int gainXp)
  {
    if (gainXp == 0)
      return;
    this._animScheduler.AddAnimation((IAnimatable2D) this.GenerateXp(gainXp), new AnimationScheduler.DoAnim(this.EmitSprite), (string[]) null, new AnimationScheduler.OnAnimOver(this.OnGetXP), new string[1]
    {
      gainXp.ToString()
    });
    SfxManager.Play2dAudioClip(GameAudio.GetXP);
  }

  public void GainPoints(int gainPts)
  {
    if (gainPts == 0)
      return;
    this._animScheduler.AddAnimation((IAnimatable2D) this.GeneratePts(gainPts), new AnimationScheduler.DoAnim(this.EmitSprite), (string[]) null, new AnimationScheduler.OnAnimOver(this.OnGetPts), new string[1]
    {
      gainPts.ToString()
    });
    SfxManager.Play2dAudioClip(GameAudio.GetPoints);
  }

  public void Update()
  {
    if (this._isOnScreen && this._isTemporaryDisplay && (double) Time.time > (double) this._xpBarHideTime)
      this.PopdownOffScreen();
    this._animScheduler.Draw();
  }

  public void Draw() => this._entireGroup.Draw(0.0f, 0.0f);

  public void DisplayPermanently()
  {
    this._isTemporaryDisplay = false;
    this._textGroup.Hide();
    this._screenYOffset = 50f;
    this._entireGroup.MoveTo(this._groupPosition, 0.0f, startDelay: 0.0f);
    this._isOnScreen = true;
  }

  public void PopupTemporarily()
  {
    this._isTemporaryDisplay = true;
    this._screenYOffset = 0.0f;
    this._entireGroup.MoveTo(this._groupPosition, 0.1f, EaseType.InOut, 0.0f);
    this._isOnScreen = true;
    this._xpBarHideTime = Time.time + 3f;
  }

  public void PopdownOffScreen(float animTime = 0.1f)
  {
    this._entireGroup.MoveTo(new Vector2(this._groupPosition.x, this._groupPosition.y + this._translationDistance), animTime, EaseType.InOut, 0.0f);
    this._isOnScreen = false;
  }

  private void ResetHud()
  {
    this.OnScreenResolutionChange(new ScreenResolutionEvent());
    this.OnTeamChange(new OnSetPlayerTeamEvent()
    {
      TeamId = TeamID.NONE
    });
    this.ResetTransform();
  }

  public void OnTeamChange(OnSetPlayerTeamEvent ev)
  {
    switch (ev.TeamId)
    {
      case TeamID.NONE:
      case TeamID.BLUE:
        Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._xpDigits);
        Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._ptsDigits);
        Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._xpText);
        Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._ptsText);
        Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._curLevelText);
        Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._nextLevelText);
        this._xpBarEmptySprite.Texture = (Texture) HudTextures.XPBarEmptyBlue;
        this._glowBlur.Color = HudStyleUtility.GLOW_BLUR_BLUE_COLOR;
        break;
      case TeamID.RED:
        Singleton<HudStyleUtility>.Instance.SetRedStyle(this._xpDigits);
        Singleton<HudStyleUtility>.Instance.SetRedStyle(this._ptsDigits);
        Singleton<HudStyleUtility>.Instance.SetRedStyle(this._xpText);
        Singleton<HudStyleUtility>.Instance.SetRedStyle(this._ptsText);
        Singleton<HudStyleUtility>.Instance.SetRedStyle(this._curLevelText);
        Singleton<HudStyleUtility>.Instance.SetRedStyle(this._nextLevelText);
        this._xpBarEmptySprite.Texture = (Texture) HudTextures.XPBarEmptyRed;
        this._glowBlur.Color = HudStyleUtility.GLOW_BLUR_RED_COLOR;
        break;
    }
    this._xpText.BitmapMeshText.AlphaMin = 0.4f;
    this._ptsText.BitmapMeshText.AlphaMin = 0.4f;
  }

  public void OnScreenResolutionChange(ScreenResolutionEvent ev)
  {
    this._curScreenWidth = (float) Screen.width;
    this._curScreenHeight = (float) Screen.height;
    this.ResetTransform();
  }

  private Animatable2DGroup GenerateXp(int xp)
  {
    float num1 = 0.3f;
    float num2 = 10f;
    Animatable2DGroup xp1 = new Animatable2DGroup();
    BitmapFont interparkBitmapFont = HudAssets.Instance.InterparkBitmapFont;
    TextAnchor textAnchor = TextAnchor.UpperLeft;
    MeshGUIText meshText3D1 = new MeshGUIText(xp <= 0 ? string.Empty : "+", interparkBitmapFont, textAnchor);
    MeshGUIText meshText3D2 = new MeshGUIText(xp.ToString() + "XP", interparkBitmapFont, textAnchor);
    Singleton<HudStyleUtility>.Instance.SetTeamStyle(meshText3D1);
    Singleton<HudStyleUtility>.Instance.SetTeamStyle(meshText3D2);
    xp1.Group.Add((IAnimatable2D) meshText3D1);
    xp1.Group.Add((IAnimatable2D) meshText3D2);
    meshText3D1.Position = new Vector2(0.0f, 0.0f);
    meshText3D1.Scale = new Vector2(num1, num1);
    meshText3D2.Position = new Vector2(meshText3D1.Size.x + num2, 0.0f);
    meshText3D2.Scale = new Vector2(num1, num1);
    xp1.Position = new Vector2(this._xpPtsSpawnPoint.x - xp1.GetRect().width / 2f, this._xpPtsSpawnPoint.y);
    xp1.UpdateMeshGUIPosition();
    return xp1;
  }

  private Animatable2DGroup GeneratePts(int pts)
  {
    float num1 = 0.3f;
    float num2 = 10f;
    Animatable2DGroup pts1 = new Animatable2DGroup();
    BitmapFont interparkBitmapFont = HudAssets.Instance.InterparkBitmapFont;
    TextAnchor textAnchor = TextAnchor.UpperLeft;
    MeshGUIText meshText3D1 = new MeshGUIText(pts <= 0 ? string.Empty : "+", interparkBitmapFont, textAnchor);
    MeshGUIText meshText3D2 = new MeshGUIText(pts.ToString() + "PTS", interparkBitmapFont, textAnchor);
    Singleton<HudStyleUtility>.Instance.SetTeamStyle(meshText3D1);
    Singleton<HudStyleUtility>.Instance.SetTeamStyle(meshText3D2);
    pts1.Group.Add((IAnimatable2D) meshText3D1);
    pts1.Group.Add((IAnimatable2D) meshText3D2);
    meshText3D1.Position = new Vector2(0.0f, 0.0f);
    meshText3D1.Scale = new Vector2(num1, num1);
    meshText3D2.Position = new Vector2(meshText3D1.Size.x + num2, 0.0f);
    meshText3D2.Scale = new Vector2(num1, num1);
    pts1.Position = new Vector2(this._xpPtsSpawnPoint.x - pts1.GetRect().width / 2f, this._xpPtsSpawnPoint.y);
    pts1.UpdateMeshGUIPosition();
    return pts1;
  }

  [DebuggerHidden]
  private IEnumerator OnGetXP(IAnimatable2D animatable, string[] args) => (IEnumerator) new XpPtsHud.\u003COnGetXP\u003Ec__Iterator4F()
  {
    args = args,
    animatable = animatable,
    \u003C\u0024\u003Eargs = args,
    \u003C\u0024\u003Eanimatable = animatable,
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator OnGetPts(IAnimatable2D animatable, string[] args) => (IEnumerator) new XpPtsHud.\u003COnGetPts\u003Ec__Iterator50()
  {
    args = args,
    animatable = animatable,
    \u003C\u0024\u003Eargs = args,
    \u003C\u0024\u003Eanimatable = animatable,
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator EmitSprite(IAnimatable2D animatable, string[] args) => (IEnumerator) new XpPtsHud.\u003CEmitSprite\u003Ec__Iterator51()
  {
    animatable = animatable,
    \u003C\u0024\u003Eanimatable = animatable,
    \u003C\u003Ef__this = this
  };

  private void ResetStyle()
  {
    Singleton<HudStyleUtility>.Instance.SetTeamStyle(this._xpDigits);
    Singleton<HudStyleUtility>.Instance.SetTeamStyle(this._ptsDigits);
    Singleton<HudStyleUtility>.Instance.SetTeamStyle(this._xpText);
    Singleton<HudStyleUtility>.Instance.SetTeamStyle(this._ptsText);
    this._xpText.BitmapMeshText.AlphaMin = 0.4f;
    this._ptsText.BitmapMeshText.AlphaMin = 0.4f;
  }

  public void ResetTransform()
  {
    this.ResetXpPtsTransform();
    this.ResetXpBarTransform();
    if (this.CurrentLevel != 0)
      this.ResetLevelTxtTransform();
    this.ResetBlurTransform();
    this.ResetGroupTransform();
    this._xpPtsSpawnPoint = new Vector2(this._curScreenWidth / 2f, this._curScreenHeight / 2f);
    this._xpPtsDiePoint = new Vector2(this._curScreenWidth * this.ScreenPosition.x, this._curScreenHeight * this.ScreenPosition.y);
  }

  private void ResetXpPtsTransform()
  {
    this._xpDigits.Text = this._curXp.ToString();
    this._xpDigits.Position = new Vector2(0.0f, 0.0f);
    this._xpDigits.Scale = new Vector2(0.7f * this.ScaleFactor, 0.7f * this.ScaleFactor);
    float num = 3f;
    float x1 = (float) (0.0 + ((double) this._xpDigits.Size.x + (double) num));
    this._xpText.Scale = new Vector2(0.35f * this.ScaleFactor, 0.35f * this.ScaleFactor);
    this._xpText.Position = new Vector2(x1, this._xpDigits.Size.y - this._xpText.Size.y * 1.2f);
    float x2 = x1 + (this._xpText.Size.x + num);
    this._ptsDigits.Text = this._curPts.ToString();
    this._ptsDigits.Scale = new Vector2(0.7f * this.ScaleFactor, 0.7f * this.ScaleFactor);
    this._ptsDigits.Position = new Vector2(x2, 0.0f);
    float x3 = x2 + (this._ptsDigits.Size.x + num);
    this._ptsText.Scale = new Vector2(0.35f * this.ScaleFactor, 0.35f * this.ScaleFactor);
    this._ptsText.Position = new Vector2(x3, this._xpText.Position.y);
  }

  private void ResetXpBarTransform()
  {
    float num1 = (float) Screen.width * HudStyleUtility.XP_BAR_WIDTH_PROPORTION_IN_SCREEN;
    this._oriXpBarEmptyScale.x = this._oriXpBarEmptyScale.y = num1 / (float) HudTextures.XPBarEmptyBlue.width;
    this._oriXpBarFillScale.y = this._oriXpBarEmptyScale.y;
    this._oriXpBarFillScale.x = this._oriXpBarFillScale.y * this._curXpPercentage;
    this._oriXpBarPos.x = (float) (-((double) num1 - (double) this._textGroup.Rect.width) / 2.0);
    this._oriXpBarPos.y = this._xpDigits.Size.y;
    float num2 = 8.5f;
    this._xpBarEmptySprite.Scale = this._oriXpBarEmptyScale;
    this._xpBarFullSprite.Scale = this._oriXpBarFillScale;
    this._xpBarEmptySprite.Position = this._oriXpBarPos;
    this._xpBarFullSprite.Position = this._oriXpBarPos;
    this._xpBarFullSprite.Scale = new Vector2((float) ((double) this._xpBarFullSprite.Scale.y * (512.0 - (double) num2 * 2.0) / 512.0) * this._curXpPercentage, this._xpBarFullSprite.Scale.y * 0.95f);
    Vector2 position = this._xpBarEmptySprite.Position;
    position.x += num2 * this._xpBarEmptySprite.Scale.x;
    position.y += num2 * this._xpBarEmptySprite.Scale.x;
    this._xpBarFullSprite.Position = position;
  }

  private void ResetLevelTxtTransform()
  {
    float num = 0.5f;
    this._curLevelText.Text = "Lvl " + (object) this.CurrentLevel;
    this._curLevelText.Position = new Vector2(this._xpBarEmptySprite.Position.x - 20f, this._xpBarEmptySprite.Position.y);
    this._curLevelText.Scale = new Vector2(num * this.ScaleFactor, num * this.ScaleFactor);
    if (this.CurrentLevel < PlayerXpUtil.MaxPlayerLevel)
    {
      this.IsNextLevelVisible = true;
      this._nextLevelText.Text = "Lvl " + (object) (this.CurrentLevel + 1);
      this._nextLevelText.Position = new Vector2((float) ((double) this._xpBarEmptySprite.Position.x + (double) this._xpBarEmptySprite.Rect.width + 20.0), this._xpBarEmptySprite.Position.y);
      this._nextLevelText.Scale = new Vector2(num * this.ScaleFactor, num * this.ScaleFactor);
    }
    else
      this.IsNextLevelVisible = false;
  }

  private void ResetBlurTransform()
  {
    float num1 = this._xpBarEmptySprite.Rect.width * HudStyleUtility.BLUR_WIDTH_SCALE_FACTOR;
    float num2 = this._xpBarGroup.Rect.height * HudStyleUtility.BLUR_HEIGHT_SCALE_FACTOR;
    this._oriBlurScale.x = num1 / (float) HudTextures.WhiteBlur128.width;
    this._oriBlurScale.y = num2 / (float) HudTextures.WhiteBlur128.height;
    this._oriBlurPos.x = (float) (((double) this._textGroup.Rect.width - (double) num1) / 2.0);
    this._oriBlurPos.y = (float) (((double) this._textGroup.Rect.height - (double) num2) / 2.0);
    if (!this.IsXpPtsTextVisible)
      this._oriBlurPos.y += num2 * 0.1f;
    this._glowBlur.Scale = this._oriBlurScale;
    this._glowBlur.Position = this._oriBlurPos;
  }

  private void ResetGroupTransform()
  {
    this._translationDistance = (float) (((double) this._entireGroup.Rect.height + (double) this._screenYOffset) * 1.5);
    this._groupPosition = new Vector2((float) ((double) this._curScreenWidth * (double) this.ScreenPosition.x - (double) this._textGroup.Rect.width / 2.0), this._curScreenHeight * this.ScreenPosition.y - this._textGroup.Rect.height - this._xpBarEmptySprite.Rect.height - this._screenYOffset);
    if (!this._isOnScreen)
      this._entireGroup.Position = new Vector2(this._groupPosition.x, this._groupPosition.y + this._translationDistance);
    else
      this._entireGroup.Position = this._groupPosition;
  }

  [DebuggerHidden]
  private IEnumerator OnXpIncrease() => (IEnumerator) new XpPtsHud.\u003COnXpIncrease\u003Ec__Iterator52()
  {
    \u003C\u003Ef__this = this
  };

  private void UpdateXpPercentage()
  {
    if (this.CurrentLevel == PlayerXpUtil.MaxPlayerLevel)
    {
      this.SetXpPercentage(1f);
    }
    else
    {
      if (this.TotalXpOnGameStart + this._curXp > this.CurrentLevelMaxXp)
        ++this.CurrentLevel;
      int num = this.CurrentLevelMaxXp - this.CurrentLevelMinXp;
      if (num == 0)
        return;
      this.SetXpPercentage((float) (this.TotalXpOnGameStart + this._curXp - this.CurrentLevelMinXp) / (float) num);
    }
  }

  private void SetXp(int xp)
  {
    bool flag = xp > this._curXp;
    this._curXp = xp >= 0 ? xp : 0;
    this.UpdateXpPercentage();
    this.ResetTransform();
    if (!flag)
      return;
    MonoRoutine.Start(this.OnXpIncrease());
  }

  private void SetPts(int pts)
  {
    this._curPts = pts >= 0 ? pts : 0;
    this.ResetTransform();
  }

  private void SetXpPercentage(float xpPercentage)
  {
    this._curXpPercentage = Mathf.Clamp01(xpPercentage);
    this.ResetXpBarTransform();
  }

  private int CurrentLevel
  {
    get => this._curLevel;
    set
    {
      this._curLevel = Mathf.Clamp(value, 1, PlayerXpUtil.MaxPlayerLevel);
      int minXp;
      int maxXp;
      PlayerXpUtil.GetXpRangeForLevel(this._curLevel, out minXp, out maxXp);
      this.CurrentLevelMinXp = minXp;
      this.CurrentLevelMaxXp = maxXp;
      this.UpdateXpPercentage();
    }
  }

  private int CurrentLevelMinXp { get; set; }

  private int CurrentLevelMaxXp { get; set; }

  private int TotalXpOnGameStart { get; set; }
}
