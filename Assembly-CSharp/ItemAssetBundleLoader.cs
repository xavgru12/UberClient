// Decompiled with JetBrains decompiler
// Type: ItemAssetBundleLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ItemAssetBundleLoader
{
  private static List<string> assetDirectory;

  [DebuggerHidden]
  public static IEnumerator LoadItemAssetBundle(Action<float> progress = null, Action<string> onError = null) => (IEnumerator) new ItemAssetBundleLoader.\u003CLoadItemAssetBundle\u003Ec__Iterator53()
  {
    onError = onError,
    progress = progress,
    \u003C\u0024\u003EonError = onError,
    \u003C\u0024\u003Eprogress = progress
  };

  [DebuggerHidden]
  private static IEnumerator OnAssetBundleLoaded(AssetBundle bundle, Action<float> progress = null) => (IEnumerator) new ItemAssetBundleLoader.\u003COnAssetBundleLoaded\u003Ec__Iterator54()
  {
    bundle = bundle,
    progress = progress,
    \u003C\u0024\u003Ebundle = bundle,
    \u003C\u0024\u003Eprogress = progress
  };

  public static string GetSuffixForChannel(ChannelType channel)
  {
    switch (channel)
    {
      case ChannelType.OSXDashboard:
      case ChannelType.WindowsStandalone:
      case ChannelType.MacAppStore:
      case ChannelType.OSXStandalone:
        return "HD";
      case ChannelType.IPhone:
      case ChannelType.IPad:
        return "iOS";
      case ChannelType.Android:
        return "Android";
      default:
        return "SD";
    }
  }

  private class ProgressCombiner
  {
    private Action<float> finalProgress;
    private float downloadProgress;
    private float initializeProgress;

    public ProgressCombiner(Action<float> finalProgress) => this.finalProgress = finalProgress;

    public void SetDownloadProgress(float p)
    {
      this.downloadProgress = p;
      this.UpdateTotalProgress();
    }

    public void SetInitializeProgress(float p)
    {
      this.initializeProgress = p;
      this.UpdateTotalProgress();
    }

    private void UpdateTotalProgress()
    {
      if (this.finalProgress == null)
        return;
      this.finalProgress((float) ((double) this.downloadProgress * 0.5 + (double) this.initializeProgress * 0.5));
    }
  }
}
