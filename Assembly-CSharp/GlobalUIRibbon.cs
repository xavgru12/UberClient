// Decompiled with JetBrains decompiler
// Type: GlobalUIRibbon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.DataCenter.Common.Entities;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class GlobalUIRibbon : MonoBehaviour
{
  private const int NEWSFEED_HEIGHT = 0;
  private const int PAGETABS_HEIGHT = 0;
  private const int STATUSBAR_HEIGHT = 44;
  private const int ButtonY = 0;
  private int _height = 44;
  private GuiDropDown _optionsDropdown;
  private bool _isReady;
  private int _maxXPforThisLevel;
  private int _minXPforThisLevel;
  private float _yOffset;
  private float _alphaIcons;
  private Rect _pageGroupRect;
  private Rect _pageToggleRect;
  private Dictionary<GlobalUIRibbon.EventType, GlobalUIRibbon.RibbonEvent> _ribbonEvents;

  public static GlobalUIRibbon Instance { get; private set; }

  public bool IsEnabled => this.enabled;

  public static bool IsVisible { get; set; }

  public int Height() => (int) this._yOffset + this._height - 1;

  private float CreditsAlpha
  {
    get
    {
      GlobalUIRibbon.RibbonEvent ribbonEvent;
      return this._ribbonEvents.TryGetValue(GlobalUIRibbon.EventType.CreditEvent, out ribbonEvent) ? ribbonEvent.Alpha : 1f;
    }
  }

  private int CreditsValue
  {
    get
    {
      GlobalUIRibbon.RibbonEvent ribbonEvent;
      return this._ribbonEvents.TryGetValue(GlobalUIRibbon.EventType.CreditEvent, out ribbonEvent) ? ribbonEvent.Value : PlayerDataManager.Credits;
    }
  }

  private float PointsAlpha
  {
    get
    {
      GlobalUIRibbon.RibbonEvent ribbonEvent;
      return this._ribbonEvents.TryGetValue(GlobalUIRibbon.EventType.PointEvent, out ribbonEvent) ? ribbonEvent.Alpha : 1f;
    }
  }

  private int PointsValue
  {
    get
    {
      GlobalUIRibbon.RibbonEvent ribbonEvent;
      return this._ribbonEvents.TryGetValue(GlobalUIRibbon.EventType.PointEvent, out ribbonEvent) ? ribbonEvent.Value : PlayerDataManager.Points;
    }
  }

  private float XpAlpha
  {
    get
    {
      GlobalUIRibbon.RibbonEvent ribbonEvent;
      return this._ribbonEvents.TryGetValue(GlobalUIRibbon.EventType.XpEvent, out ribbonEvent) ? ribbonEvent.Alpha : 1f;
    }
  }

  private int XpValue
  {
    get
    {
      GlobalUIRibbon.RibbonEvent ribbonEvent;
      return this._ribbonEvents.TryGetValue(GlobalUIRibbon.EventType.XpEvent, out ribbonEvent) ? ribbonEvent.Value : PlayerDataManager.PlayerExperience;
    }
  }

  private void Awake()
  {
    GlobalUIRibbon.Instance = this;
    this._ribbonEvents = new Dictionary<GlobalUIRibbon.EventType, GlobalUIRibbon.RibbonEvent>();
  }

  private void Start()
  {
    this._yOffset = (float) -this._height;
    this.InitOptionsDropdown();
    this.enabled = false;
    this._isReady = true;
  }

  private void InitOptionsDropdown()
  {
    this._optionsDropdown = new GuiDropDown();
    this._optionsDropdown.Caption = new GUIContent((Texture) GlobalUiIcons.QuadpanelButtonOptions);
    this._optionsDropdown.Add(new GUIContent(" " + LocalizedStrings.Help, (Texture) GlobalUiIcons.QuadpanelButtonHelp), (Action) (() => PanelManager.Instance.OpenPanel(PanelType.Help)));
    this._optionsDropdown.Add(new GUIContent(" " + LocalizedStrings.Options, (Texture) GlobalUiIcons.QuadpanelButtonOptions), (Action) (() => PanelManager.Instance.OpenPanel(PanelType.Options)));
    this._optionsDropdown.Add(new GUIContent(" " + LocalizedStrings.Audio, (Texture) GlobalUiIcons.QuadpanelButtonSoundoff), new GUIContent(" " + LocalizedStrings.Audio, (Texture) GlobalUiIcons.QuadpanelButtonSoundon), (Func<bool>) (() => ApplicationDataManager.ApplicationOptions.AudioEnabled), (Action) (() =>
    {
      ApplicationDataManager.ApplicationOptions.AudioEnabled = !ApplicationDataManager.ApplicationOptions.AudioEnabled;
      AutoMonoBehaviour<SfxManager>.Instance.EnableAudio(ApplicationDataManager.ApplicationOptions.AudioEnabled);
      ApplicationDataManager.ApplicationOptions.SaveApplicationOptions();
    }));
    if (!ApplicationDataManager.IsMobile)
      this._optionsDropdown.Add(new GUIContent(" " + LocalizedStrings.Windowed, (Texture) GlobalUiIcons.QuadpanelButtonNormalize), new GUIContent(" " + LocalizedStrings.Fullscreen, (Texture) GlobalUiIcons.QuadpanelButtonFullscreen), (Func<bool>) (() => Screen.fullScreen), (Action) (() => ScreenResolutionManager.IsFullScreen = !Screen.fullScreen));
    else
      this._optionsDropdown.Add(new GUIContent(" " + LocalizedStrings.Logout, (Texture) GlobalUiIcons.QuadpanelButtonLogout), (Action) (() => PopupSystem.ShowMessage("Logout", "Are you sure you wish to logout?", PopupSystem.AlertType.OKCancel, new Action(Singleton<AuthenticationManager>.Instance.StartLogout))));
    CmuneEventHandler.AddListener<LoginEvent>((Action<LoginEvent>) (ev =>
    {
      if (ev.AccessLevel <= MemberAccessLevel.Default)
        return;
      this._optionsDropdown.Add(new GUIContent(" " + LocalizedStrings.Moderate, (Texture) GlobalUiIcons.QuadpanelButtonModerate), (Action) (() =>
      {
        if (PlayerDataManager.AccessLevelSecure <= MemberAccessLevel.Default)
          return;
        PanelManager.Instance.OpenPanel(PanelType.Moderation);
      }));
    }));
  }

  private void Update()
  {
    if (!this._isReady)
      return;
    if (this._ribbonEvents.Count > 0)
    {
      foreach (int num in Enum.GetValues(typeof (GlobalUIRibbon.EventType)))
      {
        GlobalUIRibbon.EventType key = (GlobalUIRibbon.EventType) num;
        GlobalUIRibbon.RibbonEvent ribbonEvent;
        if (this._ribbonEvents.TryGetValue(key, out ribbonEvent))
        {
          if (ribbonEvent.IsDone())
            this._ribbonEvents.Remove(key);
          else
            ribbonEvent.Animate();
        }
      }
    }
    this._yOffset = (double) this._yOffset >= 0.0 ? 0.0f : Mathf.Lerp(this._yOffset, 0.1f, Time.deltaTime * 8f);
    PlayerXpUtil.GetXpRangeForLevel(PlayerDataManager.PlayerLevel, out this._minXPforThisLevel, out this._maxXPforThisLevel);
    this._alphaIcons = Mathf.Clamp01((float) (0.5 + (double) Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup * 2f)) * 0.5));
  }

  private void OnGUI()
  {
    if (!this._isReady)
      return;
    GUI.depth = 7;
    GUI.Label(new Rect(0.0f, this._yOffset, (float) Screen.width, 44f), GUIContent.none, BlueStonez.window_standard_grey38);
    this.DoMenuBar(new Rect(0.0f, this._yOffset, (float) Screen.width, 44f));
    if (MenuPageManager.Instance.GetCurrentPage() == PageType.Home || MenuPageManager.Instance.GetCurrentPage() != PageType.Shop)
      ;
    if (this._ribbonEvents.Count > 0)
    {
      foreach (int key in Enum.GetValues(typeof (GlobalUIRibbon.EventType)))
      {
        GlobalUIRibbon.RibbonEvent ribbonEvent;
        if (this._ribbonEvents.TryGetValue((GlobalUIRibbon.EventType) key, out ribbonEvent))
          ribbonEvent.Draw();
      }
    }
    GuiManager.DrawTooltip();
  }

  private void DoMenuBar(Rect rect)
  {
    GUI.enabled = !Singleton<SceneLoader>.Instance.IsLoading;
    if ((UnityEngine.Object) MenuPageManager.Instance != (UnityEngine.Object) null && MenuPageManager.Instance.GetCurrentPage() != PageType.Home)
    {
      if (GUITools.Button(new Rect(rect.x + 9f, rect.y + 6f, 100f, 32f), new GUIContent("Back"), BlueStonez.button_white))
        MenuPageManager.Instance.LoadPage(PageType.Home);
    }
    else if (GameState.HasCurrentGame && GUI.Button(new Rect(rect.x + 9f, rect.y + 6f, 100f, 32f), "Back", BlueStonez.button_white))
    {
      if (GameState.CurrentGame.IsGameStarted)
        PopupSystem.ShowMessage(LocalizedStrings.LeavingGame, LocalizedStrings.LeaveGameWarningMsg, PopupSystem.AlertType.OKCancel, (Action) (() => Singleton<GameStateController>.Instance.LeaveGame()), LocalizedStrings.LeaveCaps, (Action) null, LocalizedStrings.CancelCaps, PopupSystem.ActionType.Negative);
      else
        Singleton<GameStateController>.Instance.LeaveGame();
    }
    int num = 0;
    if (ApplicationDataManager.IsMobile)
      num = 44;
    if (!GameState.HasCurrentGame || GamePageManager.Instance.HasPage)
    {
      Rect position1 = new Rect(rect.width - 420f + (float) num, 12f, 100f, 20f);
      GUIContent content1 = new GUIContent(this.PointsValue.ToString("N0"), (Texture) ShopIcons.IconPoints20x20);
      GUI.color = new Color(1f, 1f, 1f, this.PointsAlpha);
      GUI.Label(position1, content1, BlueStonez.label_interparkbold_13pt);
      Rect position2 = new Rect(rect.width - 310f + (float) num, 12f, 100f, 20f);
      GUIContent content2 = new GUIContent(this.CreditsValue.ToString("N0"), (Texture) ShopIcons.IconCredits20x20);
      GUI.color = new Color(1f, 1f, 1f, this.CreditsAlpha);
      GUI.Label(position2, content2, BlueStonez.label_interparkbold_13pt);
      GUI.color = Color.white;
      if (GUITools.Button(new Rect(rect.width - 200f + (float) num, rect.y + 9f, 100f, 26f), new GUIContent("Get Credits", LocalizedStrings.ClickHereBuyCreditsMsg), BlueStonez.buttongold_medium))
        ApplicationDataManager.OpenBuyCredits();
    }
    if (!ApplicationDataManager.IsMobile && GUI.Button(new Rect((float) (Screen.width - 88), this._yOffset, 44f, 44f), !Screen.fullScreen ? new GUIContent(string.Empty, (Texture) GlobalUiIcons.QuadpanelButtonFullscreen, "Enter Fullscreen mode.") : new GUIContent(string.Empty, (Texture) GlobalUiIcons.QuadpanelButtonNormalize, "Return to windowed mode."), BlueStonez.buttondark_medium))
      ScreenResolutionManager.IsFullScreen = !Screen.fullScreen;
    this._optionsDropdown.SetRect(new Rect((float) (Screen.width - 44), this._yOffset, 44f, 44f));
    this._optionsDropdown.Draw();
  }

  private void DoFpsAndVersion()
  {
    string text = string.Format("{0} v{1}", (object) ApplicationDataManager.FrameRate, (object) "4.3.10");
    GUI.color = Color.white.SetAlpha(0.3f);
    GUI.Label(new Rect(5f, (float) (Screen.height - 25), 190f, 20f), text, BlueStonez.label_interparkmed_11pt_right);
    GUI.color = Color.white;
  }

  public void Show()
  {
    this.enabled = GlobalUIRibbon.IsVisible;
    CmuneEventHandler.Route((object) new OnGlobalUIRibbonChanged());
  }

  public void Hide()
  {
    this.enabled = false;
    this._yOffset = (float) -this._height;
    CmuneEventHandler.Route((object) new OnGlobalUIRibbonChanged());
  }

  public void UpdateLevelBounds(int level)
  {
    int minXp;
    int maxXp;
    PlayerXpUtil.GetXpRangeForLevel(level, out minXp, out maxXp);
    this._maxXPforThisLevel = maxXp;
    this._minXPforThisLevel = minXp;
  }

  public void AddXPEvent(int deltaXP)
  {
    if (deltaXP > 0)
    {
      this._ribbonEvents[GlobalUIRibbon.EventType.XpEvent] = (GlobalUIRibbon.RibbonEvent) new GlobalUIRibbon.GainEvent(370, Color.white, deltaXP, PlayerDataManager.PlayerExperienceSecure);
    }
    else
    {
      if (deltaXP >= 0)
        return;
      this._ribbonEvents[GlobalUIRibbon.EventType.XpEvent] = (GlobalUIRibbon.RibbonEvent) new GlobalUIRibbon.LoseEvent(370, Color.white, deltaXP, PlayerDataManager.PlayerExperienceSecure);
    }
  }

  public void AddCreditsEvent(int deltaCredits)
  {
    if (deltaCredits > 0)
    {
      this._ribbonEvents[GlobalUIRibbon.EventType.CreditEvent] = (GlobalUIRibbon.RibbonEvent) new GlobalUIRibbon.GainEvent(Screen.width - 130, Color.white, deltaCredits, PlayerDataManager.CreditsSecure);
      SfxManager.Play2dAudioClip(GameAudio.GetPoints);
    }
    else
    {
      if (deltaCredits >= 0)
        return;
      this._ribbonEvents[GlobalUIRibbon.EventType.CreditEvent] = (GlobalUIRibbon.RibbonEvent) new GlobalUIRibbon.LoseEvent(Screen.width - 130, Color.white, deltaCredits, PlayerDataManager.CreditsSecure);
    }
  }

  public void AddPointsEvent(int deltaPoints)
  {
    if (deltaPoints > 0)
    {
      this._ribbonEvents[GlobalUIRibbon.EventType.PointEvent] = (GlobalUIRibbon.RibbonEvent) new GlobalUIRibbon.GainEvent(Screen.width - 220, Color.white, deltaPoints, PlayerDataManager.PointsSecure);
      SfxManager.Play2dAudioClip(GameAudio.GetPoints);
    }
    else
    {
      if (deltaPoints >= 0)
        return;
      this._ribbonEvents[GlobalUIRibbon.EventType.PointEvent] = (GlobalUIRibbon.RibbonEvent) new GlobalUIRibbon.LoseEvent(Screen.width - 220, Color.white, deltaPoints, PlayerDataManager.PointsSecure);
    }
  }

  private abstract class RibbonEvent
  {
    protected const float _timeStage1 = 1f;
    protected const float _timeStage2 = 6f;
    protected float _alpha;
    protected float _scale;
    protected float _timer;
    protected float _speed;
    protected float _duration = 8f;
    protected float _height = 68f;
    protected float _value;
    protected float _delta;
    protected Rect _rect;
    protected Color _color;
    protected GUIStyle _style;
    protected string _content;

    public RibbonEvent(int horizontalPosition, Color color, int deltaValue, int currentValue)
    {
      this._value = (float) (currentValue - deltaValue);
      this._delta = (float) deltaValue;
      this._timer = 0.0f;
      this._alpha = 1f;
      this._scale = 1f;
      this._color = color;
      this._speed = Mathf.Sign((float) deltaValue) * 20f;
      this._style = BlueStonez.label_interparkbold_32pt;
      this._content = (double) this._speed <= 0.0 ? string.Format("-{0}", (object) Mathf.Abs(deltaValue).ToString("N0")) : string.Format("+{0}", (object) deltaValue.ToString("N0"));
      Vector2 vector2 = this._style.CalcSize(new GUIContent(this._content));
      this._rect = new Rect((float) horizontalPosition, this._height, vector2.x, vector2.y);
    }

    public abstract int Value { get; }

    public abstract float Alpha { get; }

    public void Draw()
    {
      GUIUtility.ScaleAroundPivot(new Vector2(this._scale, this._scale), new Vector2(this._rect.x + this._rect.width / 2f, this._rect.y + this._rect.height / 2f));
      GUI.contentColor = new Color(0.0f, 0.0f, 0.0f, this._alpha);
      GUI.Label(new Rect(this._rect.x + 1f, this._rect.y + 1f, this._rect.width, this._rect.height), this._content, this._style);
      GUI.contentColor = new Color(this._color.r, this._color.g, this._color.b, this._alpha);
      GUI.Label(this._rect, this._content, this._style);
      GUI.contentColor = Color.white;
      GUI.matrix = Matrix4x4.identity;
    }

    public abstract void Animate();

    public abstract bool IsDone();
  }

  private class GainEvent : GlobalUIRibbon.RibbonEvent
  {
    private float _moveTime = 0.3f;
    private float _stayTime = 0.5f;
    private float _scaleTime = 0.3f;

    public GainEvent(int x, Color color, int deltaValue, int currentValue)
      : base(x, color, deltaValue, currentValue)
    {
      this._alpha = 0.0f;
      this._scale = 1f;
      this._rect.y = 0.0f;
    }

    public override void Animate()
    {
      if ((double) this._timer < (double) this._moveTime)
      {
        this._rect.y = Mathfx.Berp(0.0f, this._height, this._timer / this._moveTime);
        this._alpha = Mathf.Lerp(this._alpha, 1f, 8f * Time.deltaTime);
      }
      else if ((double) this._timer > (double) this._moveTime + (double) this._scaleTime && (double) this._timer < (double) this._moveTime + (double) this._stayTime + (double) this._scaleTime)
      {
        this._scale = Mathf.Lerp(this._scale, 0.5f, 15f * Time.deltaTime);
        this._alpha = Mathf.Lerp(this._alpha, 0.2f, 10f * Time.deltaTime);
      }
      this._timer += Time.deltaTime;
    }

    public override int Value => Mathf.RoundToInt(this._value + (float) ((double) this._delta * (double) this._timer / ((double) this._moveTime + (double) this._stayTime + (double) this._scaleTime)));

    public override float Alpha => 1f - this._alpha;

    public override bool IsDone() => (double) this._timer >= (double) this._moveTime + (double) this._scaleTime + (double) this._stayTime;
  }

  private class LoseEvent : GlobalUIRibbon.RibbonEvent
  {
    private float _scaleTime = 0.5f;
    private float _moveTime = 0.5f;

    public LoseEvent(int x, Color color, int deltaValue, int currentValue)
      : base(x, color, deltaValue, currentValue)
    {
      this._scale = 0.3f;
    }

    public override void Animate()
    {
      if ((double) this._timer < (double) this._scaleTime)
        this._scale = Mathfx.Berp(0.3f, 1f, (float) ((double) this._timer / (double) this._scaleTime * 2.0));
      else if ((double) this._timer < (double) this._scaleTime + (double) this._moveTime)
      {
        this._rect.y = Mathf.Lerp(this._rect.y, 0.0f, 10f * Time.deltaTime);
        this._alpha = Mathf.Lerp(this._alpha, 0.0f, 10f * Time.deltaTime);
      }
      this._timer += Time.deltaTime;
    }

    public override int Value => Mathf.RoundToInt(this._value + (float) ((double) this._delta * (double) this._timer / ((double) this._moveTime + (double) this._scaleTime)));

    public override float Alpha => 1f - this._alpha;

    public override bool IsDone() => (double) this._timer >= (double) this._moveTime + (double) this._scaleTime;
  }

  private enum EventType
  {
    XpEvent,
    PointEvent,
    CreditEvent,
  }

  private class LiveFeed
  {
    private List<GlobalUIRibbon.LiveFeed.FeedItem> _content;
    private int _index;
    private bool _isRotating;
    private float _rotateY;

    public LiveFeed() => this._content = new List<GlobalUIRibbon.LiveFeed.FeedItem>(10);

    public void SetContent(List<LiveFeedView> feeds)
    {
      this._content.Clear();
      foreach (LiveFeedView feed in feeds)
      {
        GlobalUIRibbon.LiveFeed.FeedItem feedItem = new GlobalUIRibbon.LiveFeed.FeedItem();
        feedItem.Timer = 0.0f;
        feedItem.View = feed;
        feedItem.Date = feed.Date.ToShortDateString();
        feedItem.Length = BlueStonez.label_interparkbold_11pt_left.CalcSize(new GUIContent(feed.Description)).x;
        if (feed.Priority == 0)
          this._content.Insert(0, feedItem);
        else
          this._content.Add(feedItem);
      }
    }

    public void Update()
    {
      if (this._content.Count == 0 || this._content[0].View.Priority == 0)
        return;
      if (this._isRotating)
      {
        this._rotateY = Mathf.Clamp(this._rotateY + Time.deltaTime * 10f, 0.0f, 16f);
        if ((double) this._rotateY != 16.0)
          return;
        this._isRotating = false;
        this._index = (this._index + 1) % this._content.Count;
      }
      else
      {
        GlobalUIRibbon.LiveFeed.FeedItem feedItem = this._content[this._index];
        if ((double) feedItem.Timer > 5.0)
        {
          feedItem.Timer = 0.0f;
          this._rotateY = 0.0f;
          this._isRotating = true;
        }
        else
          feedItem.Timer += Time.deltaTime;
      }
    }

    public void Draw(Rect rect)
    {
      if (this._content.Count == 0)
        return;
      GlobalUIRibbon.LiveFeed.FeedItem feedItem1 = this._content[this._index];
      GUI.BeginGroup(rect);
      if (this._isRotating)
      {
        GlobalUIRibbon.LiveFeed.FeedItem feedItem2 = this._content[(this._index + 1) % this._content.Count];
        feedItem1.Draw(new Rect(0.0f, -this._rotateY, rect.width, rect.height));
        feedItem2.Draw(new Rect(0.0f, 16f - this._rotateY, rect.width, rect.height));
      }
      else
        feedItem1.Draw(new Rect(0.0f, 0.0f, rect.width, rect.height));
      GUI.EndGroup();
    }

    private class FeedItem
    {
      public float Timer;
      public string Date;
      public float Length;
      public LiveFeedView View;

      public void Draw(Rect rect)
      {
        GUI.Label(new Rect(8f, rect.y + 1f, 160f, 14f), this.Date, BlueStonez.label_interparkmed_11pt_left);
        if (this.View.Priority == 0)
          GUI.color = Color.red;
        GUI.Label(new Rect(80f, rect.y, this.Length, 14f), this.View.Description, BlueStonez.label_interparkbold_11pt_left);
        GUI.color = Color.white;
        GUI.contentColor = this.View.Priority != 0 ? ColorScheme.UberStrikeYellow : Color.red;
        if (!string.IsNullOrEmpty(this.View.Url) && GUITools.Button(new Rect(90f + this.Length, rect.y, 78f, 16f), new GUIContent(LocalizedStrings.MoreInfo, LocalizedStrings.OpenThisLinkInANewBrowserWindow), BlueStonez.buttondark_medium))
        {
          ScreenResolutionManager.IsFullScreen = false;
          ApplicationDataManager.OpenUrl(this.View.Description, this.View.Url);
        }
        GUI.contentColor = Color.white;
      }
    }
  }
}
