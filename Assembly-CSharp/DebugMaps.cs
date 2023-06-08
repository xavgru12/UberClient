using UnityEngine;

public class DebugMaps : IDebugPage
{
	private Vector2 scroll;

	public string Title => "Maps";

	public void Draw()
	{
		scroll = GUILayout.BeginScrollView(scroll);
		foreach (UberstrikeMap allMap in Singleton<MapManager>.Instance.AllMaps)
		{
			GUILayout.Label(allMap.Id.ToString() + ", Modes: " + allMap.View.SupportedGameModes.ToString() + ", Item: " + allMap.View.RecommendedItemId.ToString() + ", Scene: " + allMap.SceneName);
		}
		GUILayout.EndScrollView();
	}
}
