// Decompiled with JetBrains decompiler
// Type: UIAudio
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public static class UIAudio
{
  private static Dictionary<UIAudio.Clips, AudioClip> _allClips = new Dictionary<UIAudio.Clips, AudioClip>();

  static UIAudio()
  {
    AudioClipConfigurator component = ((GameObject) Resources.Load(nameof (UIAudio), typeof (GameObject))).GetComponent<AudioClipConfigurator>();
    UIAudio._allClips[UIAudio.Clips.AudioVolumn] = component.Assets[0];
    UIAudio._allClips[UIAudio.Clips.ButtonClick] = component.Assets[1];
    UIAudio._allClips[UIAudio.Clips.ButtonRollover] = component.Assets[2];
    UIAudio._allClips[UIAudio.Clips.ClickReady] = component.Assets[3];
    UIAudio._allClips[UIAudio.Clips.ClickUnready] = component.Assets[4];
    UIAudio._allClips[UIAudio.Clips.CloseLoadout] = component.Assets[5];
    UIAudio._allClips[UIAudio.Clips.ClosePanel] = component.Assets[6];
  }

  public static AudioClip Get(UIAudio.Clips clip) => UIAudio._allClips[clip];

  public enum Clips
  {
    AudioVolumn,
    ButtonClick,
    ButtonRollover,
    ClickReady,
    ClickUnready,
    CloseLoadout,
    ClosePanel,
  }
}
