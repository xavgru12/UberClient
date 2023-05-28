// Decompiled with JetBrains decompiler
// Type: GUITools
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class GUITools
{
  private static int _screenX;
  private static int _screenY;
  private static int _screenHalfX;
  private static int _screenHalfY;
  private static float _aspectRatio;
  private static float _lastClick;
  private static float _lastRepeatClick;
  private static float _repeatButtonPressed;
  private static Stack<bool> _stateStack;
  private static Color _lastGuiColor;
  private static int HoverButtonHash = "Button".GetHashCode();

  static GUITools()
  {
    GUITools._screenX = 1;
    GUITools._screenY = 1;
    GUITools._screenHalfX = 1;
    GUITools._screenHalfY = 1;
    GUITools._aspectRatio = 1f;
    GUITools._lastClick = 0.0f;
    GUITools._lastRepeatClick = 0.0f;
    GUITools._repeatButtonPressed = 0.0f;
    GUITools._stateStack = new Stack<bool>();
  }

  public static float AspectRatio => GUITools._aspectRatio;

  public static float SinusPulse => (float) (((double) Mathf.Sin(Time.time * 2f) + 1.2999999523162842) * 0.5);

  public static float FastSinusPulse => (float) (((double) Mathf.Sin(Time.time * 5f) + 1.2999999523162842) * 0.5);

  [DebuggerHidden]
  public static IEnumerator StartScreenSizeListener(float s) => (IEnumerator) new GUITools.\u003CStartScreenSizeListener\u003Ec__IteratorE()
  {
    s = s,
    \u003C\u0024\u003Es = s
  };

  public static void UpdateScreenSize()
  {
    if (GUITools.ScreenWidth != Screen.width)
    {
      GUITools.ScreenWidth = Screen.width;
      GUITools._aspectRatio = (float) (GUITools.ScreenWidth / GUITools.ScreenHeight);
    }
    if (GUITools.ScreenHeight == Screen.height)
      return;
    GUITools.ScreenHeight = Screen.height;
    GUITools._aspectRatio = (float) (GUITools.ScreenWidth / GUITools.ScreenHeight);
  }

  public static int ScreenHalfWidth => GUITools._screenHalfX;

  public static int ScreenHalfHeight => GUITools._screenHalfY;

  public static int ScreenWidth
  {
    get => GUITools._screenX;
    private set
    {
      GUITools._screenX = Mathf.Max(value, 1);
      GUITools._screenHalfX = GUITools._screenX >> 1;
    }
  }

  public static int ScreenHeight
  {
    get => GUITools._screenY;
    private set
    {
      GUITools._screenY = Mathf.Max(value, 1);
      GUITools._screenHalfY = GUITools._screenY >> 1;
    }
  }

  public static bool IsScreenResolutionChanged() => Screen.width != Screen.width || Screen.height != Screen.height;

  public static bool RepeatClick(float vel)
  {
    if ((double) Mathf.Abs(Time.time - GUITools._lastRepeatClick - Time.deltaTime) < 9.9999997473787516E-05)
      GUITools._repeatButtonPressed += Time.deltaTime;
    else
      GUITools._repeatButtonPressed = 0.0f;
    GUITools._lastRepeatClick = Time.time;
    if ((double) GUITools._repeatButtonPressed == 0.0)
      return true;
    return (double) GUITools._repeatButtonPressed > 0.5 && GUITools.SaveClickIn(vel);
  }

  public static float LastClick => GUITools._lastClick;

  public static bool SaveClick => (double) GUITools._lastClick + 0.5 < (double) Time.time;

  public static bool SaveClickIn(float t) => (double) GUITools._lastClick + (double) t < (double) Time.time;

  public static void ClickAndUse()
  {
    if (Event.current != null)
      Event.current.Use();
    GUITools._lastClick = Time.time;
  }

  public static void Clicked() => GUITools._lastClick = Time.time;

  public static int CheckGUIState() => GUITools._stateStack.Count;

  public static void PushGUIState()
  {
    if (GUITools._stateStack.Count < 100)
      GUITools._stateStack.Push(GUI.enabled);
    else
      UnityEngine.Debug.LogError((object) "Check your calls of PushGUIState");
  }

  public static void PopGUIState() => GUI.enabled = GUITools._stateStack.Pop();

  public static void BeginGUIColor(Color color)
  {
    GUITools._lastGuiColor = GUI.color;
    GUI.color = color;
  }

  public static void EndGUIColor() => GUI.color = GUITools._lastGuiColor;

  public static void LabelShadow(Rect rect, string text, GUIStyle style, Color color)
  {
    GUI.color = new Color(0.0f, 0.0f, 0.0f, color.a * 0.5f);
    GUI.Label(new Rect(rect.x + 1f, rect.y + 1f, rect.width, rect.height), text, style);
    GUI.color = color;
    GUI.Label(rect, text, style);
    GUI.color = Color.white;
  }

  public static bool Button(Rect rect, GUIContent content, GUIStyle style) => GUITools.Button(rect, content, style, GameAudio.ButtonClick);

  public static bool Button(Rect rect, GUIContent content, GUIStyle style, AudioClip soundEffect)
  {
    if (!GUI.Button(rect, content, style) || !((Object) AutoMonoBehaviour<SfxManager>.Instance != (Object) null))
      return false;
    SfxManager.Play2dAudioClip(soundEffect);
    return true;
  }

  public static bool IsScrolling { get; private set; }

  public static Rect ToGlobal(Rect rect)
  {
    Vector2 screenPoint = GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y));
    return new Rect(screenPoint.x, screenPoint.y, rect.width, rect.height);
  }

  public static bool Label(Rect rect, Texture2D image, GUIStyle style)
  {
    GUI.Label(rect, (Texture) image, style);
    return rect.Contains(Event.current.mousePosition);
  }

  public static Vector2 BeginScrollView(
    Rect position,
    Vector2 scrollPosition,
    Rect contentRect,
    bool showHorizontalScrollbar,
    bool showVerticalScrollbar,
    GUIStyle hStyle,
    GUIStyle vStyle,
    bool allowDrag = true)
  {
    return GUI.BeginScrollView(position, scrollPosition, contentRect);
  }

  public static Vector2 BeginScrollView(
    Rect position,
    Vector2 scrollPosition,
    Rect contentRect,
    bool useHorizontal = false,
    bool useVertical = false,
    bool allowDrag = true)
  {
    return GUITools.BeginScrollView(position, scrollPosition, contentRect, useHorizontal, useVertical, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, allowDrag);
  }

  public static Vector2 BeginScrollView(
    Rect position,
    Vector2 scrollPosition,
    Rect contentRect,
    GUIStyle hStyle,
    GUIStyle vStyle)
  {
    return GUITools.BeginScrollView(position, scrollPosition, contentRect, false, false, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar);
  }

  public static void EndScrollView() => GUI.EndScrollView();

  public static bool BeginList(ref bool showList, Rect listRect, Rect buttonRect)
  {
    if (!showList)
      return false;
    if (Input.GetMouseButtonUp(0) && !listRect.ContainsTouch((Vector2) Input.mousePosition) && !buttonRect.ContainsTouch((Vector2) Input.mousePosition))
      showList = false;
    if (Input.touchCount > 0)
    {
      foreach (Touch touch in Input.touches)
      {
        if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && !listRect.ContainsTouch(touch.position) && !buttonRect.ContainsTouch(touch.position))
          showList = false;
      }
    }
    return showList;
  }

  public static UnityEngine.EventType HoverButton(
    Rect position,
    GUIContent content,
    GUIStyle style)
  {
    int controlId = GUIUtility.GetControlID(GUITools.HoverButtonHash, FocusType.Native);
    switch (Event.current.GetTypeForControl(controlId))
    {
      case UnityEngine.EventType.MouseDown:
        if (position.Contains(Event.current.mousePosition))
        {
          GUIUtility.hotControl = controlId;
          Event.current.Use();
          return UnityEngine.EventType.MouseDown;
        }
        break;
      case UnityEngine.EventType.MouseUp:
        if (GUIUtility.hotControl == controlId)
        {
          GUIUtility.hotControl = 0;
          if (position.Contains(Event.current.mousePosition))
          {
            Event.current.Use();
            return UnityEngine.EventType.MouseUp;
          }
        }
        else if (position.Contains(Event.current.mousePosition))
          return UnityEngine.EventType.DragExited;
        return UnityEngine.EventType.Ignore;
      case UnityEngine.EventType.MouseDrag:
        if (GUIUtility.hotControl != controlId)
          return UnityEngine.EventType.Ignore;
        Event.current.Use();
        return UnityEngine.EventType.MouseDrag;
      case UnityEngine.EventType.Repaint:
        style.Draw(position, content, controlId);
        return position.Contains(Event.current.mousePosition) ? UnityEngine.EventType.MouseMove : UnityEngine.EventType.Repaint;
    }
    return position.Contains(Event.current.mousePosition) ? UnityEngine.EventType.MouseMove : UnityEngine.EventType.Ignore;
  }

  public static string PasswordField(Rect mPosition, string mPassword)
  {
    string text;
    if (Event.current.type == UnityEngine.EventType.Repaint || Event.current.type == UnityEngine.EventType.MouseDown)
    {
      text = string.Empty;
      for (int index = 0; index < mPassword.Length; ++index)
        text += "*";
    }
    else
      text = mPassword;
    GUI.changed = false;
    string str = GUI.TextField(mPosition, text, 20);
    if (GUI.changed)
      mPassword = str;
    return mPassword;
  }

  public static string PasswordField(string mPassword)
  {
    string text;
    if (Event.current.type == UnityEngine.EventType.Repaint || Event.current.type == UnityEngine.EventType.MouseDown)
    {
      text = string.Empty;
      for (int index = 0; index < mPassword.Length; ++index)
        text += "*";
    }
    else
      text = mPassword;
    GUI.changed = false;
    string str = GUILayout.TextField(text, 24, GUILayout.Height(30f));
    if (GUI.changed)
      mPassword = str;
    return mPassword;
  }

  public static Vector2 DoScrollArea(
    Rect position,
    GUIContent[] buttons,
    int buttonHeight,
    Vector2 listScroller)
  {
    float num = 0.0f;
    if (buttons.Length > 0)
      num = (float) ((buttons.Length - 1) * buttonHeight);
    listScroller = GUITools.BeginScrollView(position, listScroller, new Rect(0.0f, 0.0f, position.width - 20f, num + (float) buttonHeight));
    int index = 0;
    while (index < buttons.Length && (double) ((index + 1) * buttonHeight) <= (double) listScroller.y)
      ++index;
    for (; index < buttons.Length && (double) (index * buttonHeight) < (double) listScroller.y + (double) position.height; ++index)
      GUI.Button(new Rect(0.0f, (float) (index * buttonHeight), position.width - 16f, (float) buttonHeight), buttons[index]);
    GUITools.EndScrollView();
    return listScroller;
  }

  public static void OutlineLabel(Rect position, string text) => GUITools.OutlineLabel(position, text, (GUIStyle) "SuperBigTitle", 1);

  public static void OutlineLabel(Rect position, string text, GUIStyle style) => GUITools.OutlineLabel(position, text, style, 1);

  public static void OutlineLabel(Rect position, string text, GUIStyle style, Color c) => GUITools.OutlineLabel(position, text, style, 1, Color.white, c);

  public static void OutlineLabel(Rect position, string text, GUIStyle style, int Offset) => GUITools.OutlineLabel(position, text, style, Offset, Color.white, Color.black);

  public static void OutlineLabel(
    Rect position,
    string text,
    GUIStyle style,
    int Offset,
    Color textColor,
    Color outlineColor)
  {
    Color textColor1 = style.normal.textColor;
    style.normal.textColor = outlineColor;
    if (Offset > 0)
    {
      GUI.Label(new Rect(position.x, position.y + (float) Offset, position.width, position.height), text, style);
      GUI.Label(new Rect(position.x - (float) Offset, position.y, position.width, position.height), text, style);
      GUI.Label(new Rect(position.x + (float) Offset, position.y, position.width, position.height), text, style);
      GUI.Label(new Rect(position.x, position.y - (float) Offset, position.width, position.height), text, style);
      if (Offset > 1)
      {
        GUI.Label(new Rect(position.x - (float) Offset, position.y - (float) Offset, position.width, position.height), text, style);
        GUI.Label(new Rect(position.x - (float) Offset, position.y + (float) Offset, position.width, position.height), text, style);
        GUI.Label(new Rect(position.x + (float) Offset, position.y + (float) Offset, position.width, position.height), text, style);
        GUI.Label(new Rect(position.x + (float) Offset, position.y - (float) Offset, position.width, position.height), text, style);
      }
    }
    else
      GUI.Label(new Rect(position.x, position.y + 1f, position.width, position.height), text, style);
    style.normal.textColor = textColor1;
    GUI.color = textColor;
    GUI.Label(position, text, style);
    GUI.color = Color.white;
  }

  public static void ProgressBar(
    Rect position,
    string text,
    float percentage,
    Color barColor,
    int barWidth,
    string value)
  {
    GUI.BeginGroup(position);
    GUI.Label(new Rect(0.0f, 0.0f, (float) ((double) position.width - (double) (barWidth + 4) - 2.0 - 30.0), 14f), text, BlueStonez.label_interparkbold_11pt_right);
    GUI.Label(new Rect((float) ((double) position.width - (double) barWidth - 30.0), 1f, (float) barWidth, 12f), GUIContent.none, BlueStonez.progressbar_background);
    GUI.color = barColor;
    GUI.Label(new Rect((float) ((double) position.width - (double) barWidth - 30.0 + 2.0), 3f, (float) (barWidth - 4) * Mathf.Clamp01(percentage), 8f), string.Empty, BlueStonez.progressbar_thumb);
    GUI.color = Color.white;
    if (!string.IsNullOrEmpty(value))
      GUI.Label(new Rect(position.width - 25f, 0.0f, 30f, 14f), value, BlueStonez.label_interparkmed_10pt_left);
    GUI.EndGroup();
  }

  public static void DrawWarmupBar(Rect position, float value, float maxValue)
  {
    GUI.BeginGroup(position);
    GUI.Box(new Rect(0.0f, 0.0f, position.width, position.height), GUIContent.none, StormFront.ProgressBackground);
    float width1 = (position.width - 8f) * value / maxValue;
    GUI.Box(new Rect(4f, 4f, width1, position.height - 8f), GUIContent.none, StormFront.ProgressForeground);
    float width2 = position.height * 0.5f;
    GUI.Box(new Rect((float) (4.0 + (double) width1 - (double) width2 * 0.5), 2f, width2, position.height - 4f), GUIContent.none, StormFront.ProgressThumb);
    GUI.EndGroup();
  }
}
