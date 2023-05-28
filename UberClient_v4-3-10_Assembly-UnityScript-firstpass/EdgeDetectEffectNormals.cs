// Decompiled with JetBrains decompiler
// Type: EdgeDetectEffectNormals
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B73E9D7-7440-409D-A393-CC5E4EE1D985
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Edge Detection (Geometry)")]
[Serializable]
public class EdgeDetectEffectNormals : PostEffectsBase
{
  public EdgeDetectMode mode;
  public float sensitivityDepth;
  public float sensitivityNormals;
  public float edgeExp;
  public float sampleDist;
  public float edgesOnly;
  public Color edgesOnlyBgColor;
  public Shader edgeDetectShader;
  private Material edgeDetectMaterial;
  private EdgeDetectMode oldMode;

  public EdgeDetectEffectNormals()
  {
    this.mode = EdgeDetectMode.SobelDepthThin;
    this.sensitivityDepth = 1f;
    this.sensitivityNormals = 1f;
    this.edgeExp = 1f;
    this.sampleDist = 1f;
    this.edgesOnlyBgColor = Color.white;
    this.oldMode = EdgeDetectMode.SobelDepthThin;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.edgeDetectMaterial = this.CheckShaderAndCreateMaterial(this.edgeDetectShader, this.edgeDetectMaterial);
    if (this.mode != this.oldMode)
      this.SetCameraFlag();
    this.oldMode = this.mode;
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public override void Start() => this.oldMode = this.mode;

  public virtual void SetCameraFlag()
  {
    if (this.mode > EdgeDetectMode.RobertsCrossDepthNormals)
      this.camera.depthTextureMode |= DepthTextureMode.Depth;
    else
      this.camera.depthTextureMode |= DepthTextureMode.DepthNormals;
  }

  public override void OnEnable() => this.SetCameraFlag();

  [ImageEffectOpaque]
  public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      Vector2 vector2 = new Vector2(this.sensitivityDepth, this.sensitivityNormals);
      this.edgeDetectMaterial.SetVector("_Sensitivity", new Vector4(vector2.x, vector2.y, 1f, vector2.y));
      this.edgeDetectMaterial.SetFloat("_BgFade", this.edgesOnly);
      this.edgeDetectMaterial.SetFloat("_SampleDistance", this.sampleDist);
      this.edgeDetectMaterial.SetVector("_BgColor", (Vector4) this.edgesOnlyBgColor);
      this.edgeDetectMaterial.SetFloat("_Exponent", this.edgeExp);
      Graphics.Blit((Texture) source, destination, this.edgeDetectMaterial, (int) this.mode);
    }
  }

  public override void Main()
  {
  }
}
