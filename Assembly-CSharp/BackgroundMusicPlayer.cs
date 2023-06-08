using UnityEngine;

public class BackgroundMusicPlayer : AutoMonoBehaviour<BackgroundMusicPlayer>
{
	private MusicFader musicFaderA;

	private MusicFader musicFaderB;

	private bool toggle;

	public float Volume
	{
		set
		{
			Current.Source.volume = value;
		}
	}

	private MusicFader Current
	{
		get
		{
			if (toggle)
			{
				return musicFaderA;
			}
			return musicFaderB;
		}
	}

	private void Awake()
	{
		AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
		audioSource.volume = 0f;
		audioSource.loop = true;
		AudioSource audioSource2 = base.gameObject.AddComponent<AudioSource>();
		audioSource2.volume = 0f;
		audioSource2.loop = true;
		musicFaderA = new MusicFader(audioSource);
		musicFaderB = new MusicFader(audioSource2);
	}

	public void Play(AudioClip clip)
	{
		if (Current.Source.clip != clip)
		{
			Current.FadeOut();
			toggle = !toggle;
			Current.Source.clip = clip;
			Current.FadeIn(SfxManager.MusicAudioVolume);
		}
		else
		{
			Current.FadeIn(SfxManager.MusicAudioVolume);
		}
	}

	public void Stop()
	{
		Current.FadeOut();
	}
}
