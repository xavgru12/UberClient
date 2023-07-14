// Decompiled with JetBrains decompiler
// Type: EventStreamHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class EventStreamHud : Singleton<EventStreamHud>
{
  private Animatable2DGroup _textGroup;
  private Animatable2DGroup _entireGroup;
  private MeshGUIQuad _glowBlur;
  private float _textScale;
  private float _vertGapBetweenText;
  private float _horzGapBetweenText;
  private float _maxDisplayTime;
  private int _maxEventNum;
  private float _nextRemoveEventTime;

  private EventStreamHud()
  {
    this._maxEventNum = 8;
    this._maxDisplayTime = 5f;
    this._textScale = 0.2f;
    this._vertGapBetweenText = 5f;
    this._horzGapBetweenText = 5f;
    this._glowBlur = new MeshGUIQuad((Texture) HudTextures.WhiteBlur128);
    this._glowBlur.Color = HudStyleUtility.GLOW_BLUR_BLUE_COLOR;
    this._glowBlur.Name = "EventStreamHudGlow";
    this._glowBlur.Depth = 1f;
    this._textGroup = new Animatable2DGroup();
    this._entireGroup = new Animatable2DGroup();
    this._entireGroup.Group.Add((IAnimatable2D) this._textGroup);
    this._entireGroup.Group.Add((IAnimatable2D) this._glowBlur);
    this.Enabled = false;
    CmuneEventHandler.AddListener<OnSetPlayerTeamEvent>(new Action<OnSetPlayerTeamEvent>(this.OnTeamChange));
    CmuneEventHandler.AddListener<ScreenResolutionEvent>(new Action<ScreenResolutionEvent>(this.OnScreenResolutionChange));
    CmuneEventHandler.AddListener<OnGlobalUIRibbonChanged>(new Action<OnGlobalUIRibbonChanged>(this.OnGlobalUIRibbonChange));
  }

  public bool Enabled
  {
    get => this._entireGroup.IsVisible;
    set
    {
      if (value)
        this._entireGroup.Show();
      else
        this._entireGroup.Hide();
    }
  }

  public void Draw() => this._entireGroup.Draw(0.0f, 0.0f);

  public void Update()
  {
    if (this._textGroup.Group.Count <= 0 || (double) Time.time <= (double) this._nextRemoveEventTime)
      return;
    this.DequeueEvent();
    this.ResetTransform();
  }

  public void AddEventText(
    string subjective,
    TeamID subTeamId,
    string verb,
    string objective = "",
    TeamID objTeamId = TeamID.NONE)
  {
    if (this._textGroup.Group.Count == 0)
      this.UpdateNextRemoveEventTime();
    if (this.ReachMaxEventNum())
      this.DequeueEvent();
    this._textGroup.Group.Add((IAnimatable2D) this.GenerateEventText(subjective, subTeamId, verb, objective, objTeamId));
    this.ResetTransform();
  }

  public void ClearAllEvents()
  {
    this._textGroup.ClearAndFree();
    this.ResetTransform();
  }

  private void DequeueEvent()
  {
    this._textGroup.RemoveAndFree(0);
    this.UpdateNextRemoveEventTime();
  }

  private bool ReachMaxEventNum() => this._textGroup.Group.Count == this._maxEventNum && this._maxEventNum > 0;

  private void UpdateNextRemoveEventTime() => this._nextRemoveEventTime = Time.time + this._maxDisplayTime;

  private Animatable2DGroup GenerateEventText(
    string subjective,
    TeamID subTeamId,
    string verb,
    string objective = "",
    TeamID objTeamId = TeamID.NONE)
  {
    MeshGUIText text1 = new MeshGUIText(subjective, HudAssets.Instance.InterparkBitmapFont, TextAnchor.UpperRight);
    text1.NamePrefix = nameof (EventStreamHud);
    MeshGUIText text2 = new MeshGUIText(verb, HudAssets.Instance.InterparkBitmapFont, TextAnchor.UpperRight);
    text2.NamePrefix = nameof (EventStreamHud);
    MeshGUIText text3 = new MeshGUIText(objective, HudAssets.Instance.InterparkBitmapFont, TextAnchor.UpperRight);
    text3.NamePrefix = nameof (EventStreamHud);
    this.SetTextStyleByTeamId(text1, subTeamId);
    this.SetTextStyleByTeamId(text2, TeamID.NONE);
    this.SetTextStyleByTeamId(text3, objTeamId);
    float num = 0.0f;
    text3.Position = Vector2.zero;
    text3.Scale = Vector2.one * this._textScale;
    float x1 = num - (text3.Rect.width + this._horzGapBetweenText);
    text2.Position = new Vector2(x1, 0.0f);
    text2.Scale = Vector2.one * this._textScale;
    float x2 = x1 - (text2.Rect.width + this._horzGapBetweenText);
    text1.Position = new Vector2(x2, 0.0f);
    text1.Scale = Vector2.one * this._textScale;
    return new Animatable2DGroup()
    {
      Group = {
        (IAnimatable2D) text1,
        (IAnimatable2D) text2,
        (IAnimatable2D) text3
      }
    };
  }

  private void SetTextStyleByTeamId(MeshGUIText text, TeamID teamId)
  {
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(text);
    switch (teamId)
    {
      case TeamID.BLUE:
        text.BitmapMeshText.ShadowColor = HudStyleUtility.DEFAULT_BLUE_COLOR;
        break;
      case TeamID.RED:
        text.BitmapMeshText.ShadowColor = HudStyleUtility.DEFAULT_RED_COLOR;
        break;
    }
  }

  private void OnTeamChange(OnSetPlayerTeamEvent ev)
  {
    if (ev.TeamId == TeamID.RED)
      this._glowBlur.Color = HudStyleUtility.GLOW_BLUR_RED_COLOR;
    else
      this._glowBlur.Color = HudStyleUtility.GLOW_BLUR_BLUE_COLOR;
  }

  private void OnScreenResolutionChange(ScreenResolutionEvent ev) => this.ResetTransform();

  private void OnGlobalUIRibbonChange(OnGlobalUIRibbonChanged ev) => this.ResetTransform();

  private void ResetTransform()
  {
    this.ResetTextGroupTransform();
    this.ResetBlurTransform();
    float y = (float) Screen.height * 0.02f;
    if (GlobalUIRibbon.Instance.IsEnabled)
      y += 44f;
    this._entireGroup.Position = new Vector2((float) Screen.width * 0.95f, y);
  }

  private void ResetTextGroupTransform()
  {
    for (int index = 0; index < this._textGroup.Group.Count; ++index)
    {
      Animatable2DGroup eventTextGroup = this._textGroup.Group[index] as Animatable2DGroup;
      this.ResetEventTextTransform(eventTextGroup);
      eventTextGroup.Position = new Vector2(0.0f, (float) index * (eventTextGroup.Rect.height + this._vertGapBetweenText));
      eventTextGroup.UpdateMeshGUIPosition();
    }
  }

  private void ResetEventTextTransform(Animatable2DGroup eventTextGroup)
  {
    MeshGUIText meshGuiText1 = eventTextGroup.Group[0] as MeshGUIText;
    MeshGUIText meshGuiText2 = eventTextGroup.Group[1] as MeshGUIText;
    MeshGUIText meshGuiText3 = eventTextGroup.Group[2] as MeshGUIText;
    float num = 0.0f;
    meshGuiText3.Position = Vector2.zero;
    meshGuiText3.Scale = Vector2.one * this._textScale;
    float x1 = num - (meshGuiText3.Rect.width + this._horzGapBetweenText);
    meshGuiText2.Position = new Vector2(x1, 0.0f);
    meshGuiText2.Scale = Vector2.one * this._textScale;
    meshGuiText2.BitmapMeshText.ShadowColor = new Color(1f, 1f, 1f, 0.0f);
    float x2 = x1 - (meshGuiText2.Rect.width + this._horzGapBetweenText);
    meshGuiText1.Position = new Vector2(x2, 0.0f);
    meshGuiText1.Scale = Vector2.one * this._textScale;
  }

  private void ResetBlurTransform()
  {
    float num1 = (float) ((double) this._textGroup.Rect.width * (double) HudStyleUtility.BLUR_WIDTH_SCALE_FACTOR * 0.800000011920929);
    float num2 = this._textGroup.Rect.height * HudStyleUtility.BLUR_HEIGHT_SCALE_FACTOR;
    this._glowBlur.Scale = new Vector2(num1 / (float) HudTextures.WhiteBlur128.width, num2 / (float) HudTextures.WhiteBlur128.height);
    this._glowBlur.Position = new Vector2((float) ((-(double) num1 - (double) this._textGroup.Rect.width) / 2.0), (float) (((double) this._textGroup.Rect.height - (double) num2) / 2.0));
  }
}
