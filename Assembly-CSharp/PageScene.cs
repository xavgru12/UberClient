using UnityEngine;

public abstract class PageScene : MonoBehaviour
{
	[SerializeField]
	protected Vector3 _mouseOrbitConfig;

	[SerializeField]
	private Vector3 _mouseOrbitPivot;

	[SerializeField]
	protected Transform _avatarAnchor;

	[SerializeField]
	protected int _guiWidth = -1;

	[SerializeField]
	private bool _haveMouseOrbitCamera;

	public bool HaveMouseOrbitCamera => _haveMouseOrbitCamera;

	public int GuiWidth => _guiWidth;

	public Transform AvatarAnchor => _avatarAnchor;

	public Vector3 MouseOrbitConfig => _mouseOrbitConfig;

	public Vector3 MouseOrbitPivot => _mouseOrbitPivot;

	public abstract PageType PageType
	{
		get;
	}

	public bool IsEnabled => base.gameObject.activeSelf;

	private void Awake()
	{
		_mouseOrbitConfig.x = (_mouseOrbitConfig.x + 360f) % 360f;
		_mouseOrbitConfig.y = (_mouseOrbitConfig.y + 360f) % 360f;
	}

	public void Load()
	{
		base.gameObject.SetActive(value: true);
		OnLoad();
	}

	public void Unload()
	{
		base.gameObject.SetActive(value: false);
		OnUnload();
	}

	protected virtual void OnLoad()
	{
	}

	protected virtual void OnUnload()
	{
	}
}
