// Decompiled with JetBrains decompiler
// Type: RenderSettingsController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Reflection;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class RenderSettingsController : MonoBehaviour
{
  private static volatile RenderSettingsController _instance;
  private static object _lock = new object();
  [SerializeField]
  private Color ambientLight;
  [SerializeField]
  private Material skyBox;
  [SerializeField]
  private int fogStart;
  [SerializeField]
  private int fogEnd;
  [SerializeField]
  private Color32 fogColor;
  [SerializeField]
  private Color32 underwaterFogColor;
  [SerializeField]
  private GameObject advancedWater;
  [SerializeField]
  private GameObject simpleWater;
  [SerializeField]
  private bool forceBloom;
  private MobileBloom mobileBloom;

  public static RenderSettingsController Instance
  {
    get
    {
      if ((UnityEngine.Object) RenderSettingsController._instance == (UnityEngine.Object) null)
      {
        lock (RenderSettingsController._lock)
        {
          if ((UnityEngine.Object) RenderSettingsController._instance == (UnityEngine.Object) null)
          {
            ConstructorInfo constructor = typeof (RenderSettingsController).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, (Binder) null, new System.Type[0], (ParameterModifier[]) null);
            RenderSettingsController._instance = constructor != null && !constructor.IsAssembly ? (RenderSettingsController) constructor.Invoke((object[]) null) : throw new Exception(string.Format("A private or protected constructor is missing for '{0}'.", (object) typeof (RenderSettingsController).Name));
          }
        }
      }
      return RenderSettingsController._instance;
    }
  }

  private void OnEnable()
  {
    RenderSettingsController._instance = this;
    this.mobileBloom = this.GetComponent<MobileBloom>();
    if ((UnityEngine.Object) this.simpleWater != (UnityEngine.Object) null)
      this.simpleWater.SetActive(false);
    if ((UnityEngine.Object) this.advancedWater != (UnityEngine.Object) null)
      this.advancedWater.SetActive(true);
    RenderSettings.fog = true;
    RenderSettings.fogMode = UnityEngine.FogMode.Linear;
    RenderSettings.ambientLight = this.ambientLight;
    RenderSettings.skybox = this.skyBox;
    RenderSettings.fogColor = (Color) this.fogColor;
    RenderSettings.fogStartDistance = (float) this.fogStart;
    RenderSettings.fogEndDistance = (float) this.fogEnd;
    LevelCamera.Instance.EnableLowPassFilter(false);
  }

  public void ShowAgonyTint(float damageValue)
  {
    if (!((UnityEngine.Object) this.mobileBloom != (UnityEngine.Object) null) && !this.forceBloom)
      return;
    this.mobileBloom.agonyTint = Mathf.Clamp01(damageValue);
  }

  private void Update()
  {
    if (!((UnityEngine.Object) LevelCamera.Instance != (UnityEngine.Object) null))
      return;
    if (GameState.HasCurrentPlayer && GameState.LocalCharacter.Is(PlayerStates.DIVING) && !Singleton<PlayerSpectatorControl>.Instance.IsEnabled)
    {
      RenderSettings.fogColor = (Color) this.underwaterFogColor;
      RenderSettings.fogEndDistance = 50f;
      if (LevelCamera.Instance.LowpassFilterEnabled)
        return;
      LevelCamera.Instance.EnableLowPassFilter(true);
    }
    else
    {
      RenderSettings.fogColor = (Color) this.fogColor;
      RenderSettings.fogEndDistance = (float) this.fogEnd;
      if (!LevelCamera.Instance.LowpassFilterEnabled)
        return;
      LevelCamera.Instance.EnableLowPassFilter(false);
    }
  }
}
