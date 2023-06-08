public class PlayPageScene : PageScene
{
	public override PageType PageType => PageType.Play;

	protected override void OnLoad()
	{
		PlayPageGUI.Instance.Show();
	}

	protected override void OnUnload()
	{
		PlayPageGUI.Instance.Hide();
	}

	private void OnDisable()
	{
		PlayPageGUI.Instance.Hide();
	}
}
