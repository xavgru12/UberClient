// Decompiled with JetBrains decompiler
// Type: UberstrikeMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;

public class UberstrikeMap
{
  public UberstrikeMap(MapView view)
  {
    this.View = view;
    this.IsVisible = true;
    this.Icon = new DynamicTexture(ApplicationDataManager.BaseImageURL + "MapIcons/" + this.View.SceneName + ".jpg");
  }

  public bool IsVisible { get; set; }

  public MapView View { get; private set; }

  public DynamicTexture Icon { get; private set; }

  public bool IsBuiltIn { get; set; }

  public bool IsBluebox => this.View.IsBlueBox;

  public int Id => this.View.MapId;

  public string Name => this.View.DisplayName;

  public string Description => this.View.Description;

  public string SceneName => this.View.SceneName;

  public bool IsGameModeSupported(GameModeType mode) => this.View.Settings != null && this.View.Settings.ContainsKey(mode);
}
