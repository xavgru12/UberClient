// Decompiled with JetBrains decompiler
// Type: PointsUnityItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class PointsUnityItem : IUnityItem
{
  public PointsUnityItem(int points)
  {
    this.Name = points.ToString("N0") + " Points";
    PointsUnityItem.DummyItemView dummyItemView = new PointsUnityItem.DummyItemView();
    dummyItemView.Description = string.Format("An extra {0:N0} Points to fatten up your UberWallet!", (object) points);
    this.ItemView = (BaseUberStrikeItemView) dummyItemView;
    this.ItemId = 1;
  }

  public Texture2D Icon
  {
    get => ShopIcons.Points48x48;
    set
    {
    }
  }

  public int ItemId { get; set; }

  public string Name { get; private set; }

  public string PrefabName => string.Empty;

  public UberstrikeItemType ItemType { get; private set; }

  public UberstrikeItemClass ItemClass { get; private set; }

  public BaseUberStrikeItemView ItemView { get; private set; }

  public MonoBehaviour Prefab { get; set; }

  private class DummyItemView : BaseUberStrikeItemView
  {
    public override UberstrikeItemType ItemType => UberstrikeItemType.Special;
  }
}
