// Decompiled with JetBrains decompiler
// Type: GameModeObjectiveHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class GameModeObjectiveHud : Singleton<GameModeObjectiveHud>
{
  private MeshGUIText _gameModeText;
  private MeshGUITextFormat _objectiveText;
  private Animatable2DGroup _entireGroup;
  private float _textScale;
  private float _textHideTime;
  private float _textDisplayTime;
  private float _fadeInOutAnimTime;
  private bool _isDisplaying;
  private static string GAME_MODE_TITLE_SURFIX = "_";
  private static string OBJECTIVE_PREFIX = "> ";

  private GameModeObjectiveHud()
  {
    this._entireGroup = new Animatable2DGroup();
    this._gameModeText = new MeshGUIText("GameModeTitle", HudAssets.Instance.InterparkBitmapFont);
    this._objectiveText = new MeshGUITextFormat(string.Empty, HudAssets.Instance.InterparkBitmapFont);
    this._objectiveText.SetStyleFunc = new MeshGUITextFormat.SetStyle(this.SetMeshTextStyle);
    this._entireGroup.Group.Add((IAnimatable2D) this._gameModeText);
    this._entireGroup.Group.Add((IAnimatable2D) this._objectiveText);
    this._textDisplayTime = 10f;
    this._fadeInOutAnimTime = 0.5f;
    this.ResetHud();
    this.Enabled = true;
    CmuneEventHandler.AddListener<OnSetPlayerTeamEvent>(new Action<OnSetPlayerTeamEvent>(this.OnTeamChange));
    CmuneEventHandler.AddListener<ScreenResolutionEvent>(new Action<ScreenResolutionEvent>(this.OnScreenResolutionChange));
    CmuneEventHandler.AddListener<OnGlobalUIRibbonChanged>(new Action<OnGlobalUIRibbonChanged>(this.OnGlobalUIRibbonChange));
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

  public void Update()
  {
    if (!this._isDisplaying || (double) Time.time <= (double) this._textHideTime)
      return;
    this._entireGroup.Flicker(this._fadeInOutAnimTime, 0.02f);
    this._entireGroup.FadeAlphaTo(0.0f, this._fadeInOutAnimTime, EaseType.In);
    this._isDisplaying = false;
  }

  public void Draw() => this._entireGroup.Draw(0.0f, 0.0f);

  public void Clear()
  {
    this._gameModeText.Text = string.Empty;
    this._objectiveText.FormatText = string.Empty;
  }

  public void DisplayGameMode(GameMode gameMode)
  {
    this.SetGameModeText(gameMode);
    this.FadeInGameModeText();
  }

  private void SetGameModeText(GameMode gameMode)
  {
    switch (gameMode)
    {
      case GameMode.TeamDeathMatch:
        this._gameModeText.Text = "Team Deathmatch" + GameModeObjectiveHud.GAME_MODE_TITLE_SURFIX;
        this._objectiveText.FormatText = GameModeObjectiveHud.OBJECTIVE_PREFIX + "Your team must get more kills than the opposing team \n before the time runs out.";
        break;
      case GameMode.DeathMatch:
        this._gameModeText.Text = "Deathmatch" + GameModeObjectiveHud.GAME_MODE_TITLE_SURFIX;
        this._objectiveText.FormatText = GameModeObjectiveHud.OBJECTIVE_PREFIX + "Get as many kills as you can before the time runs out.";
        break;
      case GameMode.TeamElimination:
        this._gameModeText.Text = "Elimination" + GameModeObjectiveHud.GAME_MODE_TITLE_SURFIX;
        this._objectiveText.FormatText = GameModeObjectiveHud.OBJECTIVE_PREFIX + "Kill all members of the opposing team to win the round.";
        break;
    }
    this.ResetStyle();
  }

  private void FadeInGameModeText()
  {
    this._entireGroup.Flicker(this._fadeInOutAnimTime, 0.02f);
    this._entireGroup.FadeAlphaTo(1f, this._fadeInOutAnimTime, EaseType.In);
    this._isDisplaying = true;
    this._textHideTime = Time.time + this._textDisplayTime;
  }

  private void ResetHud()
  {
    this.ResetStyle();
    this.ResetTransform();
  }

  private void ResetStyle() => this.SetMeshTextStyle(this._gameModeText);

  private void OnTeamChange(OnSetPlayerTeamEvent ev)
  {
    this.SetMeshTextStyle(this._gameModeText);
    this._objectiveText.UpdateStyle();
  }

  private void OnScreenResolutionChange(ScreenResolutionEvent ev) => this.ResetTransform();

  private void OnGlobalUIRibbonChange(OnGlobalUIRibbonChanged ev) => this.ResetTransform();

  private void SetMeshTextStyle(MeshGUIText meshText)
  {
    Singleton<HudStyleUtility>.Instance.SetTeamStyle(meshText);
    meshText.Alpha = 0.0f;
  }

  private void ResetTransform()
  {
    this._textScale = 0.454999983f;
    this._gameModeText.Scale = new Vector2(this._textScale, this._textScale);
    this._gameModeText.Position = Vector2.zero;
    this._objectiveText.Scale = new Vector2(this._textScale, this._textScale);
    this._objectiveText.Position = new Vector2(50f, (float) Screen.height * 0.08f);
    float num = 0.0f;
    if (GlobalUIRibbon.Instance.IsEnabled)
      num = 50f;
    this._entireGroup.Position = new Vector2((float) Screen.width * 0.05f, (float) Screen.height * 0.02f + num);
  }
}
