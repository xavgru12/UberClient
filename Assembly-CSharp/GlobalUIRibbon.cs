using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.DataCenter.Common.Entities;
using UnityEngine;

public class GlobalUIRibbon : MonoBehaviour
{
	private abstract class RibbonEvent
	{
		protected const float _timeStage1 = 1f;

		protected const float _timeStage2 = 6f;

		protected float _alpha;

		protected float _scale;

		protected float _timer;

		protected float _speed;

		protected float _duration = 8f;

		protected float _height = 10f;

		protected float _value;

		protected float _delta;

		protected Rect _rect;

		protected Color _color;

		protected GUIStyle _style;

		protected string _content;

		public abstract int Value
		{
			get;
		}

		public abstract float Alpha
		{
			get;
		}

		public RibbonEvent(int horizontalPosition, Color color, int deltaValue, int currentValue)
		{
			_value = currentValue - deltaValue;
			_delta = deltaValue;
			_timer = 0f;
			_alpha = 1f;
			_scale = 1f;
			_color = color;
			_speed = Mathf.Sign(deltaValue) * 20f;
			_style = BlueStonez.label_interparkbold_32pt;
			if (_speed > 0f)
			{
				_content = string.Format("+{0}", deltaValue.ToString("N0"));
			}
			else
			{
				_content = string.Format("-{0}", Mathf.Abs(deltaValue).ToString("N0"));
			}
			Vector2 vector = _style.CalcSize(new GUIContent(_content));
			_rect = new Rect(horizontalPosition, _height, vector.x, vector.y);
		}

		public void Draw()
		{
			GUIUtility.ScaleAroundPivot(new Vector2(_scale, _scale), new Vector2(_rect.x + _rect.width / 2f, _rect.y + _rect.height / 2f));
			GUI.contentColor = new Color(0f, 0f, 0f, _alpha);
			GUI.Label(new Rect(_rect.x + 1f, _rect.y + 1f, _rect.width, _rect.height), _content, _style);
			GUI.contentColor = new Color(_color.r, _color.g, _color.b, _alpha);
			GUI.Label(_rect, _content, _style);
			GUI.contentColor = Color.white;
			GUI.matrix = Matrix4x4.identity;
		}

		public abstract void Animate();

		public abstract bool IsDone();
	}

	private class GainEvent : RibbonEvent
	{
		private float _moveTime = 0.3f;

		private float _stayTime = 0.5f;

		private float _scaleTime = 0.3f;

		public override int Value => Mathf.RoundToInt(_value + _delta * _timer / (_moveTime + _stayTime + _scaleTime));

		public override float Alpha => 1f - _alpha;

		public GainEvent(int x, Color color, int deltaValue, int currentValue)
			: base(x, color, deltaValue, currentValue)
		{
			_alpha = 0f;
			_scale = 1f;
			_rect.y = 0f;
		}

		public override void Animate()
		{
			if (_timer < _moveTime)
			{
				_rect.y = Mathfx.Berp(0f, _height, _timer / _moveTime);
				_alpha = Mathf.Lerp(_alpha, 1f, 8f * Time.deltaTime);
			}
			else if (_timer > _moveTime + _scaleTime && _timer < _moveTime + _stayTime + _scaleTime)
			{
				_scale = Mathf.Lerp(_scale, 0.5f, 15f * Time.deltaTime);
				_alpha = Mathf.Lerp(_alpha, 0.2f, 10f * Time.deltaTime);
			}
			_timer += Time.deltaTime;
		}

		public override bool IsDone()
		{
			return _timer >= _moveTime + _scaleTime + _stayTime;
		}
	}

	private class LoseEvent : RibbonEvent
	{
		private float _scaleTime = 0.5f;

		private float _moveTime = 0.5f;

		public override int Value => Mathf.RoundToInt(_value + _delta * _timer / (_moveTime + _scaleTime));

		public override float Alpha => 1f - _alpha;

		public LoseEvent(int x, Color color, int deltaValue, int currentValue)
			: base(x, color, deltaValue, currentValue)
		{
			_scale = 0.3f;
		}

		public override void Animate()
		{
			if (_timer < _scaleTime)
			{
				_scale = Mathfx.Berp(0.3f, 1f, _timer / _scaleTime * 2f);
			}
			else if (_timer < _scaleTime + _moveTime)
			{
				_rect.y = Mathf.Lerp(_rect.y, 0f, 10f * Time.deltaTime);
				_alpha = Mathf.Lerp(_alpha, 0f, 10f * Time.deltaTime);
			}
			_timer += Time.deltaTime;
		}

		public override bool IsDone()
		{
			return _timer >= _moveTime + _scaleTime;
		}
	}

	private enum EventType
	{
		XpEvent,
		PointEvent,
		CreditEvent
	}

	private class LiveFeed
	{
		private class FeedItem
		{
			public float Timer;

			public string Date;

			public float Length;

			public LiveFeedView View;

			public void Draw(Rect rect)
			{
				GUI.Label(new Rect(8f, rect.y + 1f, 160f, 14f), Date, BlueStonez.label_interparkmed_11pt_left);
				if (View.Priority == 0)
				{
					GUI.color = Color.red;
				}
				GUI.Label(new Rect(80f, rect.y, Length, 14f), View.Description, BlueStonez.label_interparkbold_11pt_left);
				GUI.color = Color.white;
				GUI.contentColor = ((View.Priority != 0) ? ColorScheme.UberStrikeYellow : Color.red);
				if (!string.IsNullOrEmpty(View.Url) && GUITools.Button(new Rect(90f + Length, rect.y, 78f, 16f), new GUIContent(LocalizedStrings.MoreInfo, LocalizedStrings.OpenThisLinkInANewBrowserWindow), BlueStonez.buttondark_medium))
				{
					ScreenResolutionManager.IsFullScreen = false;
					ApplicationDataManager.OpenUrl(View.Description, View.Url);
				}
				GUI.contentColor = Color.white;
			}
		}

		private List<FeedItem> _content;

		private int _index;

		private bool _isRotating;

		private float _rotateY;

		public LiveFeed()
		{
			_content = new List<FeedItem>(10);
		}

		public void SetContent(List<LiveFeedView> feeds)
		{
			_content.Clear();
			foreach (LiveFeedView feed in feeds)
			{
				FeedItem feedItem = new FeedItem();
				feedItem.Timer = 0f;
				feedItem.View = feed;
				feedItem.Date = feed.Date.ToShortDateString();
				Vector2 vector = BlueStonez.label_interparkbold_11pt_left.CalcSize(new GUIContent(feed.Description));
				feedItem.Length = vector.x;
				if (feed.Priority == 0)
				{
					_content.Insert(0, feedItem);
				}
				else
				{
					_content.Add(feedItem);
				}
			}
		}

		public void Update()
		{
			if (_content.Count == 0 || _content[0].View.Priority == 0)
			{
				return;
			}
			if (_isRotating)
			{
				_rotateY = Mathf.Clamp(_rotateY + Time.deltaTime * 10f, 0f, 16f);
				if (_rotateY == 16f)
				{
					_isRotating = false;
					_index = (_index + 1) % _content.Count;
				}
				return;
			}
			FeedItem feedItem = _content[_index];
			if (feedItem.Timer > 5f)
			{
				feedItem.Timer = 0f;
				_rotateY = 0f;
				_isRotating = true;
			}
			else
			{
				feedItem.Timer += Time.deltaTime;
			}
		}

		public void Draw(Rect rect)
		{
			if (_content.Count != 0)
			{
				FeedItem feedItem = _content[_index];
				GUI.BeginGroup(rect);
				if (_isRotating)
				{
					FeedItem feedItem2 = _content[(_index + 1) % _content.Count];
					feedItem.Draw(new Rect(0f, 0f - _rotateY, rect.width, rect.height));
					feedItem2.Draw(new Rect(0f, 16f - _rotateY, rect.width, rect.height));
				}
				else
				{
					feedItem.Draw(new Rect(0f, 0f, rect.width, rect.height));
				}
				GUI.EndGroup();
			}
		}
	}

	private const int NEWSFEED_HEIGHT = 0;

	private const int PAGETABS_HEIGHT = 0;

	private const int STATUSBAR_HEIGHT = 44;

	private const int ButtonY = 0;

	private int _height = 44;

	private GuiDropDown _optionsDropdown;

	private bool _isReady;

	private float _yOffset;

	private Dictionary<EventType, RibbonEvent> _ribbonEvents;

	public static GlobalUIRibbon Instance
	{
		get;
		private set;
	}

	public bool IsVisible => base.enabled;

	public int StatusBarHeight
	{
		get
		{
			if (!ApplicationDataManager.IsMobile)
			{
				return 0;
			}
			return 44;
		}
	}

	private float CreditsAlpha
	{
		get
		{
			if (_ribbonEvents.TryGetValue(EventType.CreditEvent, out RibbonEvent value))
			{
				return value.Alpha;
			}
			return 1f;
		}
	}

	private int CreditsValue
	{
		get
		{
			if (_ribbonEvents.TryGetValue(EventType.CreditEvent, out RibbonEvent value))
			{
				return value.Value;
			}
			return PlayerDataManager.Credits;
		}
	}

	private float PointsAlpha
	{
		get
		{
			if (_ribbonEvents.TryGetValue(EventType.PointEvent, out RibbonEvent value))
			{
				return value.Alpha;
			}
			return 1f;
		}
	}

	private int PointsValue
	{
		get
		{
			if (_ribbonEvents.TryGetValue(EventType.PointEvent, out RibbonEvent value))
			{
				return value.Value;
			}
			return PlayerDataManager.Points;
		}
	}

	private float XpAlpha
	{
		get
		{
			if (_ribbonEvents.TryGetValue(EventType.XpEvent, out RibbonEvent value))
			{
				return value.Alpha;
			}
			return 1f;
		}
	}

	private int XpValue
	{
		get
		{
			if (_ribbonEvents.TryGetValue(EventType.XpEvent, out RibbonEvent value))
			{
				return value.Value;
			}
			return PlayerDataManager.PlayerExperience;
		}
	}

	public int Height()
	{
		return (int)_yOffset + _height - 1;
	}

	private void Awake()
	{
		Instance = this;
		_ribbonEvents = new Dictionary<EventType, RibbonEvent>();
		EventHandler.Global.AddListener<GameEvents.PlayerPause>(OnPlayerPaused);
		EventHandler.Global.AddListener<GameEvents.MatchEnd>(OnMatchEndEvent);
		EventHandler.Global.AddListener<GameEvents.PlayerIngame>(OnPlayerInGameEvent);
		EventHandler.Global.AddListener<GameEvents.PlayerDied>(OnPlayerKilled);
	}

	private void OnPlayerKilled(GameEvents.PlayerDied ev)
	{
		Show();
	}

	private void OnMatchEndEvent(GameEvents.MatchEnd ev)
	{
		Show();
	}

	private void OnPlayerPaused(GameEvents.PlayerPause ev)
	{
		Show();
	}

	private void OnPlayerInGameEvent(GameEvents.PlayerIngame ev)
	{
		Hide();
	}

	private void Start()
	{
		_yOffset = -_height;
		InitOptionsDropdown();
		base.enabled = false;
		_isReady = true;
	}

	private void InitOptionsDropdown()
	{
		_optionsDropdown = new GuiDropDown();
		_optionsDropdown.Caption = new GUIContent(GlobalUiIcons.QuadpanelButtonOptions);
		_optionsDropdown.Add(new GUIContent(" " + LocalizedStrings.Help, GlobalUiIcons.QuadpanelButtonHelp), delegate
		{
			PanelManager.Instance.OpenPanel(PanelType.Help);
		});
		_optionsDropdown.Add(new GUIContent(" " + LocalizedStrings.Options, GlobalUiIcons.QuadpanelButtonOptions), delegate
		{
			PanelManager.Instance.OpenPanel(PanelType.Options);
		});
		_optionsDropdown.Add(new GUIContent(" " + LocalizedStrings.Audio, GlobalUiIcons.QuadpanelButtonSoundoff), new GUIContent(" " + LocalizedStrings.Audio, GlobalUiIcons.QuadpanelButtonSoundon), () => ApplicationDataManager.ApplicationOptions.AudioEnabled, delegate
		{
			ApplicationDataManager.ApplicationOptions.AudioEnabled = !ApplicationDataManager.ApplicationOptions.AudioEnabled;
			AutoMonoBehaviour<SfxManager>.Instance.EnableAudio(ApplicationDataManager.ApplicationOptions.AudioEnabled);
			ApplicationDataManager.ApplicationOptions.SaveApplicationOptions();
			AutoMonoBehaviour<SfxManager>.Instance.UpdateEffectsVolume();
		});
		if (Application.isWebPlayer)
		{
			_optionsDropdown.Add(new GUIContent(" " + LocalizedStrings.Windowed, GlobalUiIcons.QuadpanelButtonNormalize), new GUIContent(" " + LocalizedStrings.Fullscreen, GlobalUiIcons.QuadpanelButtonFullscreen), () => Screen.fullScreen, delegate
			{
				ScreenResolutionManager.IsFullScreen = !Screen.fullScreen;
			});
		}
		else
		{
			if (PlayerDataManager.AccessLevel == MemberAccessLevel.Admin)
			{
				_optionsDropdown.Add(new GUIContent(" CONSOLE"), delegate
				{
					DebugConsoleManager.Instance.IsDebugConsoleEnabled = true;
				});
			}
			_optionsDropdown.Add(new GUIContent(" " + LocalizedStrings.Logout, GlobalUiIcons.QuadpanelButtonLogout), delegate
			{
				PopupSystem.ShowMessage("Logout", "This will log out your UberStrike account and allow you to login to another account.", PopupSystem.AlertType.OKCancel, delegate
				{
					Singleton<AuthenticationManager>.Instance.StartLogout();
				});
			});
		}
		EventHandler.Global.AddListener(delegate(GlobalEvents.Login ev)
		{
			if (ev.AccessLevel >= MemberAccessLevel.Moderator)
			{
				_optionsDropdown.Add(new GUIContent(" " + LocalizedStrings.Moderate, GlobalUiIcons.QuadpanelButtonModerate), delegate
				{
					if (PlayerDataManager.AccessLevel >= MemberAccessLevel.Moderator)
					{
						PanelManager.Instance.OpenPanel(PanelType.Moderation);
					}
				});
			}
		});
	}

	private void Update()
	{
		if (_isReady)
		{
			if (_ribbonEvents.Count > 0)
			{
				foreach (int value2 in Enum.GetValues(typeof(EventType)))
				{
					if (_ribbonEvents.TryGetValue((EventType)value2, out RibbonEvent value))
					{
						if (value.IsDone())
						{
							_ribbonEvents.Remove((EventType)value2);
						}
						else
						{
							value.Animate();
						}
					}
				}
			}
			if (_yOffset < 0f)
			{
				_yOffset = Mathf.Lerp(_yOffset, 0.1f, Time.deltaTime * 8f);
			}
			else
			{
				_yOffset = 0f;
			}
		}
	}

	private void OnGUI()
	{
		if (!_isReady || !Singleton<AuthenticationManager>.Instance.IsAuthComplete)
		{
			return;
		}
		GUI.depth = 7;
		GUI.Label(new Rect(0f, _yOffset, Screen.width, 44f), GUIContent.none, BlueStonez.window_standard_grey38);
		DoMenuBar(new Rect(0f, _yOffset, Screen.width, 44f));
		if (_ribbonEvents.Count > 0)
		{
			EventType[] array = (EventType[])Enum.GetValues(typeof(EventType));
			for (int i = 0; i < array.Length; i++)
			{
				if (_ribbonEvents.TryGetValue(array[i], out RibbonEvent value))
				{
					value.Draw();
				}
			}
		}
		GuiManager.DrawTooltip();
	}

	private void DoMenuBar(Rect rect)
	{
		GUI.enabled = !Singleton<SceneLoader>.Instance.IsLoading;
		if (MenuPageManager.Instance == null)
		{
			if (GUI.Button(new Rect(rect.x + 9f, rect.y + 6f, 100f, 32f), "Back", BlueStonez.button_white))
			{
				Singleton<GameStateController>.Instance.LeaveGame(warnBeforeLeaving: true);
			}
		}
		else if (!MenuPageManager.Instance.IsCurrentPage(PageType.Home) && GUITools.Button(new Rect(rect.x + 9f, rect.y + 6f, 100f, 32f), new GUIContent("Back"), BlueStonez.button_white))
		{
			if (MenuPageManager.Instance.IsCurrentPage(PageType.Play) && Singleton<GameServerController>.Instance.SelectedServer != null)
			{
				PlayPageGUI.Instance.ShowServerSelection();
			}
			else if (MenuPageManager.Instance.IsCurrentPage(PageType.Training))
			{
				MenuPageManager.Instance.LoadPage(PageType.Play);
			}
			else
			{
				Singleton<GameStateController>.Instance.Client.Disconnect();
				MenuPageManager.Instance.LoadPage(PageType.Home);
			}
		}
		int num = 0;
		if (ApplicationDataManager.IsMobile)
		{
			num = 44;
		}
		if (!GameState.Current.HasJoinedGame || GamePageManager.HasPage)
		{
			Rect position = new Rect(rect.width - 420f + (float)num, 12f, 100f, 20f);
			GUIContent content = new GUIContent(PointsValue.ToString("N0"), ShopIcons.IconPoints20x20);
			GUI.color = new Color(1f, 1f, 1f, PointsAlpha);
			GUI.Label(position, content, BlueStonez.label_interparkbold_13pt);
			Rect position2 = new Rect(rect.width - 310f + (float)num, 12f, 100f, 20f);
			GUIContent content2 = new GUIContent(CreditsValue.ToString("N0"), ShopIcons.IconCredits20x20);
			GUI.color = new Color(1f, 1f, 1f, CreditsAlpha);
			GUI.Label(position2, content2, BlueStonez.label_interparkbold_13pt);
			GUI.color = Color.white;
			if (GUITools.Button(new Rect(rect.width - 200f + (float)num, rect.y + 9f, 100f, 26f), new GUIContent("Get Credits", LocalizedStrings.ClickHereBuyCreditsMsg), BlueStonez.buttongold_medium))
			{
				ApplicationDataManager.OpenBuyCredits();
			}
		}
		if (!ApplicationDataManager.IsMobile)
		{
			GUIContent content3 = (!Screen.fullScreen) ? new GUIContent(string.Empty, GlobalUiIcons.QuadpanelButtonFullscreen, "Enter Fullscreen mode.") : new GUIContent(string.Empty, GlobalUiIcons.QuadpanelButtonNormalize, "Return to windowed mode.");
			if (GUI.Button(new Rect(Screen.width - 88, _yOffset, 44f, 44f), content3, BlueStonez.buttondark_medium))
			{
				ScreenResolutionManager.IsFullScreen = !Screen.fullScreen;
			}
		}
		_optionsDropdown.SetRect(new Rect(Screen.width - 44, _yOffset, 44f, 44f));
		_optionsDropdown.Draw();
	}

	private void DoFpsAndVersion()
	{
		string text = $"{ApplicationDataManager.FrameRate} v{ApplicationDataManager.Version}";
		GUI.color = Color.white.SetAlpha(0.3f);
		GUI.Label(new Rect(5f, Screen.height - 25, 190f, 20f), text, BlueStonez.label_interparkmed_11pt_right);
		GUI.color = Color.white;
	}

	public void Show()
	{
		base.enabled = true;
		EventHandler.Global.Fire(new GlobalEvents.GlobalUIRibbonChanged());
	}

	public void Hide()
	{
		base.enabled = false;
		_yOffset = -_height;
		EventHandler.Global.Fire(new GlobalEvents.GlobalUIRibbonChanged());
	}

	public void AddXPEvent(int deltaXP)
	{
		if (deltaXP > 0)
		{
			_ribbonEvents[EventType.XpEvent] = new GainEvent(370, Color.white, deltaXP, PlayerDataManager.PlayerExperience);
		}
		else if (deltaXP < 0)
		{
			_ribbonEvents[EventType.XpEvent] = new LoseEvent(370, Color.white, deltaXP, PlayerDataManager.PlayerExperience);
		}
	}

	public void AddCreditsEvent(int deltaCredits)
	{
		int x = Screen.width - 310 + StatusBarHeight;
		if (deltaCredits > 0)
		{
			_ribbonEvents[EventType.CreditEvent] = new GainEvent(x, Color.white, deltaCredits, PlayerDataManager.Credits);
			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.GetPoints, 0uL);
		}
		else if (deltaCredits < 0)
		{
			_ribbonEvents[EventType.CreditEvent] = new LoseEvent(x, Color.white, deltaCredits, PlayerDataManager.Credits);
		}
	}

	public void AddPointsEvent(int deltaPoints)
	{
		int x = Screen.width - 420 + StatusBarHeight;
		if (deltaPoints > 0)
		{
			_ribbonEvents[EventType.PointEvent] = new GainEvent(x, Color.white, deltaPoints, PlayerDataManager.Points);
			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.GetPoints, 0uL);
		}
		else if (deltaPoints < 0)
		{
			_ribbonEvents[EventType.PointEvent] = new LoseEvent(x, Color.white, deltaPoints, PlayerDataManager.Points);
		}
	}
}
