// Decompiled with JetBrains decompiler
// Type: BundleUnityView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;

public class BundleUnityView
{
  public BundleUnityView(BundleView bundleView)
  {
    this.BundleView = bundleView;
    this.Icon = new DynamicTexture(this.BundleView.IconUrl);
    this.Image = new DynamicTexture(this.BundleView.ImageUrl);
  }

  public BundleView BundleView { get; private set; }

  public string CurrencySymbol { get; set; }

  public string Price { get; set; }

  public DynamicTexture Icon { get; private set; }

  public DynamicTexture Image { get; private set; }

  public bool IsOwned { get; set; }

  public bool IsValid => !string.IsNullOrEmpty(this.Price);

  public BundleCategoryType Category => this.BundleView.Category;
}
