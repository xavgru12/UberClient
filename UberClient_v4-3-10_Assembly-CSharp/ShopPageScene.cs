// Decompiled with JetBrains decompiler
// Type: ShopPageScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ShopPageScene : PageScene
{
  public override PageType PageType => PageType.Shop;

  protected override void OnLoad()
  {
    if (GameState.HasCurrentGame)
      return;
    if ((bool) (Object) this._avatarAnchor)
      GameState.LocalDecorator.SetPosition(this._avatarAnchor.position, this._avatarAnchor.rotation);
    AutoMonoBehaviour<AvatarAnimationManager>.Instance.ResetAnimationState(this.PageType);
    if ((Object) GameState.LocalDecorator != (Object) null)
      GameState.LocalDecorator.HideWeapons();
    if (GameState.HasCurrentGame && !GameState.LocalPlayer.IsGamePaused)
      GameState.LocalPlayer.Pause();
    Singleton<ArmorHud>.Instance.Enabled = true;
  }

  protected override void OnUnload()
  {
    if (GameState.HasCurrentGame)
      return;
    Singleton<TemporaryLoadoutManager>.Instance.ResetGearLoadout();
    Singleton<ArmorHud>.Instance.Enabled = false;
  }
}
