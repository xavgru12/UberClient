// Decompiled with JetBrains decompiler
// Type: HelpPanelGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class HelpPanelGUI : PanelGuiBase
{
  private const int WIDTH = 500;
  private Rect _rect;
  private GUIContent[] _helpTabs;
  private int _selectedHelpTab;
  private Vector2 _scrollBasics;
  private Vector2 _scrollGameplay;
  private Vector2 _scrollItems;
  private DateTime baseTime = new DateTime(2012, 12, 14);
  private float newPlayersPerSecond = 0.23f;
  private int currentPlayers = 12821196;

  private void Start() => this._helpTabs = new GUIContent[4]
  {
    new GUIContent(LocalizedStrings.General),
    new GUIContent(LocalizedStrings.Gameplay),
    new GUIContent(LocalizedStrings.Items),
    new GUIContent("About")
  };

  private void OnGUI()
  {
    int height = Mathf.RoundToInt((float) (Screen.height - 56) * 0.75f);
    this._rect = new Rect((float) (Screen.width - 630) * 0.5f, (float) GlobalUIRibbon.Instance.Height(), 630f, (float) height);
    GUI.BeginGroup(this._rect, GUIContent.none, BlueStonez.window_standard_grey38);
    this.DrawHelpPanel();
    GUI.EndGroup();
  }

  private void DrawHelpPanel()
  {
    GUI.depth = 3;
    GUI.Label(new Rect(0.0f, 0.0f, this._rect.width, 56f), LocalizedStrings.HelpCaps, BlueStonez.tab_strip);
    this._selectedHelpTab = UnityGUI.Toolbar(new Rect(2f, 31f, 360f, 22f), this._selectedHelpTab, this._helpTabs, this._helpTabs.Length, BlueStonez.tab_medium);
    GUI.BeginGroup(new Rect(16f, 55f, this._rect.width - 32f, (float) ((double) this._rect.height - 56.0 - 44.0)), string.Empty, BlueStonez.window_standard_grey38);
    switch (this._selectedHelpTab)
    {
      case 0:
        this.DrawGeneralGroup();
        break;
      case 1:
        this.DrawGameplayGroup();
        break;
      case 2:
        this.DrawItemsGroup();
        break;
      case 3:
        this.DrawCreditsGroup();
        break;
    }
    GUI.EndGroup();
    if (!GUI.Button(new Rect(this._rect.width - 136f, this._rect.height - 40f, 120f, 32f), new GUIContent(LocalizedStrings.OkCaps), BlueStonez.button))
      return;
    PanelManager.Instance.ClosePanel(PanelType.Help);
  }

  private void DrawRuleGroup()
  {
    GUI.skin = BlueStonez.Skin;
    int height = 550;
    this._scrollItems = GUITools.BeginScrollView(new Rect(1f, 2f, this._rect.width - 33f, (float) ((double) this._rect.height - 54.0 - 50.0)), this._scrollItems, new Rect(0.0f, 0.0f, 560f, (float) height));
    Rect rect = new Rect(14f, 16f, 530f, (float) (height - 30));
    this.DrawGroupControl(rect, "In-game Rules", BlueStonez.label_group_interparkbold_18pt);
    GUI.BeginGroup(rect);
    this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(10f, "Introduction", "Before we let you loose into the wild world of UberStrike, we've written a few simple guidelines that are in place to make your gaming experience as fun and fair as possible. Having a good time in a multiplayer game is a team effort! So do your part to help our community enjoy themselves;)\n\nWe hope that you have a pleasant stay in Uberstrike!"), "Chatting", "1: No swearing or inappropriate content. Every time an inappropriate word is typed, three puppies and a kitten get caught in a revolving door.\n2: No \"Caps lock\" (using it for emphasis is okay). Please only emphazise with discretion and tact.\n3: No spamming. This includes baloney, rubbish, prattle, balderdash, hogwash, fatuity, drivel, mumbo jumbo, and canned precooked meat products. \n4: Do not personally attack any person(s). If you happen to be a hata, don't be hatin,' becasue the mods gonna be moderatin.' \n5: No backseat moderating. Believe it or not, we didn't add the convenient little 'Report Button' just because it looks pretty up there in the corner of the screen, although it does go nicely with that cute little gear symbol.\n6: Do not discuss topics that involve race, color, creed, religion, sex, or politics. It's not like we play games to get extra exposure to the many issues we face constantly in our daily lives."), "General", "1: Alternate or \"Second\" Accounts in-game ARE allowed, although we all love you just the way you are.\n2: No account sharing! Your account is yours, and if another player is caught using it, all parties will get banned. Sharing definitely isn't caring round these parts.\n3: Exploiting of glitches will not be tolerated. Cheating of any kind will result in a permanent ban, which may or may not include eternal banishment to the land of angry ankle-biting woodchucks.\n4: Be respectful to the Administrators/Moderators/QAs. These people work hard for you, so please show them respect. If you do, you might even get a cookie!\n5: Advertising of any content unrelated to UberStrike is not permitted.\n6: Please do not try to cleverly circumvent the rules listed here. Although some of these rules are flexible, they are here for a reason, and will be enforced.\n7: Join a server in your area. You will not get banned for lagging, although you may get kicked from the current game.\n8: Above all, use common sense. Studies have shown it works 87% better than no sense at all!\n9: Have fun!");
    GUI.EndGroup();
    GUITools.EndScrollView();
  }

  private void DrawGeneralGroup()
  {
    GUI.skin = BlueStonez.Skin;
    int height = 490;
    this._scrollBasics = GUITools.BeginScrollView(new Rect(1f, 2f, this._rect.width - 33f, (float) ((double) this._rect.height - 54.0 - 50.0)), this._scrollBasics, new Rect(0.0f, 0.0f, 560f, (float) height));
    Rect rect = new Rect(14f, 16f, 530f, (float) (height - 30));
    this.DrawGroupControl(rect, LocalizedStrings.WelcomeToUS, BlueStonez.label_group_interparkbold_18pt);
    GUI.BeginGroup(rect);
    this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(10f, LocalizedStrings.Introduction, LocalizedStrings.IntroHelpDesc), LocalizedStrings.Home, LocalizedStrings.HomeHelpDesc), LocalizedStrings.Play, LocalizedStrings.PlayHelpDesc), LocalizedStrings.Profile, LocalizedStrings.ProfileHelpDesc), LocalizedStrings.Shop, LocalizedStrings.ShopHelpDesc);
    GUI.EndGroup();
    GUITools.EndScrollView();
  }

  private void DrawGameplayGroup()
  {
    GUI.skin = BlueStonez.Skin;
    int height = 950;
    this._scrollGameplay = GUITools.BeginScrollView(new Rect(1f, 2f, this._rect.width - 33f, (float) ((double) this._rect.height - 54.0 - 50.0)), this._scrollGameplay, new Rect(0.0f, 0.0f, 560f, (float) height));
    Rect rect = new Rect(14f, 16f, 530f, (float) (height - 30));
    this.DrawGroupControl(rect, LocalizedStrings.Gameplay, BlueStonez.label_group_interparkbold_18pt);
    GUI.BeginGroup(rect);
    this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(10f, "Character Level", "Your character level in UberStrike determines what items you have access to in the Shop. The higher your level, the more items you are able to get. Your character levels up by earning XP in the game."), "Earning XP", "There are five XP granting events in UberStrike. XP events stack, so if for example you get a headshot splat with a melee weapon you get 3 XP, one for the splat, one for the headshot, and one for the melee."), "Splatting an Enemy", "When you deal the final below to an enemy you get 1 XP."), "Headshot Splats", "When you splat an enemy with a headshot you get 1 XP."), "Nutshot Splats", "When you splat an enemy with a nutshot you get 1 XP."), "Melee Splats", "When you splat an enemy with a melee weapon you get 1 XP."), "Deal 100 Damage", "For every 100 damage you deal you get 1 XP."), "Health", "Health is what you need to survive. You start every life with 100 health, and if it reaches zero, you are splatted and have to respawn. If you take damage, you can replenish your health by picking up health packs in game."), "Armor Points", "Armor Points are picked up in the game. They absorb a percentage of the damage you receive. The percentage depends on the gear you have equipped."), "Looking Around", "UberStrike is a 3D environment, which means you need to be able to look around. To make your character do this you need to move the mouse."), "Moving Around", "In UberStrike you use the WASD keys to control the movement of your character. This means that pressing the W key on your keyboard will cause your character to walk forwards. With just the W key and the mouse you can navigate your character to almost every location in the game environment. Pressing the S key will cause you to walk backwards, and pressing the A and D keys will cause you to move left and right (called 'strafing').The final key you抣l need to know to get around in UberStrike is the spacebar. Pressing this key will cause your character to jump, which is essential for quickly getting around certain obstacles in the game. If you can get the hang of using the WASD keys to move, the spacebar to jump over obstacles, and the mouse to look around all at the same time, then you have mastered the basics of navigating a first person 3D environment. The use of these keys is common throughout many first person games, so practice them in UberStrike and you抣l be a pro in no time."), "Selecting Different Weapons", "By scrolling the mouse wheel you can cycle through all of your available weapons. You can also choose specific weapons by pressing the number keys 1 through 5."), "Combat", "In UberStrike your character carries weapons that you can use to splat other players. You use your weapons by clicking the mouse buttons. Pressing the left mouse button will cause the weapon to shoot, called 'Primary Fire' and pressing the right mouse button will use the weapon抯 special functions, called 'Alternate Fire' Be aware that not all weapons have an Alternate Fire function, and for those that do, it is often a different function for each weapon. An example of an Alternate Fire function would be the zoom, which is the Alternate Fire for Sniper Rifle class weapons.");
    GUI.EndGroup();
    GUITools.EndScrollView();
  }

  private void DrawItemsGroup()
  {
    GUI.skin = BlueStonez.Skin;
    int height = 690;
    this._scrollItems = GUITools.BeginScrollView(new Rect(1f, 2f, this._rect.width - 33f, (float) ((double) this._rect.height - 54.0 - 50.0)), this._scrollItems, new Rect(0.0f, 0.0f, 560f, (float) height));
    Rect rect = new Rect(14f, 16f, 530f, (float) (height - 30));
    this.DrawGroupControl(rect, LocalizedStrings.Items, BlueStonez.label_group_interparkbold_18pt);
    GUI.BeginGroup(rect);
    this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(10f, "Weapons", "Your character gets access to weapons after you buy them. You can also pick them up within the game, but you do not get to keep them when you are splatted or when you leave the game. Weapons are divided into eight classes: Melee, Handguns, Machine Guns, Shotguns, Sniper Rifles, Splatter Guns, Cannons, and Launchers. Each weapon class functions differently in game and is applicable in different combat contexts. For example, shotgun class weapons are generally better for close range battles, while the sniper rifle class weapons are better from a distance. Weapons have an expiry date of up to 90 days, depending on how long you buy them for. After the expiry date has passed, the weapon will disappear from your inventory."), "Gear", "Gear items are used to customize your character and increase your in-game protection. They have an effect on your speed, the amount of AP you can carry without depletion, and the amount of damage each AP absorbs."), "Loadout", "The Loadout is a list of all items that you own that your character currently has equipped. Your loadout dictates your character抯 appearance in the game."), "Inventory", "The Inventory is a list of all the items that you own that your character does NOT have equipped. All items here have an expiry time, after which they will disappear from your inventory."), "Shop", "The Shop is a place where you can buy items for standardized prices. It has the widest variety of items in UberStrike. Purchasing items in the shop is restricted according to your character level. If an item has a level that is above your character level, you cannot purchase it. You can increase your character level by playing the game and earning XP (see gameplay)."), "Underground", "The Underground is a special shop that sells rare and unique items that cannot be found elsewhere. Items in the Underground have no level restrictions."), "Points", "Points are used to purchase items from the Shop. You get at least 500 points every day you log into UberStrike."), "Credits", "Credits are used to purchase powerful items from the Shop. You can obtain credits by clicking on the 'Get Credits' button in the bottom right hand corner of the shop, or on the Taskbar at the top of the screen. Credits are the only currency that can be used to purchase rare items in the Underground.");
    GUI.EndGroup();
    GUITools.EndScrollView();
  }

  private void DrawCreditsGroup()
  {
    GUI.skin = BlueStonez.Skin;
    int height = 365;
    this._scrollItems = GUITools.BeginScrollView(new Rect(1f, 2f, this._rect.width - 33f, (float) ((double) this._rect.height - 54.0 - 50.0)), this._scrollItems, new Rect(0.0f, 0.0f, 560f, (float) height));
    Rect rect = new Rect(14f, 16f, 530f, (float) (height - 30));
    this.DrawGroupControl(rect, "About Uberstrike", BlueStonez.label_group_interparkbold_18pt);
    GUI.BeginGroup(rect);
    float yOffset = this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(this.DrawGroupLabel(5f, "The Team", "Roman Anastasini, Ludovic Bodin, Nad Chishtie, Jonny Farrell, Tommy Franken, Lanmay Jung, Jamin Lee, Kate Li, Monika Michalak, Shaun Lelacheur Sales, Dagmara Sitek, Paolo Stanner, Lee Turner, Graham Vanderplank, Alex Wang, Brian Zhang, Alice Zhao"), "The Mods", "Akalron, Army of One, avanos, ~H3ADSH0T~, Gray Mouser, GUY82, king_john, niashy, Simon1700, Snake Doctor, P_U_M_B_A, The Alpha Male, THE ENDER, timewarp01, Tweex, W00t"), "The QA Testers", "ATOMjkee, Butcherr, Buford T Justice, Carlos Spicy Weine, Dark Drone, Equi|ibrium, hendronimus, KXI_SYSTEM, Neofighter, -ORTHRUS-, -Shruikan-, Simon1700, tayw97, +TrIgGeR_sPaZuM+"), "The Legends", "Chingachgook, Ehnonimus, Enzo., karanraj, Leeness, Lev175, neel4d, Stylezxy, Ultimus Maximus");
    if (PlayerDataManager.IsPlayerLoggedIn)
    {
      double num = (double) this.DrawGroupLabel(yOffset, "The Community", "YOU and " + this.CurrentPlayers().ToString("N0") + " other awesome players!");
    }
    GUI.EndGroup();
    GUITools.EndScrollView();
  }

  private float DrawGroupLabel(float yOffset, string header, string text, bool center = false)
  {
    Rect rect = new Rect(16f, yOffset + 25f, 490f, 0.0f);
    if (!string.IsNullOrEmpty(header))
    {
      GUI.color = new Color(0.87f, 0.64f, 0.035f, 1f);
      GUI.Label(new Rect(rect.x, rect.y, rect.width, 16f), header + ":", BlueStonez.label_interparkbold_13pt_left);
      GUI.color = new Color(1f, 1f, 1f, 0.8f);
    }
    float height;
    if (center)
    {
      height = BlueStonez.label_interparkbold_11pt.CalcHeight(new GUIContent(text), rect.width);
      GUI.Label(new Rect(rect.x, rect.y + 16f, rect.width, height), text, BlueStonez.label_interparkbold_11pt);
    }
    else
    {
      height = BlueStonez.label_interparkbold_11pt_left_wrap.CalcHeight(new GUIContent(text), rect.width);
      GUI.Label(new Rect(rect.x, rect.y + 16f, rect.width, height), text, BlueStonez.label_interparkbold_11pt_left_wrap);
    }
    GUI.color = Color.white;
    return (float) ((double) yOffset + 20.0 + (double) height + 16.0);
  }

  private void DrawGroupControl(Rect rect, string title, GUIStyle style)
  {
    GUI.BeginGroup(rect, string.Empty, BlueStonez.group_grey81);
    GUI.EndGroup();
    GUI.Label(new Rect(rect.x + 18f, rect.y - 8f, style.CalcSize(new GUIContent(title)).x + 10f, 16f), title, style);
  }

  private int CurrentPlayers() => this.currentPlayers + Mathf.RoundToInt((float) DateTime.Now.Subtract(this.baseTime).TotalSeconds * this.newPlayersPerSecond);
}
