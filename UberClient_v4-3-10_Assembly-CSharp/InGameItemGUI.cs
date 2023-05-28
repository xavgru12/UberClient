// Decompiled with JetBrains decompiler
// Type: InGameItemGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class InGameItemGUI : BaseItemGUI
{
  private string _promotionalText = string.Empty;

  public InGameItemGUI(
    IUnityItem item,
    string promotionalText,
    BuyingLocationType location,
    BuyingRecommendationType recommendation)
    : base(item, location, recommendation)
  {
    this._promotionalText = promotionalText;
  }

  public override void Draw(Rect rect, bool selected)
  {
    GUI.BeginGroup(rect);
    this.DrawIcon(new Rect(4f, (float) (((double) rect.height - 48.0) / 2.0), 48f, 48f));
    GUI.contentColor = ColorScheme.UberStrikeYellow;
    GUI.Label(new Rect(60f, (float) ((double) rect.height / 2.0 - 18.0), rect.width, 18f), this._promotionalText, BlueStonez.label_interparkbold_16pt_left);
    GUI.contentColor = Color.white;
    GUI.Label(new Rect(60f, (float) ((double) rect.height / 2.0 + 2.0), rect.width, 16f), this.Item.Name, BlueStonez.label_interparkbold_16pt_left);
    if (Singleton<InventoryManager>.Instance.TryGetInventoryItem(this.Item.ItemId, out InventoryItem _))
    {
      if (Singleton<LoadoutManager>.Instance.IsItemEquipped(this.Item.ItemId))
        GUI.Label(new Rect(rect.width - 80f, (float) ((double) rect.height / 2.0 - 25.0), 80f, 22f), new GUIContent("EQUIPPED", (Texture) ShopIcons.CheckMark), BlueStonez.label_interparkbold_11pt_left);
      else
        this.DrawEquipButton(new Rect(rect.width - 80f, (float) ((double) rect.height / 2.0 - 25.0), 80f, 22f), "EQUIP NOW");
    }
    else
      this.DrawBuyButton(new Rect(rect.width - 80f, (float) ((double) rect.height / 2.0 - 25.0), 80f, 22f), "BUY NOW");
    GUI.EndGroup();
    if (Event.current.type != UnityEngine.EventType.MouseDown || !rect.Contains(Event.current.mousePosition))
      return;
    CmuneEventHandler.Route((object) new SelectShopItemEvent()
    {
      Item = this.Item
    });
  }
}
