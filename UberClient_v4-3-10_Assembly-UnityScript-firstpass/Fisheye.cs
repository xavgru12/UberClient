// Decompiled with JetBrains decompiler
// Type: Fisheye
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B73E9D7-7440-409D-A393-CC5E4EE1D985
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Fisheye")]
[Serializable]
public class Fisheye : PostEffectsBase
{
  public float strengthX;
  public float strengthY;
  public Shader fishEyeShader;
  private Material fisheyeMaterial;

  public Fisheye()
  {
    this.strengthX = 0.05f;
    this.strengthY = 0.05f;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.fisheyeMaterial = this.CheckShaderAndCreateMaterial(this.fishEyeShader, this.fisheyeMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      float num1 = 5f / 32f;
      float num2 = (float) ((double) source.width * 1.0 / ((double) source.height * 1.0));
      this.fisheyeMaterial.SetVector("intensity", new Vector4(this.strengthX * num2 * num1, this.strengthY * num1, this.strengthX * num2 * num1, this.strengthY * num1));
      Graphics.Blit((Texture) source, destination, this.fisheyeMaterial);
    }
  }

  public override void Main()
  {
  }
}
