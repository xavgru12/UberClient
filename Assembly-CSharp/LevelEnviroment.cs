using UnityEngine;

public class LevelEnviroment : MonoBehaviour
{
	public EnviromentSettings Settings;

	public static LevelEnviroment Instance
	{
		get;
		private set;
	}

	public static bool Exists => Instance != null;

	private void Awake()
	{
		Instance = this;
	}
}
