// Decompiled with JetBrains decompiler
// Type: AssetBundleLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AssetBundleLoader
{
  private static readonly Dictionary<string, AssetBundleLoader.LoadingState> _loadingState = new Dictionary<string, AssetBundleLoader.LoadingState>();

  public static void CancelLoading(string fileName)
  {
    AssetBundleLoader.LoadingState loadingState;
    if (!AssetBundleLoader._loadingState.TryGetValue(fileName, out loadingState) || loadingState != AssetBundleLoader.LoadingState.Loading)
      return;
    AssetBundleLoader._loadingState[fileName] = AssetBundleLoader.LoadingState.Cancelled;
  }

  public static void SetStateToLoaded(string fileName) => AssetBundleLoader._loadingState[fileName] = AssetBundleLoader.LoadingState.Loaded;

  public static AssetBundleLoader.LoadingState State(string fileName)
  {
    AssetBundleLoader.LoadingState loadingState;
    return AssetBundleLoader._loadingState.TryGetValue(fileName, out loadingState) ? loadingState : AssetBundleLoader.LoadingState.None;
  }

  public static Coroutine LoadItemAssetBundle(
    string ItemFileName,
    Action<float> progress = null,
    Action<AssetBundle> onLoaded = null,
    Action<string> onError = null)
  {
    return MonoRoutine.Start(AssetBundleLoader.LoadAssetBundle(ItemFileName, ApplicationDataManager.BaseItemsURL, progress, onLoaded, onError));
  }

  public static Coroutine LoadMapAssetBundle(
    string MapFileName,
    Action<float> progress = null,
    Action<AssetBundle> onLoaded = null,
    Action<string> onError = null)
  {
    return MonoRoutine.Start(AssetBundleLoader.LoadAssetBundle(MapFileName, ApplicationDataManager.BaseMapsURL, progress, onLoaded, onError));
  }

  [DebuggerHidden]
  public static IEnumerator LoadWebPlayerBundle(
    string fileName,
    Action<float> progress = null,
    Action<AssetBundle> onLoaded = null,
    Action<string> onError = null)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AssetBundleLoader.\u003CLoadWebPlayerBundle\u003Ec__Iterator3()
    {
      fileName = fileName,
      progress = progress,
      onError = onError,
      onLoaded = onLoaded,
      \u003C\u0024\u003EfileName = fileName,
      \u003C\u0024\u003Eprogress = progress,
      \u003C\u0024\u003EonError = onError,
      \u003C\u0024\u003EonLoaded = onLoaded
    };
  }

  [DebuggerHidden]
  public static IEnumerator LoadAssetBundle(
    string fileName,
    string alternativeBaseUrl,
    Action<float> progress = null,
    Action<AssetBundle> onLoaded = null,
    Action<string> onError = null)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AssetBundleLoader.\u003CLoadAssetBundle\u003Ec__Iterator4()
    {
      fileName = fileName,
      progress = progress,
      alternativeBaseUrl = alternativeBaseUrl,
      onError = onError,
      onLoaded = onLoaded,
      \u003C\u0024\u003EfileName = fileName,
      \u003C\u0024\u003Eprogress = progress,
      \u003C\u0024\u003EalternativeBaseUrl = alternativeBaseUrl,
      \u003C\u0024\u003EonError = onError,
      \u003C\u0024\u003EonLoaded = onLoaded
    };
  }

  [DebuggerHidden]
  public static IEnumerator LoadAssetBundleNoCache(
    string path,
    Action<float> progress = null,
    Action<AssetBundle> onLoaded = null,
    Action<string> onError = null)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AssetBundleLoader.\u003CLoadAssetBundleNoCache\u003Ec__Iterator5()
    {
      path = path,
      progress = progress,
      onError = onError,
      onLoaded = onLoaded,
      \u003C\u0024\u003Epath = path,
      \u003C\u0024\u003Eprogress = progress,
      \u003C\u0024\u003EonError = onError,
      \u003C\u0024\u003EonLoaded = onLoaded
    };
  }

  public enum LoadingState
  {
    None,
    Loading,
    Loaded,
    Cancelled,
    Error,
  }
}
