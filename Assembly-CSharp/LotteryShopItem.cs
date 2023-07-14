// Decompiled with JetBrains decompiler
// Type: LotteryShopItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;

public abstract class LotteryShopItem
{
  public int Id { get; set; }

  public string Name { get; set; }

  public DynamicTexture Icon { get; set; }

  public ItemPrice Price { get; set; }

  public BundleCategoryType Category { get; set; }

  public abstract string Description { get; }

  public abstract void Use();
}
