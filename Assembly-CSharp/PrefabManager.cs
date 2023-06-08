using UnityEngine;

public class PrefabManager : MonoBehaviour
{
	[SerializeField]
	private LocalPlayer _player;

	[SerializeField]
	private AvatarDecorator _defaultAvatar;

	[SerializeField]
	private AvatarDecoratorConfig _defaultRagdoll;

	[SerializeField]
	private CharacterConfig _remoteCharacter;

	[SerializeField]
	private CharacterConfig _localCharacter;

	public static PrefabManager Instance
	{
		get;
		private set;
	}

	public static bool Exists => Instance != null;

	public AvatarDecorator DefaultAvatar => _defaultAvatar;

	public AvatarDecoratorConfig DefaultRagdoll => _defaultRagdoll;

	public CharacterConfig InstantiateLocalCharacter()
	{
		return Object.Instantiate(_localCharacter) as CharacterConfig;
	}

	public CharacterConfig InstantiateRemoteCharacter()
	{
		return Object.Instantiate(_remoteCharacter) as CharacterConfig;
	}

	public LocalPlayer InstantiateLocalPlayer()
	{
		LocalPlayer localPlayer = Object.Instantiate(_player) as LocalPlayer;
		Object.DontDestroyOnLoad(localPlayer);
		localPlayer.SetEnabled(enabled: false);
		return localPlayer;
	}

	private void Awake()
	{
		Instance = this;
	}
}
