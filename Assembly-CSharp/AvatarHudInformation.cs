using UberStrike.Core.Models;
using UnityEngine;

public class AvatarHudInformation : MonoBehaviour
{
	[SerializeField]
	private Vector2 _barOffset = new Vector2(0f, 0f);

	[SerializeField]
	private Color _color = Color.white;

	[SerializeField]
	private Vector3 _offset = new Vector3(0f, 2f, 0f);

	private string _text;

	private Transform _transform;

	private Vector3 _screenPosition;

	private float _barValue;

	private Vector2 _textSize;

	private bool _isInViewport;

	private bool _show;

	private float _timer;

	private GameActorInfo _info;

	private GUIStyle _nameTextStyle;

	private bool _isMe;

	private bool _isEnemy;

	private void Awake()
	{
		_transform = base.transform;
		_nameTextStyle = BlueStonez.label_interparkbold_13pt;
	}

	private void OnGUI()
	{
		if (_isInViewport)
		{
			GUI.depth = 100;
			if (!_isEnemy && !_isMe && GameState.Current.HasJoinedGame)
			{
				Rect position = new Rect(_screenPosition.x - 50f, (float)Screen.height - _screenPosition.y + _barOffset.y, 100f, 6f);
				DrawBar(position, 100);
				Rect position2 = new Rect(position.xMin, (float)Screen.height - _screenPosition.y - _textSize.y - 6f, _textSize.x, _textSize.y);
				DrawName(position2);
			}
			else if (_isMe || _show)
			{
				Rect position3 = new Rect(_screenPosition.x - _textSize.x * 0.5f, (float)Screen.height - _screenPosition.y - _textSize.y - 6f, _textSize.x, _textSize.y);
				DrawName(position3);
			}
		}
	}

	private void LateUpdate()
	{
		_isInViewport = IsTransformInViewport();
		_isEnemy = (_info != null && (_info.TeamID == TeamID.NONE || _info.TeamID != GameState.Current.PlayerData.Player.TeamID) && !GameState.Current.PlayerData.IsSpectator);
		_isMe = (_info == null || _info.Cmid == PlayerDataManager.Cmid);
		_color.a = Mathf.Clamp01(_color.a + Time.deltaTime * 2f);
		if (_timer > 0f)
		{
			_timer = Mathf.Max(_timer - Time.deltaTime, 0f);
			_color.a = Mathf.Clamp01(_timer);
			_show = (_timer > 0f);
		}
	}

	public void Show()
	{
		_show = true;
		if (_isEnemy)
		{
			_timer = 2f;
		}
	}

	public void Hide()
	{
		_show = false;
	}

	public void SetAvatarLabel(string name)
	{
		_text = name;
		_textSize = _nameTextStyle.CalcSize(new GUIContent(name));
	}

	public void SetHealthBarValue(float value)
	{
		_barValue = Mathf.Clamp01(value);
	}

	public void SetCharacterInfo(GameActorInfo info)
	{
		if (info != null)
		{
			SetAvatarLabel((!string.IsNullOrEmpty(info.ClanTag)) ? ("[" + info.ClanTag + "] " + info.PlayerName) : info.PlayerName);
			_info = info;
		}
	}

	private bool IsTransformInViewport()
	{
		if ((bool)Camera.main)
		{
			_screenPosition = Vector3.Lerp(_screenPosition, Camera.main.WorldToScreenPoint(_transform.position + _offset), Time.deltaTime * 30f);
			Vector3 rhs = _transform.position + _offset - Camera.main.transform.position;
			Camera.main.ResetWorldToCameraMatrix();
			if (Vector3.Dot(Camera.main.transform.forward, rhs) > 0f && _screenPosition.x >= 0f && _screenPosition.x <= (float)Screen.width && _screenPosition.y >= 0f)
			{
				return _screenPosition.y <= (float)Screen.height;
			}
			return false;
		}
		return false;
	}

	private void DrawName(Rect position)
	{
		if (_color.a > 0f)
		{
			GUI.color = new Color(0f, 0f, 0f, _color.a);
			GUI.Label(new Rect(position.x + 1f, position.y + 1f, position.width, position.height), _text, _nameTextStyle);
			GUI.color = _color;
			GUI.Label(position, _text, _nameTextStyle);
			GUI.color = Color.white;
		}
	}

	private void DrawBar(Rect position, int barWidth)
	{
		if (_color.a > 0f)
		{
			GUI.color = new Color(1f, 1f, 1f, _color.a);
			GUI.DrawTexture(position, UberstrikeIconsHelper.White);
			GUI.color = Color.green.SetAlpha(_color.a);
			GUI.DrawTexture(new Rect(position.xMin + 1f, position.yMin + 1f, (position.width - 2f) * _barValue, position.height - 2f), UberstrikeIconsHelper.White);
			GUI.color = Color.white;
		}
	}
}
