// Decompiled with JetBrains decompiler
// Type: PerformanceTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PerformanceTest : MonoBehaviour
{
  private bool showDialog;

  public static PerformanceTest Instance { get; private set; }

  public static bool Exists => (Object) PerformanceTest.Instance != (Object) null;

  public static void RunPerformanceCheck()
  {
    if (SystemInfo.graphicsShaderLevel < 30)
    {
      ApplicationDataManager.ApplicationOptions.VideoBloomAndFlares = false;
      ApplicationDataManager.ApplicationOptions.VideoVignetting = false;
      ApplicationDataManager.ApplicationOptions.VideoMotionBlur = false;
      ApplicationDataManager.ApplicationOptions.VideoWaterMode = 0;
      QualitySettings.SetQualityLevel(0);
      Shader.globalMaximumLOD = 100;
      QualitySettings.maxQueuedFrames = 0;
    }
    else
    {
      if (!SystemInfo.supportsImageEffects)
        return;
      ApplicationDataManager.ApplicationOptions.VideoBloomAndFlares = true;
      ApplicationDataManager.ApplicationOptions.VideoVignetting = true;
      ApplicationDataManager.ApplicationOptions.VideoMotionBlur = true;
    }
  }

  private void Awake() => PerformanceTest.Instance = this;

  private void Start()
  {
    if (SystemInfo.graphicsShaderLevel >= 30 || SystemInfo.graphicsMemorySize >= 128)
      return;
    this.showDialog = true;
  }

  private void OnGUI()
  {
    if (!this.showDialog)
      return;
    Rect position = new Rect((float) ((Screen.width - 430) / 2), (float) ((Screen.height - 260) / 2), 430f, 260f);
    GUI.BeginGroup(position, GUIContent.none, BlueStonez.window);
    GUI.depth = 3;
    GUI.BeginGroup(new Rect(20f, 20f, position.width - 40f, position.height - 40f), GUIContent.none, BlueStonez.window_standard_grey38);
    GUI.Label(new Rect(0.0f, -50f, 380f, 160f), "Uh Oh!", BlueStonez.label_interparkbold_32pt);
    GUI.Label(new Rect(0.0f, 0.0f, 380f, 160f), "It looks like your computer doesn't pack", BlueStonez.label_interparkbold_13pt);
    GUI.Label(new Rect(0.0f, 0.0f, 380f, 190f), "enough punch to run UberStrike optimally.", BlueStonez.label_interparkbold_13pt);
    GUI.Label(new Rect(0.0f, 0.0f, 380f, 260f), "You may experience a performance hit.", BlueStonez.label_interparkbold_13pt);
    if (GUITools.Button(new Rect((float) ((double) position.width / 2.0 - 80.0), 165f, 120f, 32f), new GUIContent(LocalizedStrings.OkCaps), BlueStonez.button_green))
      this.enabled = false;
    GUI.EndGroup();
    GUI.EndGroup();
  }
}
