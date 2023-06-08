using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace UberStrike.Core.Models
{
	public class GameActorInfoDelta
	{
		public enum Keys
		{
			AccessLevel,
			ArmorPointCapacity,
			ArmorPoints,
			Channel,
			ClanTag,
			Cmid,
			CurrentFiringMode,
			CurrentWeaponSlot,
			Deaths,
			FunctionalItems,
			Gear,
			Health,
			Kills,
			Level,
			Ping,
			PlayerId,
			PlayerName,
			PlayerState,
			QuickItems,
			Rank,
			SkinColor,
			StepSound,
			TeamID,
			Weapons
		}

		public readonly Dictionary<Keys, object> Changes = new Dictionary<Keys, object>();

		public int DeltaMask
		{
			get;
			set;
		}

		public byte Id
		{
			get;
			set;
		}

		public void Apply(GameActorInfo instance)
		{
			foreach (KeyValuePair<Keys, object> change in Changes)
			{
				switch (change.Key)
				{
				case Keys.AccessLevel:
					instance.AccessLevel = (MemberAccessLevel)(int)change.Value;
					break;
				case Keys.ArmorPointCapacity:
					instance.ArmorPointCapacity = (byte)change.Value;
					break;
				case Keys.ArmorPoints:
					instance.ArmorPoints = (byte)change.Value;
					break;
				case Keys.Channel:
					instance.Channel = (ChannelType)(int)change.Value;
					break;
				case Keys.ClanTag:
					instance.ClanTag = (string)change.Value;
					break;
				case Keys.Cmid:
					instance.Cmid = (int)change.Value;
					break;
				case Keys.CurrentFiringMode:
					instance.CurrentFiringMode = (FireMode)(int)change.Value;
					break;
				case Keys.CurrentWeaponSlot:
					instance.CurrentWeaponSlot = (byte)change.Value;
					break;
				case Keys.Deaths:
					instance.Deaths = (short)change.Value;
					break;
				case Keys.FunctionalItems:
					instance.FunctionalItems = (List<int>)change.Value;
					break;
				case Keys.Gear:
					instance.Gear = (List<int>)change.Value;
					break;
				case Keys.Health:
					instance.Health = (short)change.Value;
					break;
				case Keys.Kills:
					instance.Kills = (short)change.Value;
					break;
				case Keys.Level:
					instance.Level = (int)change.Value;
					break;
				case Keys.Ping:
					instance.Ping = (ushort)change.Value;
					break;
				case Keys.PlayerId:
					instance.PlayerId = (byte)change.Value;
					break;
				case Keys.PlayerName:
					instance.PlayerName = (string)change.Value;
					break;
				case Keys.PlayerState:
					instance.PlayerState = (PlayerStates)(byte)change.Value;
					break;
				case Keys.QuickItems:
					instance.QuickItems = (List<int>)change.Value;
					break;
				case Keys.Rank:
					instance.Rank = (byte)change.Value;
					break;
				case Keys.SkinColor:
					instance.SkinColor = (Color)change.Value;
					break;
				case Keys.StepSound:
					instance.StepSound = (SurfaceType)(int)change.Value;
					break;
				case Keys.TeamID:
					instance.TeamID = (TeamID)(int)change.Value;
					break;
				case Keys.Weapons:
					instance.Weapons = (List<int>)change.Value;
					break;
				}
			}
		}
	}
}
