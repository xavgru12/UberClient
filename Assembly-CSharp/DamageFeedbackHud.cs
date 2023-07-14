// Decompiled with JetBrains decompiler
// Type: DamageFeedbackHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class DamageFeedbackHud : Singleton<DamageFeedbackHud>
{
  private const float PEAKTIME = 0.04f;
  private const float ENDTIME = 0.08f;
  private List<DamageFeedbackHud.DamageFeedbackMark> _damageFeedbackMarkList;

  private DamageFeedbackHud()
  {
    this._damageFeedbackMarkList = new List<DamageFeedbackHud.DamageFeedbackMark>();
    this.Enabled = true;
  }

  public void Draw()
  {
    if (!this.Enabled)
      return;
    for (int index = 0; index < this._damageFeedbackMarkList.Count; ++index)
    {
      GUIUtility.RotateAroundPivot(Quaternion.LookRotation(LevelCamera.Instance.TransformCache.InverseTransformDirection(Quaternion.Euler(0.0f, this._damageFeedbackMarkList[index].DamageDirection, 0.0f) * Vector3.back)).eulerAngles.y, new Vector2((float) Screen.width * 0.5f, (float) Screen.height * 0.5f));
      GUI.color = new Color(0.975f, 0.201f, 0.135f, this._damageFeedbackMarkList[index].DamageAlpha);
      int width = Mathf.RoundToInt(128f * this._damageFeedbackMarkList[index].DamageAmount);
      GUI.DrawTexture(new Rect((float) ((double) Screen.width * 0.5 - (double) width * 0.5), (float) ((double) Screen.height * 0.5 - 256.0), (float) width, 128f), (Texture) HudTextures.DamageFeedbackMark);
      GUI.matrix = Matrix4x4.identity;
    }
    GUI.color = Color.white;
  }

  public void Update()
  {
    if (this._damageFeedbackMarkList.Count <= 0)
      return;
    for (int index = 0; index < this._damageFeedbackMarkList.Count; ++index)
    {
      if ((double) this._damageFeedbackMarkList[index].DamageAlpha < 0.0)
        this._damageFeedbackMarkList.RemoveAt(index);
    }
    for (int index = 0; index < this._damageFeedbackMarkList.Count; ++index)
      this._damageFeedbackMarkList[index].DamageAlpha -= Time.deltaTime * 0.5f;
  }

  public void AddDamageMark(float normalizedDamage, float horizontalAngle)
  {
    this._damageFeedbackMarkList.Add(new DamageFeedbackHud.DamageFeedbackMark(normalizedDamage, horizontalAngle));
    LevelCamera.Instance.DoFeedback(LevelCamera.FeedbackType.GetDamage, Vector3.back, 0.1f, normalizedDamage, 0.04f, 0.08f, 10f, Vector3.forward);
  }

  public void ClearAll() => this._damageFeedbackMarkList.Clear();

  public bool Enabled { get; set; }

  public class DamageFeedbackMark
  {
    public float DamageAlpha;
    public float DamageAmount;
    public float DamageDirection;

    public DamageFeedbackMark(float normalizedDamage, float horizontalAngle)
    {
      this.DamageAlpha = normalizedDamage;
      this.DamageAmount = normalizedDamage;
      this.DamageDirection = horizontalAngle;
    }
  }
}
