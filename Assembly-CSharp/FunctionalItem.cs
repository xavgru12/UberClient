using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class FunctionalItem : IUnityItem
{
	private Texture2D _icon;

	public bool Equippable => false;

	public string Name
	{
		get
		{
			return View.Name;
		}
		set
		{
			View.Name = value;
		}
	}

	public UberstrikeItemClass ItemClass => View.ItemClass;

	public string PrefabName => string.Empty;

	public bool IsLoaded => true;

	public GameObject Prefab => null;

	public BaseUberStrikeItemView View
	{
		get;
		private set;
	}

	public FunctionalItem(BaseUberStrikeItemView view)
	{
		View = view;
		_icon = UnityItemConfiguration.Instance.GetFunctionalItemIcon(view.ID);
	}

	public void Unload()
	{
	}

	public GameObject Create(Vector3 position, Quaternion rotation)
	{
		return null;
	}

	public void DrawIcon(Rect position)
	{
		if (_icon != null)
		{
			GUI.DrawTexture(position, _icon);
		}
		else
		{
			Debug.LogError("Can't find icon for item:" + View.ID.ToString() + ", " + View.Name);
		}
	}
}
