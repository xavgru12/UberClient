// Decompiled with JetBrains decompiler
// Type: DepthOfFieldScatter
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B73E9D7-7440-409D-A393-CC5E4EE1D985
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-UnityScript-firstpass.dll

using Boo.Lang.Runtime;
using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Depth of Field (Lens Blur, Scatter, DX11)")]
[Serializable]
public class DepthOfFieldScatter : PostEffectsBase
{
  public bool visualizeFocus;
  public float focalLength;
  public float focalSize;
  public float aperture;
  public Transform focalTransform;
  public float maxBlurSize;
  public bool highResolution;
  public DepthOfFieldScatter.BlurType blurType;
  public DepthOfFieldScatter.BlurSampleCount blurSampleCount;
  public bool nearBlur;
  public float foregroundOverlap;
  public Shader dofHdrShader;
  private Material dofHdrMaterial;
  public Shader dx11BokehShader;
  private Material dx11bokehMaterial;
  public float dx11BokehThreshhold;
  public float dx11SpawnHeuristic;
  public Texture2D dx11BokehTexture;
  public float dx11BokehScale;
  public float dx11BokehIntensity;
  private float focalDistance01;
  private ComputeBuffer cbDrawArgs;
  private ComputeBuffer cbPoints;
  private float internalBlurWidth;

  public DepthOfFieldScatter()
  {
    this.focalLength = 10f;
    this.focalSize = 0.05f;
    this.aperture = 11.5f;
    this.maxBlurSize = 2f;
    this.blurType = DepthOfFieldScatter.BlurType.DiscBlur;
    this.blurSampleCount = DepthOfFieldScatter.BlurSampleCount.High;
    this.foregroundOverlap = 1f;
    this.dx11BokehThreshhold = 0.5f;
    this.dx11SpawnHeuristic = 0.0875f;
    this.dx11BokehScale = 1.2f;
    this.dx11BokehIntensity = 2.5f;
    this.focalDistance01 = 10f;
    this.internalBlurWidth = 1f;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.dofHdrMaterial = this.CheckShaderAndCreateMaterial(this.dofHdrShader, this.dofHdrMaterial);
    if (this.supportDX11 && this.blurType == DepthOfFieldScatter.BlurType.DX11)
    {
      this.dx11bokehMaterial = this.CheckShaderAndCreateMaterial(this.dx11BokehShader, this.dx11bokehMaterial);
      this.CreateComputeResources();
    }
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public override void OnEnable() => this.camera.depthTextureMode |= DepthTextureMode.Depth;

  public virtual void OnDisable()
  {
    this.ReleaseComputeResources();
    if ((bool) (UnityEngine.Object) this.dofHdrMaterial)
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.dofHdrMaterial);
    this.dofHdrMaterial = (Material) null;
    if ((bool) (UnityEngine.Object) this.dx11bokehMaterial)
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.dx11bokehMaterial);
    this.dx11bokehMaterial = (Material) null;
  }

  public virtual void ReleaseComputeResources()
  {
    if (this.cbDrawArgs != null)
      this.cbDrawArgs.Release();
    this.cbDrawArgs = (ComputeBuffer) null;
    if (this.cbPoints != null)
      this.cbPoints.Release();
    this.cbPoints = (ComputeBuffer) null;
  }

  public virtual void CreateComputeResources()
  {
    if (RuntimeServices.EqualityOperator((object) this.cbDrawArgs, (object) null))
    {
      this.cbDrawArgs = new ComputeBuffer(1, 16, ComputeBufferType.DrawIndirect);
      this.cbDrawArgs.SetData((Array) new int[4]
      {
        0,
        1,
        0,
        0
      });
    }
    if (!RuntimeServices.EqualityOperator((object) this.cbPoints, (object) null))
      return;
    this.cbPoints = new ComputeBuffer(90000, 28, ComputeBufferType.Append);
  }

  public virtual float FocalDistance01(float worldDist) => this.camera.WorldToViewportPoint((worldDist - this.camera.nearClipPlane) * this.camera.transform.forward + this.camera.transform.position).z / (this.camera.farClipPlane - this.camera.nearClipPlane);

  private void WriteCoc(
    RenderTexture fromTo,
    RenderTexture temp1,
    RenderTexture temp2,
    bool fgDilate)
  {
    this.dofHdrMaterial.SetTexture("_FgOverlap", (Texture) null);
    if (this.nearBlur && fgDilate)
    {
      Graphics.Blit((Texture) fromTo, temp2, this.dofHdrMaterial, 4);
      float num = this.internalBlurWidth * this.foregroundOverlap;
      this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, num, 0.0f, num));
      Graphics.Blit((Texture) temp2, temp1, this.dofHdrMaterial, 2);
      this.dofHdrMaterial.SetVector("_Offsets", new Vector4(num, 0.0f, 0.0f, num));
      Graphics.Blit((Texture) temp1, temp2, this.dofHdrMaterial, 2);
      this.dofHdrMaterial.SetTexture("_FgOverlap", (Texture) temp2);
      Graphics.Blit((Texture) fromTo, fromTo, this.dofHdrMaterial, 13);
    }
    else
      Graphics.Blit((Texture) fromTo, fromTo, this.dofHdrMaterial, 0);
  }

  public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      if ((double) this.aperture < 0.0)
        this.aperture = 0.0f;
      if ((double) this.maxBlurSize < 0.10000000149011612)
        this.maxBlurSize = 0.1f;
      this.focalSize = Mathf.Clamp(this.focalSize, 0.0f, 2f);
      this.internalBlurWidth = Mathf.Max(this.maxBlurSize, 0.0f);
      this.focalDistance01 = !(bool) (UnityEngine.Object) this.focalTransform ? this.FocalDistance01(this.focalLength) : this.camera.WorldToViewportPoint(this.focalTransform.position).z / this.camera.farClipPlane;
      this.dofHdrMaterial.SetVector("_CurveParams", new Vector4(1f, this.focalSize, this.aperture / 10f, this.focalDistance01));
      RenderTexture renderTexture1 = (RenderTexture) null;
      float num1 = this.internalBlurWidth * this.foregroundOverlap;
      RenderTexture temporary1;
      if (this.visualizeFocus)
      {
        temporary1 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
        renderTexture1 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
        this.WriteCoc(source, temporary1, renderTexture1, true);
        Graphics.Blit((Texture) source, destination, this.dofHdrMaterial, 16);
      }
      else if (this.blurType == DepthOfFieldScatter.BlurType.DX11 && (bool) (UnityEngine.Object) this.dx11bokehMaterial)
      {
        if (this.highResolution)
        {
          this.internalBlurWidth = (double) this.internalBlurWidth >= 0.10000000149011612 ? this.internalBlurWidth : 0.1f;
          float num2 = this.internalBlurWidth * this.foregroundOverlap;
          temporary1 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
          RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
          this.WriteCoc(source, (RenderTexture) null, (RenderTexture) null, false);
          RenderTexture temporary3 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
          RenderTexture temporary4 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
          Graphics.Blit((Texture) source, temporary3, this.dofHdrMaterial, 15);
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, 1.5f, 0.0f, 1.5f));
          Graphics.Blit((Texture) temporary3, temporary4, this.dofHdrMaterial, 19);
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(1.5f, 0.0f, 0.0f, 1.5f));
          Graphics.Blit((Texture) temporary4, temporary3, this.dofHdrMaterial, 19);
          if (this.nearBlur)
            Graphics.Blit((Texture) source, temporary4, this.dofHdrMaterial, 4);
          this.dx11bokehMaterial.SetTexture("_BlurredColor", (Texture) temporary3);
          this.dx11bokehMaterial.SetFloat("_SpawnHeuristic", this.dx11SpawnHeuristic);
          this.dx11bokehMaterial.SetVector("_BokehParams", new Vector4(this.dx11BokehScale, this.dx11BokehIntensity, Mathf.Clamp(this.dx11BokehThreshhold, 0.005f, 4f), this.internalBlurWidth));
          this.dx11bokehMaterial.SetTexture("_FgCocMask", !this.nearBlur ? (Texture) null : (Texture) temporary4);
          Graphics.SetRandomWriteTarget(1, this.cbPoints);
          Graphics.Blit((Texture) source, temporary1, this.dx11bokehMaterial, 0);
          Graphics.ClearRandomWriteTargets();
          if (this.nearBlur)
          {
            this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, num2, 0.0f, num2));
            Graphics.Blit((Texture) temporary4, temporary3, this.dofHdrMaterial, 2);
            this.dofHdrMaterial.SetVector("_Offsets", new Vector4(num2, 0.0f, 0.0f, num2));
            Graphics.Blit((Texture) temporary3, temporary4, this.dofHdrMaterial, 2);
            Graphics.Blit((Texture) temporary4, temporary1, this.dofHdrMaterial, 3);
          }
          Graphics.Blit((Texture) temporary1, temporary2, this.dofHdrMaterial, 20);
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(this.internalBlurWidth, 0.0f, 0.0f, this.internalBlurWidth));
          Graphics.Blit((Texture) temporary1, source, this.dofHdrMaterial, 5);
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, this.internalBlurWidth, 0.0f, this.internalBlurWidth));
          Graphics.Blit((Texture) source, temporary2, this.dofHdrMaterial, 21);
          Graphics.SetRenderTarget(temporary2);
          ComputeBuffer.CopyCount(this.cbPoints, this.cbDrawArgs, 0);
          this.dx11bokehMaterial.SetBuffer("pointBuffer", this.cbPoints);
          this.dx11bokehMaterial.SetTexture("_MainTex", (Texture) this.dx11BokehTexture);
          this.dx11bokehMaterial.SetVector("_Screen", (Vector4) new Vector3((float) (1.0 / (1.0 * (double) source.width)), (float) (1.0 / (1.0 * (double) source.height)), this.internalBlurWidth));
          this.dx11bokehMaterial.SetPass(2);
          Graphics.DrawProceduralIndirect(MeshTopology.Points, this.cbDrawArgs, 0);
          Graphics.Blit((Texture) temporary2, destination);
          RenderTexture.ReleaseTemporary(temporary2);
          RenderTexture.ReleaseTemporary(temporary3);
          RenderTexture.ReleaseTemporary(temporary4);
        }
        else
        {
          temporary1 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
          renderTexture1 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
          float num3 = this.internalBlurWidth * this.foregroundOverlap;
          this.WriteCoc(source, (RenderTexture) null, (RenderTexture) null, false);
          source.filterMode = FilterMode.Bilinear;
          Graphics.Blit((Texture) source, temporary1, this.dofHdrMaterial, 6);
          RenderTexture temporary5 = RenderTexture.GetTemporary(temporary1.width >> 1, temporary1.height >> 1, 0, temporary1.format);
          RenderTexture temporary6 = RenderTexture.GetTemporary(temporary1.width >> 1, temporary1.height >> 1, 0, temporary1.format);
          Graphics.Blit((Texture) temporary1, temporary5, this.dofHdrMaterial, 15);
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, 1.5f, 0.0f, 1.5f));
          Graphics.Blit((Texture) temporary5, temporary6, this.dofHdrMaterial, 19);
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(1.5f, 0.0f, 0.0f, 1.5f));
          Graphics.Blit((Texture) temporary6, temporary5, this.dofHdrMaterial, 19);
          RenderTexture renderTexture2 = (RenderTexture) null;
          if (this.nearBlur)
          {
            renderTexture2 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
            Graphics.Blit((Texture) source, renderTexture2, this.dofHdrMaterial, 4);
          }
          this.dx11bokehMaterial.SetTexture("_BlurredColor", (Texture) temporary5);
          this.dx11bokehMaterial.SetFloat("_SpawnHeuristic", this.dx11SpawnHeuristic);
          this.dx11bokehMaterial.SetVector("_BokehParams", new Vector4(this.dx11BokehScale, this.dx11BokehIntensity, Mathf.Clamp(this.dx11BokehThreshhold, 0.005f, 4f), this.internalBlurWidth));
          this.dx11bokehMaterial.SetTexture("_FgCocMask", (Texture) renderTexture2);
          Graphics.SetRandomWriteTarget(1, this.cbPoints);
          Graphics.Blit((Texture) temporary1, renderTexture1, this.dx11bokehMaterial, 0);
          Graphics.ClearRandomWriteTargets();
          RenderTexture.ReleaseTemporary(temporary5);
          RenderTexture.ReleaseTemporary(temporary6);
          if (this.nearBlur)
          {
            this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, num3, 0.0f, num3));
            Graphics.Blit((Texture) renderTexture2, temporary1, this.dofHdrMaterial, 2);
            this.dofHdrMaterial.SetVector("_Offsets", new Vector4(num3, 0.0f, 0.0f, num3));
            Graphics.Blit((Texture) temporary1, renderTexture2, this.dofHdrMaterial, 2);
            Graphics.Blit((Texture) renderTexture2, renderTexture1, this.dofHdrMaterial, 3);
          }
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(this.internalBlurWidth, 0.0f, 0.0f, this.internalBlurWidth));
          Graphics.Blit((Texture) renderTexture1, temporary1, this.dofHdrMaterial, 5);
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, this.internalBlurWidth, 0.0f, this.internalBlurWidth));
          Graphics.Blit((Texture) temporary1, renderTexture1, this.dofHdrMaterial, 5);
          Graphics.SetRenderTarget(renderTexture1);
          ComputeBuffer.CopyCount(this.cbPoints, this.cbDrawArgs, 0);
          this.dx11bokehMaterial.SetBuffer("pointBuffer", this.cbPoints);
          this.dx11bokehMaterial.SetTexture("_MainTex", (Texture) this.dx11BokehTexture);
          this.dx11bokehMaterial.SetVector("_Screen", (Vector4) new Vector3((float) (1.0 / (1.0 * (double) renderTexture1.width)), (float) (1.0 / (1.0 * (double) renderTexture1.height)), this.internalBlurWidth));
          this.dx11bokehMaterial.SetPass(1);
          Graphics.DrawProceduralIndirect(MeshTopology.Points, this.cbDrawArgs, 0);
          this.dofHdrMaterial.SetTexture("_LowRez", (Texture) renderTexture1);
          this.dofHdrMaterial.SetTexture("_FgOverlap", (Texture) renderTexture2);
          this.dofHdrMaterial.SetVector("_Offsets", (float) (1.0 * (double) source.width / (1.0 * (double) renderTexture1.width)) * this.internalBlurWidth * Vector4.one);
          Graphics.Blit((Texture) source, destination, this.dofHdrMaterial, 9);
          if ((bool) (UnityEngine.Object) renderTexture2)
            RenderTexture.ReleaseTemporary(renderTexture2);
        }
      }
      else
      {
        temporary1 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
        renderTexture1 = RenderTexture.GetTemporary(source.width >> 1, source.height >> 1, 0, source.format);
        source.filterMode = FilterMode.Bilinear;
        if (this.highResolution)
          this.internalBlurWidth *= 2f;
        this.WriteCoc(source, temporary1, renderTexture1, true);
        int pass = this.blurSampleCount == DepthOfFieldScatter.BlurSampleCount.High || this.blurSampleCount == DepthOfFieldScatter.BlurSampleCount.Medium ? 17 : 11;
        if (this.highResolution)
        {
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, this.internalBlurWidth, 0.025f, this.internalBlurWidth));
          Graphics.Blit((Texture) source, destination, this.dofHdrMaterial, pass);
        }
        else
        {
          this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, this.internalBlurWidth, 0.1f, this.internalBlurWidth));
          Graphics.Blit((Texture) source, temporary1, this.dofHdrMaterial, 6);
          Graphics.Blit((Texture) temporary1, renderTexture1, this.dofHdrMaterial, pass);
          this.dofHdrMaterial.SetTexture("_LowRez", (Texture) renderTexture1);
          this.dofHdrMaterial.SetTexture("_FgOverlap", (Texture) null);
          this.dofHdrMaterial.SetVector("_Offsets", Vector4.one * (float) (1.0 * (double) source.width / (1.0 * (double) renderTexture1.width)) * this.internalBlurWidth);
          Graphics.Blit((Texture) source, destination, this.dofHdrMaterial, this.blurSampleCount != DepthOfFieldScatter.BlurSampleCount.High ? 12 : 18);
        }
      }
      if ((bool) (UnityEngine.Object) temporary1)
        RenderTexture.ReleaseTemporary(temporary1);
      if (!(bool) (UnityEngine.Object) renderTexture1)
        return;
      RenderTexture.ReleaseTemporary(renderTexture1);
    }
  }

  public override void Main()
  {
  }

  [Serializable]
  public enum BlurType
  {
    DiscBlur,
    DX11,
  }

  [Serializable]
  public enum BlurSampleCount
  {
    Low,
    Medium,
    High,
  }
}
