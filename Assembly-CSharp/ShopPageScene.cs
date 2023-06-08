public class ShopPageScene : PageScene
{
	public override PageType PageType => PageType.Shop;

	protected override void OnLoad()
	{
		if (!GameState.Current.HasJoinedGame)
		{
			if ((bool)_avatarAnchor)
			{
				GameState.Current.Player.ResetShopCharPos(_avatarAnchor.rotation);
				GameState.Current.Avatar.Decorator.SetPosition(_avatarAnchor.position, _avatarAnchor.rotation);
			}
			if (GameState.Current.Avatar != null)
			{
				GameState.Current.Avatar.HideWeapons();
			}
			EventHandler.Global.Fire(new GameEvents.PlayerPause());
		}
	}

	protected override void OnUnload()
	{
		if (!GameState.Current.HasJoinedGame)
		{
			Singleton<TemporaryLoadoutManager>.Instance.ResetLoadout();
		}
	}
}
