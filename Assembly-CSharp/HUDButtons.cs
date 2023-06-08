using UberStrike.Core.Types;
using UnityEngine;

public class HUDButtons : MonoBehaviour
{
	[SerializeField]
	private UIEventReceiver continueButton;

	[SerializeField]
	private UIEventReceiver respawnButton;

	[SerializeField]
	private UIEventReceiver changeTeamButton;

	[SerializeField]
	private UIEventReceiver loadoutButton;

	[SerializeField]
	private UILabel loadoutButtonLabel;

	private void Start()
	{
		continueButton.gameObject.SetActive(value: false);
		respawnButton.gameObject.SetActive(value: false);
		changeTeamButton.gameObject.SetActive(value: false);
		GameData.Instance.PlayerState.AddEventAndFire(delegate(PlayerStateId el)
		{
			bool flag2 = el == PlayerStateId.Paused;
			bool flag3 = el == PlayerStateId.Killed;
			bool flag4 = GameState.Current.GameMode == GameModeType.None;
			respawnButton.gameObject.SetActive(flag3 && flag4);
			continueButton.gameObject.SetActive(flag2);
			changeTeamButton.gameObject.SetActive(flag2 && GameStateHelper.CanChangeTeam());
			loadoutButton.gameObject.SetActive(flag2 | flag3);
			loadoutButtonLabel.text = ((!flag2 | flag4) ? "Loadout" : "Chat");
		}, this);
		GameData.Instance.OnRespawnCountdown.AddEvent(delegate(int el)
		{
			bool flag = el == 0;
			respawnButton.gameObject.SetActive(flag);
			changeTeamButton.gameObject.SetActive(flag && GameStateHelper.CanChangeTeam());
		}, this);
		continueButton.OnClicked = delegate
		{
			if (!(PanelManager.Instance != null) || (!PanelManager.Instance.IsPanelOpen(PanelType.Options) && !PanelManager.Instance.IsPanelOpen(PanelType.Help)))
			{
				InputManager.SkipFrame = Time.frameCount;
				GameState.Current.PlayerState.PopState();
				EventHandler.Global.Fire(new GameEvents.PlayerUnpause());
				GamePageManager.Instance.UnloadCurrentPage();
			}
		};
		respawnButton.OnClicked = delegate
		{
			RenderSettingsController.Instance.ResetInterpolation();
			if (!(PanelManager.Instance != null) || (!PanelManager.Instance.IsPanelOpen(PanelType.Options) && !PanelManager.Instance.IsPanelOpen(PanelType.Help)))
			{
				respawnButton.gameObject.SetActive(value: false);
				changeTeamButton.gameObject.SetActive(value: false);
				if (GameState.Current.GameMode == GameModeType.None)
				{
					GameStateHelper.RespawnLocalPlayerAtRandom();
					GameState.Current.PlayerState.SetState(PlayerStateId.Playing);
				}
				else
				{
					GameState.Current.Actions.RequestRespawn();
				}
				GamePageManager.Instance.UnloadCurrentPage();
			}
		};
		changeTeamButton.OnClicked = delegate
		{
			respawnButton.gameObject.SetActive(value: false);
			changeTeamButton.gameObject.SetActive(value: false);
			GamePageManager.Instance.UnloadCurrentPage();
			GameData.Instance.OnNotification.Fire("Changing Team...");
			GameState.Current.Actions.ChangeTeam();
			if (GameData.Instance.PlayerState.Value == PlayerStateId.Killed)
			{
				GameState.Current.Actions.RequestRespawn();
			}
		};
		loadoutButton.OnClicked = delegate
		{
			if (GamePageManager.IsCurrentPage(IngamePageType.None))
			{
				if (GameState.Current.IsSinglePlayer)
				{
					GamePageManager.Instance.LoadPage(IngamePageType.PausedOffline);
				}
				else if (!GameState.Current.IsMatchRunning || !GameState.Current.PlayerData.IsAlive)
				{
					GamePageManager.Instance.LoadPage(IngamePageType.PausedWaiting);
				}
				else
				{
					GamePageManager.Instance.LoadPage(IngamePageType.Paused);
				}
			}
			else
			{
				GamePageManager.Instance.UnloadCurrentPage();
			}
		};
	}

	private void OnEnable()
	{
		continueButton.gameObject.SetActive(value: false);
		respawnButton.gameObject.SetActive(value: false);
		changeTeamButton.gameObject.SetActive(value: false);
		loadoutButton.gameObject.SetActive(value: false);
		GameData.Instance.PlayerState.Fire();
	}
}
