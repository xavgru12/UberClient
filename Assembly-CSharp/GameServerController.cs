using System.Collections;

public class GameServerController : Singleton<GameServerController>
{
	public PhotonServer SelectedServer
	{
		get;
		set;
	}

	private GameServerController()
	{
	}

	public void JoinFastestServer()
	{
		UnityRuntime.StartRoutine(StartJoiningBestGameServer());
	}

	public void CreateOnFastestServer()
	{
		UnityRuntime.StartRoutine(StartCreatingOnBestGameServer());
	}

	private IEnumerator StartJoiningBestGameServer()
	{
		if (Singleton<GameServerController>.Instance.SelectedServer == null)
		{
			ProgressPopupDialog _autoJoinPopup = PopupSystem.ShowProgress(LocalizedStrings.LoadingGameList, LocalizedStrings.FindingAServerToJoin);
			yield return UnityRuntime.StartRoutine(Singleton<GameServerManager>.Instance.StartUpdatingLatency(delegate(float progress)
			{
				_autoJoinPopup.Progress = progress;
			}));
			PhotonServer bestServer = Singleton<GameServerManager>.Instance.GetBestServer();
			if (bestServer == null)
			{
				PopupSystem.HideMessage(_autoJoinPopup);
				PopupSystem.ShowMessage("Could not find server", "No suitable server could be located! Please try again soon.");
				yield break;
			}
			Singleton<GameServerController>.Instance.SelectedServer = bestServer;
			PopupSystem.HideMessage(_autoJoinPopup);
		}
		MenuPageManager.Instance.LoadPage(PageType.Play);
	}

	private IEnumerator StartCreatingOnBestGameServer()
	{
		if (Singleton<GameServerController>.Instance.SelectedServer == null)
		{
			ProgressPopupDialog _autoJoinPopup = PopupSystem.ShowProgress(LocalizedStrings.LoadingGameList, LocalizedStrings.FindingAServerToJoin);
			yield return UnityRuntime.StartRoutine(Singleton<GameServerManager>.Instance.StartUpdatingLatency(delegate(float progress)
			{
				_autoJoinPopup.Progress = progress;
			}));
			PhotonServer bestServer = Singleton<GameServerManager>.Instance.GetBestServer();
			if (bestServer == null)
			{
				PopupSystem.HideMessage(_autoJoinPopup);
				PopupSystem.ShowMessage("Could not find server", "No suitable server could be located! Please try again soon.");
				yield break;
			}
			Singleton<GameServerController>.Instance.SelectedServer = bestServer;
			PopupSystem.HideMessage(_autoJoinPopup);
		}
		PanelManager.Instance.OpenPanel(PanelType.CreateGame);
	}
}
