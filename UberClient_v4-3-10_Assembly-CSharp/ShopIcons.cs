// Decompiled with JetBrains decompiler
// Type: ShopIcons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public static class ShopIcons
{
  static ShopIcons()
  {
    Texture2DConfigurator component = GameObject.Find(nameof (ShopIcons)).GetComponent<Texture2DConfigurator>();
    ShopIcons.StatsMostWeaponSplatsMelee = !((UnityEngine.Object) component == (UnityEngine.Object) null) ? component.Assets[0] : throw new Exception("Missing instance of the prefab with name: ShopIcons!");
    ShopIcons.StatsMostWeaponSplatsHandgun = component.Assets[1];
    ShopIcons.StatsMostWeaponSplatsMachinegun = component.Assets[2];
    ShopIcons.StatsMostWeaponSplatsShotgun = component.Assets[3];
    ShopIcons.StatsMostWeaponSplatsSniperRifle = component.Assets[4];
    ShopIcons.StatsMostWeaponSplatsCannon = component.Assets[5];
    ShopIcons.StatsMostWeaponSplatsSplattergun = component.Assets[6];
    ShopIcons.StatsMostWeaponSplatsLauncher = component.Assets[7];
    ShopIcons.Boots = component.Assets[8];
    ShopIcons.Head = component.Assets[9];
    ShopIcons.Face = component.Assets[10];
    ShopIcons.Upperbody = component.Assets[11];
    ShopIcons.Lowerbody = component.Assets[12];
    ShopIcons.Gloves = component.Assets[13];
    ShopIcons.Holos = component.Assets[14];
    ShopIcons.RecentItems = component.Assets[15];
    ShopIcons.FunctionalItems = component.Assets[16];
    ShopIcons.WeaponItems = component.Assets[17];
    ShopIcons.GearItems = component.Assets[18];
    ShopIcons.QuickItems = component.Assets[19];
    ShopIcons.NewItems = component.Assets[20];
    ShopIcons.LoadoutTabWeapons = component.Assets[21];
    ShopIcons.LoadoutTabGear = component.Assets[22];
    ShopIcons.LoadoutTabItems = component.Assets[23];
    ShopIcons.LabsInventory = component.Assets[24];
    ShopIcons.LabsShop = component.Assets[25];
    ShopIcons.LabsUndergroundIcon = component.Assets[26];
    ShopIcons.BundleIcon32x32 = component.Assets[27];
    ShopIcons.IconLottery = component.Assets[28];
    ShopIcons.CreditsIcon32x32 = component.Assets[29];
    ShopIcons.CreditsIcon48x48 = component.Assets[30];
    ShopIcons.CreditsIcon75x75 = component.Assets[31];
    ShopIcons.Points48x48 = component.Assets[32];
    ShopIcons.IconPoints20x20 = component.Assets[33];
    ShopIcons.IconCredits20x20 = component.Assets[34];
    ShopIcons.Stats1Kills20x20 = component.Assets[35];
    ShopIcons.Stats2Smackdowns20x20 = component.Assets[36];
    ShopIcons.Stats3Headshots20x20 = component.Assets[37];
    ShopIcons.Stats4Nutshots20x20 = component.Assets[38];
    ShopIcons.Stats5Damage20x20 = component.Assets[39];
    ShopIcons.Stats6Deaths20x20 = component.Assets[40];
    ShopIcons.Stats7Kdr20x20 = component.Assets[41];
    ShopIcons.Stats8Suicides20x20 = component.Assets[42];
    ShopIcons.New = component.Assets[43];
    ShopIcons.Hot = component.Assets[44];
    ShopIcons.Sale = component.Assets[45];
    ShopIcons.BlankItemFrame = component.Assets[46];
    ShopIcons.CheckMark = component.Assets[47];
    ShopIcons.ItemexpirationIcon = component.Assets[48];
    ShopIcons.ItemarmorpointsIcon = component.Assets[49];
    ShopIcons.ArrowBigShop = component.Assets[50];
    ShopIcons.ArrowSmallDownWhite = component.Assets[51];
    ShopIcons.ArrowSmallUpWhite = component.Assets[52];
    ShopIcons.ItemSlotSelected = component.Assets[53];
  }

  public static Texture2D StatsMostWeaponSplatsMelee { get; private set; }

  public static Texture2D StatsMostWeaponSplatsHandgun { get; private set; }

  public static Texture2D StatsMostWeaponSplatsMachinegun { get; private set; }

  public static Texture2D StatsMostWeaponSplatsShotgun { get; private set; }

  public static Texture2D StatsMostWeaponSplatsSniperRifle { get; private set; }

  public static Texture2D StatsMostWeaponSplatsCannon { get; private set; }

  public static Texture2D StatsMostWeaponSplatsSplattergun { get; private set; }

  public static Texture2D StatsMostWeaponSplatsLauncher { get; private set; }

  public static Texture2D Boots { get; private set; }

  public static Texture2D Head { get; private set; }

  public static Texture2D Face { get; private set; }

  public static Texture2D Upperbody { get; private set; }

  public static Texture2D Lowerbody { get; private set; }

  public static Texture2D Gloves { get; private set; }

  public static Texture2D Holos { get; private set; }

  public static Texture2D RecentItems { get; private set; }

  public static Texture2D FunctionalItems { get; private set; }

  public static Texture2D WeaponItems { get; private set; }

  public static Texture2D GearItems { get; private set; }

  public static Texture2D QuickItems { get; private set; }

  public static Texture2D NewItems { get; private set; }

  public static Texture2D LoadoutTabWeapons { get; private set; }

  public static Texture2D LoadoutTabGear { get; private set; }

  public static Texture2D LoadoutTabItems { get; private set; }

  public static Texture2D LabsInventory { get; private set; }

  public static Texture2D LabsShop { get; private set; }

  public static Texture2D LabsUndergroundIcon { get; private set; }

  public static Texture2D BundleIcon32x32 { get; private set; }

  public static Texture2D IconLottery { get; private set; }

  public static Texture2D CreditsIcon32x32 { get; private set; }

  public static Texture2D CreditsIcon48x48 { get; private set; }

  public static Texture2D CreditsIcon75x75 { get; private set; }

  public static Texture2D Points48x48 { get; private set; }

  public static Texture2D IconPoints20x20 { get; private set; }

  public static Texture2D IconCredits20x20 { get; private set; }

  public static Texture2D Stats1Kills20x20 { get; private set; }

  public static Texture2D Stats2Smackdowns20x20 { get; private set; }

  public static Texture2D Stats3Headshots20x20 { get; private set; }

  public static Texture2D Stats4Nutshots20x20 { get; private set; }

  public static Texture2D Stats5Damage20x20 { get; private set; }

  public static Texture2D Stats6Deaths20x20 { get; private set; }

  public static Texture2D Stats7Kdr20x20 { get; private set; }

  public static Texture2D Stats8Suicides20x20 { get; private set; }

  public static Texture2D New { get; private set; }

  public static Texture2D Hot { get; private set; }

  public static Texture2D Sale { get; private set; }

  public static Texture2D BlankItemFrame { get; private set; }

  public static Texture2D CheckMark { get; private set; }

  public static Texture2D ItemexpirationIcon { get; private set; }

  public static Texture2D ItemarmorpointsIcon { get; private set; }

  public static Texture2D ArrowBigShop { get; private set; }

  public static Texture2D ArrowSmallDownWhite { get; private set; }

  public static Texture2D ArrowSmallUpWhite { get; private set; }

  public static Texture2D ItemSlotSelected { get; private set; }
}
