// Decompiled with JetBrains decompiler
// Type: HomePageGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class HomePageGUI : MonoBehaviour
{
  private Dictionary<string, HomePageGUI.MenuTile> _tiles;
  private Dictionary<string, HomePageGUI.MenuTile> _playTiles;
  private bool _isReady;
  private bool _isPlayActive;
  private bool _isTransitioning;
  private Rect _screenRect;
  private Rect _canvasRect;
  private Rect _menuRect;
  private Vector2 _xpBarPosition;
  private float _aspectRatio = 1.4f;
  private int _minWidth = 480;
  private int _maxWidth = 960;
  private float _animationSpeed = 13f;
  private DynamicTexture _smallAd;
  [SerializeField]
  private Texture _contentGradient;
  [SerializeField]
  private Texture _playTile;
  [SerializeField]
  private Texture _shopTile;
  [SerializeField]
  private Texture _optionsTile;
  [SerializeField]
  private Texture _profileTile;
  [SerializeField]
  private Texture _clansTile;
  [SerializeField]
  private Texture _inboxTile;
  [SerializeField]
  private Texture _chatTile;
  [SerializeField]
  private Texture _inviteFbFriendsTile;
  [SerializeField]
  private Texture _joinGameTile;
  [SerializeField]
  private Texture _newGameTile;
  [SerializeField]
  private Texture _exploreMapsTile;
  [SerializeField]
  private Texture _backTile;
  private XpPtsHud _xpPtsHud;
  private List<DynamicTexture> _facePileTextures;

  private void Awake()
  {
    this._tiles = new Dictionary<string, HomePageGUI.MenuTile>();
    this._tiles.Add("Play", new HomePageGUI.MenuTile(0.5f, 0.24f));
    this._tiles.Add("Shop", new HomePageGUI.MenuTile(0.5f, 0.24f));
    this._tiles.Add("MenuOne", new HomePageGUI.MenuTile(0.25f, 0.15f));
    this._tiles.Add("MenuTwo", new HomePageGUI.MenuTile(0.25f, 0.15f));
    this._tiles.Add("MenuThree", new HomePageGUI.MenuTile(0.25f, 0.15f));
    this._tiles.Add("MenuFour", new HomePageGUI.MenuTile(0.25f, 0.15f));
    this._tiles.Add("AdLarge", new HomePageGUI.MenuTile(0.7f, 0.61f));
    this._tiles.Add("Chat", new HomePageGUI.MenuTile(0.3f, 0.33f));
    this._tiles.Add("AdSmall", new HomePageGUI.MenuTile(0.3f, 0.28f));
    this._playTiles = new Dictionary<string, HomePageGUI.MenuTile>();
    this._playTiles.Add("JoinGame", new HomePageGUI.MenuTile(0.3f, 0.2f));
    this._playTiles.Add("NewGame", new HomePageGUI.MenuTile(0.3f, 0.2f));
    this._playTiles.Add("ExploreMaps", new HomePageGUI.MenuTile(0.3f, 0.2f));
    this._playTiles.Add("Back", new HomePageGUI.MenuTile(0.3f, 0.2f));
    this._smallAd = new DynamicTexture("https://static-ssl.cmune.com/UberStrike/Images/WeeklySpecials/SmallAd.jpg");
    this._facePileTextures = new List<DynamicTexture>();
    this._xpPtsHud = new XpPtsHud();
    this._xpPtsHud.ScreenPosition.x = 0.81f;
    this._xpPtsHud.ScaleFactor = 0.5f;
    this._xpPtsHud.IsNextLevelVisible = true;
    this._xpPtsHud.IsXpPtsTextVisible = false;
    CmuneEventHandler.AddListener<ScreenResolutionEvent>(new Action<ScreenResolutionEvent>(this._xpPtsHud.OnScreenResolutionChange));
    this.LayoutCanvas();
    this.LayoutTiles();
  }

  [DebuggerHidden]
  private IEnumerator Start() => (IEnumerator) new HomePageGUI.\u003CStart\u003Ec__Iterator12()
  {
    \u003C\u003Ef__this = this
  };

  private void OnEnable()
  {
    AutoMonoBehaviour<BackgroundMusicPlayer>.Instance.Play(GameAudio.SeletronRadioShort);
    Singleton<AvatarBuilder>.Instance.UpdateLocalAvatar(Singleton<LoadoutManager>.Instance.GearLoadout);
    this._xpPtsHud.DisplayPermanently();
    this._xpPtsHud.Enabled = true;
    this._xpPtsHud.ResetXp();
    this._xpPtsHud.ResetTransform();
  }

  private void OnDisable()
  {
    this.ResetMenu();
    this._xpPtsHud.Enabled = false;
  }

  private void Update()
  {
    this.LayoutCanvas();
    this.LayoutTiles();
    if (this._facePileTextures.Count != AutoMonoBehaviour<FacebookInterface>.Instance.FacebookFriendUrls.Count)
    {
      this._facePileTextures = new List<DynamicTexture>();
      foreach (string facebookFriendUrl in AutoMonoBehaviour<FacebookInterface>.Instance.FacebookFriendUrls)
        this._facePileTextures.Add(new DynamicTexture(facebookFriendUrl));
    }
    Singleton<HudUtil>.Instance.Update();
    this._xpPtsHud.ScreenPosition = this._xpBarPosition;
    this._xpPtsHud.Update();
  }

  private void LayoutCanvas()
  {
    this._screenRect = new Rect(0.0f, (float) GlobalUIRibbon.Instance.Height(), (float) Screen.width, (float) (Screen.height - GlobalUIRibbon.Instance.Height()));
    this._canvasRect = new Rect(20f, 10f, (float) (Mathf.RoundToInt(this._screenRect.width * 0.65f) - 40), (float) (Mathf.RoundToInt(this._screenRect.height) - 20));
    Vector2 vector2 = new Vector2((float) Mathf.RoundToInt(this._canvasRect.height * this._aspectRatio), (float) Mathf.RoundToInt(this._canvasRect.width * (1f / this._aspectRatio)));
    if ((double) this._canvasRect.height < (double) this._canvasRect.width)
    {
      if ((double) vector2.y < (double) this._canvasRect.height)
      {
        vector2.x = this._canvasRect.width;
        vector2.y = this._canvasRect.width * (1f / this._aspectRatio);
      }
      else
      {
        vector2.y = this._canvasRect.height;
        vector2.x = vector2.y * this._aspectRatio;
      }
    }
    else if ((double) vector2.x < (double) this._canvasRect.width)
    {
      vector2.y = this._canvasRect.height;
      vector2.x = this._canvasRect.height * this._aspectRatio;
    }
    else
    {
      vector2.x = this._canvasRect.width;
      vector2.y = this._canvasRect.width * (1f / this._aspectRatio);
    }
    vector2.x = Mathf.Clamp(vector2.x, (float) this._minWidth, (float) this._maxWidth);
    vector2.y = (float) Mathf.RoundToInt(vector2.x * (1f / this._aspectRatio));
    this._menuRect = new Rect((float) Mathf.RoundToInt(this._canvasRect.HalfWidth() - vector2.x * 0.5f), (float) Mathf.RoundToInt(this._canvasRect.HalfHeight() - vector2.y * 0.5f), vector2.x, vector2.y);
    this._xpBarPosition = new Vector2(0.81f, (this._screenRect.y + this._canvasRect.y + this._menuRect.yMax) / this._screenRect.height);
  }

  private void LayoutTiles()
  {
    this._tiles["Play"].SetRect(0.0f, 0.0f, this._menuRect);
    this._tiles["Shop"].SetRect(this._tiles["Play"].Rect.xMax, 0.0f, this._menuRect);
    this._tiles["MenuOne"].SetRect(0.0f, this._tiles["Play"].Rect.yMax, this._menuRect);
    this._tiles["MenuTwo"].SetRect(this._tiles["MenuOne"].Rect.xMax, this._tiles["Play"].Rect.yMax, this._menuRect);
    this._tiles["MenuThree"].SetRect(this._tiles["MenuTwo"].Rect.xMax, this._tiles["Play"].Rect.yMax, this._menuRect);
    this._tiles["MenuFour"].SetRect(this._tiles["MenuThree"].Rect.xMax, this._tiles["Play"].Rect.yMax, this._menuRect);
    this._tiles["AdLarge"].SetRect(0.0f, this._tiles["MenuOne"].Rect.yMax, this._menuRect);
    this._tiles["Chat"].SetRect(this._tiles["AdLarge"].Rect.xMax, this._tiles["MenuOne"].Rect.yMax, this._menuRect);
    this._tiles["AdSmall"].SetRect(this._tiles["AdLarge"].Rect.xMax, this._tiles["Chat"].Rect.yMax, this._menuRect);
    this._playTiles["JoinGame"].SetRect(this._tiles["Play"].Rect.xMax, 0.0f, this._menuRect);
    this._playTiles["NewGame"].SetRect(this._tiles["Play"].Rect.xMax, this._playTiles["JoinGame"].Rect.yMax, this._menuRect);
    this._playTiles["ExploreMaps"].SetRect(this._tiles["Play"].Rect.xMax, this._playTiles["NewGame"].Rect.yMax, this._menuRect);
    this._playTiles["Back"].SetRect(this._tiles["Play"].Rect.xMax, this._playTiles["ExploreMaps"].Rect.yMax, this._menuRect);
  }

  private void ResetMenu()
  {
    this._isReady = true;
    this._isTransitioning = false;
    this._isPlayActive = false;
    foreach (KeyValuePair<string, HomePageGUI.MenuTile> tile in this._tiles)
      this._tiles[tile.Key].Color = Color.white;
    foreach (KeyValuePair<string, HomePageGUI.MenuTile> playTile in this._playTiles)
      this._playTiles[playTile.Key].Color = Color.white;
  }

  private void OnGUI()
  {
    GUI.depth = 11;
    GUI.BeginGroup(this._screenRect);
    GUI.BeginGroup(this._canvasRect);
    this.DrawMenu(this._menuRect);
    GUI.EndGroup();
    GUI.EndGroup();
    this._xpPtsHud.Draw();
  }

  [DebuggerHidden]
  private IEnumerator StartActivateMain() => (IEnumerator) new HomePageGUI.\u003CStartActivateMain\u003Ec__Iterator13()
  {
    \u003C\u003Ef__this = this
  };

  private void TogglePlayTiles()
  {
    if (this._isPlayActive && !this._isTransitioning)
      this.StartCoroutine(this.StartDeactivatePlay());
    else
      this.StartCoroutine(this.StartActivatePlay());
  }

  [DebuggerHidden]
  private IEnumerator StartActivatePlay() => (IEnumerator) new HomePageGUI.\u003CStartActivatePlay\u003Ec__Iterator14()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator StartDeactivatePlay() => (IEnumerator) new HomePageGUI.\u003CStartDeactivatePlay\u003Ec__Iterator15()
  {
    \u003C\u003Ef__this = this
  };

  private void OpenShopPromotion()
  {
    MenuPageManager.Instance.LoadPage(PageType.Shop);
    CmuneEventHandler.Route((object) new SelectShopAreaEvent()
    {
      ShopArea = ShopArea.Shop,
      ItemType = UberstrikeItemType.Special,
      ItemClass = UberstrikeItemClass.SpecialGeneral
    });
  }

  private void DrawMenu(Rect menuRect)
  {
    float num = (float) (((double) Mathf.Sin(Time.time * 6f) + 1.2000000476837158) * 0.5);
    GUI.BeginGroup(menuRect);
    this.DrawTile(this._tiles["Play"], new GUIContent(this._playTile), new Action(this.TogglePlayTiles), 1f);
    this.DrawTile(this._tiles["Shop"], new GUIContent(this._shopTile), (Action) (() => MenuPageManager.Instance.LoadPage(PageType.Shop)), 1f);
    this.DrawTile(this._tiles["MenuOne"], new GUIContent(this._profileTile), (Action) (() => MenuPageManager.Instance.LoadPage(PageType.Stats)), 1f);
    this.DrawTile(this._tiles["MenuTwo"], new GUIContent(this._inboxTile), (Action) (() => MenuPageManager.Instance.LoadPage(PageType.Inbox)), !Singleton<InboxManager>.Instance.HasUnreadMessages && !Singleton<InboxManager>.Instance.HasUnreadRequests ? 1f : num);
    this.DrawTile(this._tiles["MenuThree"], new GUIContent(this._clansTile), (Action) (() => MenuPageManager.Instance.LoadPage(PageType.Clans)), 1f);
    this.DrawTile(this._tiles["MenuFour"], new GUIContent(this._optionsTile), (Action) (() => PanelManager.Instance.OpenPanel(PanelType.Options)), 1f);
    this.DrawTile(this._tiles["Chat"], new GUIContent(this._chatTile), (Action) (() => MenuPageManager.Instance.LoadPage(PageType.Chat)), !Singleton<ChatManager>.Instance.HasUnreadPrivateMessage && !Singleton<ChatManager>.Instance.HasUnreadClanMessage ? 1f : num);
    if (!Singleton<ItemPromotionManager>.Instance.WeeklySpecial.Texture.IsDone)
      this.DrawTile(this._tiles["AdLarge"], GUIContent.none, contentAlpha: 1f);
    if (this._isReady)
    {
      GUI.color = this._tiles["AdLarge"].Color;
      Rect rect1 = new Rect(this._tiles["AdLarge"].Rect.x + 1f, this._tiles["AdLarge"].Rect.y + 1f, this._tiles["AdLarge"].Rect.width - 2f, this._tiles["AdLarge"].Rect.height - 2f);
      Singleton<ItemPromotionManager>.Instance.WeeklySpecial.Texture.Draw(rect1, this._isPlayActive);
      if (Singleton<ItemPromotionManager>.Instance.WeeklySpecial.Texture.IsDone)
      {
        float height = (float) Mathf.RoundToInt(rect1.height * 0.25f);
        Rect rect2 = new Rect(rect1.x, rect1.yMax - height, rect1.width, height);
        GUI.DrawTexture(rect2, this._contentGradient);
        GUITools.LabelShadow(rect2.OffsetBy(0.0f, rect1.height * 0.05f), Singleton<ItemPromotionManager>.Instance.WeeklySpecial.Title, BlueStonez.label_interparkbold_18pt, this._tiles["AdLarge"].Color);
      }
      if ((double) this._tiles["AdLarge"].Color.a > 1.0 / 1000.0 && GUITools.Button(this._tiles["AdLarge"].Rect, GUIContent.none, GUIStyle.none) && !this._isTransitioning)
        this.OpenShopPromotion();
      GUI.color = Color.white;
    }
    if (ApplicationDataManager.Channel == ChannelType.WebFacebook)
    {
      this.DrawGridTile(this._tiles["AdSmall"], this._facePileTextures, (Action) (() => PanelManager.Instance.OpenPanel(PanelType.Options)));
    }
    else
    {
      if (!this._smallAd.IsDone)
        this.DrawTile(this._tiles["AdSmall"], GUIContent.none, contentAlpha: 1f);
      if (this._isReady)
      {
        GUI.color = this._tiles["AdSmall"].Color;
        this._smallAd.Draw(new Rect(this._tiles["AdSmall"].Rect.x + 1f, this._tiles["AdSmall"].Rect.y + 1f, this._tiles["AdSmall"].Rect.width - 2f, this._tiles["AdSmall"].Rect.height - 2f), this._isPlayActive);
        if ((double) this._tiles["AdSmall"].Color.a > 1.0 / 1000.0 && GUITools.Button(this._tiles["AdSmall"].Rect, GUIContent.none, GUIStyle.none) && !this._isTransitioning)
          this.OpenShopPromotion();
        GUI.color = Color.white;
      }
    }
    if (this._isPlayActive)
    {
      this.DrawTile(this._playTiles["JoinGame"], new GUIContent(this._joinGameTile), (Action) (() => Singleton<GameServerController>.Instance.JoinFastestServer()), 1f);
      this.DrawTile(this._playTiles["NewGame"], new GUIContent(this._newGameTile), (Action) (() => Singleton<GameServerController>.Instance.CreateOnFastestServer()), 1f);
      this.DrawTile(this._playTiles["ExploreMaps"], new GUIContent(this._exploreMapsTile), (Action) (() => this.OpenPageFromPlay(PageType.Training)), 1f);
      this.DrawTile(this._playTiles["Back"], new GUIContent(this._backTile), new Action(this.TogglePlayTiles), 1f);
    }
    GUI.EndGroup();
  }

  private void OpenPageFromPlay(PageType pageType) => MenuPageManager.Instance.LoadPage(pageType);

  private void DrawTile(
    HomePageGUI.MenuTile menuTile,
    GUIContent guiContent,
    Action action = null,
    float contentAlpha = 1)
  {
    GUI.color = menuTile.Color;
    GUI.contentColor = Color.white.SetAlpha(contentAlpha);
    if ((double) menuTile.Color.a > 1.0 / 1000.0 && GUITools.Button(menuTile.Rect, guiContent, StormFront.MenuTile) && action != null && !this._isTransitioning)
      action();
    GUI.contentColor = Color.white;
    GUI.color = Color.white;
  }

  private void DrawGridTile(
    HomePageGUI.MenuTile menuTile,
    List<DynamicTexture> textureList,
    Action action = null)
  {
    bool buttonEnabled = (double) menuTile.Color.a > 1.0 / 1000.0;
    GUI.color = menuTile.Color;
    GUI.BeginGroup(menuTile.Rect);
    Vector2 vector2 = new Vector2((float) Mathf.RoundToInt(menuTile.Rect.width * 0.333f), (float) Mathf.RoundToInt(menuTile.Rect.height * 0.333f));
    this.DrawGridItem(new Rect(0.0f, 0.0f, vector2.x, vector2.y), textureList.Count <= 0 ? (DynamicTexture) null : textureList[0], buttonEnabled);
    this.DrawGridItem(new Rect(vector2.x, 0.0f, vector2.x, vector2.y), textureList.Count <= 1 ? (DynamicTexture) null : textureList[1], buttonEnabled);
    this.DrawGridItem(new Rect(vector2.x * 2f, 0.0f, vector2.x, vector2.y), textureList.Count <= 2 ? (DynamicTexture) null : textureList[2], buttonEnabled);
    this.DrawGridItem(new Rect(0.0f, vector2.y, vector2.x, vector2.y), textureList.Count <= 3 ? (DynamicTexture) null : textureList[3], buttonEnabled);
    this.DrawGridItem(new Rect(vector2.x, vector2.y, vector2.x, vector2.y), textureList.Count <= 4 ? (DynamicTexture) null : textureList[4], buttonEnabled);
    this.DrawGridItem(new Rect(vector2.x * 2f, vector2.y, vector2.x, vector2.y), textureList.Count <= 5 ? (DynamicTexture) null : textureList[5], buttonEnabled);
    this.DrawGridItem(new Rect(0.0f, vector2.y * 2f, vector2.x, vector2.y), textureList.Count <= 6 ? (DynamicTexture) null : textureList[6], buttonEnabled);
    this.DrawGridItem(new Rect(vector2.x, vector2.y * 2f, vector2.x, vector2.y), textureList.Count <= 7 ? (DynamicTexture) null : textureList[7], buttonEnabled);
    this.DrawGridItem(new Rect(vector2.x * 2f, vector2.y * 2f, vector2.x, vector2.y), textureList.Count <= 8 ? (DynamicTexture) null : textureList[8], buttonEnabled);
    GUI.EndGroup();
    GUI.color = Color.white;
  }

  private void DrawGridItem(Rect rect, DynamicTexture texture, bool buttonEnabled = true)
  {
    if (texture != null)
    {
      texture.Draw(rect.Contract(1, 1), true);
      if (!buttonEnabled || !GUITools.Button(rect, GUIContent.none, BlueStonez.black_background))
        return;
      AutoMonoBehaviour<FacebookInterface>.Instance.OpenInviteFbFriends();
    }
    else
    {
      if (!buttonEnabled || !GUITools.Button(rect, new GUIContent(this._inviteFbFriendsTile), StormFront.MenuTile))
        return;
      AutoMonoBehaviour<FacebookInterface>.Instance.OpenInviteFbFriends();
    }
  }

  private class MenuTile
  {
    public Rect Rect;
    public Color Color;
    private float _relativeWidth;
    private float _relativeHeight;

    public MenuTile(float relativeWidth, float relativeHeight)
    {
      this.Rect.x = 0.0f;
      this.Rect.y = 0.0f;
      this._relativeWidth = relativeWidth;
      this._relativeHeight = relativeHeight;
      this.Color = Color.white;
    }

    public void SetRect(float x, float y, Rect canvasRect) => this.Rect = new Rect((float) Mathf.RoundToInt(x), (float) Mathf.RoundToInt(y), (float) Mathf.RoundToInt(canvasRect.width * this._relativeWidth), (float) Mathf.RoundToInt(canvasRect.height * this._relativeHeight));
  }
}
