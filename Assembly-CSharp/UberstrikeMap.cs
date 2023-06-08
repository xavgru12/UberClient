using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;

public class UberstrikeMap
{
	public bool IsVisible
	{
		get;
		set;
	}

	public MapView View
	{
		get;
		private set;
	}

	public DynamicTexture Icon
	{
		get;
		private set;
	}

	public bool IsBuiltIn
	{
		get;
		set;
	}

	public int Id => View.MapId;

	public string Name => View.DisplayName;

	public string Description => View.Description;

	public string SceneName => View.SceneName;

	public string MapIconUrl
	{
		get;
		private set;
	}

	public UberstrikeMap(MapView view)
	{
		View = view;
		IsVisible = true;
		MapIconUrl = ApplicationDataManager.ImagePath + "maps/" + View.SceneName + ".jpg";
		bool loadNow = View.SceneName != "Menu";
		Icon = new DynamicTexture(MapIconUrl, loadNow);
	}

	public bool IsGameModeSupported(GameModeType mode)
	{
		if (View.Settings != null)
		{
			return View.Settings.ContainsKey(mode);
		}
		return false;
	}
}
