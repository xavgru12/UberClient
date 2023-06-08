using UnityEngine;

[RequireComponent(typeof(Collider))]
public class VolumeEnviromentSettings : MonoBehaviour
{
	public EnviromentSettings Settings;

	private void Awake()
	{
		base.collider.isTrigger = true;
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (!(collider.tag == "Player"))
		{
			return;
		}
		GameState.Current.Player.MoveController.SetEnviroment(Settings, base.collider.bounds);
		if (Settings.Type == EnviromentSettings.TYPE.WATER)
		{
			Vector3 velocity = GameState.Current.Player.MoveController.Velocity;
			float y = velocity.y;
			if (y < -20f)
			{
				AutoMonoBehaviour<SfxManager>.Instance.Play3dAudioClip(GameAudio.BigSplash, collider.transform.position);
			}
			else if (y < -10f)
			{
				AutoMonoBehaviour<SfxManager>.Instance.Play3dAudioClip(GameAudio.MediumSplash, collider.transform.position);
			}
		}
	}

	private void OnTriggerExit(Collider c)
	{
		if (c.tag == "Player")
		{
			GameState.Current.Player.MoveController.ResetEnviroment();
		}
	}
}
