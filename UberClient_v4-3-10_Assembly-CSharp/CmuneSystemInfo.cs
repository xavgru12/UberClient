// Decompiled with JetBrains decompiler
// Type: CmuneSystemInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Text;
using UnityEngine;

public class CmuneSystemInfo
{
  public string OperatingSystem;
  public string ProcessorType;
  public string ProcessorCount;
  public string SystemMemorySize;
  public string GraphicsDeviceName;
  public string GraphicsDeviceVendor;
  public string GraphicsDeviceVersion;
  public string GraphicsMemorySize;
  public string GraphicsShaderLevel;
  public string GraphicsPixelFillRate;
  public string SupportsImageEffects;
  public string SupportsRenderTextures;
  public string SupportsShadows;
  public string SupportsVertexPrograms;
  public string Platform;
  public string RunInBackground;
  public string AbsoluteURL;
  public string DataPath;
  public string BackgroundLoadingPriority;
  public string SrcValue;
  public string SystemLanguage;
  public string TargetFrameRate;
  public string UnityVersion;
  public string Gravity;
  public string BounceThreshold;
  public string MaxAngularVelocity;
  public string MinPenetrationForPenalty;
  public string PenetrationPenaltyForce;
  public string SleepAngularVelocity;
  public string SleepVelocity;
  public string SolverIterationCount;
  public string CurrentResolution;
  public string AmbientLight;
  public string FlareStrength;
  public string FogEnabled;
  public string FogColor;
  public string FogDensity;
  public string HaloStrength;
  public string CurrentQualityLevel;
  public string AnisotropicFiltering;
  public string MasterTextureLimit;
  public string MaxQueuedFrames;
  public string PixelLightCount;
  public string ShadowCascades;
  public string ShadowDistance;
  public string SoftVegetationEnabled;
  public string BrowserIdentifier;
  public string BrowserVersion;
  public string BrowserMajorVersion;
  public string BrowserMinorVersion;
  public string BrowserEngine;
  public string BrowserEngineVersion;
  public string BrowserUserAgent;

  public CmuneSystemInfo()
  {
    this.OperatingSystem = SystemInfo.operatingSystem;
    this.ProcessorType = SystemInfo.processorType;
    this.ProcessorCount = SystemInfo.processorCount.ToString("N0");
    this.SystemMemorySize = SystemInfo.systemMemorySize.ToString("N0") + "Mb";
    this.GraphicsDeviceName = SystemInfo.graphicsDeviceName;
    this.GraphicsDeviceVendor = SystemInfo.graphicsDeviceVendor;
    this.GraphicsDeviceVersion = SystemInfo.graphicsDeviceVersion;
    this.GraphicsMemorySize = SystemInfo.graphicsMemorySize.ToString("N0") + "Mb";
    this.GraphicsShaderLevel = this.GetShaderLevelName(SystemInfo.graphicsShaderLevel);
    this.GraphicsPixelFillRate = SystemInfo.graphicsPixelFillrate.ToString();
    this.SupportsImageEffects = SystemInfo.supportsImageEffects.ToString();
    this.SupportsRenderTextures = SystemInfo.supportsRenderTextures.ToString();
    this.SupportsShadows = SystemInfo.supportsShadows.ToString();
    this.SupportsVertexPrograms = SystemInfo.supportsVertexPrograms.ToString();
    this.Platform = ((Enum) Application.platform).ToString();
    this.RunInBackground = Application.runInBackground.ToString();
    this.AbsoluteURL = Application.absoluteURL;
    this.DataPath = Application.dataPath;
    this.BackgroundLoadingPriority = ((Enum) Application.backgroundLoadingPriority).ToString();
    this.SrcValue = Application.srcValue;
    this.SystemLanguage = ((Enum) Application.systemLanguage).ToString();
    this.TargetFrameRate = Application.targetFrameRate.ToString("N0");
    this.UnityVersion = Application.unityVersion;
    this.Gravity = Physics.gravity.ToString();
    this.BounceThreshold = Physics.bounceThreshold.ToString("N2");
    this.MaxAngularVelocity = Physics.maxAngularVelocity.ToString("N2");
    this.MinPenetrationForPenalty = Physics.minPenetrationForPenalty.ToString("N2");
    this.PenetrationPenaltyForce = Physics.penetrationPenaltyForce.ToString("N2");
    this.SleepAngularVelocity = Physics.sleepAngularVelocity.ToString("N2");
    this.SleepVelocity = Physics.sleepVelocity.ToString("N2");
    this.SolverIterationCount = Physics.solverIterationCount.ToString("N2");
    this.CurrentResolution = "X " + Screen.width.ToString() + ", Y " + Screen.height.ToString() + ", Refresh " + Screen.currentResolution.refreshRate.ToString("N0") + "Hz";
    this.AmbientLight = RenderSettings.ambientLight.ToString();
    this.FlareStrength = RenderSettings.flareStrength.ToString("N2");
    this.FogEnabled = RenderSettings.fog.ToString();
    this.FogColor = RenderSettings.fogColor.ToString();
    this.FogDensity = RenderSettings.fogDensity.ToString("N2");
    this.HaloStrength = RenderSettings.haloStrength.ToString("N2");
    this.CurrentQualityLevel = QualitySettings.GetQualityLevel().ToString();
    this.AnisotropicFiltering = ((Enum) QualitySettings.anisotropicFiltering).ToString();
    this.MasterTextureLimit = QualitySettings.masterTextureLimit.ToString();
    this.MaxQueuedFrames = QualitySettings.maxQueuedFrames.ToString();
    this.PixelLightCount = QualitySettings.pixelLightCount.ToString();
    this.ShadowCascades = QualitySettings.shadowCascades.ToString();
    this.ShadowDistance = QualitySettings.shadowDistance.ToString("N2");
    this.SoftVegetationEnabled = QualitySettings.softVegetation.ToString();
    this.BrowserIdentifier = this.BrowserVersion = this.BrowserMajorVersion = this.BrowserMinorVersion = this.BrowserEngine = this.BrowserEngineVersion = this.BrowserUserAgent = "No information.";
  }

  private string GetShaderLevelName(int shaderLevel)
  {
    int num = shaderLevel;
    switch (num)
    {
      case 5:
        return "Fixed function, DirectX 5.";
      case 6:
        return "Fixed function, DirectX 6.";
      case 7:
        return "Fixed function, DirectX 7.";
      case 10:
        return "Shader Model 1.x";
      default:
        if (num == 20)
          return "Shader Model 2.x";
        return num == 30 ? "Shader Model 3.0 - We like!" : "Unknown";
    }
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendLine("///SYSTEM INFO REPORT///");
    stringBuilder.AppendLine(string.Empty);
    stringBuilder.AppendLine("UNITY SYSTEM INFO");
    stringBuilder.AppendLine("   Operating System: " + this.OperatingSystem);
    stringBuilder.AppendLine("   ProcessorType: " + this.ProcessorType);
    stringBuilder.AppendLine("   ProcessorCount: " + this.ProcessorCount);
    stringBuilder.AppendLine("   SystemMemorySize: " + this.SystemMemorySize);
    stringBuilder.AppendLine("   GraphicsDeviceName: " + this.GraphicsDeviceName);
    stringBuilder.AppendLine("   GraphicsDeviceVendor: " + this.GraphicsDeviceVendor);
    stringBuilder.AppendLine("   GraphicsDeviceVersion: " + this.GraphicsDeviceVersion);
    stringBuilder.AppendLine("   GraphicsMemorySize: " + this.GraphicsMemorySize);
    stringBuilder.AppendLine("   GraphicsShaderLevel: " + this.GraphicsShaderLevel);
    stringBuilder.AppendLine("   GraphicsPixelFillRate: " + this.GraphicsPixelFillRate);
    stringBuilder.AppendLine("   SupportsImageEffects: " + this.SupportsImageEffects);
    stringBuilder.AppendLine("   SupportsRenderTextures: " + this.SupportsRenderTextures);
    stringBuilder.AppendLine("   SupportsShadows: " + this.SupportsShadows);
    stringBuilder.AppendLine("   SupportsVertexPrograms: " + this.SupportsVertexPrograms);
    stringBuilder.AppendLine(string.Empty);
    stringBuilder.AppendLine("UNITY APPLICATION INFO");
    stringBuilder.AppendLine("   Platform: " + this.Platform);
    stringBuilder.AppendLine("   Run In Background: " + this.RunInBackground);
    stringBuilder.AppendLine("   Absolute URL: " + this.AbsoluteURL);
    stringBuilder.AppendLine("   Data Path: " + this.DataPath);
    stringBuilder.AppendLine("   Background Loading Priority: " + this.BackgroundLoadingPriority);
    stringBuilder.AppendLine("   Src Value: " + this.SrcValue);
    stringBuilder.AppendLine("   System Language: " + this.SystemLanguage);
    stringBuilder.AppendLine("   Target Frame Rate: " + this.TargetFrameRate);
    stringBuilder.AppendLine("   Unity Version: " + this.UnityVersion);
    stringBuilder.AppendLine(string.Empty);
    stringBuilder.AppendLine("UNITY PHYSICS INFO");
    stringBuilder.AppendLine("   Gravity: " + this.Gravity);
    stringBuilder.AppendLine("   Bounce Threshold: " + this.BounceThreshold);
    stringBuilder.AppendLine("   Max Angular Velocity: " + this.MaxAngularVelocity);
    stringBuilder.AppendLine("   Min Penetration For Penalty: " + this.MinPenetrationForPenalty);
    stringBuilder.AppendLine("   Penetration Penalty Force: " + this.PenetrationPenaltyForce);
    stringBuilder.AppendLine("   Sleep Angular Velocity: " + this.SleepAngularVelocity);
    stringBuilder.AppendLine("   Sleep Velocity: " + this.SleepVelocity);
    stringBuilder.AppendLine("   Solver Iteration Count: " + this.SolverIterationCount);
    stringBuilder.AppendLine(string.Empty);
    stringBuilder.AppendLine("UNITY RENDERING INFO");
    stringBuilder.AppendLine("   Current Resolution: " + this.CurrentResolution);
    stringBuilder.AppendLine("   Ambient Light: " + this.AmbientLight);
    stringBuilder.AppendLine("   Flare Strength: " + this.FlareStrength);
    stringBuilder.AppendLine("   Fog Enabled: " + this.FogEnabled);
    stringBuilder.AppendLine("   Fog Color: " + this.FogColor);
    stringBuilder.AppendLine("   Fog Density: " + this.FogDensity);
    stringBuilder.AppendLine("   Halo Strength: " + this.HaloStrength);
    stringBuilder.AppendLine(string.Empty);
    stringBuilder.AppendLine("UNITY QUALITY SETTINGS INFO");
    stringBuilder.AppendLine("   Current Quality Level: " + this.CurrentQualityLevel);
    stringBuilder.AppendLine("   Anisotropic Filtering: " + this.AnisotropicFiltering);
    stringBuilder.AppendLine("   Master Texture Limit: " + this.MasterTextureLimit);
    stringBuilder.AppendLine("   Max Queued Frames: " + this.MaxQueuedFrames);
    stringBuilder.AppendLine("   Pixel Light Count: " + this.PixelLightCount);
    stringBuilder.AppendLine("   Shadow Cascades: " + this.ShadowCascades);
    stringBuilder.AppendLine("   Shadow Distance: " + this.ShadowDistance);
    stringBuilder.AppendLine("   Soft Vegetation Enabled: " + this.SoftVegetationEnabled);
    stringBuilder.AppendLine(string.Empty);
    stringBuilder.AppendLine("BROWSER INFO");
    stringBuilder.AppendLine("   Browser Identifier: " + this.BrowserIdentifier);
    stringBuilder.AppendLine("   Browser Version: " + this.BrowserVersion);
    stringBuilder.AppendLine("   Browser Major Version: " + this.BrowserMajorVersion);
    stringBuilder.AppendLine("   Browser Minor Version: " + this.BrowserMinorVersion);
    stringBuilder.AppendLine("   Browser Engine: " + this.BrowserEngine);
    stringBuilder.AppendLine("   Browser Engine Version: " + this.BrowserEngineVersion);
    stringBuilder.AppendLine("   Browser User Agent: " + this.BrowserUserAgent);
    stringBuilder.AppendLine("GAME SERVER INFO");
    if (Singleton<GameServerManager>.Instance.PhotonServerCount > 0)
    {
      foreach (GameServerView photonServer in Singleton<GameServerManager>.Instance.PhotonServerList)
        stringBuilder.AppendLine(string.Format("   Server:{0} Ping:{1}", (object) photonServer.ConnectionString, (object) photonServer.Latency));
    }
    else
      stringBuilder.AppendLine("   No Game Server Information available.");
    stringBuilder.AppendLine("END OF REPORT");
    return stringBuilder.ToString();
  }

  public string ToHTML()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendLine("<h2>SYSTEM INFO REPORT</h2>");
    stringBuilder.AppendLine("<h3>UNITY SYSTEM INFO</h3>");
    stringBuilder.AppendLine("        Operating System: " + this.OperatingSystem);
    stringBuilder.AppendLine("<br/>   ProcessorType: " + this.ProcessorType);
    stringBuilder.AppendLine("<br/>   ProcessorCount: " + this.ProcessorCount);
    stringBuilder.AppendLine("<br/>   SystemMemorySize: " + this.SystemMemorySize);
    stringBuilder.AppendLine("<br/>   GraphicsDeviceName: " + this.GraphicsDeviceName);
    stringBuilder.AppendLine("<br/>   GraphicsDeviceVendor: " + this.GraphicsDeviceVendor);
    stringBuilder.AppendLine("<br/>   GraphicsDeviceVersion: " + this.GraphicsDeviceVersion);
    stringBuilder.AppendLine("<br/>   GraphicsMemorySize: " + this.GraphicsMemorySize);
    stringBuilder.AppendLine("<br/>   GraphicsShaderLevel: " + this.GraphicsShaderLevel);
    stringBuilder.AppendLine("<br/>   GraphicsPixelFillRate: " + this.GraphicsPixelFillRate);
    stringBuilder.AppendLine("<br/>   SupportsImageEffects: " + this.SupportsImageEffects);
    stringBuilder.AppendLine("<br/>   SupportsRenderTextures: " + this.SupportsRenderTextures);
    stringBuilder.AppendLine("<br/>   SupportsShadows: " + this.SupportsShadows);
    stringBuilder.AppendLine("<br/>   SupportsVertexPrograms: " + this.SupportsVertexPrograms);
    stringBuilder.AppendLine("<br/><h3>UNITY APPLICATION INFO</h3>");
    stringBuilder.AppendLine("        Platform: " + this.Platform);
    stringBuilder.AppendLine("<br/>   Run In Background: " + this.RunInBackground);
    stringBuilder.AppendLine("<br/>   Absolute URL: " + this.AbsoluteURL);
    stringBuilder.AppendLine("<br/>   Data Path: " + this.DataPath);
    stringBuilder.AppendLine("<br/>   Background Loading Priority: " + this.BackgroundLoadingPriority);
    stringBuilder.AppendLine("<br/>   Src Value: " + this.SrcValue);
    stringBuilder.AppendLine("<br/>   System Language: " + this.SystemLanguage);
    stringBuilder.AppendLine("<br/>   Target Frame Rate: " + this.TargetFrameRate);
    stringBuilder.AppendLine("<br/>   Unity Version: " + this.UnityVersion);
    stringBuilder.AppendLine("<br/><h3>UNITY PHYSICS INFO</h3>");
    stringBuilder.AppendLine("        Gravity: " + this.Gravity);
    stringBuilder.AppendLine("<br/>   Bounce Threshold: " + this.BounceThreshold);
    stringBuilder.AppendLine("<br/>   Max Angular Velocity: " + this.MaxAngularVelocity);
    stringBuilder.AppendLine("<br/>   Min Penetration For Penalty: " + this.MinPenetrationForPenalty);
    stringBuilder.AppendLine("<br/>   Penetration Penalty Force: " + this.PenetrationPenaltyForce);
    stringBuilder.AppendLine("<br/>   Sleep Angular Velocity: " + this.SleepAngularVelocity);
    stringBuilder.AppendLine("<br/>   Sleep Velocity: " + this.SleepVelocity);
    stringBuilder.AppendLine("<br/>   Solver Iteration Count: " + this.SolverIterationCount);
    stringBuilder.AppendLine("<br/><h3>UNITY RENDERING INFO</h3>");
    stringBuilder.AppendLine("        Current Resolution: " + this.CurrentResolution);
    stringBuilder.AppendLine("<br/>   Ambient Light: " + this.AmbientLight);
    stringBuilder.AppendLine("<br/>   Flare Strength: " + this.FlareStrength);
    stringBuilder.AppendLine("<br/>   Fog Enabled: " + this.FogEnabled);
    stringBuilder.AppendLine("<br/>   Fog Color: " + this.FogColor);
    stringBuilder.AppendLine("<br/>   Fog Density: " + this.FogDensity);
    stringBuilder.AppendLine("<br/>   Halo Strength: " + this.HaloStrength);
    stringBuilder.AppendLine("<br/><h3>UNITY QUALITY SETTINGS INFO</h3>");
    stringBuilder.AppendLine("        Current Quality Level: " + this.CurrentQualityLevel);
    stringBuilder.AppendLine("<br/>   Anisotropic Filtering: " + this.AnisotropicFiltering);
    stringBuilder.AppendLine("<br/>   Master Texture Limit: " + this.MasterTextureLimit);
    stringBuilder.AppendLine("<br/>   Max Queued Frames: " + this.MaxQueuedFrames);
    stringBuilder.AppendLine("<br/>   Pixel Light Count: " + this.PixelLightCount);
    stringBuilder.AppendLine("<br/>   Shadow Cascades: " + this.ShadowCascades);
    stringBuilder.AppendLine("<br/>   Shadow Distance: " + this.ShadowDistance);
    stringBuilder.AppendLine("<br/>   Soft Vegetation Enabled: " + this.SoftVegetationEnabled);
    stringBuilder.AppendLine("<br/><h3>BROWSER INFO</h3>");
    stringBuilder.AppendLine("        Browser Identifier: " + this.BrowserIdentifier);
    stringBuilder.AppendLine("<br/>   Browser Version: " + this.BrowserVersion);
    stringBuilder.AppendLine("<br/>   Browser Major Version: " + this.BrowserMajorVersion);
    stringBuilder.AppendLine("<br/>   Browser Minor Version: " + this.BrowserMinorVersion);
    stringBuilder.AppendLine("<br/>   Browser Engine: " + this.BrowserEngine);
    stringBuilder.AppendLine("<br/>   Browser Engine Version: " + this.BrowserEngineVersion);
    stringBuilder.AppendLine("<br/>   Browser User Agent: " + this.BrowserUserAgent);
    stringBuilder.AppendLine("<br/><h3>GAME SERVER INFO</h3>");
    if (Singleton<GameServerManager>.Instance.PhotonServerCount > 0)
    {
      foreach (GameServerView photonServer in Singleton<GameServerManager>.Instance.PhotonServerList)
        stringBuilder.AppendLine(string.Format(" Server:{0} Ping:{1}<br/>", (object) photonServer.ConnectionString, (object) photonServer.Latency));
    }
    else
      stringBuilder.AppendLine("No Game Server Information available.<br/>");
    stringBuilder.AppendLine("<h3>END OF REPORT</h3>");
    return stringBuilder.ToString();
  }
}
