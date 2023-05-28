// Decompiled with JetBrains decompiler
// Type: CombatRangeTier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class CombatRangeTier
{
  public int CloseRange = 1;
  public int MediumRange = 1;
  public int LongRange = 1;

  public CombatRangeCategory RangeCategory
  {
    get
    {
      int num = Mathf.Max(new int[3]
      {
        this.CloseRange,
        this.MediumRange,
        this.LongRange
      });
      return (CombatRangeCategory) ((this.CloseRange != num ? 0 : 1) | (this.MediumRange != num ? 0 : 2) | (this.LongRange != num ? 0 : 4));
    }
  }

  public int GetTierForRange(CombatRangeCategory range)
  {
    switch (range)
    {
      case CombatRangeCategory.Close:
        return this.CloseRange;
      case CombatRangeCategory.Medium:
        return this.MediumRange;
      case CombatRangeCategory.CloseMedium:
        return Mathf.RoundToInt((float) (this.CloseRange + this.MediumRange) / 2f);
      case CombatRangeCategory.Far:
        return this.LongRange;
      case CombatRangeCategory.MediumFar:
        return Mathf.RoundToInt((float) (this.MediumRange + this.LongRange) / 2f);
      default:
        return Mathf.RoundToInt((float) (this.CloseRange + this.MediumRange + this.LongRange) / 3f);
    }
  }
}
