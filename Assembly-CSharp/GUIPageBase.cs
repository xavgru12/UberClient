using System;
using System.Collections;
using UnityEngine;

public class GUIPageBase : MonoBehaviour
{
	[SerializeField]
	public float dismissDuration = 0.2f;

	[SerializeField]
	public float bringInDuration = 0.8f;

	public IEnumerator AnimateAlpha(float to, float duration, params UIButton[] buttons)
	{
		yield return StartCoroutine(AnimateAlpha(to, duration, Array.ConvertAll(buttons, (UIButton el) => el.GetComponent<UIPanel>())));
	}

	public IEnumerator AnimateAlpha(float to, float duration, params UIEventReceiver[] buttons)
	{
		yield return StartCoroutine(AnimateAlpha(to, duration, Array.ConvertAll(buttons, (UIEventReceiver el) => el.GetComponent<UIPanel>())));
	}

	public IEnumerator AnimateAlpha(float to, float duration, params GameObject[] objects)
	{
		yield return StartCoroutine(AnimateAlpha(to, duration, Array.ConvertAll(objects, (GameObject el) => el.GetComponent<UIPanel>())));
	}

	public IEnumerator AnimateAlpha(float to, float duration, params UIPanel[] buttons)
	{
		TweenAlpha[] array = Array.ConvertAll(buttons, (UIPanel el) => TweenAlpha.Begin(el.gameObject, duration, to));
		TweenAlpha[] array2 = array;
		TweenAlpha[] array3 = array2;
		foreach (TweenAlpha el2 in array3)
		{
			while (el2.enabled)
			{
				yield return 0;
			}
		}
	}

	public void Dismiss(Action onFinished)
	{
		StopAllCoroutines();
		StartCoroutine(DismissCrt(onFinished));
	}

	private IEnumerator DismissCrt(Action onFinished)
	{
		yield return StartCoroutine(OnDismiss());
		onFinished?.Invoke();
	}

	protected virtual IEnumerator OnDismiss()
	{
		yield return 0;
	}

	public void BringIn()
	{
		StopAllCoroutines();
		StartCoroutine(OnBringIn());
	}

	protected virtual IEnumerator OnBringIn()
	{
		yield return 0;
	}
}
