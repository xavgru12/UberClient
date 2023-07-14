// Decompiled with JetBrains decompiler
// Type: CreditsUnityItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class CreditsUnityItem : IUnityItem
{
  public CreditsUnityItem(int credits)
  {
    this.Name = credits.ToString("N0") + " Credits";
    CreditsUnityItem.DummyItemView dummyItemView = new CreditsUnityItem.DummyItemView();
    dummyItemView.Description = string.Format("An extra {0:N0} Credits to fatten up your UberWallet!", (object) credits);
    this.ItemView = (BaseUberStrikeItemView) dummyItemView;
    this.ItemId = 1;
  }

  public string PrefabName => string.Empty;

  public Texture2D Icon
  {
    get => ShopIcons.CreditsIcon48x48;
    set
    {
    }
  }

  public int ItemId { get; set; }

  public string Name { get; private set; }

  public UberstrikeItemType ItemType { get; private set; }

  public UberstrikeItemClass ItemClass { get; private set; }

  public BaseUberStrikeItemView ItemView { get; private set; }

  public MonoBehaviour Prefab { get; set; }

  private class DummyItemView : BaseUberStrikeItemView
  {
    public override UberstrikeItemType ItemType => UberstrikeItemType.Special;
  }
}
