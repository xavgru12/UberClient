public class StatsPageScene : PageScene
{
	public override PageType PageType => PageType.Stats;

	protected override void OnLoad()
	{
		if ((bool)_avatarAnchor)
		{
			GameState.Current.Avatar.Decorator.SetPosition(_avatarAnchor.position, _avatarAnchor.rotation);
		}
	}
}
