// Decompiled with JetBrains decompiler
// Type: InputManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class InputManager : AutoMonoBehaviour<InputManager>
{
  private bool _inputEnabled;
  private bool _unassignedKeyMappings;
  private Dictionary<int, UserInputMap> _keyMapping = new Dictionary<int, UserInputMap>();
  private bool _isGamepadEnabled;
  private float _mouseScrollThreshold;

  private void Awake()
  {
    this._mouseScrollThreshold = 0.01f;
    this.SetDefaultKeyMapping();
  }

  private void Update()
  {
    KeyInput.Update();
    if (this.IsInputEnabled)
    {
      foreach (UserInputMap userInputMap in this._keyMapping.Values)
      {
        if (userInputMap != null && userInputMap.Channel != null)
        {
          userInputMap.Channel.Listen();
          if (userInputMap.IsEventSender && userInputMap.Channel.IsChanged)
            CmuneEventHandler.Route((object) new InputChangeEvent(userInputMap.Slot, userInputMap.Channel.Value));
        }
      }
    }
    if ((double) this.RawValue(GameInputKey.Fullscreen) == 0.0 || !GUITools.SaveClickIn(0.2f))
      return;
    GUITools.Clicked();
    ScreenResolutionManager.IsFullScreen = !Screen.fullScreen;
  }

  private void OnGUI()
  {
    KeyInput.OnGUI();
    Singleton<MouseInput>.Instance.OnGUI();
    if (!Event.current.shift || Event.current.type != UnityEngine.EventType.ScrollWheel)
      return;
    if ((double) Event.current.delta.x > 0.0)
      CmuneEventHandler.Route((object) new InputChangeEvent(GameInputKey.PrevWeapon, Event.current.delta.x));
    if ((double) Event.current.delta.x >= 0.0)
      return;
    CmuneEventHandler.Route((object) new InputChangeEvent(GameInputKey.NextWeapon, Event.current.delta.x));
  }

  public static bool GetMouseButtonDown(int button) => (Event.current == null || Event.current.type == UnityEngine.EventType.Layout) && Input.GetMouseButtonDown(button);

  public bool ListenForNewKeyAssignment(UserInputMap map)
  {
    if (Event.current.keyCode == KeyCode.Escape)
    {
      this.IsSettingKeymap = false;
      return true;
    }
    if (Event.current.keyCode != KeyCode.None)
      map.Channel = (IInputChannel) new KeyInputChannel(Event.current.keyCode);
    else if (Event.current.shift)
    {
      if (Input.GetKey(KeyCode.LeftShift))
        map.Channel = (IInputChannel) new KeyInputChannel(KeyCode.LeftShift);
      if (Input.GetKey(KeyCode.RightShift))
        map.Channel = (IInputChannel) new KeyInputChannel(KeyCode.RightShift);
    }
    else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(3) || Input.GetMouseButtonDown(4))
      map.Channel = (IInputChannel) new MouseInputChannel(Event.current.button);
    else if ((double) Mathf.Abs(Input.GetAxisRaw("Mouse ScrollWheel")) > 0.10000000149011612)
      map.Channel = (IInputChannel) new AxisInputChannel("Mouse ScrollWheel", 0.1f, (double) Input.GetAxisRaw("Mouse ScrollWheel") <= 0.0 ? AxisInputChannel.AxisReadingMethod.NegativeOnly : AxisInputChannel.AxisReadingMethod.PositiveOnly);
    else if ((double) Mathf.Abs(Input.GetAxisRaw("GamePadHorizontal1")) > 0.10000000149011612)
      map.Channel = (IInputChannel) new AxisInputChannel("GamePadHorizontal1", 0.1f, (double) Input.GetAxisRaw("GamePadHorizontal1") <= 0.0 ? AxisInputChannel.AxisReadingMethod.NegativeOnly : AxisInputChannel.AxisReadingMethod.PositiveOnly);
    else if ((double) Mathf.Abs(Input.GetAxisRaw("GamePadVertical1")) > 0.10000000149011612)
      map.Channel = (IInputChannel) new AxisInputChannel("GamePadVertical1", 0.1f, (double) Input.GetAxisRaw("GamePadVertical1") <= 0.0 ? AxisInputChannel.AxisReadingMethod.NegativeOnly : AxisInputChannel.AxisReadingMethod.PositiveOnly);
    else if ((double) Mathf.Abs(Input.GetAxisRaw("GamePadHorizontal2")) > 0.10000000149011612)
      map.Channel = (IInputChannel) new AxisInputChannel("GamePadHorizontal2", 0.1f, (double) Input.GetAxisRaw("GamePadHorizontal2") <= 0.0 ? AxisInputChannel.AxisReadingMethod.NegativeOnly : AxisInputChannel.AxisReadingMethod.PositiveOnly);
    else if ((double) Mathf.Abs(Input.GetAxisRaw("GamePadVertical2")) > 0.10000000149011612)
      map.Channel = (IInputChannel) new AxisInputChannel("GamePadVertical2", 0.1f, (double) Input.GetAxisRaw("GamePadVertical2") <= 0.0 ? AxisInputChannel.AxisReadingMethod.NegativeOnly : AxisInputChannel.AxisReadingMethod.PositiveOnly);
    else if ((double) Mathf.Abs(Input.GetAxisRaw("GamePadHorizontal3")) > 0.10000000149011612)
      map.Channel = (IInputChannel) new AxisInputChannel("GamePadHorizontal3", 0.1f, (double) Input.GetAxisRaw("GamePadHorizontal3") <= 0.0 ? AxisInputChannel.AxisReadingMethod.NegativeOnly : AxisInputChannel.AxisReadingMethod.PositiveOnly);
    else if ((double) Mathf.Abs(Input.GetAxisRaw("GamePadVertical3")) > 0.10000000149011612)
      map.Channel = (IInputChannel) new AxisInputChannel("GamePadVertical3", 0.1f, (double) Input.GetAxisRaw("GamePadVertical3") <= 0.0 ? AxisInputChannel.AxisReadingMethod.NegativeOnly : AxisInputChannel.AxisReadingMethod.PositiveOnly);
    else if ((double) Mathf.Abs(Input.GetAxisRaw("GamePadTrigger")) > 0.10000000149011612)
      map.Channel = (IInputChannel) new AxisInputChannel("GamePadTrigger", 0.1f, (double) Input.GetAxisRaw("GamePadTrigger") <= 0.0 ? AxisInputChannel.AxisReadingMethod.NegativeOnly : AxisInputChannel.AxisReadingMethod.PositiveOnly);
    else if (Input.GetButton("GamePadButtonA"))
      map.Channel = (IInputChannel) new ButtonInputChannel("GamePadButtonA");
    else if (Input.GetButton("GamePadButtonB"))
      map.Channel = (IInputChannel) new ButtonInputChannel("GamePadButtonB");
    else if (Input.GetButton("GamePadButtonX"))
      map.Channel = (IInputChannel) new ButtonInputChannel("GamePadButtonX");
    else if (Input.GetButton("GamePadButtonY"))
      map.Channel = (IInputChannel) new ButtonInputChannel("GamePadButtonY");
    else if (Input.GetButton("GamePadButtonLB"))
      map.Channel = (IInputChannel) new ButtonInputChannel("GamePadButtonLB");
    else if (Input.GetButton("GamePadButtonRB"))
      map.Channel = (IInputChannel) new ButtonInputChannel("GamePadButtonRB");
    else if (Input.GetButton("GamePadButtonStart"))
      map.Channel = (IInputChannel) new ButtonInputChannel("GamePadButtonStart");
    else if (Input.GetButton("GamePadButtonBack"))
      map.Channel = (IInputChannel) new ButtonInputChannel("GamePadButtonBack");
    else if (Input.GetButton("GamePadButtonLS"))
      map.Channel = (IInputChannel) new ButtonInputChannel("GamePadButtonLS");
    else if (Input.GetButton("GamePadButtonRS"))
      map.Channel = (IInputChannel) new ButtonInputChannel("GamePadButtonRS");
    else if (Input.GetButton("GamePadButton10"))
      map.Channel = (IInputChannel) new ButtonInputChannel("GamePadButton10");
    else if (Input.GetButton("GamePadButton11"))
    {
      map.Channel = (IInputChannel) new ButtonInputChannel("GamePadButton11");
    }
    else
    {
      this.IsSettingKeymap = true;
      return false;
    }
    CmuneEventHandler.Route((object) new InputAssignmentEvent());
    Event.current.Use();
    this.ResolveMultipleAssignment(map);
    this.WriteAllKeyMappings();
    this.IsSettingKeymap = false;
    return true;
  }

  public void Reset()
  {
    this._keyMapping.Clear();
    this.SetDefaultKeyMapping();
    this.IsGamepadEnabled = false;
    this.WriteAllKeyMappings();
  }

  public float RawValue(GameInputKey slot)
  {
    UserInputMap userInputMap;
    return !this.IsSettingKeymap && this._keyMapping.TryGetValue((int) slot, out userInputMap) ? userInputMap.RawValue() : 0.0f;
  }

  public float GetValue(GameInputKey slot)
  {
    UserInputMap userInputMap;
    return !this.IsSettingKeymap && this.IsInputEnabled && this._keyMapping.TryGetValue((int) slot, out userInputMap) ? userInputMap.Value : 0.0f;
  }

  public bool IsDown(GameInputKey slot)
  {
    UserInputMap userInputMap;
    return !this.IsSettingKeymap && this._keyMapping.TryGetValue((int) slot, out userInputMap) && (double) userInputMap.Value != 0.0;
  }

  public string GetKeyAssignmentString(GameInputKey slot)
  {
    UserInputMap userInputMap;
    return this._keyMapping.TryGetValue((int) slot, out userInputMap) && userInputMap != null ? userInputMap.Assignment : "Not set";
  }

  public string GetSlotName(GameInputKey slot)
  {
    switch (slot)
    {
      case GameInputKey.None:
        return "None";
      case GameInputKey.HorizontalLook:
        return "HorizontalLook";
      case GameInputKey.VerticalLook:
        return "VerticalLook";
      case GameInputKey.Forward:
        return "Forward";
      case GameInputKey.Backward:
        return "Backward";
      case GameInputKey.Left:
        return "Left";
      case GameInputKey.Right:
        return "Right";
      case GameInputKey.Jump:
        return "Jump";
      case GameInputKey.Crouch:
        return "Crouch";
      case GameInputKey.PrimaryFire:
        return "Primary Fire";
      case GameInputKey.SecondaryFire:
        return "Secondary Fire";
      case GameInputKey.Weapon1:
        return "Primary Weapon";
      case GameInputKey.Weapon2:
        return "Secondary Weapon";
      case GameInputKey.Weapon3:
        return "Tertiary Weapon";
      case GameInputKey.Weapon4:
        return "Pickup Weapon";
      case GameInputKey.WeaponMelee:
        return "Melee Weapon";
      case GameInputKey.QuickItem1:
        return "Quick Item 1";
      case GameInputKey.QuickItem2:
        return "Quick Item 2";
      case GameInputKey.QuickItem3:
        return "Quick Item 3";
      case GameInputKey.NextWeapon:
        return "Next Weapon / Zoom In";
      case GameInputKey.PrevWeapon:
        return "Prev Weapon / Zoom Out";
      case GameInputKey.Pause:
        return "Pause";
      case GameInputKey.Fullscreen:
        return "Fullscreen";
      case GameInputKey.Tabscreen:
        return "Tabscreen";
      case GameInputKey.Chat:
        return "Chat";
      case GameInputKey.Loadout:
        return "Loadout";
      case GameInputKey.UseQuickItem:
        return "Use QuickItem";
      case GameInputKey.ChangeTeam:
        return "Change Team";
      case GameInputKey.NextQuickItem:
        return "Cycle QuickItems";
      default:
        return "No Name";
    }
  }

  private void ResolveMultipleAssignment(UserInputMap map)
  {
    foreach (UserInputMap userInputMap in this._keyMapping.Values)
    {
      if (userInputMap != map && userInputMap.Channel != null && userInputMap.Channel.ChannelType == map.Channel.ChannelType && map.Assignment == userInputMap.Assignment)
      {
        userInputMap.Channel = (IInputChannel) null;
        break;
      }
    }
  }

  private bool IsChannelTaken(IInputChannel channel)
  {
    bool flag = false;
    foreach (UserInputMap userInputMap in this._keyMapping.Values)
    {
      if (userInputMap.Channel.Equals((object) channel))
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  private void SetDefaultKeyMapping()
  {
    this._keyMapping[1] = new UserInputMap(this.GetSlotName(GameInputKey.HorizontalLook), GameInputKey.HorizontalLook, (IInputChannel) new AxisInputChannel("Mouse X", 0.0f), false, false);
    this._keyMapping[2] = new UserInputMap(this.GetSlotName(GameInputKey.VerticalLook), GameInputKey.VerticalLook, (IInputChannel) new AxisInputChannel("Mouse Y", 1f), false, false);
    this._keyMapping[21] = new UserInputMap(this.GetSlotName(GameInputKey.Pause), GameInputKey.Pause, (IInputChannel) new KeyInputChannel(KeyCode.Backspace));
    this._keyMapping[23] = new UserInputMap(this.GetSlotName(GameInputKey.Tabscreen), GameInputKey.Tabscreen, (IInputChannel) new KeyInputChannel(KeyCode.Tab));
    this._keyMapping[22] = new UserInputMap(this.GetSlotName(GameInputKey.Fullscreen), GameInputKey.Fullscreen, (IInputChannel) new KeyInputChannel(KeyCode.F), prefix: KeyCode.LeftAlt);
    this._keyMapping[3] = new UserInputMap(this.GetSlotName(GameInputKey.Forward), GameInputKey.Forward, (IInputChannel) new KeyInputChannel(KeyCode.W));
    this._keyMapping[5] = new UserInputMap(this.GetSlotName(GameInputKey.Left), GameInputKey.Left, (IInputChannel) new KeyInputChannel(KeyCode.A));
    this._keyMapping[4] = new UserInputMap(this.GetSlotName(GameInputKey.Backward), GameInputKey.Backward, (IInputChannel) new KeyInputChannel(KeyCode.S));
    this._keyMapping[6] = new UserInputMap(this.GetSlotName(GameInputKey.Right), GameInputKey.Right, (IInputChannel) new KeyInputChannel(KeyCode.D));
    this._keyMapping[7] = new UserInputMap(this.GetSlotName(GameInputKey.Jump), GameInputKey.Jump, (IInputChannel) new KeyInputChannel(KeyCode.Space));
    this._keyMapping[8] = new UserInputMap(this.GetSlotName(GameInputKey.Crouch), GameInputKey.Crouch, (IInputChannel) new KeyInputChannel(KeyCode.LeftShift));
    this._keyMapping[9] = new UserInputMap(this.GetSlotName(GameInputKey.PrimaryFire), GameInputKey.PrimaryFire, (IInputChannel) new MouseInputChannel(0));
    this._keyMapping[10] = new UserInputMap(this.GetSlotName(GameInputKey.SecondaryFire), GameInputKey.SecondaryFire, (IInputChannel) new MouseInputChannel(1));
    this._keyMapping[19] = new UserInputMap(this.GetSlotName(GameInputKey.NextWeapon), GameInputKey.NextWeapon, (IInputChannel) new AxisInputChannel("Mouse ScrollWheel", this._mouseScrollThreshold, AxisInputChannel.AxisReadingMethod.PositiveOnly));
    this._keyMapping[20] = new UserInputMap(this.GetSlotName(GameInputKey.PrevWeapon), GameInputKey.PrevWeapon, (IInputChannel) new AxisInputChannel("Mouse ScrollWheel", this._mouseScrollThreshold, AxisInputChannel.AxisReadingMethod.NegativeOnly));
    this._keyMapping[15] = new UserInputMap(this.GetSlotName(GameInputKey.WeaponMelee), GameInputKey.WeaponMelee, (IInputChannel) new KeyInputChannel(KeyCode.Alpha1));
    this._keyMapping[11] = new UserInputMap(this.GetSlotName(GameInputKey.Weapon1), GameInputKey.Weapon1, (IInputChannel) new KeyInputChannel(KeyCode.Alpha2));
    this._keyMapping[12] = new UserInputMap(this.GetSlotName(GameInputKey.Weapon2), GameInputKey.Weapon2, (IInputChannel) new KeyInputChannel(KeyCode.Alpha3));
    this._keyMapping[13] = new UserInputMap(this.GetSlotName(GameInputKey.Weapon3), GameInputKey.Weapon3, (IInputChannel) new KeyInputChannel(KeyCode.Alpha4));
    this._keyMapping[14] = new UserInputMap(this.GetSlotName(GameInputKey.Weapon4), GameInputKey.Weapon4, (IInputChannel) new KeyInputChannel(KeyCode.Alpha5));
    this._keyMapping[16] = new UserInputMap(this.GetSlotName(GameInputKey.QuickItem1), GameInputKey.QuickItem1, (IInputChannel) new KeyInputChannel(KeyCode.Alpha6));
    this._keyMapping[17] = new UserInputMap(this.GetSlotName(GameInputKey.QuickItem2), GameInputKey.QuickItem2, (IInputChannel) new KeyInputChannel(KeyCode.Alpha7));
    this._keyMapping[18] = new UserInputMap(this.GetSlotName(GameInputKey.QuickItem3), GameInputKey.QuickItem3, (IInputChannel) new KeyInputChannel(KeyCode.Alpha8));
    this._keyMapping[27] = new UserInputMap(this.GetSlotName(GameInputKey.ChangeTeam), GameInputKey.ChangeTeam, (IInputChannel) new KeyInputChannel(KeyCode.M), prefix: KeyCode.LeftAlt);
    this._keyMapping[26] = new UserInputMap(this.GetSlotName(GameInputKey.UseQuickItem), GameInputKey.UseQuickItem, (IInputChannel) new KeyInputChannel(KeyCode.E));
    this._keyMapping[28] = new UserInputMap(this.GetSlotName(GameInputKey.NextQuickItem), GameInputKey.NextQuickItem, (IInputChannel) new KeyInputChannel(KeyCode.R));
  }

  private static CmunePrefs.Key GetPrefsKeyForSlot(int slot)
  {
    switch (slot)
    {
      case 0:
        return CmunePrefs.Key.Keymap_None;
      case 1:
        return CmunePrefs.Key.Keymap_HorizontalLook;
      case 2:
        return CmunePrefs.Key.Keymap_VerticalLook;
      case 3:
        return CmunePrefs.Key.Keymap_Forward;
      case 4:
        return CmunePrefs.Key.Keymap_Backward;
      case 5:
        return CmunePrefs.Key.Keymap_Left;
      case 6:
        return CmunePrefs.Key.Keymap_Right;
      case 7:
        return CmunePrefs.Key.Keymap_Jump;
      case 8:
        return CmunePrefs.Key.Keymap_Crouch;
      case 9:
        return CmunePrefs.Key.Keymap_PrimaryFire;
      case 10:
        return CmunePrefs.Key.Keymap_SecondaryFire;
      case 11:
        return CmunePrefs.Key.Keymap_Weapon1;
      case 12:
        return CmunePrefs.Key.Keymap_Weapon2;
      case 13:
        return CmunePrefs.Key.Keymap_Weapon3;
      case 14:
        return CmunePrefs.Key.Keymap_Weapon4;
      case 15:
        return CmunePrefs.Key.Keymap_WeaponMelee;
      case 16:
        return CmunePrefs.Key.Keymap_QuickItem1;
      case 17:
        return CmunePrefs.Key.Keymap_QuickItem2;
      case 18:
        return CmunePrefs.Key.Keymap_QuickItem3;
      case 19:
        return CmunePrefs.Key.Keymap_NextWeapon;
      case 20:
        return CmunePrefs.Key.Keymap_PrevWeapon;
      case 21:
        return CmunePrefs.Key.Keymap_Pause;
      case 22:
        return CmunePrefs.Key.Keymap_Fullscreen;
      case 23:
        return CmunePrefs.Key.Keymap_Tabscreen;
      case 24:
        return CmunePrefs.Key.Keymap_Chat;
      case 25:
        return CmunePrefs.Key.Keymap_Inventory;
      case 26:
        return CmunePrefs.Key.Keymap_UseQuickItem;
      case 27:
        return CmunePrefs.Key.Keymap_ChangeTeam;
      case 28:
        return CmunePrefs.Key.Keymap_NextQuickItem;
      default:
        return CmunePrefs.Key.Keymap_None;
    }
  }

  private void WriteAllKeyMappings()
  {
    this._unassignedKeyMappings = false;
    foreach (KeyValuePair<int, UserInputMap> keyValuePair in this._keyMapping)
    {
      if (keyValuePair.Value.IsConfigurable)
      {
        string prefString = keyValuePair.Value.GetPrefString();
        CmunePrefs.WriteKey<string>(InputManager.GetPrefsKeyForSlot(keyValuePair.Key), prefString);
        if (keyValuePair.Value.Channel == null)
          this._unassignedKeyMappings = true;
      }
    }
  }

  public void ReadAllKeyMappings()
  {
    this._unassignedKeyMappings = false;
    foreach (KeyValuePair<int, UserInputMap> keyValuePair in this._keyMapping)
    {
      string pref;
      if (keyValuePair.Value.IsConfigurable && CmunePrefs.TryGetKey<string>(InputManager.GetPrefsKeyForSlot(keyValuePair.Key), out pref))
      {
        keyValuePair.Value.FromPrefString(pref);
        if (keyValuePair.Value.Channel == null)
          this._unassignedKeyMappings = true;
      }
    }
  }

  public bool IsGamepadEnabled
  {
    get => this._isGamepadEnabled;
    set
    {
      this._isGamepadEnabled = value;
      if (this._isGamepadEnabled)
      {
        this.KeyMapping[1].Channel = (IInputChannel) new AxisInputChannel("GamePadHorizontal2", 0.0f);
        this.KeyMapping[2].Channel = (IInputChannel) new AxisInputChannel("GamePadVertical2", 0.0f);
      }
      else
      {
        this.KeyMapping[1].Channel = (IInputChannel) new AxisInputChannel("Mouse X", 0.0f);
        this.KeyMapping[2].Channel = (IInputChannel) new AxisInputChannel("Mouse Y", 0.0f);
      }
    }
  }

  public Dictionary<int, UserInputMap> KeyMapping => this._keyMapping;

  public bool IsAnyDown
  {
    get
    {
      if (this.IsInputEnabled)
      {
        foreach (UserInputMap userInputMap in this._keyMapping.Values)
        {
          if ((double) userInputMap.Value != 0.0)
            return true;
        }
      }
      return false;
    }
  }

  public bool IsInputEnabled
  {
    get => this._inputEnabled;
    set
    {
      this._inputEnabled = value;
      if (this._inputEnabled)
        return;
      foreach (UserInputMap userInputMap in this._keyMapping.Values)
      {
        if (userInputMap != null && userInputMap.Channel != null)
        {
          userInputMap.Channel.Reset();
          if (userInputMap.IsEventSender && userInputMap.Channel.IsChanged)
            CmuneEventHandler.Route((object) new InputChangeEvent(userInputMap.Slot, userInputMap.Channel.Value));
        }
      }
    }
  }

  public bool IsSettingKeymap { get; private set; }

  public bool HasUnassignedKeyMappings => this._unassignedKeyMappings;

  public string InputChannelForSlot(GameInputKey keySlot)
  {
    UserInputMap userInputMap;
    return this.KeyMapping.TryGetValue((int) keySlot, out userInputMap) ? userInputMap.Assignment : "None";
  }
}
