using UnityEngine;

[RequireComponent(typeof(AudioSource))]
internal class StreamedAudio : MonoBehaviour
{
	[SerializeField]
	private string _clipName;

	private void OnEnable()
	{
		AutoMonoBehaviour<StreamedAudioPlayer>.Instance.PlayMusic(base.audio, _clipName);
	}

	private void OnDisable()
	{
		AutoMonoBehaviour<StreamedAudioPlayer>.Instance.StopMusic(base.audio);
	}
}
