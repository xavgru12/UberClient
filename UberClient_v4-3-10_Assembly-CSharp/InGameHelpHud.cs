// Decompiled with JetBrains decompiler
// Type: InGameHelpHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class InGameHelpHud : Singleton<InGameHelpHud>
{
  private float _overlayBoxHeight;
  private float _curTextScaleFactor;
  private MeshGUIQuad _scoreBoradKeyIcon;
  private MeshGUIText _scoreBoardKey;
  private Animatable2DGroup _scoreBoardKeyGroup;
  private MeshGUIText _scoreBoardHelpText;
  private Sprite2DGUI _scoreBoardOverlay;
  private Animatable2DGroup _scoreBoardHelpGroup;
  private MeshGUIText _fullscreenKey1;
  private Sprite2DGUI _fullscreenOverlay1;
  private MeshGUIText _fullscreenKey2;
  private Sprite2DGUI _fullscreenOverlay2;
  private MeshGUIText _fullscreenKeyPlus;
  private MeshGUIText _fullscreenHelpText;
  private Animatable2DGroup _fullScreenHelpGroup;
  private MeshGUIText _changeTeamKey1;
  private Sprite2DGUI _changeTeamOverlay1;
  private MeshGUIText _changeTeamKey2;
  private Sprite2DGUI _changeTeamOverlay2;
  private MeshGUIText _changeTeamKeyPlus;
  private MeshGUIText _changeTeamHelpText;
  private Animatable2DGroup _changeTeamHelpGroup;
  private Sprite2DButton _loadoutKeyIcon;
  private MeshGUIText _loadoutKey;
  private MeshGUIText _loadoutHelpText;
  private Sprite2DGUI _loadoutOverlay;
  private Animatable2DGroup _loadoutHelpGroup;
  private Animatable2DGroup _entireGroup;

  private InGameHelpHud()
  {
    this.InitScoreBoardHelpGroup();
    this.InitFullScreenHelpGroup();
    this.InitChangeTeamHelpGroup();
    this.InitLoadoutHelpGroup();
    this._entireGroup = new Animatable2DGroup();
    this._entireGroup.Group.Add((IAnimatable2D) this._scoreBoardHelpGroup);
    this._entireGroup.Group.Add((IAnimatable2D) this._fullScreenHelpGroup);
    this._entireGroup.Group.Add((IAnimatable2D) this._changeTeamHelpGroup);
    this._entireGroup.Group.Add((IAnimatable2D) this._loadoutHelpGroup);
    this.ResetHud();
    this.Enabled = false;
    CmuneEventHandler.AddListener<OnSetPlayerTeamEvent>(new Action<OnSetPlayerTeamEvent>(this.OnTeamChange));
    CmuneEventHandler.AddListener<ScreenResolutionEvent>(new Action<ScreenResolutionEvent>(this.OnScreenResolutionChange));
    CmuneEventHandler.AddListener<CameraWidthChangeEvent>(new Action<CameraWidthChangeEvent>(this.OnCameraRectChange));
  }

  public bool Enabled
  {
    get => this._entireGroup.IsVisible;
    set
    {
      if (value)
      {
        this._entireGroup.Show();
        if (this.EnableChangeTeamHelp)
          this._changeTeamHelpGroup.Show();
        else
          this._changeTeamHelpGroup.Hide();
        this.ResetTransform();
      }
      else
        this._entireGroup.Hide();
    }
  }

  public bool EnableChangeTeamHelp { get; set; }

  public void Draw() => this._entireGroup.Draw(0.0f, 0.0f);

  public void Update()
  {
    if (!Input.GetKeyDown(KeyCode.L))
      return;
    this.OnToggleLoadout();
  }

  private void InitScoreBoardHelpGroup()
  {
    this._scoreBoradKeyIcon = new MeshGUIQuad((Texture) HudTextures.DeathScreenTab);
    this._scoreBoardKey = new MeshGUIText("Tab", HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleLeft);
    this._scoreBoardKeyGroup = new Animatable2DGroup();
    this._scoreBoardHelpText = new MeshGUIText("Scoreboard", HudAssets.Instance.InterparkBitmapFont);
    this._scoreBoardOverlay = new Sprite2DGUI(new GUIContent(), StormFront.BlueBox);
    this._scoreBoardHelpGroup = new Animatable2DGroup();
    this._scoreBoardKeyGroup.Group.Add((IAnimatable2D) this._scoreBoardKey);
    this._scoreBoardKeyGroup.Group.Add((IAnimatable2D) this._scoreBoradKeyIcon);
    this._scoreBoardHelpGroup.Group.Add((IAnimatable2D) this._scoreBoardKeyGroup);
    this._scoreBoardHelpGroup.Group.Add((IAnimatable2D) this._scoreBoardHelpText);
    this._scoreBoardHelpGroup.Group.Add((IAnimatable2D) this._scoreBoardOverlay);
    this._scoreBoardHelpGroup.Group.Add((IAnimatable2D) this._scoreBoradKeyIcon);
  }

  private void InitFullScreenHelpGroup()
  {
    this._fullscreenKey1 = new MeshGUIText("ALT", HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
    this._fullscreenOverlay1 = new Sprite2DGUI(new GUIContent(), StormFront.BlueBox);
    this._fullscreenKey2 = new MeshGUIText("F", HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
    this._fullscreenOverlay2 = new Sprite2DGUI(new GUIContent(), StormFront.BlueBox);
    this._fullscreenKeyPlus = new MeshGUIText("+", HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
    this._fullscreenHelpText = new MeshGUIText("Fullscreen", HudAssets.Instance.InterparkBitmapFont);
    this._fullScreenHelpGroup = new Animatable2DGroup();
    this._fullScreenHelpGroup.Group.Add((IAnimatable2D) this._fullscreenKey1);
    this._fullScreenHelpGroup.Group.Add((IAnimatable2D) this._fullscreenOverlay1);
    this._fullScreenHelpGroup.Group.Add((IAnimatable2D) this._fullscreenKey2);
    this._fullScreenHelpGroup.Group.Add((IAnimatable2D) this._fullscreenOverlay2);
    this._fullScreenHelpGroup.Group.Add((IAnimatable2D) this._fullscreenKeyPlus);
    this._fullScreenHelpGroup.Group.Add((IAnimatable2D) this._fullscreenHelpText);
  }

  private void InitChangeTeamHelpGroup()
  {
    this._changeTeamKey1 = new MeshGUIText("ALT", HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
    this._changeTeamOverlay1 = new Sprite2DGUI(new GUIContent(), StormFront.BlueBox);
    this._changeTeamKey2 = new MeshGUIText("M", HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
    this._changeTeamOverlay2 = new Sprite2DGUI(new GUIContent(), StormFront.BlueBox);
    this._changeTeamKeyPlus = new MeshGUIText("+", HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
    this._changeTeamHelpText = new MeshGUIText("Change Team", HudAssets.Instance.InterparkBitmapFont);
    this._changeTeamHelpGroup = new Animatable2DGroup();
    this._changeTeamHelpGroup.Group.Add((IAnimatable2D) this._changeTeamKey1);
    this._changeTeamHelpGroup.Group.Add((IAnimatable2D) this._changeTeamOverlay1);
    this._changeTeamHelpGroup.Group.Add((IAnimatable2D) this._changeTeamKey2);
    this._changeTeamHelpGroup.Group.Add((IAnimatable2D) this._changeTeamOverlay2);
    this._changeTeamHelpGroup.Group.Add((IAnimatable2D) this._changeTeamKeyPlus);
    this._changeTeamHelpGroup.Group.Add((IAnimatable2D) this._changeTeamHelpText);
  }

  private void InitLoadoutHelpGroup()
  {
    this._loadoutKey = new MeshGUIText("L", HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
    this._loadoutOverlay = new Sprite2DGUI(new GUIContent(), StormFront.BlueBox);
    this._loadoutHelpText = new MeshGUIText("loadout", HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
    this._loadoutKeyIcon = new Sprite2DButton(GUIContent.none, StormFront.ButtonLoadout);
    this._loadoutKeyIcon.IsUsingGuiContentBounds = false;
    this._loadoutKeyIcon.GUIBounds = new Vector2(64f, 64f);
    this._loadoutKeyIcon.OnClick = new Action(this.OnToggleLoadout);
    this._loadoutHelpGroup = new Animatable2DGroup();
    this._loadoutHelpGroup.Group.Add((IAnimatable2D) this._loadoutKey);
    this._loadoutHelpGroup.Group.Add((IAnimatable2D) this._loadoutOverlay);
    this._loadoutHelpGroup.Group.Add((IAnimatable2D) this._loadoutHelpText);
    this._loadoutHelpGroup.Group.Add((IAnimatable2D) this._loadoutKeyIcon);
  }

  private void OnToggleLoadout()
  {
    if (GamePageManager.IsCurrentPage(PageType.None))
      this.EnterShop();
    else
      this.LeaveShop();
  }

  private void ResetHud()
  {
    this.ResetStyle();
    this.ResetTransform();
  }

  private void ResetStyle()
  {
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(this._scoreBoardKey);
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(this._scoreBoardHelpText);
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(this._fullscreenKey1);
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(this._fullscreenKey2);
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(this._fullscreenKeyPlus);
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(this._fullscreenHelpText);
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(this._changeTeamKey1);
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(this._changeTeamKey2);
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(this._changeTeamKeyPlus);
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(this._changeTeamHelpText);
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(this._loadoutHelpText);
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(this._loadoutKey);
  }

  private void OnTeamChange(OnSetPlayerTeamEvent ev)
  {
    if (ev.TeamId == TeamID.RED)
    {
      this._scoreBoardOverlay.Style = StormFront.RedBox;
      this._fullscreenOverlay1.Style = StormFront.RedBox;
      this._fullscreenOverlay2.Style = StormFront.RedBox;
      this._changeTeamOverlay1.Style = StormFront.RedBox;
      this._changeTeamOverlay2.Style = StormFront.RedBox;
      this._loadoutOverlay.Style = StormFront.RedBox;
      this._loadoutKeyIcon.Style = StormFront.ButtonLoadoutRed;
    }
    else
    {
      this._scoreBoardOverlay.Style = StormFront.BlueBox;
      this._fullscreenOverlay1.Style = StormFront.BlueBox;
      this._fullscreenOverlay2.Style = StormFront.BlueBox;
      this._changeTeamOverlay1.Style = StormFront.BlueBox;
      this._changeTeamOverlay2.Style = StormFront.BlueBox;
      this._loadoutOverlay.Style = StormFront.BlueBox;
      this._loadoutKeyIcon.Style = StormFront.ButtonLoadout;
    }
  }

  private void OnCameraRectChange(CameraWidthChangeEvent ev) => this.ResetTransform();

  private void OnScreenResolutionChange(ScreenResolutionEvent ev) => this.ResetTransform();

  private void ResetTransform()
  {
    this._curTextScaleFactor = 0.1f;
    this._overlayBoxHeight = (float) Screen.height * 0.06f;
    this.ResetScoreBoardHelpGroup();
    this.ResetFullscreenHelpGroup();
    this.ResetChangeTeamHelpGroup();
    this.ResetLoadoutHelpGroup();
    this._scoreBoardHelpGroup.Position = new Vector2(0.0f, 0.0f);
    float num1 = 0.0f;
    float num2 = this._overlayBoxHeight * 0.4f;
    float y1 = num1 + (this._scoreBoardHelpGroup.Rect.height + num2);
    this._fullScreenHelpGroup.Position = new Vector2(0.0f, y1);
    float y2 = y1 + (this._fullScreenHelpGroup.Rect.height + num2);
    if (this.EnableChangeTeamHelp)
    {
      this._changeTeamHelpGroup.Position = new Vector2(0.0f, y2);
      y2 += this._changeTeamHelpGroup.Rect.height + num2;
    }
    this._loadoutHelpGroup.Position = new Vector2(0.0f, y2);
    Vector2 vector2 = new Vector2((float) Screen.width * 0.05f, (float) Screen.height * 0.2f);
    vector2.x += (float) ((double) Screen.width * (1.0 - (double) Singleton<CameraRectController>.Instance.Width) / 2.0);
    this._entireGroup.Position = vector2;
    this._entireGroup.UpdateMeshGUIPosition();
  }

  private void ResetScoreBoardHelpGroup()
  {
    Rect rect = new Rect(0.0f, 0.0f, (float) Screen.width * 0.08f, this._overlayBoxHeight);
    this._scoreBoardOverlay.Position = new Vector2(0.0f, 0.0f);
    this._scoreBoardOverlay.Scale = new Vector2(rect.width / this._scoreBoardOverlay.GUIBounds.x, rect.height / this._scoreBoardOverlay.GUIBounds.y);
    this._scoreBoardKey.Scale = new Vector2(this._curTextScaleFactor * 2f, this._curTextScaleFactor * 2f);
    this._scoreBoardKey.Position = new Vector2(rect.width * 0.08f, rect.height * 0.5f);
    float num = this._scoreBoardKey.Size.x / (float) this._scoreBoradKeyIcon.Texture.width;
    this._scoreBoradKeyIcon.Scale = new Vector2(num, num);
    this._scoreBoradKeyIcon.Position = new Vector2(rect.width * 0.92f - this._scoreBoradKeyIcon.Size.x, (float) ((double) rect.height / 2.0 - (double) this._scoreBoradKeyIcon.Size.y / 2.0));
    this._scoreBoardHelpText.Scale = new Vector2(this._curTextScaleFactor * 1.5f, this._curTextScaleFactor * 1.5f);
    this._scoreBoardHelpText.Position = new Vector2(0.0f, rect.height * 1.05f);
  }

  private void ResetFullscreenHelpGroup()
  {
    Rect rect = new Rect(0.0f, 0.0f, this._overlayBoxHeight, this._overlayBoxHeight);
    this._fullscreenOverlay1.Position = new Vector2(rect.x, rect.y);
    this._fullscreenOverlay1.Scale = new Vector2(rect.width / this._fullscreenOverlay1.GUIBounds.x, rect.height / this._fullscreenOverlay1.GUIBounds.y);
    this._fullscreenKey1.Scale = new Vector2(this._curTextScaleFactor * 2f, this._curTextScaleFactor * 2f);
    this._fullscreenKey1.Position = new Vector2(rect.width / 2f, rect.height / 2f);
    this._fullscreenKeyPlus.Scale = new Vector2(this._curTextScaleFactor * 2f, this._curTextScaleFactor * 2f);
    this._fullscreenKeyPlus.Position = new Vector2(rect.width * 1.25f, rect.height / 2f);
    rect = new Rect(this._overlayBoxHeight * 1.5f, 0.0f, this._overlayBoxHeight, this._overlayBoxHeight);
    this._fullscreenOverlay2.Position = new Vector2(rect.x, rect.y);
    this._fullscreenOverlay2.Scale = new Vector2(rect.width / this._fullscreenOverlay2.GUIBounds.x, rect.height / this._fullscreenOverlay2.GUIBounds.y);
    this._fullscreenKey2.Scale = new Vector2(this._curTextScaleFactor * 2f, this._curTextScaleFactor * 2f);
    this._fullscreenKey2.Position = new Vector2(rect.x + rect.width / 2f, rect.y + rect.height / 2f);
    this._fullscreenHelpText.Scale = new Vector2(this._curTextScaleFactor * 1.5f, this._curTextScaleFactor * 1.5f);
    this._fullscreenHelpText.Position = new Vector2(0.0f, rect.height * 1.05f);
  }

  private void ResetChangeTeamHelpGroup()
  {
    Rect rect = new Rect(0.0f, 0.0f, this._overlayBoxHeight, this._overlayBoxHeight);
    this._changeTeamOverlay1.Position = new Vector2(rect.x, rect.y);
    this._changeTeamOverlay1.Scale = new Vector2(rect.width / this._changeTeamOverlay1.GUIBounds.x, rect.height / this._changeTeamOverlay1.GUIBounds.y);
    this._changeTeamKey1.Scale = new Vector2(this._curTextScaleFactor * 2f, this._curTextScaleFactor * 2f);
    this._changeTeamKey1.Position = new Vector2(rect.width / 2f, rect.height / 2f);
    this._changeTeamKeyPlus.Scale = new Vector2(this._curTextScaleFactor * 2f, this._curTextScaleFactor * 2f);
    this._changeTeamKeyPlus.Position = new Vector2(rect.width * 1.25f, rect.height / 2f);
    rect = new Rect(this._overlayBoxHeight * 1.5f, 0.0f, this._overlayBoxHeight, this._overlayBoxHeight);
    this._changeTeamOverlay2.Position = new Vector2(rect.x, rect.y);
    this._changeTeamOverlay2.Scale = new Vector2(rect.width / this._changeTeamOverlay2.GUIBounds.x, rect.height / this._changeTeamOverlay2.GUIBounds.y);
    this._changeTeamKey2.Scale = new Vector2(this._curTextScaleFactor * 2f, this._curTextScaleFactor * 2f);
    this._changeTeamKey2.Position = new Vector2(rect.x + rect.width / 2f, rect.y + rect.height / 2f);
    this._changeTeamHelpText.Scale = new Vector2(this._curTextScaleFactor * 1.5f, this._curTextScaleFactor * 1.5f);
    this._changeTeamHelpText.Position = new Vector2(0.0f, rect.height * 1.05f);
  }

  private void ResetLoadoutHelpGroup()
  {
    Rect rect = new Rect(0.0f, 0.0f, (float) Screen.width * 0.08f, this._overlayBoxHeight * 3f);
    this._loadoutOverlay.Position = new Vector2(0.0f, 0.0f);
    this._loadoutOverlay.Scale = new Vector2(rect.width / this._loadoutOverlay.GUIBounds.x, rect.height / this._loadoutOverlay.GUIBounds.y);
    this._loadoutKey.Scale = new Vector2(this._curTextScaleFactor * 3f, this._curTextScaleFactor * 3f);
    this._loadoutKey.Position = new Vector2(rect.width / 2f, rect.height * 0.2f);
    this._loadoutKeyIcon.Scale = new Vector2(1f, 1f);
    this._loadoutKeyIcon.Position = new Vector2((float) ((double) rect.width / 2.0 - (double) this._loadoutKeyIcon.Size.x / 2.0), (float) ((double) rect.height / 2.0 - (double) this._loadoutKeyIcon.Size.y / 2.0));
    this._loadoutHelpText.Scale = new Vector2(this._curTextScaleFactor * 2.7f, this._curTextScaleFactor * 2.7f);
    this._loadoutHelpText.Position = new Vector2(rect.width / 2f, rect.height * 0.85f);
  }

  private void EnterShop()
  {
    GameState.LocalPlayer.Pause();
    GamePageManager.Instance.LoadPage(PageType.Shop);
  }

  private void LeaveShop() => GamePageManager.Instance.UnloadCurrentPage();
}
