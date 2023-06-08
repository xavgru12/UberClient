using UberStrike.Core.Types;
using UnityEngine;

public class HUDQuickItem : MonoBehaviour
{
	[SerializeField]
	private UISprite cooldown;

	[SerializeField]
	private UILabel amount;

	[SerializeField]
	private UISprite ammo;

	[SerializeField]
	private UISprite armor;

	[SerializeField]
	private UISprite health;

	[SerializeField]
	private UISprite offensiveGrenade;

	[SerializeField]
	private UISprite springGrenade;

	public UIEventReceiver actionButton;

	public void SetQuickItem(QuickItem item, bool selected)
	{
		base.gameObject.SetActive(item != null);
		if (!(item == null))
		{
			ammo.enabled = (item.Logic == QuickItemLogic.AmmoPack);
			armor.enabled = (item.Logic == QuickItemLogic.ArmorPack);
			health.enabled = (item.Logic == QuickItemLogic.HealthPack);
			offensiveGrenade.enabled = (item.Logic == QuickItemLogic.ExplosiveGrenade);
			springGrenade.enabled = (item.Logic == QuickItemLogic.SpringGrenade);
			amount.text = item.Behaviour.CurrentAmount.ToString();
			SetCooldown(item.Behaviour.CooldownProgress, selected);
		}
	}

	public void SetCooldown(float progress, bool selected)
	{
		bool enabled = progress != 0f && progress != 1f;
		cooldown.enabled = enabled;
		cooldown.fillAmount = progress;
		ammo.fillAmount = progress;
		ammo.alpha = ((!selected) ? 0.35f : 1f);
		armor.fillAmount = progress;
		armor.alpha = ((!selected) ? 0.35f : 1f);
		health.fillAmount = progress;
		health.alpha = ((!selected) ? 0.35f : 1f);
		offensiveGrenade.fillAmount = progress;
		offensiveGrenade.alpha = ((!selected) ? 0.35f : 1f);
		springGrenade.fillAmount = progress;
		springGrenade.alpha = ((!selected) ? 0.35f : 1f);
		amount.alpha = ((!selected) ? 0.35f : 1f);
	}
}
