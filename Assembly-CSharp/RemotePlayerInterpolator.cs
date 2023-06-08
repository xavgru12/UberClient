using System.Collections.Generic;
using UberStrike.Core.Models;
using UnityEngine;

public class RemotePlayerInterpolator
{
	private Dictionary<byte, RemoteCharacterState> remoteStates;

	public RemotePlayerInterpolator()
	{
		remoteStates = new Dictionary<byte, RemoteCharacterState>(20);
	}

	public void Update()
	{
		foreach (RemoteCharacterState value in remoteStates.Values)
		{
			value.InterpolateMovement();
		}
	}

	public void PositionUpdate(PlayerMovement update, ushort gameFrame)
	{
		if (remoteStates.TryGetValue(update.Number, out RemoteCharacterState value))
		{
			value.PositionUpdate(update, gameFrame);
		}
	}

	public void DeltaUpdate(GameActorInfoDelta delta)
	{
		if (remoteStates.TryGetValue(delta.Id, out RemoteCharacterState value))
		{
			value.DeltaUpdate(delta);
		}
	}

	public void UpdatePositionHard(byte playerNumber, Vector3 pos)
	{
		if (remoteStates.TryGetValue(playerNumber, out RemoteCharacterState value))
		{
			value.SetPosition(pos);
		}
	}

	public void AddCharacterInfo(GameActorInfo player, PlayerMovement position)
	{
		remoteStates[player.PlayerId] = new RemoteCharacterState(player, position);
	}

	public void Reset()
	{
		remoteStates.Clear();
	}

	public void RemoveCharacterInfo(byte playerID)
	{
		if (remoteStates.TryGetValue(playerID, out RemoteCharacterState value))
		{
			remoteStates.Remove(value.Player.PlayerId);
		}
	}

	public RemoteCharacterState GetState(byte playerID)
	{
		RemoteCharacterState value = null;
		remoteStates.TryGetValue(playerID, out value);
		return value;
	}
}
