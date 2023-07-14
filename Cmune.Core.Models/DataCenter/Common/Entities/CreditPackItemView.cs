// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.CreditPackItemView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

namespace Cmune.DataCenter.Common.Entities
{
  public class CreditPackItemView
  {
    public int CreditPackId { get; set; }

    public int ItemId { get; set; }

    public BuyingDurationType Duration { get; set; }

    public ItemView ItemView { get; set; }
  }
}
