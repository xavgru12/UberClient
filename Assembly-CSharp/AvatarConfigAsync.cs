using System.Collections;
using UberStrike.Core.Models;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class AvatarConfigAsync : MonoBehaviour
{
	public static AvatarConfigAsync Instance;

	private static bool isLoading;

	private void Awake()
	{
		Instance = this;
	}

	public void CreateAvatar(GameActorInfo info, CharacterConfig character, bool isLocal, bool isTraining = false)
	{
		StartCoroutine(ConfigAvatar(info, character, isLocal, isTraining));
	}

	public IEnumerator ConfigAvatar(GameActorInfo info, CharacterConfig character, bool isLocal, bool isTraining)
	{
		while (isLoading)
		{
			yield return new WaitForSeconds(0.1f);
		}
		if (character != null && info != null)
		{
			if (isLocal)
			{
				GameState.Current.Player.SetCurrentCharacterConfig(character);
				if (!isTraining)
				{
					UberKill.IsLowGravity = GameFlags.IsFlagSet(GameFlags.GAME_FLAGS.LowGravity, GameState.Current.RoomData.GameFlags);
				}
				character.Initialize(GameState.Current.PlayerData, GameState.Current.Avatar);
			}
			else
			{
				AvatarGearParts avatargearparts = new AvatarGearParts();
				Avatar avatar = new Avatar(new Loadout(info.Gear, info.Weapons), local: false);
				yield return StartCoroutine(avatar.Loadout.GetGearPart(delegate(AvatarGearParts callback)
				{
					avatargearparts = callback;
				}));
				avatar.SetDecorator(AvatarBuilder.CreateRemoteAvatar(avatargearparts, info.SkinColor));
				character.Initialize(GameState.Current.RemotePlayerStates.GetState(info.PlayerId), avatar);
				GameData.Instance.OnHUDStreamMessage.Fire(info, LocalizedStrings.JoinedTheGame, null);
			}
			if (!info.IsAlive)
			{
				character.SetDead(Vector3.zero);
			}
		}
		else
		{
			Debug.LogError($"OnAvatarLoaded failed because loaded Avatar is {character != null} and Info is {info != null}");
		}
		isLoading = false;
	}
}
