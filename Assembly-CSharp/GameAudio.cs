// Decompiled with JetBrains decompiler
// Type: GameAudio
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class GameAudio
{
  static GameAudio()
  {
    AudioClipConfigurator component;
    try
    {
      component = GameObject.Find(nameof (GameAudio)).GetComponent<AudioClipConfigurator>();
    }
    catch
    {
      Debug.LogError((object) "Missing instance of the prefab with name: GameAudio!");
      return;
    }
    GameAudio.SeletronRadioShort = component.Assets[0];
    GameAudio.BigSplash = component.Assets[1];
    GameAudio.ImpactCement1 = component.Assets[2];
    GameAudio.ImpactCement2 = component.Assets[3];
    GameAudio.ImpactCement3 = component.Assets[4];
    GameAudio.ImpactCement4 = component.Assets[5];
    GameAudio.ImpactGlass1 = component.Assets[6];
    GameAudio.ImpactGlass2 = component.Assets[7];
    GameAudio.ImpactGlass3 = component.Assets[8];
    GameAudio.ImpactGlass4 = component.Assets[9];
    GameAudio.ImpactGlass5 = component.Assets[10];
    GameAudio.ImpactGrass1 = component.Assets[11];
    GameAudio.ImpactGrass2 = component.Assets[12];
    GameAudio.ImpactGrass3 = component.Assets[13];
    GameAudio.ImpactGrass4 = component.Assets[14];
    GameAudio.ImpactMetal1 = component.Assets[15];
    GameAudio.ImpactMetal2 = component.Assets[16];
    GameAudio.ImpactMetal3 = component.Assets[17];
    GameAudio.ImpactMetal4 = component.Assets[18];
    GameAudio.ImpactMetal5 = component.Assets[19];
    GameAudio.ImpactSand1 = component.Assets[20];
    GameAudio.ImpactSand2 = component.Assets[21];
    GameAudio.ImpactSand3 = component.Assets[22];
    GameAudio.ImpactSand4 = component.Assets[23];
    GameAudio.ImpactSand5 = component.Assets[24];
    GameAudio.ImpactStone1 = component.Assets[25];
    GameAudio.ImpactStone2 = component.Assets[26];
    GameAudio.ImpactStone3 = component.Assets[27];
    GameAudio.ImpactStone4 = component.Assets[28];
    GameAudio.ImpactStone5 = component.Assets[29];
    GameAudio.ImpactWater1 = component.Assets[30];
    GameAudio.ImpactWater2 = component.Assets[31];
    GameAudio.ImpactWater3 = component.Assets[32];
    GameAudio.ImpactWater4 = component.Assets[33];
    GameAudio.ImpactWater5 = component.Assets[34];
    GameAudio.ImpactWood1 = component.Assets[35];
    GameAudio.ImpactWood2 = component.Assets[36];
    GameAudio.ImpactWood3 = component.Assets[37];
    GameAudio.ImpactWood4 = component.Assets[38];
    GameAudio.ImpactWood5 = component.Assets[39];
    GameAudio.MediumSplash = component.Assets[40];
    GameAudio.BlueWins = component.Assets[41];
    GameAudio.CountdownTonal1 = component.Assets[42];
    GameAudio.CountdownTonal2 = component.Assets[43];
    GameAudio.Draw = component.Assets[44];
    GameAudio.Fight = component.Assets[45];
    GameAudio.FocusEnemy = component.Assets[46];
    GameAudio.GameOver = component.Assets[47];
    GameAudio.GetPoints = component.Assets[48];
    GameAudio.GetXP = component.Assets[49];
    GameAudio.LevelUp = component.Assets[50];
    GameAudio.LostLead = component.Assets[51];
    GameAudio.MatchEndingCountdown1 = component.Assets[52];
    GameAudio.MatchEndingCountdown2 = component.Assets[53];
    GameAudio.MatchEndingCountdown3 = component.Assets[54];
    GameAudio.MatchEndingCountdown4 = component.Assets[55];
    GameAudio.MatchEndingCountdown5 = component.Assets[56];
    GameAudio.RedWins = component.Assets[57];
    GameAudio.TakenLead = component.Assets[58];
    GameAudio.TiedLead = component.Assets[59];
    GameAudio.YouWin = component.Assets[60];
    GameAudio.AmmoPickup2D = component.Assets[61];
    GameAudio.ArmorShard2D = component.Assets[62];
    GameAudio.BigHealth2D = component.Assets[63];
    GameAudio.GoldArmor2D = component.Assets[64];
    GameAudio.MediumHealth2D = component.Assets[65];
    GameAudio.MegaHealth2D = component.Assets[66];
    GameAudio.SilverArmor2D = component.Assets[67];
    GameAudio.SmallHealth2D = component.Assets[68];
    GameAudio.WeaponPickup2D = component.Assets[69];
    GameAudio.FootStepDirt1 = component.Assets[70];
    GameAudio.FootStepDirt2 = component.Assets[71];
    GameAudio.FootStepDirt3 = component.Assets[72];
    GameAudio.FootStepDirt4 = component.Assets[73];
    GameAudio.FootStepGlass1 = component.Assets[74];
    GameAudio.FootStepGlass2 = component.Assets[75];
    GameAudio.FootStepGlass3 = component.Assets[76];
    GameAudio.FootStepGlass4 = component.Assets[77];
    GameAudio.FootStepGrass1 = component.Assets[78];
    GameAudio.FootStepGrass2 = component.Assets[79];
    GameAudio.FootStepGrass3 = component.Assets[80];
    GameAudio.FootStepGrass4 = component.Assets[81];
    GameAudio.FootStepHeavyMetal1 = component.Assets[82];
    GameAudio.FootStepHeavyMetal2 = component.Assets[83];
    GameAudio.FootStepHeavyMetal3 = component.Assets[84];
    GameAudio.FootStepHeavyMetal4 = component.Assets[85];
    GameAudio.FootStepMetal1 = component.Assets[86];
    GameAudio.FootStepMetal2 = component.Assets[87];
    GameAudio.FootStepMetal3 = component.Assets[88];
    GameAudio.FootStepMetal4 = component.Assets[89];
    GameAudio.FootStepRock1 = component.Assets[90];
    GameAudio.FootStepRock2 = component.Assets[91];
    GameAudio.FootStepRock3 = component.Assets[92];
    GameAudio.FootStepRock4 = component.Assets[93];
    GameAudio.FootStepSand1 = component.Assets[94];
    GameAudio.FootStepSand2 = component.Assets[95];
    GameAudio.FootStepSand3 = component.Assets[96];
    GameAudio.FootStepSand4 = component.Assets[97];
    GameAudio.FootStepSnow1 = component.Assets[98];
    GameAudio.FootStepSnow2 = component.Assets[99];
    GameAudio.FootStepSnow3 = component.Assets[100];
    GameAudio.FootStepSnow4 = component.Assets[101];
    GameAudio.FootStepWater1 = component.Assets[102];
    GameAudio.FootStepWater2 = component.Assets[103];
    GameAudio.FootStepWater3 = component.Assets[104];
    GameAudio.FootStepWood1 = component.Assets[105];
    GameAudio.FootStepWood2 = component.Assets[106];
    GameAudio.FootStepWood3 = component.Assets[107];
    GameAudio.FootStepWood4 = component.Assets[108];
    GameAudio.GotHeadshotKill = component.Assets[109];
    GameAudio.GotNutshotKill = component.Assets[110];
    GameAudio.KilledBySplatbat = component.Assets[111];
    GameAudio.LandingGrunt = component.Assets[112];
    GameAudio.LocalPlayerHitArmorRemaining = component.Assets[113];
    GameAudio.LocalPlayerHitNoArmor = component.Assets[114];
    GameAudio.LocalPlayerHitNoArmorLowHealth = component.Assets[115];
    GameAudio.NormalKill1 = component.Assets[116];
    GameAudio.NormalKill2 = component.Assets[117];
    GameAudio.NormalKill3 = component.Assets[118];
    GameAudio.QuickItemRecharge = component.Assets[119];
    GameAudio.SwimAboveWater1 = component.Assets[120];
    GameAudio.SwimAboveWater2 = component.Assets[121];
    GameAudio.SwimAboveWater3 = component.Assets[122];
    GameAudio.SwimAboveWater4 = component.Assets[123];
    GameAudio.SwimUnderWater = component.Assets[124];
    GameAudio.AmmoPickup = component.Assets[125];
    GameAudio.ArmorShard = component.Assets[126];
    GameAudio.BigHealth = component.Assets[(int) sbyte.MaxValue];
    GameAudio.GoldArmor = component.Assets[128];
    GameAudio.JumpPad = component.Assets[129];
    GameAudio.JumpPad2D = component.Assets[130];
    GameAudio.MediumHealth = component.Assets[131];
    GameAudio.MegaHealth = component.Assets[132];
    GameAudio.SilverArmor = component.Assets[133];
    GameAudio.SmallHealth = component.Assets[134];
    GameAudio.TargetDamage = component.Assets[135];
    GameAudio.TargetPopup = component.Assets[136];
    GameAudio.WeaponPickup = component.Assets[137];
    GameAudio.ButtonClick = component.Assets[138];
    GameAudio.ClickReady = component.Assets[139];
    GameAudio.ClickUnready = component.Assets[140];
    GameAudio.ClosePanel = component.Assets[141];
    GameAudio.CreateGame = component.Assets[142];
    GameAudio.DoubleKill = component.Assets[143];
    GameAudio.EndOfRound = component.Assets[144];
    GameAudio.EquipGear = component.Assets[145];
    GameAudio.EquipItem = component.Assets[146];
    GameAudio.EquipWeapon = component.Assets[147];
    GameAudio.HeadShot = component.Assets[148];
    GameAudio.JoinGame = component.Assets[149];
    GameAudio.JoinServer = component.Assets[150];
    GameAudio.KillLeft1 = component.Assets[151];
    GameAudio.KillLeft2 = component.Assets[152];
    GameAudio.KillLeft3 = component.Assets[153];
    GameAudio.KillLeft4 = component.Assets[154];
    GameAudio.KillLeft5 = component.Assets[155];
    GameAudio.LeaveServer = component.Assets[156];
    GameAudio.MegaKill = component.Assets[157];
    GameAudio.MysteryBoxMusic = component.Assets[158];
    GameAudio.MysteryBoxWin = component.Assets[159];
    GameAudio.NewMessage = component.Assets[160];
    GameAudio.NewRequest = component.Assets[161];
    GameAudio.NutShot = component.Assets[162];
    GameAudio.Objective = component.Assets[163];
    GameAudio.ObjectiveTick = component.Assets[164];
    GameAudio.OpenPanel = component.Assets[165];
    GameAudio.QuadKill = component.Assets[166];
    GameAudio.RibbonClick = component.Assets[167];
    GameAudio.Smackdown = component.Assets[168];
    GameAudio.SubObjective = component.Assets[169];
    GameAudio.TripleKill = component.Assets[170];
    GameAudio.UberKill = component.Assets[171];
    GameAudio.LauncherBounce1 = component.Assets[172];
    GameAudio.LauncherBounce2 = component.Assets[173];
    GameAudio.OutOfAmmoClick = component.Assets[174];
    GameAudio.SniperScopeIn = component.Assets[175];
    GameAudio.SniperScopeOut = component.Assets[176];
    GameAudio.SniperZoomIn = component.Assets[177];
    GameAudio.SniperZoomOut = component.Assets[178];
    GameAudio.UnderwaterExplosion1 = component.Assets[179];
    GameAudio.UnderwaterExplosion2 = component.Assets[180];
    GameAudio.WeaponSwitch = component.Assets[181];
  }

  public static AudioClip SeletronRadioShort { get; private set; }

  public static AudioClip BigSplash { get; private set; }

  public static AudioClip ImpactCement1 { get; private set; }

  public static AudioClip ImpactCement2 { get; private set; }

  public static AudioClip ImpactCement3 { get; private set; }

  public static AudioClip ImpactCement4 { get; private set; }

  public static AudioClip ImpactGlass1 { get; private set; }

  public static AudioClip ImpactGlass2 { get; private set; }

  public static AudioClip ImpactGlass3 { get; private set; }

  public static AudioClip ImpactGlass4 { get; private set; }

  public static AudioClip ImpactGlass5 { get; private set; }

  public static AudioClip ImpactGrass1 { get; private set; }

  public static AudioClip ImpactGrass2 { get; private set; }

  public static AudioClip ImpactGrass3 { get; private set; }

  public static AudioClip ImpactGrass4 { get; private set; }

  public static AudioClip ImpactMetal1 { get; private set; }

  public static AudioClip ImpactMetal2 { get; private set; }

  public static AudioClip ImpactMetal3 { get; private set; }

  public static AudioClip ImpactMetal4 { get; private set; }

  public static AudioClip ImpactMetal5 { get; private set; }

  public static AudioClip ImpactSand1 { get; private set; }

  public static AudioClip ImpactSand2 { get; private set; }

  public static AudioClip ImpactSand3 { get; private set; }

  public static AudioClip ImpactSand4 { get; private set; }

  public static AudioClip ImpactSand5 { get; private set; }

  public static AudioClip ImpactStone1 { get; private set; }

  public static AudioClip ImpactStone2 { get; private set; }

  public static AudioClip ImpactStone3 { get; private set; }

  public static AudioClip ImpactStone4 { get; private set; }

  public static AudioClip ImpactStone5 { get; private set; }

  public static AudioClip ImpactWater1 { get; private set; }

  public static AudioClip ImpactWater2 { get; private set; }

  public static AudioClip ImpactWater3 { get; private set; }

  public static AudioClip ImpactWater4 { get; private set; }

  public static AudioClip ImpactWater5 { get; private set; }

  public static AudioClip ImpactWood1 { get; private set; }

  public static AudioClip ImpactWood2 { get; private set; }

  public static AudioClip ImpactWood3 { get; private set; }

  public static AudioClip ImpactWood4 { get; private set; }

  public static AudioClip ImpactWood5 { get; private set; }

  public static AudioClip MediumSplash { get; private set; }

  public static AudioClip BlueWins { get; private set; }

  public static AudioClip CountdownTonal1 { get; private set; }

  public static AudioClip CountdownTonal2 { get; private set; }

  public static AudioClip Draw { get; private set; }

  public static AudioClip Fight { get; private set; }

  public static AudioClip FocusEnemy { get; private set; }

  public static AudioClip GameOver { get; private set; }

  public static AudioClip GetPoints { get; private set; }

  public static AudioClip GetXP { get; private set; }

  public static AudioClip LevelUp { get; private set; }

  public static AudioClip LostLead { get; private set; }

  public static AudioClip MatchEndingCountdown1 { get; private set; }

  public static AudioClip MatchEndingCountdown2 { get; private set; }

  public static AudioClip MatchEndingCountdown3 { get; private set; }

  public static AudioClip MatchEndingCountdown4 { get; private set; }

  public static AudioClip MatchEndingCountdown5 { get; private set; }

  public static AudioClip RedWins { get; private set; }

  public static AudioClip TakenLead { get; private set; }

  public static AudioClip TiedLead { get; private set; }

  public static AudioClip YouWin { get; private set; }

  public static AudioClip AmmoPickup2D { get; private set; }

  public static AudioClip ArmorShard2D { get; private set; }

  public static AudioClip BigHealth2D { get; private set; }

  public static AudioClip GoldArmor2D { get; private set; }

  public static AudioClip MediumHealth2D { get; private set; }

  public static AudioClip MegaHealth2D { get; private set; }

  public static AudioClip SilverArmor2D { get; private set; }

  public static AudioClip SmallHealth2D { get; private set; }

  public static AudioClip WeaponPickup2D { get; private set; }

  public static AudioClip FootStepDirt1 { get; private set; }

  public static AudioClip FootStepDirt2 { get; private set; }

  public static AudioClip FootStepDirt3 { get; private set; }

  public static AudioClip FootStepDirt4 { get; private set; }

  public static AudioClip FootStepGlass1 { get; private set; }

  public static AudioClip FootStepGlass2 { get; private set; }

  public static AudioClip FootStepGlass3 { get; private set; }

  public static AudioClip FootStepGlass4 { get; private set; }

  public static AudioClip FootStepGrass1 { get; private set; }

  public static AudioClip FootStepGrass2 { get; private set; }

  public static AudioClip FootStepGrass3 { get; private set; }

  public static AudioClip FootStepGrass4 { get; private set; }

  public static AudioClip FootStepHeavyMetal1 { get; private set; }

  public static AudioClip FootStepHeavyMetal2 { get; private set; }

  public static AudioClip FootStepHeavyMetal3 { get; private set; }

  public static AudioClip FootStepHeavyMetal4 { get; private set; }

  public static AudioClip FootStepMetal1 { get; private set; }

  public static AudioClip FootStepMetal2 { get; private set; }

  public static AudioClip FootStepMetal3 { get; private set; }

  public static AudioClip FootStepMetal4 { get; private set; }

  public static AudioClip FootStepRock1 { get; private set; }

  public static AudioClip FootStepRock2 { get; private set; }

  public static AudioClip FootStepRock3 { get; private set; }

  public static AudioClip FootStepRock4 { get; private set; }

  public static AudioClip FootStepSand1 { get; private set; }

  public static AudioClip FootStepSand2 { get; private set; }

  public static AudioClip FootStepSand3 { get; private set; }

  public static AudioClip FootStepSand4 { get; private set; }

  public static AudioClip FootStepSnow1 { get; private set; }

  public static AudioClip FootStepSnow2 { get; private set; }

  public static AudioClip FootStepSnow3 { get; private set; }

  public static AudioClip FootStepSnow4 { get; private set; }

  public static AudioClip FootStepWater1 { get; private set; }

  public static AudioClip FootStepWater2 { get; private set; }

  public static AudioClip FootStepWater3 { get; private set; }

  public static AudioClip FootStepWood1 { get; private set; }

  public static AudioClip FootStepWood2 { get; private set; }

  public static AudioClip FootStepWood3 { get; private set; }

  public static AudioClip FootStepWood4 { get; private set; }

  public static AudioClip GotHeadshotKill { get; private set; }

  public static AudioClip GotNutshotKill { get; private set; }

  public static AudioClip KilledBySplatbat { get; private set; }

  public static AudioClip LandingGrunt { get; private set; }

  public static AudioClip LocalPlayerHitArmorRemaining { get; private set; }

  public static AudioClip LocalPlayerHitNoArmor { get; private set; }

  public static AudioClip LocalPlayerHitNoArmorLowHealth { get; private set; }

  public static AudioClip NormalKill1 { get; private set; }

  public static AudioClip NormalKill2 { get; private set; }

  public static AudioClip NormalKill3 { get; private set; }

  public static AudioClip QuickItemRecharge { get; private set; }

  public static AudioClip SwimAboveWater1 { get; private set; }

  public static AudioClip SwimAboveWater2 { get; private set; }

  public static AudioClip SwimAboveWater3 { get; private set; }

  public static AudioClip SwimAboveWater4 { get; private set; }

  public static AudioClip SwimUnderWater { get; private set; }

  public static AudioClip AmmoPickup { get; private set; }

  public static AudioClip ArmorShard { get; private set; }

  public static AudioClip BigHealth { get; private set; }

  public static AudioClip GoldArmor { get; private set; }

  public static AudioClip JumpPad { get; private set; }

  public static AudioClip JumpPad2D { get; private set; }

  public static AudioClip MediumHealth { get; private set; }

  public static AudioClip MegaHealth { get; private set; }

  public static AudioClip SilverArmor { get; private set; }

  public static AudioClip SmallHealth { get; private set; }

  public static AudioClip TargetDamage { get; private set; }

  public static AudioClip TargetPopup { get; private set; }

  public static AudioClip WeaponPickup { get; private set; }

  public static AudioClip ButtonClick { get; private set; }

  public static AudioClip ClickReady { get; private set; }

  public static AudioClip ClickUnready { get; private set; }

  public static AudioClip ClosePanel { get; private set; }

  public static AudioClip CreateGame { get; private set; }

  public static AudioClip DoubleKill { get; private set; }

  public static AudioClip EndOfRound { get; private set; }

  public static AudioClip EquipGear { get; private set; }

  public static AudioClip EquipItem { get; private set; }

  public static AudioClip EquipWeapon { get; private set; }

  public static AudioClip HeadShot { get; private set; }

  public static AudioClip JoinGame { get; private set; }

  public static AudioClip JoinServer { get; private set; }

  public static AudioClip KillLeft1 { get; private set; }

  public static AudioClip KillLeft2 { get; private set; }

  public static AudioClip KillLeft3 { get; private set; }

  public static AudioClip KillLeft4 { get; private set; }

  public static AudioClip KillLeft5 { get; private set; }

  public static AudioClip LeaveServer { get; private set; }

  public static AudioClip MegaKill { get; private set; }

  public static AudioClip MysteryBoxMusic { get; private set; }

  public static AudioClip MysteryBoxWin { get; private set; }

  public static AudioClip NewMessage { get; private set; }

  public static AudioClip NewRequest { get; private set; }

  public static AudioClip NutShot { get; private set; }

  public static AudioClip Objective { get; private set; }

  public static AudioClip ObjectiveTick { get; private set; }

  public static AudioClip OpenPanel { get; private set; }

  public static AudioClip QuadKill { get; private set; }

  public static AudioClip RibbonClick { get; private set; }

  public static AudioClip Smackdown { get; private set; }

  public static AudioClip SubObjective { get; private set; }

  public static AudioClip TripleKill { get; private set; }

  public static AudioClip UberKill { get; private set; }

  public static AudioClip LauncherBounce1 { get; private set; }

  public static AudioClip LauncherBounce2 { get; private set; }

  public static AudioClip OutOfAmmoClick { get; private set; }

  public static AudioClip SniperScopeIn { get; private set; }

  public static AudioClip SniperScopeOut { get; private set; }

  public static AudioClip SniperZoomIn { get; private set; }

  public static AudioClip SniperZoomOut { get; private set; }

  public static AudioClip UnderwaterExplosion1 { get; private set; }

  public static AudioClip UnderwaterExplosion2 { get; private set; }

  public static AudioClip WeaponSwitch { get; private set; }
}
