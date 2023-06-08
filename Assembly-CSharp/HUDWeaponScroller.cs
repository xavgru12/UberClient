using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDWeaponScroller : MonoBehaviour
{
	[SerializeField]
	private NGUIScrollList scrollList;

	[SerializeField]
	private HUDWeaponScrollItem melee;

	[SerializeField]
	private HUDWeaponScrollItem primary;

	[SerializeField]
	private HUDWeaponScrollItem secondary;

	[SerializeField]
	private HUDWeaponScrollItem tertiary;

	private Dictionary<LoadoutSlotType, GameObject> loadoutWeapons = new Dictionary<LoadoutSlotType, GameObject>();

	private void OnEnable()
	{
		GameState.Current.PlayerData.LoadoutWeapons.Fire();
	}

	private void Start()
	{
		GameState.Current.PlayerData.LoadoutWeapons.AddEventAndFire(delegate(Dictionary<LoadoutSlotType, IUnityItem> el)
		{
			if (el != null)
			{
				loadoutWeapons.Clear();
				foreach (KeyValuePair<LoadoutSlotType, IUnityItem> item in el)
				{
					switch (item.Key)
					{
					case LoadoutSlotType.WeaponMelee:
						SetElement(item, melee);
						break;
					case LoadoutSlotType.WeaponPrimary:
						SetElement(item, primary);
						break;
					case LoadoutSlotType.WeaponSecondary:
						SetElement(item, secondary);
						break;
					case LoadoutSlotType.WeaponTertiary:
						SetElement(item, tertiary);
						break;
					}
				}
				if (loadoutWeapons.Count > 2)
				{
					scrollList.scrollType = NGUIScrollList.ScrollType.Visible3;
				}
				else
				{
					scrollList.scrollType = NGUIScrollList.ScrollType.NotCircular;
				}
				scrollList.SetActiveElements(new List<GameObject>(loadoutWeapons.Values));
			}
		}, this);
		StartCoroutine(Show(show: false, 0f, 0f));
		GameState.Current.PlayerData.NextActiveWeapon.AddEvent(delegate(WeaponSlot el)
		{
			if (el != null && loadoutWeapons.ContainsKey(el.Slot))
			{
				StopAllCoroutines();
				StartCoroutine(Show(show: true, 0f, 0.3f));
				PlayTweenOnElement(scrollList.SelectedElement, forward: false);
				scrollList.SelectElement(loadoutWeapons[el.Slot], 100f);
				PlayTweenOnElement(loadoutWeapons[el.Slot], forward: true);
				StartCoroutine(Show(show: false, 2.5f));
			}
		}, this);
	}

	private void SetElement(KeyValuePair<LoadoutSlotType, IUnityItem> item, HUDWeaponScrollItem slot)
	{
		if (item.Value != null)
		{
			slot.WeaponName = item.Value.View.Name;
			loadoutWeapons[item.Key] = slot.gameObject;
		}
	}

	private IEnumerator Show(bool show, float delay, float duration = 1f)
	{
		yield return new WaitForSeconds(delay);
		TweenAlpha.Begin(scrollList.Panel.gameObject, duration, show ? 1 : 0);
	}

	private void PlayTweenOnElement(GameObject element, bool forward)
	{
		if (element != null)
		{
			float num = (!forward) ? 1f : 1.6f;
			TweenScale.Begin(element, 0f, new Vector3(num, num, 1f));
			TweenAlpha.Begin(element, 0f, (!forward) ? 0.6f : 1f);
		}
	}
}
