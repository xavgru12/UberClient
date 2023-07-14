// Decompiled with JetBrains decompiler
// Type: LuckyDrawShopItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;

public class LuckyDrawShopItem : LotteryShopItem
{
  public LuckyDrawUnityView View { get; set; }

  public List<LuckyDrawShopItem.LuckyDrawSet> Sets { get; set; }

  public override string Description => this.View.Description;

  public override void Use() => Singleton<LotteryManager>.Instance.RunLuckyDraw(this);

  public class LuckyDrawSet
  {
    public int Id { get; set; }

    public DynamicTexture Image { get; set; }

    public List<IUnityItem> Items { get; set; }

    public LuckyDrawShopItem Parent { get; set; }

    public LuckyDrawSetUnityView View { get; set; }
  }
}
