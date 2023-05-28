// Decompiled with JetBrains decompiler
// Type: SecretTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (BoxCollider))]
public class SecretTrigger : BaseGameProp
{
  [SerializeField]
  private Renderer[] _visuals;
  [SerializeField]
  private float _activationTime = 15f;
  private SecretBehaviour _reciever;
  private float _showVisualsEndTime;

  private void Awake() => this.gameObject.layer = 21;

  private void OnDisable()
  {
    foreach (Renderer visual in this._visuals)
      visual.material.SetColor("_Color", Color.black);
  }

  private void Update()
  {
    if ((double) this._showVisualsEndTime > (double) Time.time)
    {
      foreach (Renderer visual in this._visuals)
        visual.material.SetColor("_Color", new Color((float) (((double) Mathf.Sin(Time.time * 4f) + 1.0) * 0.30000001192092896), 0.0f, 0.0f));
    }
    else
      this.enabled = false;
  }

  public override void ApplyDamage(DamageInfo shot)
  {
    if ((bool) (Object) this._reciever)
    {
      this.enabled = true;
      this._showVisualsEndTime = Time.time + this._activationTime;
      this._reciever.SetTriggerActivated(this);
    }
    else
      Debug.LogError((object) ("The SecretTrigger " + this.gameObject.name + " is not assigned to a SecretReciever!"));
  }

  public void SetSecretReciever(SecretBehaviour logic) => this._reciever = logic;

  public float ActivationTimeOut => this._showVisualsEndTime;
}
