// Decompiled with JetBrains decompiler
// Type: CmunePrefs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public static class CmunePrefs
{
  public static void Reset() => PlayerPrefs.DeleteAll();

  public static bool TryGetKey<T>(CmunePrefs.Key k, out T value)
  {
    if (PlayerPrefs.HasKey(k.ToString()))
    {
      value = CmunePrefs.ReadKey<T>(k, default (T));
      return true;
    }
    value = default (T);
    return false;
  }

  public static bool HasKey(CmunePrefs.Key k) => PlayerPrefs.HasKey(k.ToString());

  public static T ReadKey<T>(CmunePrefs.Key k, T defaultValue)
  {
    T obj = defaultValue;
    if (typeof (T) == typeof (bool))
      obj = (T) (ValueType) (PlayerPrefs.GetInt(k.ToString(), !(bool) (object) defaultValue ? 0 : 1) == 1);
    else if (typeof (T) == typeof (int))
      obj = (T) (ValueType) PlayerPrefs.GetInt(k.ToString(), (int) (object) defaultValue);
    else if (typeof (T) == typeof (float))
      obj = (T) (ValueType) PlayerPrefs.GetFloat(k.ToString(), (float) (object) defaultValue);
    else if (typeof (T) == typeof (string))
      obj = (T) PlayerPrefs.GetString(k.ToString(), (string) (object) defaultValue);
    else
      Debug.LogError((object) string.Format("Key {0} couldn't be read because type {1} not supported.", (object) k, (object) typeof (T)));
    return obj;
  }

  public static T ReadKey<T>(CmunePrefs.Key k) => CmunePrefs.ReadKey<T>(k, default (T));

  public static void WriteKey<T>(CmunePrefs.Key k, T val)
  {
    if (typeof (T) == typeof (bool))
      PlayerPrefs.SetInt(k.ToString(), !(bool) (object) val ? 0 : 1);
    else if (typeof (T) == typeof (int))
      PlayerPrefs.SetInt(k.ToString(), (int) (object) val);
    else if (typeof (T) == typeof (float))
      PlayerPrefs.SetFloat(k.ToString(), (float) (object) val);
    else if (typeof (T) == typeof (string))
      PlayerPrefs.SetString(k.ToString(), (string) (object) val);
    else
      Debug.LogError((object) string.Format("Key {0} couldn't be read because type {1} not supported.", (object) k, (object) typeof (T)));
  }

  public enum Key
  {
    Player_Name = 50, // 0x00000032
    Player_Email = 51, // 0x00000033
    Player_Password = 52, // 0x00000034
    Player_AutoLogin = 54, // 0x00000036
    Options_VideoIsUsingCustom = 95, // 0x0000005F
    Options_VideoMaxQueuedFrames = 96, // 0x00000060
    Options_VideoTextureQuality = 97, // 0x00000061
    Options_VideoVSyncCount = 98, // 0x00000062
    Options_VideoAntiAliasing = 99, // 0x00000063
    Options_GeneralTargetFrameRate = 100, // 0x00000064
    Options_VideoWaterMode = 101, // 0x00000065
    Options_VideoCurrentQualityLevel = 102, // 0x00000066
    Options_VideoAdvancedWater = 103, // 0x00000067
    Options_VideoBloomAndFlares = 104, // 0x00000068
    Options_VideoColorCorrection = 105, // 0x00000069
    Options_VideoMotionBlur = 106, // 0x0000006A
    Options_InputXMouseSensitivity = 107, // 0x0000006B
    Options_InputYMouseSensitivity = 108, // 0x0000006C
    Options_InputMouseRotationMaxX = 109, // 0x0000006D
    Options_InputMouseRotationMaxY = 110, // 0x0000006E
    Options_InputMouseRotationMinX = 111, // 0x0000006F
    Options_InputMouseRotationMinY = 112, // 0x00000070
    Options_InputInvertMouse = 113, // 0x00000071
    Options_GameplayAutoPickupEnabled = 114, // 0x00000072
    Options_GameplayAutoEquipEnabled = 115, // 0x00000073
    Options_GameplayRagdollEnabled = 116, // 0x00000074
    Options_InputEnableGamepad = 117, // 0x00000075
    Options_VideoBloomHitEffect = 118, // 0x00000076
    Options_AudioEnabled = 120, // 0x00000078
    Options_AudioEffectsVolume = 121, // 0x00000079
    Options_AudioMusicVolume = 122, // 0x0000007A
    Options_AudioMasterVolume = 123, // 0x0000007B
    Options_VideoHardcoreMode = 124, // 0x0000007C
    Options_VideoScreenRes = 125, // 0x0000007D
    Options_VideoIsFullscreen = 126, // 0x0000007E
    Keymap_None = 300, // 0x0000012C
    Keymap_HorizontalLook = 301, // 0x0000012D
    Keymap_VerticalLook = 302, // 0x0000012E
    Keymap_Forward = 303, // 0x0000012F
    Keymap_Backward = 304, // 0x00000130
    Keymap_Left = 305, // 0x00000131
    Keymap_Right = 306, // 0x00000132
    Keymap_Jump = 307, // 0x00000133
    Keymap_Crouch = 308, // 0x00000134
    Keymap_PrimaryFire = 309, // 0x00000135
    Keymap_SecondaryFire = 310, // 0x00000136
    Keymap_Weapon1 = 311, // 0x00000137
    Keymap_Weapon2 = 312, // 0x00000138
    Keymap_Weapon3 = 313, // 0x00000139
    Keymap_Weapon4 = 314, // 0x0000013A
    Keymap_WeaponMelee = 315, // 0x0000013B
    Keymap_QuickItem1 = 316, // 0x0000013C
    Keymap_QuickItem2 = 317, // 0x0000013D
    Keymap_QuickItem3 = 318, // 0x0000013E
    Keymap_NextWeapon = 319, // 0x0000013F
    Keymap_PrevWeapon = 320, // 0x00000140
    Keymap_Pause = 321, // 0x00000141
    Keymap_Fullscreen = 322, // 0x00000142
    Keymap_Tabscreen = 323, // 0x00000143
    Keymap_Chat = 324, // 0x00000144
    Keymap_Inventory = 325, // 0x00000145
    Keymap_UseQuickItem = 326, // 0x00000146
    Keymap_ChangeTeam = 327, // 0x00000147
    Keymap_NextQuickItem = 328, // 0x00000148
    Shop_RecentlyUsedItems = 400, // 0x00000190
    App_ClientRegistered = 500, // 0x000001F4
  }
}
