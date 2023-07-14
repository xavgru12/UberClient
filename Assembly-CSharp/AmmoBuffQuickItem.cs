// Decompiled with JetBrains decompiler
// Type: AmmoBuffQuickItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class AmmoBuffQuickItem : QuickItem
{
  [SerializeField]
  private AmmoBuffConfiguration _config;
  private StateMachine machine = new StateMachine();

  public override QuickItemConfiguration Configuration
  {
    get => (QuickItemConfiguration) this._config;
    set => this._config = (AmmoBuffConfiguration) value;
  }

  protected override void OnActivated()
  {
    if (!this.machine.ContainsState(1))
      this.machine.RegisterState(1, (IState) new AmmoBuffQuickItem.ActivatedState(this));
    Singleton<QuickItemSfxController>.Instance.ShowThirdPersonEffect(GameState.LocalPlayer.Character, QuickItemLogic.AmmoPack, this._config.RobotLifeTimeMilliSeconds, this._config.ScrapsLifeTimeMilliSeconds, this._config.IsInstant);
    GameState.CurrentGame.ActivateQuickItem(QuickItemLogic.AmmoPack, this._config.RobotLifeTimeMilliSeconds, this._config.ScrapsLifeTimeMilliSeconds, this._config.IsInstant);
    this.machine.SetState(1);
  }

  private void Update() => this.machine.Update();

  private void OnGUI()
  {
    if (!this.Behaviour.IsBusy || (double) this.Behaviour.FocusTimeRemaining <= 0.0)
      return;
    float height = Mathf.Clamp((float) Screen.height * 0.03f, 10f, 40f);
    float width = height * 10f;
    GUI.Label(new Rect((float) (((double) Screen.width - (double) width) * 0.5), (float) (Screen.height / 2 + 20), width, height), "Charging Ammo", BlueStonez.label_interparkbold_16pt);
    GUITools.DrawWarmupBar(new Rect((float) (((double) Screen.width - (double) width) * 0.5), (float) (Screen.height / 2 + 50), width, height), this.Behaviour.FocusTimeTotal - this.Behaviour.FocusTimeRemaining, this.Behaviour.FocusTimeTotal);
  }

  private class ActivatedState : IState
  {
    private AmmoBuffQuickItem _item;
    private float _nextHealthIncrease;
    private float _increaseCounter;

    public ActivatedState(AmmoBuffQuickItem configuration) => this._item = configuration;

    public void OnEnter()
    {
      if (this._item._config.IncreaseTimes > 0)
      {
        this._increaseCounter = (float) this._item._config.IncreaseTimes;
        this._nextHealthIncrease = 0.0f;
      }
      else
      {
        this.SendAmmoIncrease();
        this._item.machine.PopState();
      }
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
      if ((double) this._nextHealthIncrease >= (double) Time.time)
        return;
      --this._increaseCounter;
      this._nextHealthIncrease = Time.time + (float) this._item._config.IncreaseFrequency / 1000f;
      this.SendAmmoIncrease();
      if ((double) this._increaseCounter > 0.0)
        return;
      this._item.machine.PopState();
    }

    public void OnGUI()
    {
    }

    private void SendAmmoIncrease()
    {
      switch (this._item._config.AmmoIncrease)
      {
        case IncreaseStyle.Absolute:
          CmuneEventHandler.Route((object) new AddAmmoIncreaseEvent()
          {
            Amount = this._item._config.PointsGain
          });
          break;
        case IncreaseStyle.PercentFromStart:
          CmuneEventHandler.Route((object) new AmmoAddStartEvent()
          {
            Percent = this._item._config.PointsGain
          });
          break;
        case IncreaseStyle.PercentFromMax:
          CmuneEventHandler.Route((object) new AmmoAddMaxEvent()
          {
            Percent = this._item._config.PointsGain
          });
          break;
        default:
          throw new NotImplementedException("SendAmmoIncrease for type: " + (object) this._item._config.AmmoIncrease);
      }
    }
  }
}
