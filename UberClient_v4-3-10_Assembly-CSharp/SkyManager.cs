// Decompiled with JetBrains decompiler
// Type: SkyManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SkyManager : MonoBehaviour
{
  [SerializeField]
  private float _dayNightCycle;
  [SerializeField]
  private float _sunsetOffset;
  [SerializeField]
  private float _sunsetVisibility;
  [SerializeField]
  private Color _daySkyColor;
  [SerializeField]
  private Color _horizonColor;
  [SerializeField]
  private Color _sunsetColor;
  private Vector2 _dayCloudMoveVector = new Vector2(0.0f, 0.0f);
  private Vector2 _dayCloudHorizonMoveVector = new Vector2(0.0f, 0.0f);
  private float _cloudXAxisRot = 0.005f;
  private float _cloudYAxisRot = 0.005f;
  private float _cloudXAxisRotIndex = 1f / 1000f;
  private float _cloudYAxisRotIndex = 1f / 1000f;
  private Material _skyMaterial;

  public float DayNightCycle
  {
    get => this._dayNightCycle;
    set => this._dayNightCycle = value;
  }

  public float CloudXAxisRot
  {
    get => this._cloudXAxisRot;
    set => this._cloudXAxisRot = value;
  }

  public float CloudYAxisRot
  {
    get => this._cloudYAxisRot;
    set => this._cloudYAxisRot = value;
  }

  private void OnEnable() => this._skyMaterial = new Material(this.renderer.material);

  private void OnDisable() => this.renderer.material = this._skyMaterial;

  private void Update()
  {
    this._dayCloudMoveVector.x += Time.deltaTime * this._cloudXAxisRot;
    this._dayCloudHorizonMoveVector.y += Time.deltaTime * this._cloudYAxisRot;
    if ((double) this._dayCloudMoveVector.x > 1.0)
    {
      this._dayCloudMoveVector.x = 0.0f;
      if ((double) this._cloudXAxisRot > 0.00800000037997961)
        this._cloudXAxisRotIndex = -1f / 1000f;
      if ((double) this._cloudXAxisRot < 1.0 / 500.0)
        this._cloudXAxisRotIndex = 1f / 1000f;
      this._cloudXAxisRot += this._cloudXAxisRotIndex;
    }
    if ((double) this._dayCloudHorizonMoveVector.y > 1.0)
    {
      this._dayCloudHorizonMoveVector.y = 0.0f;
      if ((double) this._cloudYAxisRot > 0.00800000037997961)
        this._cloudYAxisRotIndex = -1f / 1000f;
      if ((double) this._cloudYAxisRot < 1.0 / 500.0)
        this._cloudYAxisRotIndex = 1f / 1000f;
      this._cloudYAxisRot += this._cloudYAxisRotIndex;
    }
    this.renderer.material.SetTextureOffset("_DayCloudTex", this._dayCloudMoveVector);
    this.renderer.material.SetTextureOffset("_NightCloudTex", this._dayCloudHorizonMoveVector);
    this._dayNightCycle = Mathf.Clamp01(this._dayNightCycle);
    this.renderer.material.SetFloat("_DayNightCycle", Mathf.Clamp01(this._dayNightCycle));
    this._sunsetOffset = Mathf.Clamp01(this._sunsetOffset);
    this.renderer.material.SetFloat("_SunsetOffset", Mathf.Clamp01(this._sunsetOffset));
    this._sunsetVisibility = Mathf.Clamp01(this._sunsetVisibility);
    this.renderer.material.SetFloat("_SunsetVisibility", this._sunsetVisibility);
    this.renderer.material.SetColor("_HorizonColor", this._horizonColor);
    this.renderer.material.SetColor("_DaySkyColor", this._daySkyColor);
    this.renderer.material.SetColor("_SunSetColor", this._sunsetColor);
  }
}
