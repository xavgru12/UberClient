// Decompiled with JetBrains decompiler
// Type: DebugGraphics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DebugGraphics : IDebugPage
{
  public string Title => "Graphics";

  public void Draw()
  {
    GUILayout.Label("graphicsDeviceID: " + (object) SystemInfo.graphicsDeviceID);
    GUILayout.Label("graphicsDeviceNameD: " + SystemInfo.graphicsDeviceName);
    GUILayout.Label("graphicsDeviceVendorD: " + SystemInfo.graphicsDeviceVendor);
    GUILayout.Label("graphicsDeviceVendorIDD: " + (object) SystemInfo.graphicsDeviceVendorID);
    GUILayout.Label("graphicsDeviceVersionD: " + SystemInfo.graphicsDeviceVersion);
    GUILayout.Label("graphicsMemorySizeD: " + (object) SystemInfo.graphicsMemorySize);
    GUILayout.Label("graphicsPixelFillrateD: " + (object) SystemInfo.graphicsPixelFillrate);
    GUILayout.Label("graphicsShaderLevelD: " + (object) SystemInfo.graphicsShaderLevel);
    GUILayout.Label("supportedRenderTargetCountD: " + (object) SystemInfo.supportedRenderTargetCount);
    GUILayout.Label("supportsImageEffectsD: " + (object) SystemInfo.supportsImageEffects);
    GUILayout.Label("supportsRenderTexturesD: " + (object) SystemInfo.supportsRenderTextures);
    GUILayout.Label("supportsShadowsD: " + (object) SystemInfo.supportsShadows);
    GUILayout.Label("supportsVertexPrograms: " + (object) SystemInfo.supportsVertexPrograms);
    QualitySettings.pixelLightCount = CmuneGUI.HorizontalScrollbar("pixelLightCount: ", QualitySettings.pixelLightCount, 0, 10);
    QualitySettings.masterTextureLimit = CmuneGUI.HorizontalScrollbar("masterTextureLimit: ", QualitySettings.masterTextureLimit, 0, 20);
    QualitySettings.maxQueuedFrames = CmuneGUI.HorizontalScrollbar("maxQueuedFrames: ", QualitySettings.maxQueuedFrames, 0, 10);
    QualitySettings.maximumLODLevel = CmuneGUI.HorizontalScrollbar("maximumLODLevel: ", QualitySettings.maximumLODLevel, 0, 7);
    QualitySettings.vSyncCount = CmuneGUI.HorizontalScrollbar("vSyncCount: ", QualitySettings.vSyncCount, 0, 2);
    QualitySettings.antiAliasing = CmuneGUI.HorizontalScrollbar("antiAliasing: ", QualitySettings.antiAliasing, 0, 4);
    QualitySettings.lodBias = CmuneGUI.HorizontalScrollbar("lodBias: ", QualitySettings.lodBias, 0, 4);
    Shader.globalMaximumLOD = CmuneGUI.HorizontalScrollbar("globalMaximumLOD: ", Shader.globalMaximumLOD, 100, 600);
  }
}
