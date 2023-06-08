using System.Collections;
using UnityEngine;

public class XPBarView : MonoBehaviour
{
	[SerializeField]
	private UILabel currentLevel;

	[SerializeField]
	private UILabel nextLevel;

	[SerializeField]
	private UISprite bgr;

	[SerializeField]
	private UISprite bar;

	[SerializeField]
	private float animageSpeed = 2f;

	private float cachedXP = -1f;

	private IEnumerator Animate(float percentage01)
	{
		percentage01 = Mathf.Clamp01(percentage01);
		Transform tr = bar.transform;
		Vector3 localScale = bgr.transform.localScale;
		float fullWidth = localScale.x;
		while (true)
		{
			Vector3 localScale2 = tr.localScale;
			if (Mathf.Abs(localScale2.x / fullWidth - percentage01) > 0.01f)
			{
				Vector3 localScale3 = tr.localScale;
				localScale3.x = Mathf.MoveTowards(localScale3.x, fullWidth * percentage01, Time.deltaTime * animageSpeed * fullWidth);
				tr.localScale = localScale3;
				yield return 0;
				continue;
			}
			break;
		}
	}

	private void Update()
	{
		int playerExperience = PlayerDataManager.PlayerExperience;
		if ((float)playerExperience != cachedXP)
		{
			cachedXP = playerExperience;
			int levelForXp = XpPointsUtil.GetLevelForXp(playerExperience);
			currentLevel.text = "Lvl " + levelForXp.ToString();
			nextLevel.text = "Lvl " + Mathf.Clamp(levelForXp + 1, 1, XpPointsUtil.MaxPlayerLevel).ToString();
			XpPointsUtil.GetXpRangeForLevel(levelForXp, out int minXp, out int maxXp);
			StopAllCoroutines();
			StartCoroutine(Animate((float)(playerExperience - minXp) / (float)(maxXp - minXp)));
		}
	}
}
