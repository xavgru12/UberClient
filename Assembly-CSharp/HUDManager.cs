using UnityEngine;

public class HUDManager : MonoBehaviour
{
	[SerializeField]
	private PageControllerBase pregameLoadoutPage;

	[SerializeField]
	private PageControllerBase matchRunningPage;

	[SerializeField]
	private PageControllerBase endOfMatchPage;

	private void Start()
	{
		GameData.Instance.GameState.AddEventAndFire(delegate(GameStateId el)
		{
			bool flag = el == GameStateId.MatchRunning;
			bool active = el == GameStateId.PregameLoadout;
			bool flag2 = el == GameStateId.WaitingForPlayers;
			bool active2 = el == GameStateId.EndOfMatch;
			bool flag3 = el == GameStateId.PrepareNextRound;
			TrySetActive(pregameLoadoutPage, active);
			TrySetActive(matchRunningPage, flag | flag2 | flag3);
			TrySetActive(endOfMatchPage, active2);
			GameData.Instance.PlayerState.Fire();
		}, this);
		EventHandler.Global.AddListener<GlobalEvents.CameraWidthChanged>(OnCameraWidthChanged);
		OnCameraWidthChanged(null);
	}

	private void OnDestroy()
	{
		EventHandler.Global.RemoveListener<GlobalEvents.CameraWidthChanged>(OnCameraWidthChanged);
	}

	private void OnCameraWidthChanged(GlobalEvents.CameraWidthChanged obj)
	{
		UICamera.eventHandler.cachedCamera.rect = new Rect(0f, 0f, AutoMonoBehaviour<CameraRectController>.Instance.NormalizedWidth, 1f);
	}

	private void TrySetActive(MonoBehaviour page, bool active)
	{
		if (page != null)
		{
			page.gameObject.SetActive(active);
		}
	}
}
