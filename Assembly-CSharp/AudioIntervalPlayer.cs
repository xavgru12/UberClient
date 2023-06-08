using System.Collections;
using UnityEngine;

public class AudioIntervalPlayer : MonoBehaviour
{
	[SerializeField]
	private float waitTime = 30f;

	[SerializeField]
	private bool waitForClipLength;

	private IEnumerator Start()
	{
		base.audio.loop = false;
		while (true)
		{
			base.audio.Play();
			if (waitForClipLength && base.audio.clip != null)
			{
				yield return new WaitForSeconds(base.audio.clip.length);
			}
			yield return new WaitForSeconds(waitTime);
		}
	}
}
