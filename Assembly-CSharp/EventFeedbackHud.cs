// Decompiled with JetBrains decompiler
// Type: EventFeedbackHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class EventFeedbackHud : Singleton<EventFeedbackHud>
{
  private float _textScale;
  private Queue<EventFeedbackHud.FeedbackMessage> _eventFeedbackQueue;
  private EventFeedbackHud.FeedbackMessage _curShownMessage;
  private MeshGUITextFormat _feedbackText;
  private float _defaultTextDisplayTime;
  private float _animFadeOutTime;
  private float _lastEventShownTime;
  private bool _isFadingOutText;
  private float _curCameraPixelWidth;

  private EventFeedbackHud()
  {
    this._animFadeOutTime = 1f;
    this._defaultTextDisplayTime = 2f;
    this._eventFeedbackQueue = new Queue<EventFeedbackHud.FeedbackMessage>();
    this.CurrentFeedbackMessage = (EventFeedbackHud.FeedbackMessage) null;
    this._feedbackText = new MeshGUITextFormat(string.Empty, HudAssets.Instance.InterparkBitmapFont, TextAlignment.Center);
    this.ResetHud();
    this.Enabled = true;
    this._curCameraPixelWidth = 1f;
    CmuneEventHandler.AddListener<CameraWidthChangeEvent>(new Action<CameraWidthChangeEvent>(this.OnCameraRectChange));
  }

  public bool Enabled
  {
    get => this._feedbackText.IsVisible;
    set
    {
      if (value)
        this._feedbackText.Show();
      else
        this._feedbackText.Hide();
    }
  }

  public void Draw()
  {
    if (!this.Enabled)
      return;
    this.ScheduleEventFeedback();
    this.DrawFeedbackMessage();
  }

  public void EnqueueFeedback(InGameEventFeedbackType eventFeedbackType, string customMessage) => this.EnqueueFeedback(eventFeedbackType, customMessage, this._defaultTextDisplayTime);

  public void EnqueueFeedback(
    InGameEventFeedbackType eventFeedbackType,
    string customMessage,
    float time)
  {
    this._eventFeedbackQueue.Enqueue(new EventFeedbackHud.FeedbackMessage(customMessage, eventFeedbackType, time));
  }

  public void ClearQueue() => this._eventFeedbackQueue.Clear();

  public void ClearAll()
  {
    this._eventFeedbackQueue.Clear();
    this._feedbackText.FormatText = string.Empty;
  }

  private EventFeedbackHud.FeedbackMessage CurrentFeedbackMessage
  {
    get => this._curShownMessage;
    set
    {
      this._curShownMessage = value;
      if (this._curShownMessage == null)
        return;
      this._feedbackText.FormatText = this._curShownMessage.Message;
      this.ResetHud();
    }
  }

  private void OnCameraRectChange(CameraWidthChangeEvent ev)
  {
    this._curCameraPixelWidth = ev.Width;
    this.ResetTransform();
  }

  private void ScheduleEventFeedback()
  {
    if (this._eventFeedbackQueue.Count <= 0 || this.CurrentFeedbackMessage != null)
      return;
    this.CurrentFeedbackMessage = this._eventFeedbackQueue.Dequeue();
    this._lastEventShownTime = Time.time;
    this._isFadingOutText = false;
  }

  private void DrawFeedbackMessage()
  {
    if (this.CurrentFeedbackMessage != null)
    {
      if ((double) Time.time < (double) this._lastEventShownTime + (double) this.CurrentFeedbackMessage.Time)
      {
        if ((double) Time.time > (double) this._lastEventShownTime + (double) this.CurrentFeedbackMessage.Time - (double) this._animFadeOutTime && !this._isFadingOutText)
          this.FadeOutFeedbackText();
      }
      else
        this.CurrentFeedbackMessage = (EventFeedbackHud.FeedbackMessage) null;
    }
    this._feedbackText.Draw(0.0f, 0.0f);
    this._feedbackText.ShadowColorAnim.Alpha = 0.0f;
  }

  private void FadeOutFeedbackText()
  {
    this._feedbackText.FadeAlphaTo(0.0f, this._animFadeOutTime, EaseType.Out);
    this._isFadingOutText = true;
  }

  private void ResetHud()
  {
    this.ResetStyle();
    this.ResetTransform();
  }

  private void ResetStyle()
  {
    foreach (IAnimatable2D meshText3D in this._feedbackText.Group)
      Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(meshText3D as MeshGUIText);
  }

  private void ResetTransform()
  {
    this._textScale = 0.45f;
    this._feedbackText.Scale = new Vector2(this._textScale, this._textScale);
    this._feedbackText.LineGap = this._feedbackText.LineHeight * this._textScale * this._textScale;
    this._feedbackText.Position = new Vector2((float) (Screen.width / 2), (float) ((double) Screen.height * 0.30000001192092896 + (double) Screen.height * 0.64999997615814209 * (1.0 - (double) this._curCameraPixelWidth)));
  }

  private class FeedbackMessage
  {
    public FeedbackMessage(string msg, InGameEventFeedbackType type, float time)
    {
      this.Message = msg;
      this.Type = type;
      this.Time = time;
    }

    public InGameEventFeedbackType Type { get; private set; }

    public string Message { get; private set; }

    public float Time { get; private set; }
  }
}
