// Decompiled with JetBrains decompiler
// Type: VolumeEnviromentSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (Collider))]
public class VolumeEnviromentSettings : MonoBehaviour
{
  public EnviromentSettings Settings;

  private void Awake() => this.collider.isTrigger = true;

  private void OnTriggerEnter(Collider collider)
  {
    if (!(collider.tag == "Player"))
      return;
    GameState.LocalPlayer.MoveController.SetEnviroment(this.Settings, this.collider.bounds);
    if (this.Settings.Type != EnviromentSettings.TYPE.WATER)
      return;
    float y = GameState.LocalPlayer.MoveController.Velocity.y;
    if ((double) y < -20.0)
    {
      SfxManager.Play3dAudioClip(GameAudio.BigSplash, collider.transform.position);
    }
    else
    {
      if ((double) y >= -10.0)
        return;
      SfxManager.Play3dAudioClip(GameAudio.MediumSplash, collider.transform.position);
    }
  }

  private void OnTriggerExit(Collider c)
  {
    if (!(c.tag == "Player"))
      return;
    GameState.LocalPlayer.MoveController.ResetEnviroment();
  }
}
