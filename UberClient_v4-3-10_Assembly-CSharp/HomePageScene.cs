// Decompiled with JetBrains decompiler
// Type: HomePageScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class HomePageScene : PageScene
{
  private bool _lastIsPlayerInClan;
  private string _lastClanTag;

  public override PageType PageType => PageType.Home;

  protected override void OnLoad()
  {
    if ((bool) (Object) this._avatarAnchor)
      GameState.LocalDecorator.SetPosition(this._avatarAnchor.position, this._avatarAnchor.rotation);
    if ((Object) GameState.LocalDecorator != (Object) null)
      GameState.LocalDecorator.HideWeapons();
    AutoMonoBehaviour<AvatarAnimationManager>.Instance.ResetAnimationState(this.PageType);
    Singleton<EventPopupManager>.Instance.ShowNextPopup(1);
  }

  private void Update()
  {
    if (this._lastIsPlayerInClan == PlayerDataManager.IsPlayerInClan && !(this._lastClanTag != PlayerDataManager.ClanTag))
      return;
    GameState.LocalDecorator.HudInformation.SetAvatarLabel(!PlayerDataManager.IsPlayerInClan ? PlayerDataManager.Name : string.Format("[{0}] {1}", (object) PlayerDataManager.ClanTag, (object) PlayerDataManager.Name));
    this._lastIsPlayerInClan = PlayerDataManager.IsPlayerInClan;
    this._lastClanTag = PlayerDataManager.ClanTag;
  }
}
