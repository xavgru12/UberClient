// Decompiled with JetBrains decompiler
// Type: BaseGameProp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class BaseGameProp : MonoBehaviour, IShootable
{
  protected Transform _transform;
  private Rigidbody _rigidbody;
  private bool _isMoved;
  private bool _isSleeping;
  private bool _isFreezed;
  private bool _isPassive;
  private float _originalMass = 1f;
  private Component[] _rbs;
  private float[] _oriMass;
  private float[] _adrag;
  private float[] _drag;
  [SerializeField]
  protected bool _recieveProjectileDamage = true;

  private void FreezeObject(bool b)
  {
    if (b == this._isFreezed)
      return;
    if (b)
    {
      this._rbs = this.GetComponentsInChildren(typeof (Rigidbody));
      this._oriMass = new float[this._rbs.Length];
      this._adrag = new float[this._rbs.Length];
      this._drag = new float[this._rbs.Length];
    }
    for (int index = 0; index < this._rbs.Length; ++index)
    {
      Rigidbody rb = (Rigidbody) this._rbs[index];
      rb.velocity = Vector3.zero;
      rb.angularVelocity = Vector3.zero;
      rb.useGravity = !b;
      rb.freezeRotation = b;
      if (b)
      {
        this._oriMass[index] = rb.mass;
        this._adrag[index] = rb.angularDrag;
        this._drag[index] = rb.drag;
        rb.angularDrag = 10000f;
        rb.drag = 10000f;
        rb.mass = 0.1f;
      }
      else
      {
        rb.mass = this._oriMass[index];
        rb.angularDrag = this._adrag[index];
        rb.drag = this._drag[index];
      }
    }
    this._isFreezed = b;
  }

  public virtual void ApplyDamage(DamageInfo shot) => this.ApplyForce(shot.Hitpoint, shot.Force * 5f);

  public virtual bool IsVulnerable => true;

  public virtual bool IsLocal => false;

  public virtual void ApplyForce(Vector3 position, Vector3 direction)
  {
    if (!this.HasRigidbody)
      return;
    this.Rigidbody.AddForceAtPosition(direction, position);
  }

  public virtual bool CanApplyDamage => true;

  public Vector3 Scale => this.Transform.localScale;

  public Vector3 Position => this.Transform.position;

  public Quaternion Rotation => this.Transform.rotation;

  public Vector3 Velocity => this.HasRigidbody ? this._rigidbody.velocity : Vector3.zero;

  public Vector3 AngularVelocity => this.HasRigidbody ? this._rigidbody.angularVelocity : Vector3.zero;

  public Transform Transform
  {
    get
    {
      if ((Object) this._transform == (Object) null)
        this._transform = this.transform;
      return this._transform;
    }
  }

  public bool HasRigidbody => (Object) this.Rigidbody != (Object) null;

  public Rigidbody Rigidbody
  {
    get
    {
      if ((Object) this._rigidbody == (Object) null)
        this._rigidbody = this.rigidbody;
      return this._rigidbody;
    }
  }

  public bool IsMoved
  {
    get => this._isMoved;
    set => this._isMoved = value;
  }

  public bool IsSleeping
  {
    get => this._isSleeping;
    set
    {
      this._isSleeping = value;
      if (!this.HasRigidbody)
        return;
      if (this._isSleeping)
        this.Rigidbody.Sleep();
      else
        this.Rigidbody.WakeUp();
    }
  }

  public float Mass
  {
    get => this._originalMass;
    set
    {
      this._originalMass = value;
      if (!this.HasRigidbody)
        return;
      this.Rigidbody.mass = this._originalMass;
    }
  }

  public virtual bool IsFreezed
  {
    get => this._isFreezed;
    set => this.FreezeObject(value);
  }

  public virtual bool IsPassiv
  {
    get => this._isPassive;
    set => this._isPassive = value;
  }

  public bool RecieveProjectileDamage => this._recieveProjectileDamage;
}
