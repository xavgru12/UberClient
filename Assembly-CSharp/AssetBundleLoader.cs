using System;
using System.Collections;
using UnityEngine;

public static class AssetBundleLoader
{
	public static IEnumerator LoadAssetBundleNoCache(string path, Action<float> progress = null, Action<AssetBundle> onLoaded = null, Action<string> onError = null)
	{
		Debug.Log("LOADING ASSETBUNDLE: " + path);
		WWW loader = new WWW(path);
		while (!loader.isDone)
		{
			yield return new WaitForEndOfFrame();
			progress?.Invoke(loader.progress);
		}
		if (!string.IsNullOrEmpty(loader.error))
		{
			Debug.LogError("Failed to locate Asset " + path + ". Error" + loader.error);
			onError?.Invoke("Failed to locate Asset " + path + ". Error" + loader.error);
		}
		else
		{
			onLoaded?.Invoke(loader.assetBundle);
		}
		progress?.Invoke(1f);
		loader.Dispose();
	}
}
