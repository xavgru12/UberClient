using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UnityEngine;

public class HUDKilledBy : MonoBehaviour
{
	[SerializeField]
	private UIPanel panel;

	[SerializeField]
	private UILabel nameLabel;

	[SerializeField]
	private UIHorizontalAligner healthArmorAligner;

	[SerializeField]
	private UILabel healthLabel;

	[SerializeField]
	private UILabel armorLabel;

	[SerializeField]
	private UIHorizontalAligner weaponAligner;

	[SerializeField]
	private UILabel weaponNameLabel;

	[SerializeField]
	private UILabel respawnLabel;

	[SerializeField]
	private UILabel respawnCountdown;

	private void Start()
	{
		GameData.Instance.OnPlayerKilled.AddEvent(delegate(GameActorInfo shooter, GameActorInfo target, UberstrikeItemClass weapon, BodyPart body)
		{
			if (target != null && target.Cmid == PlayerDataManager.Cmid)
			{
				panel.alpha = 1f;
				if (shooter == null || shooter.Cmid == PlayerDataManager.Cmid)
				{
					nameLabel.text = LocalizedStrings.CongratulationsYouKilledYourself;
					healthArmorAligner.gameObject.SetActive(value: false);
				}
				else
				{
					nameLabel.text = "Killed by " + shooter.PlayerName;
					healthArmorAligner.gameObject.SetActive(value: true);
					healthLabel.text = Mathf.Clamp(shooter.Health, 0, 200).ToString();
					armorLabel.text = Mathf.Clamp(shooter.ArmorPoints, 0, 200).ToString();
					healthArmorAligner.Reposition();
				}
				respawnCountdown.gameObject.SetActive(value: false);
				respawnLabel.gameObject.SetActive(value: false);
			}
		}, this);
		GameData.Instance.PlayerState.AddEvent(delegate(PlayerStateId el)
		{
			panel.alpha = ((el == PlayerStateId.Killed) ? 1 : 0);
		}, this);
		panel.alpha = 0f;
		GameData.Instance.OnRespawnCountdown.AddEvent(delegate(int el)
		{
			respawnCountdown.gameObject.SetActive(el > 0);
			respawnLabel.gameObject.SetActive(el > 0);
			if (el > 0)
			{
				respawnCountdown.text = el.ToString();
				UITweener.Begin<TweenScale>(respawnCountdown.gameObject, 0.5f);
				UITweener.Begin<TweenAlpha>(respawnCountdown.gameObject, 0.25f);
			}
		}, this);
	}
}
