// Decompiled with JetBrains decompiler
// Type: OutlineEffectController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class OutlineEffectController : MonoBehaviour
{
  private Dictionary<GameObject, OutlineEffectController.OutlineProperty> _outlinedGroup;
  private int _distanceToAttenuateOutline = 3;
  private int _distanceToHideOutline = 60;
  [SerializeField]
  private Material _outlineMaterial;

  private OutlineEffectController() => this._outlinedGroup = new Dictionary<GameObject, OutlineEffectController.OutlineProperty>();

  public static OutlineEffectController Instance { get; private set; }

  public static bool Exists => (Object) OutlineEffectController.Instance != (Object) null;

  private void Awake() => OutlineEffectController.Instance = this;

  private void Update()
  {
    Camera main = Camera.main;
    if ((Object) main == (Object) null)
      return;
    foreach (KeyValuePair<GameObject, OutlineEffectController.OutlineProperty> keyValuePair in this._outlinedGroup)
    {
      GameObject key = keyValuePair.Key;
      if ((Object) key == (Object) null)
      {
        this._outlinedGroup.Remove(key);
        break;
      }
      OutlineEffectController.OutlineProperty outlineProperty = keyValuePair.Value;
      float num = Mathf.Pow(1f - Mathf.Clamp((main.WorldToScreenPoint(key.transform.position).z - (float) this._distanceToAttenuateOutline) / (float) (this._distanceToHideOutline - this._distanceToAttenuateOutline), 0.0f, 1f), 3f) * (main.fieldOfView / 60f);
      foreach (Material material in outlineProperty.MaterialGroup)
        material.SetFloat("_Outline_Size", outlineProperty.OutlineSize * num);
    }
  }

  public void AddOutlineObject(
    GameObject gameObject,
    List<Material> materialGroup,
    Color outlineColor,
    float outlineSize = 0.01f)
  {
    if (this._outlinedGroup.ContainsKey(gameObject) || (Object) this._outlineMaterial == (Object) null)
      return;
    OutlineEffectController.OutlineProperty outlineProp = new OutlineEffectController.OutlineProperty(outlineColor, outlineSize, materialGroup);
    this._outlinedGroup.Add(gameObject, outlineProp);
    this.SetOutlineMaterial(outlineProp);
  }

  public void RemoveOutlineObject(GameObject gameObject)
  {
    OutlineEffectController.OutlineProperty outlineProperty = (OutlineEffectController.OutlineProperty) null;
    this._outlinedGroup.TryGetValue(gameObject, out outlineProperty);
    if (outlineProperty == null)
      return;
    this.SetDefaultMaterial(outlineProperty.MaterialGroup);
    this._outlinedGroup.Remove(gameObject);
  }

  private void SetOutlineMaterial(
    OutlineEffectController.OutlineProperty outlineProp)
  {
    if ((Object) this._outlineMaterial == (Object) null)
      return;
    foreach (Material material in outlineProp.MaterialGroup)
    {
      material.shader = this._outlineMaterial.shader;
      material.SetFloat("_Outline_Size", outlineProp.OutlineSize);
      material.SetColor("_Outline_Color", outlineProp.OutlineColor);
    }
  }

  private void SetOutlineSize(
    OutlineEffectController.OutlineProperty outlineProp,
    float size)
  {
    foreach (Material material in outlineProp.MaterialGroup)
    {
      material.shader = this._outlineMaterial.shader;
      material.SetFloat("_Outline_Size", outlineProp.OutlineSize);
    }
  }

  private void SetDefaultMaterial(List<Material> materialGroup)
  {
    foreach (Material material in materialGroup)
      material.shader = Shader.Find("Diffuse");
  }

  private class OutlineProperty
  {
    private List<Material> _materialGroup;

    public OutlineProperty(Color outlineColor, float outlineSize, List<Material> materialGroup)
    {
      this.OutlineColor = outlineColor;
      this.OutlineSize = outlineSize;
      this._materialGroup = materialGroup;
    }

    public Color OutlineColor { get; set; }

    public float OutlineSize { get; set; }

    public List<Material> MaterialGroup => this._materialGroup;
  }
}
