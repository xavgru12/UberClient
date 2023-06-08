using System;
using UberStrike.Core.Types;
using UnityEngine;

public class HealthBuffQuickItem : QuickItem
{
	private class ActivatedState : IState
	{
		private HealthBuffQuickItem _item;

		private float _nextHealthIncrease;

		private float _increaseCounter;

		public ActivatedState(HealthBuffQuickItem configuration)
		{
			_item = configuration;
		}

		public void OnEnter()
		{
			if (_item._config.IncreaseTimes > 0)
			{
				_increaseCounter = _item._config.IncreaseTimes;
				_nextHealthIncrease = 0f;
			}
			else
			{
				SendHealthIncrease();
				_item.machine.PopState();
			}
		}

		public void OnResume()
		{
		}

		public void OnExit()
		{
		}

		public void OnUpdate()
		{
			if (_nextHealthIncrease < Time.time)
			{
				_increaseCounter -= 1f;
				_nextHealthIncrease = Time.time + (float)_item._config.IncreaseFrequency / 1000f;
				SendHealthIncrease();
				if (_increaseCounter <= 0f)
				{
					_item.machine.PopState();
				}
			}
		}

		private void SendHealthIncrease()
		{
			int num = 0;
			switch (_item._config.HealthIncrease)
			{
			case IncreaseStyle.Absolute:
				num = _item._config.PointsGain;
				break;
			case IncreaseStyle.PercentFromMax:
				num = Mathf.RoundToInt(200f * Mathf.Clamp01((float)_item._config.PointsGain / 100f));
				break;
			case IncreaseStyle.PercentFromStart:
				num = Mathf.RoundToInt(100f * Mathf.Clamp01((float)_item._config.PointsGain / 100f));
				break;
			default:
				throw new NotImplementedException("SendHealthIncrease for type: " + _item._config.HealthIncrease.ToString());
			}
			GameState.Current.Actions.IncreaseHealthAndArmor(num, 0);
			GameState.Current.PlayerData.Health.Value += num;
		}
	}

	[SerializeField]
	private HealthBuffConfiguration _config;

	private StateMachine machine = new StateMachine();

	public override QuickItemConfiguration Configuration
	{
		get
		{
			return _config;
		}
		set
		{
			_config = (HealthBuffConfiguration)value;
		}
	}

	protected override void OnActivated()
	{
		if (!machine.ContainsState(1))
		{
			machine.RegisterState(1, new ActivatedState(this));
		}
		Singleton<QuickItemSfxController>.Instance.ShowThirdPersonEffect(GameState.Current.Player.Character, QuickItemLogic.HealthPack, _config.RobotLifeTimeMilliSeconds, _config.ScrapsLifeTimeMilliSeconds, _config.IsHealInstant);
		GameState.Current.Actions.ActivateQuickItem(QuickItemLogic.HealthPack, _config.RobotLifeTimeMilliSeconds, _config.ScrapsLifeTimeMilliSeconds, _config.IsHealInstant);
		machine.SetState(1);
	}

	private void Update()
	{
		machine.Update();
	}

	private void OnGUI()
	{
		if (base.Behaviour.IsCoolingDown && base.Behaviour.FocusTimeRemaining > 0f)
		{
			float num = Mathf.Clamp((float)Screen.height * 0.03f, 10f, 40f);
			float num2 = num * 10f;
			GUI.Label(new Rect(((float)Screen.width - num2) * 0.5f, Screen.height / 2 + 20, num2, num), "Charging Health", BlueStonez.label_interparkbold_16pt);
			GUITools.DrawWarmupBar(new Rect(((float)Screen.width - num2) * 0.5f, Screen.height / 2 + 50, num2, num), base.Behaviour.FocusTimeTotal - base.Behaviour.FocusTimeRemaining, base.Behaviour.FocusTimeTotal);
		}
	}
}
