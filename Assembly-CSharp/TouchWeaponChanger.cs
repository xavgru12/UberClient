// Decompiled with JetBrains decompiler
// Type: TouchWeaponChanger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Core.Types;
using UnityEngine;

public class TouchWeaponChanger : TouchButton
{
  private Texture[] weapons;
  private MeshGUIQuad _quad;
  private MeshGUIQuad _incomingQuad;
  private float _startWeaponSwitch;
  private Rect _leftIconPos;
  private Rect _rightIconPos;
  private Vector2 _touchStartPos;
  private bool _touchUsed;
  private bool _hasWeapon;
  private UberstrikeItemClass _currWeaponClass;
  private bool _moveLeft = true;
  public float WeaponSwitchTime = 0.3f;
  public float SwipeThreshold = 4f;
  private Vector2 _position;

  public TouchWeaponChanger(Texture[] weaponIcons)
  {
    this.weapons = weaponIcons;
    this._position = Vector2.zero;
    this.OnTouchBegan += new Action<Vector2>(this.TouchWeaponChanger_OnTouchBegan);
    this.OnTouchMoved += new Action<Vector2, Vector2>(this.TouchWeaponChanger_OnTouchMoved);
    this.OnTouchEnded += new Action<Vector2>(this.TouchWeaponChanger_OnTouchEnded);
  }

  public event Action OnNextWeapon;

  public event Action OnPrevWeapon;

  public Vector2 Position
  {
    get => this._position;
    set
    {
      this._position = value;
      if (this._quad != null)
        this._quad.Position = this._position;
      this.Boundary = new Rect(this._position.x - (float) (this.weapons[0].width / 2), this._position.y - (float) (this.weapons[0].height / 2), (float) this.weapons[0].width, (float) this.weapons[0].height);
      this._leftIconPos = new Rect(this.Boundary.x - (float) MobileIcons.TouchArrowLeft.width, this.Boundary.y + this.Boundary.height / 2f - (float) (MobileIcons.TouchArrowLeft.height / 2), (float) MobileIcons.TouchArrowLeft.width, (float) MobileIcons.TouchArrowLeft.height);
      this._rightIconPos = new Rect(this.Boundary.xMax, this.Boundary.y + (float) (((double) this.Boundary.height - (double) MobileIcons.TouchArrowRight.height) / 2.0), (float) MobileIcons.TouchArrowRight.width, (float) MobileIcons.TouchArrowRight.height);
    }
  }

  public override bool Enabled
  {
    get => base.Enabled;
    set
    {
      base.Enabled = value;
      if (!base.Enabled)
      {
        if (this._quad != null)
        {
          this._quad.FreeObject();
          this._quad = (MeshGUIQuad) null;
        }
        if (this._incomingQuad == null)
          return;
        this._incomingQuad.FreeObject();
        this._incomingQuad = (MeshGUIQuad) null;
      }
      else
        this.Start();
    }
  }

  private void TouchWeaponChanger_OnTouchEnded(Vector2 obj)
  {
    if (!this._touchUsed)
    {
      if (this.OnNextWeapon != null)
        this.OnNextWeapon();
      this.GenerateNewQuad(Singleton<WeaponController>.Instance.GetCurrentWeapon().Item.ItemClass, false);
    }
    this._touchUsed = true;
  }

  private void TouchWeaponChanger_OnTouchMoved(Vector2 pos, Vector2 delta)
  {
    if (this._touchUsed)
      return;
    if ((double) this._touchStartPos.x - (double) pos.x > (double) this.SwipeThreshold)
    {
      this._touchUsed = true;
      if (this.OnPrevWeapon != null)
        this.OnPrevWeapon();
      this.GenerateNewQuad(Singleton<WeaponController>.Instance.GetCurrentWeapon().Item.ItemClass);
    }
    else
    {
      if ((double) this._touchStartPos.x - (double) pos.x >= -(double) this.SwipeThreshold)
        return;
      this._touchUsed = true;
      if (this.OnNextWeapon != null)
        this.OnNextWeapon();
      this.GenerateNewQuad(Singleton<WeaponController>.Instance.GetCurrentWeapon().Item.ItemClass, false);
    }
  }

  private void TouchWeaponChanger_OnTouchBegan(Vector2 obj)
  {
    this._touchStartPos = obj;
    this._touchUsed = false;
  }

  protected void Start()
  {
    if (this._quad != null)
      this._quad.FreeObject();
    if (this._incomingQuad != null)
    {
      this._incomingQuad.FreeObject();
      this._incomingQuad.QuadMesh.renderer.material.mainTextureOffset = Vector2.zero;
      this._incomingQuad = (MeshGUIQuad) null;
    }
    WeaponSlot currentWeapon = Singleton<WeaponController>.Instance.GetCurrentWeapon();
    if (currentWeapon != null)
    {
      this._hasWeapon = true;
      this._quad = new MeshGUIQuad(this.weapons[(int) currentWeapon.Item.ItemClass], TextAnchor.MiddleCenter);
      this._quad.Position = this.Position;
      this._quad.Scale = new Vector2(1f, 1f);
    }
    else
      this._hasWeapon = false;
    this._startWeaponSwitch = 0.0f;
  }

  public override void Draw()
  {
    base.Draw();
    if (!this._hasWeapon)
      return;
    GUI.Label(this._leftIconPos, (Texture) MobileIcons.TouchArrowLeft);
    GUI.Label(this._rightIconPos, (Texture) MobileIcons.TouchArrowRight);
  }

  public void CheckWeaponChanged()
  {
    if (Singleton<WeaponController>.Instance.GetCurrentWeapon() != null)
    {
      UberstrikeItemClass itemClass = Singleton<WeaponController>.Instance.GetCurrentWeapon().Item.ItemClass;
      if (itemClass == this._currWeaponClass)
        return;
      this.GenerateNewQuad(itemClass);
    }
    else
      this._hasWeapon = false;
  }

  private void GenerateNewQuad(UberstrikeItemClass weaponClass, bool moveLeft = true)
  {
    this._currWeaponClass = weaponClass;
    if (this._incomingQuad != null)
    {
      this._quad.FreeObject();
      this._quad = this._incomingQuad;
    }
    this._incomingQuad = new MeshGUIQuad(this.weapons[(int) weaponClass], TextAnchor.MiddleCenter);
    this._incomingQuad.Position = this.Position;
    this._incomingQuad.QuadMesh.renderer.material.mainTextureOffset = new Vector2(1f, 0.0f);
    this._incomingQuad.Scale = new Vector2(1f, 1f);
    this._incomingQuad.Alpha = 0.0f;
    this._startWeaponSwitch = Time.time;
    this._moveLeft = moveLeft;
  }

  public override void FinalUpdate()
  {
    base.FinalUpdate();
    float from = -0.5f;
    float num = 0.0f;
    float to = 0.5f;
    if (!this._moveLeft)
    {
      from = 0.5f;
      to = -0.5f;
    }
    if ((double) this._startWeaponSwitch + (double) this.WeaponSwitchTime > (double) Time.time)
    {
      float t = (Time.time - this._startWeaponSwitch) / this.WeaponSwitchTime;
      this._incomingQuad.QuadMesh.renderer.material.mainTextureOffset = new Vector2(Mathf.Lerp(from, num, t), 0.0f);
      this._incomingQuad.Alpha = Mathf.Lerp(-1f, 1f, t);
      this._quad.QuadMesh.renderer.material.mainTextureOffset = new Vector2(Mathf.Lerp(num, to, t), 0.0f);
      this._quad.Alpha = Mathf.Lerp(1f, -1f, t);
    }
    else
    {
      if (this._incomingQuad == null)
        return;
      this._incomingQuad.QuadMesh.renderer.material.mainTextureOffset = Vector2.zero;
      this._incomingQuad.Alpha = 1f;
      if (this._quad != null)
        this._quad.FreeObject();
      this._quad = this._incomingQuad;
      this._incomingQuad = (MeshGUIQuad) null;
    }
  }
}
