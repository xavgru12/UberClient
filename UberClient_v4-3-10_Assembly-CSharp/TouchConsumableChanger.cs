// Decompiled with JetBrains decompiler
// Type: TouchConsumableChanger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class TouchConsumableChanger : TouchButton
{
  private bool _touchUsed;
  private Vector2 _touchStartPos;
  public float TimeBeforeTap = 0.2f;
  public float SwipeThreshold = 4f;
  private Rect _leftArrow;
  private Rect _rightArrow;
  private GUIContent _leftArrowContent;
  private GUIContent _rightArrowContent;
  private bool _didStartTap;

  public TouchConsumableChanger()
  {
    this.OnTouchBegan += new Action<Vector2>(this.TouchConsumableChanger_OnTouchBegan);
    this.OnTouchMoved += new Action<Vector2, Vector2>(this.TouchConsumableChanger_OnTouchMoved);
    this.OnTouchEnded += new Action<Vector2>(this.TouchConsumableChanger_OnTouchEnded);
    this._leftArrowContent = new GUIContent((Texture) MobileIcons.TouchArrowLeft);
    this._rightArrowContent = new GUIContent((Texture) MobileIcons.TouchArrowRight);
    this.ConsumablesHeld = 0;
  }

  public event Action OnNextConsumable;

  public event Action OnPrevConsumable;

  public event Action OnStartUseConsumable;

  public event Action OnEndUseConsumable;

  public int ConsumablesHeld { get; private set; }

  public void UpdateConsumablesHeld()
  {
    this.ConsumablesHeld = 0;
    if (Singleton<LoadoutManager>.Instance.HasItemInSlot(LoadoutSlotType.QuickUseItem1))
      ++this.ConsumablesHeld;
    if (Singleton<LoadoutManager>.Instance.HasItemInSlot(LoadoutSlotType.QuickUseItem2))
      ++this.ConsumablesHeld;
    if (!Singleton<LoadoutManager>.Instance.HasItemInSlot(LoadoutSlotType.QuickUseItem3))
      return;
    ++this.ConsumablesHeld;
  }

  public override Rect Boundary
  {
    get => base.Boundary;
    set
    {
      float num = value.y + value.height / 2f;
      this._leftArrow = new Rect(value.x - (float) MobileIcons.TouchArrowLeft.width, num - (float) (MobileIcons.TouchArrowLeft.height / 2), (float) MobileIcons.TouchArrowLeft.width, (float) MobileIcons.TouchArrowLeft.height);
      this._rightArrow = new Rect(value.xMax, num - (float) (MobileIcons.TouchArrowLeft.height / 2), (float) MobileIcons.TouchArrowLeft.width, (float) MobileIcons.TouchArrowLeft.height);
      value.x -= this._leftArrow.width;
      value.width += this._leftArrow.width + this._rightArrow.width;
      base.Boundary = value;
    }
  }

  private void TouchConsumableChanger_OnTouchEnded(Vector2 obj)
  {
    if (this.ConsumablesHeld == 0)
      return;
    if (!this._touchUsed)
    {
      if (this.OnStartUseConsumable != null)
        this.OnStartUseConsumable();
      this._touchUsed = true;
      this._didStartTap = true;
    }
    if (!this._didStartTap || this.OnEndUseConsumable == null)
      return;
    this.OnEndUseConsumable();
  }

  private void TouchConsumableChanger_OnTouchMoved(Vector2 pos, Vector2 delta)
  {
    if (this._touchUsed || this.ConsumablesHeld == 0)
      return;
    if ((double) this.finger.StartTouchTime + (double) this.TimeBeforeTap < (double) Time.time)
    {
      if (this.OnStartUseConsumable != null)
        this.OnStartUseConsumable();
      this._touchUsed = true;
      this._didStartTap = true;
    }
    else
    {
      if (this.ConsumablesHeld <= 1)
        return;
      if ((double) this._touchStartPos.x - (double) pos.x > (double) this.SwipeThreshold)
      {
        this._touchUsed = true;
        if (this.OnPrevConsumable == null)
          return;
        this.OnPrevConsumable();
      }
      else
      {
        if ((double) this._touchStartPos.x - (double) pos.x >= -(double) this.SwipeThreshold)
          return;
        this._touchUsed = true;
        if (this.OnNextConsumable == null)
          return;
        this.OnNextConsumable();
      }
    }
  }

  private void TouchConsumableChanger_OnTouchBegan(Vector2 obj)
  {
    if (this.ConsumablesHeld == 0)
      return;
    this._touchStartPos = obj;
    this._touchUsed = false;
    this._didStartTap = false;
  }

  public override void Draw()
  {
    base.Draw();
    if (this.ConsumablesHeld <= 1)
      return;
    GUI.Label(this._leftArrow, this._leftArrowContent);
    GUI.Label(this._rightArrow, this._rightArrowContent);
  }
}
