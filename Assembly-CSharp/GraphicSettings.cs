// Decompiled with JetBrains decompiler
// Type: GraphicSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

internal static class GraphicSettings
{
  public static void SetQualityLevel(int level)
  {
    if (level > 2 || level < 0)
      level = 1;
    QualitySettings.SetQualityLevel(level, true);
    ApplicationDataManager.ApplicationOptions.VideoQualityLevel = level;
    switch (level)
    {
      case 0:
        ApplicationDataManager.ApplicationOptions.VideoBloomAndFlares = false;
        ApplicationDataManager.ApplicationOptions.VideoVignetting = false;
        ApplicationDataManager.ApplicationOptions.VideoMotionBlur = false;
        ApplicationDataManager.ApplicationOptions.VideoWaterMode = 0;
        break;
      case 1:
        ApplicationDataManager.ApplicationOptions.VideoBloomAndFlares = true;
        ApplicationDataManager.ApplicationOptions.VideoVignetting = false;
        ApplicationDataManager.ApplicationOptions.VideoMotionBlur = true;
        ApplicationDataManager.ApplicationOptions.VideoWaterMode = 1;
        break;
      case 2:
        ApplicationDataManager.ApplicationOptions.VideoBloomAndFlares = true;
        ApplicationDataManager.ApplicationOptions.VideoMotionBlur = true;
        ApplicationDataManager.ApplicationOptions.VideoVignetting = true;
        ApplicationDataManager.ApplicationOptions.VideoWaterMode = Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXWebPlayer ? 1 : 2;
        break;
    }
  }
}
