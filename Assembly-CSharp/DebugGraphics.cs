using UnityEngine;

public class DebugGraphics : IDebugPage
{
	public string Title => "Graphics";

	public void Draw()
	{
		GUILayout.Label("graphicsDeviceID: " + SystemInfo.graphicsDeviceID.ToString());
		GUILayout.Label("graphicsDeviceNameD: " + SystemInfo.graphicsDeviceName);
		GUILayout.Label("graphicsDeviceVendorD: " + SystemInfo.graphicsDeviceVendor);
		GUILayout.Label("graphicsDeviceVendorIDD: " + SystemInfo.graphicsDeviceVendorID.ToString());
		GUILayout.Label("graphicsDeviceVersionD: " + SystemInfo.graphicsDeviceVersion);
		GUILayout.Label("graphicsMemorySizeD: " + SystemInfo.graphicsMemorySize.ToString());
		GUILayout.Label("graphicsPixelFillrateD: " + SystemInfo.graphicsPixelFillrate.ToString());
		GUILayout.Label("graphicsShaderLevelD: " + SystemInfo.graphicsShaderLevel.ToString());
		GUILayout.Label("supportedRenderTargetCountD: " + SystemInfo.supportedRenderTargetCount.ToString());
		GUILayout.Label("supportsImageEffectsD: " + SystemInfo.supportsImageEffects.ToString());
		GUILayout.Label("supportsRenderTexturesD: " + SystemInfo.supportsRenderTextures.ToString());
		GUILayout.Label("supportsShadowsD: " + SystemInfo.supportsShadows.ToString());
		GUILayout.Label("supportsVertexPrograms: " + SystemInfo.supportsVertexPrograms.ToString());
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
