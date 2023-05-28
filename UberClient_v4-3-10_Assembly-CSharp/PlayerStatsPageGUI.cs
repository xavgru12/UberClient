// Decompiled with JetBrains decompiler
// Type: PlayerStatsPageGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

internal class PlayerStatsPageGUI : PageGUI
{
  public override void DrawGUI(Rect rect)
  {
    GUI.BeginGroup(rect, GUIContent.none, BlueStonez.window_standard_grey38);
    this.DrawStats(new Rect(2f, 2f, rect.width - 4f, 168f));
    this.DrawRewards(new Rect(2f, 170f, rect.width - 4f, rect.height * 0.2f));
    GUI.EndGroup();
  }

  private void DrawStats(Rect rect)
  {
    GUI.Button(new Rect(rect.x, rect.y, rect.width, 40f), string.Empty, BlueStonez.box_grey50);
    GUI.Label(new Rect(rect.x + 10f, rect.y + 2f, rect.width, 40f), "MY STATUS", BlueStonez.label_interparkbold_18pt_left);
    float width = rect.width;
    float height1 = rect.height - 40f;
    float height2 = 32f;
    GUI.BeginGroup(new Rect(rect.x, rect.y + 40f, rect.width, rect.height - 40f), GUIContent.none, BlueStonez.window);
    GUI.BeginGroup(new Rect(0.0f, 0.0f, (float) ((double) width / 2.0 + 1.0), height1), BlueStonez.group_grey81);
    GUI.Label(new Rect(5f, height2 * 0.0f, (float) ((double) width / 2.0 + 1.0), height2), new GUIContent(LocalizedStrings.KillXP, (Texture) ShopIcons.Stats1Kills20x20), BlueStonez.label_interparkbold_18pt_left);
    GUI.Label(new Rect(0.0f, height2 * 0.0f, (float) ((double) width / 2.0 - 5.0), height2), Singleton<EndOfMatchStats>.Instance.KillXP, BlueStonez.label_interparkbold_18pt_right);
    GUI.Label(new Rect(5f, height2 * 1f, (float) ((double) width / 2.0 + 1.0), height2), new GUIContent(LocalizedStrings.DeathsCaps, (Texture) ShopIcons.Stats6Deaths20x20), BlueStonez.label_interparkbold_18pt_left);
    GUI.Label(new Rect(0.0f, height2 * 1f, (float) ((double) width / 2.0 - 5.0), height2), Singleton<EndOfMatchStats>.Instance.Deaths, BlueStonez.label_interparkbold_18pt_right);
    GUI.Label(new Rect(5f, height2 * 2f, (float) ((double) width / 2.0 + 1.0), height2), new GUIContent(LocalizedStrings.KDR, (Texture) ShopIcons.Stats7Kdr20x20), BlueStonez.label_interparkbold_18pt_left);
    GUI.Label(new Rect(0.0f, height2 * 2f, (float) ((double) width / 2.0 - 5.0), height2), Singleton<EndOfMatchStats>.Instance.KDR, BlueStonez.label_interparkbold_18pt_right);
    GUI.Label(new Rect(5f, height2 * 3f, (float) ((double) width / 2.0 + 1.0), height2), new GUIContent(LocalizedStrings.SuicideXP, (Texture) ShopIcons.Stats8Suicides20x20), BlueStonez.label_interparkbold_18pt_left);
    GUI.Label(new Rect(0.0f, height2 * 3f, (float) ((double) width / 2.0 - 5.0), height2), Singleton<EndOfMatchStats>.Instance.Suicides, BlueStonez.label_interparkbold_18pt_right);
    GUI.EndGroup();
    GUI.BeginGroup(new Rect(width / 2f, 0.0f, width / 2f, height1), BlueStonez.group_grey81);
    GUI.Label(new Rect(5f, height2 * 0.0f, (float) ((double) width / 2.0 + 1.0), height2), new GUIContent(LocalizedStrings.HeadshotXP, (Texture) ShopIcons.Stats3Headshots20x20), BlueStonez.label_interparkbold_18pt_left);
    GUI.Label(new Rect(0.0f, height2 * 0.0f, (float) ((double) width / 2.0 - 5.0), height2), Singleton<EndOfMatchStats>.Instance.HeadshotXP, BlueStonez.label_interparkbold_18pt_right);
    GUI.Label(new Rect(5f, height2 * 1f, (float) ((double) width / 2.0 + 1.0), height2), new GUIContent(LocalizedStrings.NutshotXP, (Texture) ShopIcons.Stats4Nutshots20x20), BlueStonez.label_interparkbold_18pt_left);
    GUI.Label(new Rect(0.0f, height2 * 1f, (float) ((double) width / 2.0 - 5.0), height2), Singleton<EndOfMatchStats>.Instance.NutshotXP, BlueStonez.label_interparkbold_18pt_right);
    GUI.Label(new Rect(5f, height2 * 2f, (float) ((double) width / 2.0 + 1.0), height2), new GUIContent(LocalizedStrings.SmackdownXP, (Texture) ShopIcons.Stats2Smackdowns20x20), BlueStonez.label_interparkbold_18pt_left);
    GUI.Label(new Rect(0.0f, height2 * 2f, (float) ((double) width / 2.0 - 5.0), height2), Singleton<EndOfMatchStats>.Instance.SmackdownXP, BlueStonez.label_interparkbold_18pt_right);
    GUI.Label(new Rect(5f, height2 * 3f, (float) ((double) width / 2.0 + 1.0), height2), new GUIContent(LocalizedStrings.DamageXP, (Texture) ShopIcons.Stats5Damage20x20), BlueStonez.label_interparkbold_18pt_left);
    GUI.Label(new Rect(0.0f, height2 * 3f, (float) ((double) width / 2.0 - 5.0), height2), Singleton<EndOfMatchStats>.Instance.DamageXP, BlueStonez.label_interparkbold_18pt_right);
    GUI.EndGroup();
    GUI.EndGroup();
  }

  private void DrawRewards(Rect rect)
  {
    GUI.Button(new Rect(rect.x, rect.y, rect.width, 40f), string.Empty, BlueStonez.box_grey50);
    GUI.Label(new Rect(rect.x + 10f, rect.y + 2f, rect.width, 40f), "MY REWARDS", BlueStonez.label_interparkbold_18pt_left);
    float height = rect.height - 40f;
    GUI.BeginGroup(new Rect(rect.x, rect.y + 40f, rect.width, height), GUIContent.none, BlueStonez.window);
    GUI.BeginGroup(new Rect(0.0f, 0.0f, rect.width / 2f, height), GUIContent.none, BlueStonez.group_grey81);
    GUI.DrawTexture(new Rect(5f, (float) (((double) height - 20.0) / 2.0), 20f, 20f), (Texture) UberstrikeIcons.IconXP20x20);
    GUI.Label(new Rect(30f, (float) (((double) height - 20.0) / 2.0), 200f, 20f), "XP EARNED", BlueStonez.label_interparkbold_18pt_left);
    GUI.Label(new Rect(0.0f, (float) (((double) height - 20.0) / 2.0), (float) ((double) rect.width / 2.0 - 5.0), 20f), Singleton<EndOfMatchStats>.Instance.XPEarned, BlueStonez.label_interparkbold_18pt_right);
    GUI.EndGroup();
    GUI.BeginGroup(new Rect((float) ((double) rect.width / 2.0 - 1.0), 0.0f, rect.width / 2f, height), GUIContent.none, BlueStonez.group_grey81);
    GUI.DrawTexture(new Rect(5f, (float) (((double) height - 20.0) / 2.0), 20f, 20f), (Texture) ShopIcons.IconPoints20x20);
    GUI.Label(new Rect(30f, (float) (((double) height - 20.0) / 2.0), 200f, 20f), "POINTS EARNED", BlueStonez.label_interparkbold_18pt_left);
    GUI.Label(new Rect(0.0f, (float) (((double) height - 20.0) / 2.0), (float) ((double) rect.width / 2.0 - 5.0), 20f), Singleton<EndOfMatchStats>.Instance.PointsEarned, BlueStonez.label_interparkbold_18pt_right);
    GUI.EndGroup();
    GUI.EndGroup();
  }
}
