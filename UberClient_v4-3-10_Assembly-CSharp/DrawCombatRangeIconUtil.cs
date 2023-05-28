// Decompiled with JetBrains decompiler
// Type: DrawCombatRangeIconUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class DrawCombatRangeIconUtil
{
  private static Texture2D[] _closeRange = new Texture2D[5]
  {
    WeaponRange.IconRange02,
    WeaponRange.IconRange05,
    WeaponRange.IconRange08,
    WeaponRange.IconRange11,
    WeaponRange.CombatRangeClose
  };
  private static Texture2D[] _midRange = new Texture2D[5]
  {
    WeaponRange.IconRange03,
    WeaponRange.IconRange06,
    WeaponRange.IconRange09,
    WeaponRange.IconRange12,
    WeaponRange.CombatRangeMedium
  };
  private static Texture2D[] _farRange = new Texture2D[5]
  {
    WeaponRange.IconRange04,
    WeaponRange.IconRange07,
    WeaponRange.IconRange10,
    WeaponRange.IconRange13,
    WeaponRange.CombatRangeFar
  };
  public static int _warningRange;

  public static void DrawWeaponRangeIcon2(Rect rect, params WeaponItem[] weapons)
  {
    int close;
    int medium;
    int far;
    DrawCombatRangeIconUtil.GetBestTiersPerCombatRange(out close, out medium, out far, weapons);
    GUI.color = new Color(1f, 0.0f, 0.0f, 0.25f * (float) close);
    GUI.DrawTexture(rect, (Texture) WeaponRange.CombatRangeClose);
    GUI.color = new Color(1f, 0.0f, 0.0f, 0.25f * (float) medium);
    GUI.DrawTexture(rect, (Texture) WeaponRange.CombatRangeMedium);
    GUI.color = new Color(1f, 0.0f, 0.0f, 0.25f * (float) far);
    GUI.DrawTexture(rect, (Texture) WeaponRange.CombatRangeFar);
    GUI.color = Color.white;
    GUI.DrawTexture(rect, (Texture) WeaponRange.CombatRangeBackground);
  }

  public static void DrawRecommendedCombatRange(
    Rect rect,
    CombatRangeTier mapRange,
    params WeaponItem[] weapons)
  {
    int close;
    int medium;
    int far;
    DrawCombatRangeIconUtil.GetBestTiersPerCombatRange(out close, out medium, out far, weapons);
    if (rect.Contains(Event.current.mousePosition))
    {
      string toolTip1 = DrawCombatRangeIconUtil.CreateToolTip(close, "You need a shotgun or a pistol here !");
      string toolTip2 = DrawCombatRangeIconUtil.CreateToolTip(medium, "You need a splatter, a canon or a machinegun here !");
      string toolTip3 = DrawCombatRangeIconUtil.CreateToolTip(far, "You need a sniper rifle here !");
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Combat Efficiency");
      stringBuilder.Append("Long: ").AppendLine(toolTip3);
      stringBuilder.Append("Medium: ").AppendLine(toolTip2);
      stringBuilder.Append("Close: ").AppendLine(toolTip1);
      GUI.tooltip = stringBuilder.ToString();
    }
    CombatRangeCategory range = (CombatRangeCategory) 0;
    if (close - mapRange.CloseRange < 0)
      range |= CombatRangeCategory.Close;
    if (medium - mapRange.MediumRange < 0)
      range |= CombatRangeCategory.Medium;
    if (far - mapRange.LongRange < 0)
      range |= CombatRangeCategory.Far;
    if (range != (CombatRangeCategory) 0)
    {
      GUI.color = Color.white.SetAlpha((float) (((double) Mathf.Sin(Time.time * 9f) + 1.0) * 0.30000001192092896));
      foreach (Texture2D rangeTexture in DrawCombatRangeIconUtil.GetRangeTextures(range))
        GUI.DrawTexture(rect, (Texture) rangeTexture);
      GUI.color = Color.white;
    }
    DrawCombatRangeIconUtil._warningRange = (int) range;
  }

  private static string CreateToolTip(int tier, string defaultText)
  {
    switch (tier)
    {
      case 0:
        return "Weak";
      case 1:
        return "Average";
      case 2:
        return "Good";
      case 3:
        return "Excellent";
      default:
        return defaultText;
    }
  }

  private static void GetBestTiersPerCombatRange(
    out int close,
    out int medium,
    out int far,
    WeaponItem[] weapons)
  {
    close = 0;
    medium = 0;
    far = 0;
    foreach (WeaponItem weapon in weapons)
    {
      if ((Object) weapon != (Object) null && DrawCombatRangeIconUtil.IsCloseRange(weapon.Configuration.CombatRange))
        close = Mathf.Max(close, weapon.Configuration.Tier);
      if ((Object) weapon != (Object) null && DrawCombatRangeIconUtil.IsMidRange(weapon.Configuration.CombatRange))
        medium = Mathf.Max(medium, weapon.Configuration.Tier);
      if ((Object) weapon != (Object) null && DrawCombatRangeIconUtil.IsFarRange(weapon.Configuration.CombatRange))
        far = Mathf.Max(far, weapon.Configuration.Tier);
    }
    close = Mathf.Min(close, 3);
    medium = Mathf.Min(medium, 3);
    far = Mathf.Min(far, 3);
  }

  private static IEnumerable<Texture2D> GetRangeTextures(CombatRangeCategory range)
  {
    switch (range)
    {
      case CombatRangeCategory.Close:
        return (IEnumerable<Texture2D>) new Texture2D[1]
        {
          DrawCombatRangeIconUtil._closeRange[4]
        };
      case CombatRangeCategory.Medium:
        return (IEnumerable<Texture2D>) new Texture2D[1]
        {
          DrawCombatRangeIconUtil._midRange[4]
        };
      case CombatRangeCategory.CloseMedium:
        return (IEnumerable<Texture2D>) new Texture2D[2]
        {
          DrawCombatRangeIconUtil._closeRange[4],
          DrawCombatRangeIconUtil._midRange[4]
        };
      case CombatRangeCategory.Far:
        return (IEnumerable<Texture2D>) new Texture2D[1]
        {
          DrawCombatRangeIconUtil._farRange[4]
        };
      case CombatRangeCategory.CloseFar:
        return (IEnumerable<Texture2D>) new Texture2D[2]
        {
          DrawCombatRangeIconUtil._closeRange[4],
          DrawCombatRangeIconUtil._farRange[4]
        };
      case CombatRangeCategory.MediumFar:
        return (IEnumerable<Texture2D>) new Texture2D[2]
        {
          DrawCombatRangeIconUtil._midRange[4],
          DrawCombatRangeIconUtil._farRange[4]
        };
      case CombatRangeCategory.CloseMediumFar:
        return (IEnumerable<Texture2D>) new Texture2D[3]
        {
          DrawCombatRangeIconUtil._closeRange[4],
          DrawCombatRangeIconUtil._midRange[4],
          DrawCombatRangeIconUtil._farRange[4]
        };
      default:
        return (IEnumerable<Texture2D>) new Texture2D[0];
    }
  }

  public static bool IsCloseRange(CombatRangeCategory range) => (range & CombatRangeCategory.Close) == CombatRangeCategory.Close;

  public static bool IsMidRange(CombatRangeCategory range) => (range & CombatRangeCategory.Medium) == CombatRangeCategory.Medium;

  public static bool IsFarRange(CombatRangeCategory range) => (range & CombatRangeCategory.Far) == CombatRangeCategory.Far;

  public static Texture2D GetIconByRange(CombatRangeCategory range)
  {
    switch (range)
    {
      case CombatRangeCategory.Close:
        return WeaponRange.CombatRangeMiniClose;
      case CombatRangeCategory.Medium:
        return WeaponRange.CombatRangeMiniMed;
      case CombatRangeCategory.CloseMedium:
        return WeaponRange.CombatRangeMiniCloseMed;
      case CombatRangeCategory.Far:
        return WeaponRange.CombatRangeMiniFar;
      case CombatRangeCategory.MediumFar:
        return WeaponRange.CombatRangeMiniMedFar;
      default:
        Debug.LogWarning((object) ("Cannot find corresponding icon for range - [" + (object) range + "]."));
        return WeaponRange.CombatRangeMiniMedFar;
    }
  }
}
