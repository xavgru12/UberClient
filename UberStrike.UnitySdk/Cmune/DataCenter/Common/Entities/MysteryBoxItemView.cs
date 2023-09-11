// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.MysteryBoxItemView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

namespace Cmune.DataCenter.Common.Entities
{
  public class MysteryBoxItemView
  {
    public int Id { get; set; }

    public int ItemId { get; set; }

    public string Name { get; set; }

    public int Amount { get; set; }

    public BuyingDurationType DurationType { get; set; }

    public int ItemWeight { get; set; }

    public int MysteryBoxId { get; set; }
  }
}
