// Decompiled with JetBrains decompiler
// Type: QuickItemBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Text;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class QuickItemBehaviour
{
  private StateMachine _machine;
  private QuickItemBehaviour.CoolingDownState _coolDownState;
  private QuickItemBehaviour.FocusedState _focusedState;
  private QuickItem _item;
  private float _chargeTimeOut;
  public Action OnActivated;

  public QuickItemBehaviour(QuickItem item, Action onActivated)
  {
    this._item = item;
    this.OnActivated = onActivated;
    this._machine = new StateMachine();
    this._coolDownState = new QuickItemBehaviour.CoolingDownState(this);
    this._focusedState = new QuickItemBehaviour.FocusedState(this);
    this._machine.RegisterState(1, (IState) this._coolDownState);
    this._machine.RegisterState(2, (IState) this._focusedState);
  }

  public float CoolDownTimeRemaining => Mathf.Max(this._coolDownState.TimeOut - Time.time, 0.0f);

  public float CoolDownTimeTotal => (float) this._item.Configuration.CoolDownTime / 1000f;

  public float FocusTimeRemaining => Mathf.Max(this._focusedState.TimeOut - Time.time, 0.0f);

  public float FocusTimeTotal => (float) this._item.Configuration.WarmUpTime / 1000f;

  public float ChargingTimeRemaining => Mathf.Max(this._chargeTimeOut - Time.time, 0.0f);

  public float ChargingTimeTotal => (float) this._item.Configuration.RechargeTime / 1000f;

  public int CurrentAmount { get; set; }

  public GameInputKey FocusKey { get; set; }

  public bool IsBusy => this._machine.IsRunning;

  private void Activate()
  {
    if (this.CurrentAmount == this._item.Configuration.AmountRemaining)
      this._chargeTimeOut = Time.time + (float) this._item.Configuration.RechargeTime / 1000f;
    if (this._item.Configuration.CoolDownTime > 0)
      this._machine.PushState(1);
    --this.CurrentAmount;
    CmuneEventHandler.Route((object) new QuickItemAmountChangedEvent());
    if (this.OnActivated == null)
      return;
    this.OnActivated();
  }

  public bool Run()
  {
    if (this.CurrentAmount <= 0 || this._machine.IsRunning)
      return false;
    AutoMonoBehaviour<MonoRoutine>.Instance.OnUpdateEvent += new Action(this.Update);
    if (this._item.Configuration.WarmUpTime > 0)
      this._machine.PushState(2);
    else
      this.Activate();
    return true;
  }

  private void Update()
  {
    this._machine.Update();
    if (this._item.Configuration.RechargeTime > 0 && (double) this._chargeTimeOut < (double) Time.time && this.CurrentAmount < this._item.Configuration.AmountRemaining)
    {
      this.CurrentAmount = Mathf.Min(this.CurrentAmount + 1, this._item.Configuration.AmountRemaining);
      CmuneEventHandler.Route((object) new QuickItemAmountChangedEvent());
      if (this.CurrentAmount < this._item.Configuration.AmountRemaining)
        this._chargeTimeOut = Time.time + (float) this._item.Configuration.RechargeTime / 1000f;
    }
    if (this._machine.IsRunning || this.CurrentAmount != this._item.Configuration.AmountRemaining)
      return;
    AutoMonoBehaviour<MonoRoutine>.Instance.OnUpdateEvent -= new Action(this.Update);
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendLine("Name: " + this._item.Configuration.Name);
    stringBuilder.AppendLine("IsBusy: " + (object) this.IsBusy);
    stringBuilder.AppendLine("State: " + (object) this._machine.CurrentStateId);
    stringBuilder.AppendLine("Amount Current: " + (object) this.CurrentAmount);
    stringBuilder.AppendLine("Amount Total: " + (object) this._item.Configuration.AmountRemaining);
    stringBuilder.AppendLine("Time: " + this.CoolDownTimeRemaining.ToString("F2") + " || " + this.ChargingTimeRemaining.ToString("F2"));
    return stringBuilder.ToString();
  }

  private enum States
  {
    CoolingDown = 1,
    Focused = 2,
  }

  private class CoolingDownState : IState
  {
    private QuickItemBehaviour behaviour;

    public CoolingDownState(QuickItemBehaviour behaviour) => this.behaviour = behaviour;

    public float TimeOut { get; private set; }

    public void OnEnter() => this.TimeOut = Time.time + (float) this.behaviour._item.Configuration.CoolDownTime / 1000f;

    public void OnExit()
    {
    }

    public void OnGUI()
    {
    }

    public void OnUpdate()
    {
      if ((double) this.TimeOut >= (double) Time.time)
        return;
      this.behaviour._machine.PopState();
    }
  }

  private class FocusedState : IState
  {
    private QuickItemBehaviour behaviour;
    private float _originalStopSpeed;

    public FocusedState(QuickItemBehaviour behaviour) => this.behaviour = behaviour;

    public float TimeOut { get; private set; }

    public void OnEnter()
    {
      this.TimeOut = Time.time + (float) this.behaviour._item.Configuration.WarmUpTime / 1000f;
      Singleton<WeaponController>.Instance.IsEnabled = false;
      Singleton<QuickItemController>.Instance.IsCharging = true;
      Singleton<WeaponController>.Instance.PutdownCurrentWeapon();
      Singleton<HudDrawFlagGroup>.Instance.BaseDrawFlag &= ~HudDrawFlags.Reticle;
      GameState.LocalPlayer.MoveController.IsJumpDisabled = true;
      this._originalStopSpeed = LevelEnviroment.Instance.Settings.StopSpeed;
      LevelEnviroment.Instance.Settings.StopSpeed = this._originalStopSpeed * this.behaviour._item.Configuration.SlowdownOnCharge;
    }

    public void OnExit()
    {
      this.TimeOut = 0.0f;
      Singleton<WeaponController>.Instance.IsEnabled = true;
      Singleton<QuickItemController>.Instance.IsCharging = false;
      Singleton<WeaponController>.Instance.PickupCurrentWeapon();
      Singleton<HudDrawFlagGroup>.Instance.BaseDrawFlag |= HudDrawFlags.Reticle;
      GameState.LocalPlayer.MoveController.IsJumpDisabled = false;
      LevelEnviroment.Instance.Settings.StopSpeed = this._originalStopSpeed;
    }

    public void OnUpdate()
    {
      if ((double) this.TimeOut < (double) Time.time)
      {
        this.behaviour._machine.PopState();
        this.behaviour.Activate();
      }
      else
      {
        if (AutoMonoBehaviour<InputManager>.Instance.IsDown(this.behaviour.FocusKey) || AutoMonoBehaviour<InputManager>.Instance.IsDown(GameInputKey.UseQuickItem) || Singleton<QuickItemController>.Instance.IsQuickItemMobilePushed)
          return;
        this.behaviour._machine.PopState();
      }
    }

    public void OnGUI()
    {
    }
  }
}
