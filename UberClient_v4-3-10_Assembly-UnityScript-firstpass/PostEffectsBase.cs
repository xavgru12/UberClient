﻿// Decompiled with JetBrains decompiler
// Type: PostEffectsBase
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B73E9D7-7440-409D-A393-CC5E4EE1D985
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-UnityScript-firstpass.dll

using Boo.Lang.Runtime;
using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[Serializable]
public class PostEffectsBase : MonoBehaviour
{
  protected bool supportHDRTextures;
  protected bool supportDX11;
  protected bool isSupported;

  public PostEffectsBase()
  {
    this.supportHDRTextures = true;
    this.isSupported = true;
  }

  public virtual Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
  {
    if (!(bool) (UnityEngine.Object) s)
    {
      Debug.Log((object) RuntimeServices.op_Addition("Missing shader in ", this.ToString()));
      this.enabled = false;
      return (Material) null;
    }
    if (s.isSupported && (bool) (UnityEngine.Object) m2Create && (UnityEngine.Object) m2Create.shader == (UnityEngine.Object) s)
      return m2Create;
    if (!s.isSupported)
    {
      this.NotSupported();
      Debug.Log((object) RuntimeServices.op_Addition(RuntimeServices.op_Addition(RuntimeServices.op_Addition(RuntimeServices.op_Addition("The shader ", s.ToString()), " on effect "), this.ToString()), " is not supported on this platform!"));
      return (Material) null;
    }
    m2Create = new Material(s);
    m2Create.hideFlags = HideFlags.DontSave;
    return (bool) (UnityEngine.Object) m2Create ? m2Create : (Material) null;
  }

  public virtual Material CreateMaterial(Shader s, Material m2Create)
  {
    if (!(bool) (UnityEngine.Object) s)
    {
      Debug.Log((object) RuntimeServices.op_Addition("Missing shader in ", this.ToString()));
      return (Material) null;
    }
    if ((bool) (UnityEngine.Object) m2Create && (UnityEngine.Object) m2Create.shader == (UnityEngine.Object) s && s.isSupported)
      return m2Create;
    if (!s.isSupported)
      return (Material) null;
    m2Create = new Material(s);
    m2Create.hideFlags = HideFlags.DontSave;
    return (bool) (UnityEngine.Object) m2Create ? m2Create : (Material) null;
  }

  public virtual void OnEnable() => this.isSupported = true;

  public virtual bool CheckSupport() => this.CheckSupport(false);

  public virtual bool CheckResources()
  {
    Debug.LogWarning((object) RuntimeServices.op_Addition(RuntimeServices.op_Addition("CheckResources () for ", this.ToString()), " should be overwritten."));
    return this.isSupported;
  }

  public virtual void Start() => this.CheckResources();

  public virtual bool CheckSupport(bool needDepth)
  {
    this.isSupported = true;
    this.supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
    int num = SystemInfo.graphicsShaderLevel >= 50 ? 1 : 0;
    if (num != 0)
      num = SystemInfo.supportsComputeShaders ? 1 : 0;
    this.supportDX11 = num != 0;
    if (!SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures)
    {
      this.NotSupported();
      return false;
    }
    if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
    {
      this.NotSupported();
      return false;
    }
    if (needDepth)
      this.camera.depthTextureMode |= DepthTextureMode.Depth;
    return true;
  }

  public virtual bool CheckSupport(bool needDepth, bool needHdr)
  {
    if (!this.CheckSupport(needDepth))
      return false;
    if (!needHdr || this.supportHDRTextures)
      return true;
    this.NotSupported();
    return false;
  }

  public virtual bool Dx11Support() => this.supportDX11;

  public virtual void ReportAutoDisable() => Debug.LogWarning((object) RuntimeServices.op_Addition(RuntimeServices.op_Addition("The image effect ", this.ToString()), " has been disabled as it's not supported on the current platform."));

  public virtual bool CheckShader(Shader s)
  {
    Debug.Log((object) RuntimeServices.op_Addition(RuntimeServices.op_Addition(RuntimeServices.op_Addition(RuntimeServices.op_Addition("The shader ", s.ToString()), " on effect "), this.ToString()), " is not part of the Unity 3.2+ effects suite anymore. For best performance and quality, please ensure you are using the latest Standard Assets Image Effects (Pro only) package."));
    if (s.isSupported)
      return false;
    this.NotSupported();
    return false;
  }

  public virtual void NotSupported()
  {
    this.enabled = false;
    this.isSupported = false;
  }

  public virtual void DrawBorder(RenderTexture dest, Material material)
  {
    float num1 = new float();
    float num2 = new float();
    float num3 = new float();
    float num4 = new float();
    RenderTexture.active = dest;
    bool flag = true;
    GL.PushMatrix();
    GL.LoadOrtho();
    for (int pass = 0; pass < material.passCount; ++pass)
    {
      material.SetPass(pass);
      float num5 = new float();
      float num6 = new float();
      float y1;
      float y2;
      if (flag)
      {
        y1 = 1f;
        y2 = 0.0f;
      }
      else
      {
        y1 = 0.0f;
        y2 = 1f;
      }
      float x1 = 0.0f;
      float x2 = 0.0f + (float) (1.0 / ((double) dest.width * 1.0));
      float y3 = 0.0f;
      float y4 = 1f;
      GL.Begin(7);
      GL.TexCoord2(0.0f, y1);
      GL.Vertex3(x1, y3, 0.1f);
      GL.TexCoord2(1f, y1);
      GL.Vertex3(x2, y3, 0.1f);
      GL.TexCoord2(1f, y2);
      GL.Vertex3(x2, y4, 0.1f);
      GL.TexCoord2(0.0f, y2);
      GL.Vertex3(x1, y4, 0.1f);
      float x3 = (float) (1.0 - 1.0 / ((double) dest.width * 1.0));
      float x4 = 1f;
      float y5 = 0.0f;
      float y6 = 1f;
      GL.TexCoord2(0.0f, y1);
      GL.Vertex3(x3, y5, 0.1f);
      GL.TexCoord2(1f, y1);
      GL.Vertex3(x4, y5, 0.1f);
      GL.TexCoord2(1f, y2);
      GL.Vertex3(x4, y6, 0.1f);
      GL.TexCoord2(0.0f, y2);
      GL.Vertex3(x3, y6, 0.1f);
      float x5 = 0.0f;
      float x6 = 1f;
      float y7 = 0.0f;
      float y8 = 0.0f + (float) (1.0 / ((double) dest.height * 1.0));
      GL.TexCoord2(0.0f, y1);
      GL.Vertex3(x5, y7, 0.1f);
      GL.TexCoord2(1f, y1);
      GL.Vertex3(x6, y7, 0.1f);
      GL.TexCoord2(1f, y2);
      GL.Vertex3(x6, y8, 0.1f);
      GL.TexCoord2(0.0f, y2);
      GL.Vertex3(x5, y8, 0.1f);
      float x7 = 0.0f;
      float x8 = 1f;
      float y9 = (float) (1.0 - 1.0 / ((double) dest.height * 1.0));
      float y10 = 1f;
      GL.TexCoord2(0.0f, y1);
      GL.Vertex3(x7, y9, 0.1f);
      GL.TexCoord2(1f, y1);
      GL.Vertex3(x8, y9, 0.1f);
      GL.TexCoord2(1f, y2);
      GL.Vertex3(x8, y10, 0.1f);
      GL.TexCoord2(0.0f, y2);
      GL.Vertex3(x7, y10, 0.1f);
      GL.End();
    }
    GL.PopMatrix();
  }

  public virtual void Main()
  {
  }
}
