using System;
using UnityEngine;

public class AutoMonoBehaviour<T> : MonoBehaviour where T : class
{
	private const string rootGameObjectName = "AutoMonoBehaviours";

	private static T _instance;

	private static GameObject _gameObject;

	private static bool _isRunning = true;

	private static bool _isInstantiating;

	protected static bool IsRunning => _isRunning;

	public static T Instance
	{
		get
		{
			if (_instance == null && _isRunning)
			{
				if (_isInstantiating)
				{
					throw new Exception("Recursive calls to Constuctor of AutoMonoBehaviour! Check your " + typeof(T)?.ToString() + ":Awake() function for calls to " + typeof(T)?.ToString() + ".Instance");
				}
				_isInstantiating = true;
				_instance = GetInstance();
			}
			return _instance;
		}
	}

	private static T GetInstance()
	{
		GameObject gameObject = GameObject.Find("AutoMonoBehaviours");
		if (gameObject == null)
		{
			gameObject = new GameObject("AutoMonoBehaviours");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
		string name = typeof(T).Name;
		_gameObject = GameObject.Find("AutoMonoBehaviours/" + name);
		if (_gameObject == null)
		{
			_gameObject = new GameObject(name);
			_gameObject.transform.parent = gameObject.transform;
		}
		return _gameObject.AddComponent(typeof(T)) as T;
	}

	private void OnApplicationQuit()
	{
		_isRunning = false;
	}

	protected virtual void Start()
	{
		if (_instance == null)
		{
			throw new Exception("The script " + typeof(T).Name + " is self instantiating and shouldn't be attached manually to a GameObject.");
		}
	}
}
