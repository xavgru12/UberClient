using UnityEngine;

public class MobileDisableRenderer : MonoBehaviour
{
	private void OnEnable()
	{
		if (ApplicationDataManager.IsMobile)
		{
			base.renderer.enabled = false;
		}
	}
}
