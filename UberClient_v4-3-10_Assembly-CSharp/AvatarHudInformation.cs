// Decompiled with JetBrains decompiler
// Type: AvatarHudInformation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class AvatarHudInformation : MonoBehaviour
{
  private const int ImageSize = 64;
  private const int PixelOffset = 1;
  [SerializeField]
  private AvatarHudInformation.Mode _mode;
  [SerializeField]
  private bool _isTeamDebug = true;
  [SerializeField]
  private string _text = "text";
  [SerializeField]
  public Vector2 _barOffset = new Vector2(0.0f, 0.0f);
  [SerializeField]
  private Color _color = Color.white;
  [SerializeField]
  private Vector3 _offset = new Vector3(0.0f, 2f, 0.0f);
  [SerializeField]
  private Texture _healthBarImage;
  private float _distanceCapMax = 50f;
  private float _distanceCapMin = 3f;
  [SerializeField]
  private float _timeCap;
  private float _maxTime;
  private InGameEventFeedbackType _currentFeedback;
  private Transform _target;
  private Transform _transform;
  private Vector3 _screenPosition;
  [SerializeField]
  private float _barValue;
  private Vector2 _textSize;
  private bool _isVisible = true;
  private bool _showBar = true;
  public float _feedbackTimer;
  private UberStrike.Realtime.UnitySdk.CharacterInfo _info;
  private GUIStyle _nameTextStyle;
  private bool _isInViewport;
  private bool _needFadeIn;
  private float _fadeInAlpha;
  [SerializeField]
  private bool _forceShowInformation;

  private void Start()
  {
    this._transform = this.transform;
    if (this._mode == AvatarHudInformation.Mode.Robot)
      this.SetAvatarLabel(this._text);
    this._nameTextStyle = BlueStonez.label_interparkbold_11pt;
  }

  private void OnGUI()
  {
    if ((UnityEngine.Object) BlueStonez.Skin == (UnityEngine.Object) null)
      return;
    bool flag = false;
    GUI.depth = 100;
    Rect position1 = new Rect(this._screenPosition.x - 50f, (float) Screen.height - this._screenPosition.y + this._barOffset.y, 100f, 6f);
    Rect position2 = new Rect(position1.xMin, (float) ((double) Screen.height - (double) this._screenPosition.y - (double) this._textSize.y - 6.0), this._textSize.x, this._textSize.y);
    Rect position3 = new Rect(this._screenPosition.x - this._textSize.x * 0.5f, (float) ((double) Screen.height - (double) this._screenPosition.y - (double) this._textSize.y - 6.0), this._textSize.x, this._textSize.y);
    if (!this._isInViewport && !flag)
      return;
    if (Application.isEditor && this._isTeamDebug && this._info != null)
      GUI.Label(new Rect(this._screenPosition.x, (float) Screen.height - this._screenPosition.y, 50f, 20f), ((Enum) this._info.TeamID).ToString());
    if (this.IsEnemy && GameState.HasCurrentGame)
    {
      if (!this._isVisible && !this._forceShowInformation && !flag)
        return;
      this.DrawName(position3);
      if (!flag)
        return;
      this.DrawBar(position1, 100);
    }
    else if ((this.IsFriend || this._mode == AvatarHudInformation.Mode.Robot && (double) this._barValue > 0.0) && GameState.HasCurrentGame || this._forceShowInformation)
    {
      this.DrawBar(position1, 100);
      this.DrawName(position2);
    }
    else
    {
      if (GameState.HasCurrentGame || this._mode == AvatarHudInformation.Mode.Robot)
        return;
      this.DrawName(position3);
    }
  }

  private void LateUpdate()
  {
    if ((bool) (UnityEngine.Object) Camera.main && (bool) (UnityEngine.Object) this._target)
    {
      Vector3 rhs = this._target.position + this._offset - Camera.main.transform.position;
      this._screenPosition = Camera.main.WorldToScreenPoint(this._target.position + this._offset);
      this._screenPosition.x = (float) Mathf.FloorToInt(this._screenPosition.x);
      bool isInViewport = this._isInViewport;
      this._isInViewport = (double) Vector3.Dot(Camera.main.transform.forward, rhs) > 0.0 && (double) this._screenPosition.x >= 0.0 && (double) this._screenPosition.x <= (double) Screen.width && (double) this._screenPosition.y >= 0.0 && (double) this._screenPosition.y <= (double) Screen.height;
      if (this._isInViewport && !isInViewport)
        this._needFadeIn = true;
      if (!this._isInViewport && (double) this._timeCap > 0.0)
      {
        this._timeCap = 0.0f;
        this.Visibility = 0.0f;
        this._isVisible = false;
        this._currentFeedback = InGameEventFeedbackType.None;
      }
      if (GameState.HasCurrentGame || this._forceShowInformation)
      {
        if (this._mode == AvatarHudInformation.Mode.Robot)
          this.Visibility = !this._isInViewport ? 0.0f : (float) (1.0 - ((double) this._screenPosition.z - (double) this._distanceCapMin) / ((double) this._distanceCapMax - (double) this._distanceCapMin) * (double) Camera.main.fieldOfView / 60.0);
        else if (this.IsEnemy && this._isVisible)
        {
          if ((double) this._timeCap > 0.0)
          {
            this._timeCap -= Time.deltaTime;
            this.Visibility = Mathf.Clamp01(this._timeCap / this._maxTime);
          }
          else
          {
            this.Visibility = 0.0f;
            this._isVisible = false;
            this._currentFeedback = InGameEventFeedbackType.None;
          }
        }
        else if (this.IsFriend)
        {
          if (this._isInViewport)
          {
            if ((double) this._timeCap > 0.0)
            {
              this._timeCap -= Time.deltaTime;
              float num1 = Mathf.Clamp01(this._timeCap / this._maxTime);
              float num2 = (float) (1.0 - ((double) this._screenPosition.z - (double) this._distanceCapMin) / ((double) this._distanceCapMax - (double) this._distanceCapMin) * (double) Camera.main.fieldOfView / 60.0);
              this.Visibility = (double) num1 <= (double) num2 ? num2 : num1;
            }
            else
              this.Visibility = (float) (1.0 - ((double) this._screenPosition.z - (double) this._distanceCapMin) / ((double) this._distanceCapMax - (double) this._distanceCapMin) * (double) Camera.main.fieldOfView / 60.0);
          }
          else
            this.Visibility = 0.0f;
        }
        if ((double) this._barValue == 0.0)
          this.Visibility = 0.0f;
      }
      else
        this.Visibility = 1f;
      if (this._isInViewport)
        this.FadeInAlphaCorrection();
      this._transform.position = this._screenPosition;
    }
    if ((double) this._feedbackTimer < 0.0)
      return;
    this._feedbackTimer -= Time.deltaTime;
  }

  public float Visibility
  {
    get => this._color.a;
    set => this._color.a = value;
  }

  public void Show(int seconds)
  {
    this._needFadeIn = true;
    this._fadeInAlpha = Mathf.Clamp01(this.Visibility);
    this._isVisible = true;
    this._timeCap = (float) seconds + 0.3f;
    this._maxTime = (float) seconds;
  }

  public void Hide()
  {
    this._isVisible = false;
    this._timeCap = 0.0f;
    this._feedbackTimer = 0.0f;
    this._currentFeedback = InGameEventFeedbackType.None;
  }

  private void DrawName(Rect position)
  {
    if (this._forceShowInformation)
      this.Visibility = 1f;
    if ((double) this.Visibility <= 0.0)
      return;
    GUI.color = new Color(0.0f, 0.0f, 0.0f, this.Visibility);
    GUI.Label(new Rect(position.x + 1f, position.y + 1f, position.width, position.height), this._text, this._nameTextStyle);
    GUI.color = this._color;
    GUI.Label(position, this._text, this._nameTextStyle);
    GUI.color = Color.white;
  }

  private void DrawBar(Rect position, int barWidth)
  {
    if ((double) this.Visibility <= 0.0)
      return;
    GUI.color = new Color(1f, 1f, 1f, this.Visibility);
    GUI.DrawTexture(position, this._healthBarImage);
    Color rgb = ColorConverter.HsvToRgb(this._barValue / 3f, 1f, 0.9f);
    GUI.color = new Color(rgb.r, rgb.g, rgb.b, this.Visibility);
    GUI.DrawTexture(new Rect(position.xMin + 1f, position.yMin + 1f, (position.width - 2f) * this._barValue, position.height - 2f), this._healthBarImage);
    GUI.color = Color.white;
  }

  private void DrawInGameEvent(Rect position, Texture image)
  {
    if ((UnityEngine.Object) image == (UnityEngine.Object) null)
      return;
    GUI.DrawTexture(position, image);
  }

  public void SetAvatarLabel(string name)
  {
    this._text = name;
    if (BlueStonez.label_interparkbold_11pt != null)
      this._textSize = BlueStonez.label_interparkbold_11pt.CalcSize(new GUIContent(this._text));
    else
      this._textSize = new Vector2((float) (name.Length * 10), 20f);
  }

  public void SetHealthBarValue(float value) => this._barValue = Mathf.Clamp01(value);

  public void SetInGameFeedback(InGameEventFeedbackType feedbackType)
  {
    this._currentFeedback = feedbackType;
    this._feedbackTimer = 3f;
  }

  public void SetCharacterInfo(UberStrike.Realtime.UnitySdk.CharacterInfo info)
  {
    if (info == null)
      return;
    this.SetAvatarLabel(info.PlayerName);
    this._info = info;
  }

  private void FadeInAlphaCorrection()
  {
    if (!this._needFadeIn)
      return;
    this._fadeInAlpha += Time.deltaTime;
    if ((double) this._fadeInAlpha >= (double) this.Visibility)
    {
      this._needFadeIn = false;
      this._fadeInAlpha = 0.0f;
    }
    else
      this.Visibility = this._fadeInAlpha;
  }

  public UberStrike.Realtime.UnitySdk.CharacterInfo Info
  {
    get => this._info;
    set => this._info = value;
  }

  public bool IsEnemy
  {
    get
    {
      if (this._info == null || !GameState.HasCurrentPlayer)
        return false;
      return this._info.TeamID == TeamID.NONE || this._info.TeamID != GameState.LocalCharacter.TeamID;
    }
  }

  public bool IsFriend => !this.IsEnemy && !this.IsMe;

  public bool IsMe => !GameState.HasCurrentGame || !GameState.HasCurrentPlayer || this._info == null || GameState.CurrentPlayerID == this._info.ActorId;

  public bool IsBarVisible
  {
    get => this._showBar;
    set => this._showBar = value;
  }

  public Transform Target
  {
    get => this._target;
    set => this._target = value;
  }

  public float DistanceCap
  {
    get => this._distanceCapMax;
    set => this._distanceCapMax = value;
  }

  public InGameEventFeedbackType ActiveFeedbackType => this._currentFeedback;

  public bool ForceShowInformation
  {
    get => this._forceShowInformation;
    set => this._forceShowInformation = value;
  }

  public enum Mode
  {
    None,
    Robot,
  }
}
