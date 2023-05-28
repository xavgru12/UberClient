// Decompiled with JetBrains decompiler
// Type: UnityGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class UnityGUI
{
  public static int Toolbar(
    Rect position,
    int selected,
    GUIContent[] contents,
    int xCount,
    GUIStyle style)
  {
    int num1 = GUI.Toolbar(position, selected, contents, style);
    int controlId = GUIUtility.GetControlID(FocusType.Native, position);
    if (Event.current.GetTypeForControl(controlId) == UnityEngine.EventType.Repaint)
    {
      GUIStyle firstStyle = (GUIStyle) null;
      GUIStyle midStyle = (GUIStyle) null;
      GUIStyle lastStyle = (GUIStyle) null;
      UnityGUI.FindStyles(ref style, out firstStyle, out midStyle, out lastStyle, "left", "mid", "right");
      int length = contents.Length;
      int num2 = length / xCount;
      if (length % xCount != 0)
        ++num2;
      float num3 = (float) UnityGUI.CalcTotalHorizSpacing(xCount, style, firstStyle, midStyle, lastStyle);
      float num4 = (float) (Mathf.Max(style.margin.top, style.margin.bottom) * (num2 - 1));
      float elemWidth = (position.width - num3) / (float) xCount;
      float elemHeight = (position.height - num4) / (float) num2;
      int gridMouseSelection = UnityGUI.GetButtonGridMouseSelection(UnityGUI.CalcMouseRects(position, length, xCount, elemWidth, elemHeight, style, firstStyle, midStyle, lastStyle, false), Event.current.mousePosition, controlId == GUIUtility.hotControl);
      if (gridMouseSelection >= 0)
        GUI.tooltip = contents[gridMouseSelection].tooltip;
    }
    return num1;
  }

  internal static GUIContent[] Temp(string[] texts)
  {
    GUIContent[] guiContentArray = new GUIContent[texts.Length];
    for (int index = 0; index < texts.Length; ++index)
      guiContentArray[index] = new GUIContent(texts[index]);
    return guiContentArray;
  }

  public static int Toolbar(
    Rect position,
    int selected,
    string[] contents,
    int length,
    GUIStyle style)
  {
    return UnityGUI.Toolbar(position, selected, UnityGUI.Temp(contents), length, style);
  }

  public static int Toolbar(Rect position, int selected, GUIContent[] contents, GUIStyle style) => UnityGUI.Toolbar(position, selected, contents, contents.Length, style);

  internal static void FindStyles(
    ref GUIStyle style,
    out GUIStyle firstStyle,
    out GUIStyle midStyle,
    out GUIStyle lastStyle,
    string first,
    string mid,
    string last)
  {
    if (style == null)
      style = GUI.skin.button;
    string name = style.name;
    midStyle = GUI.skin.FindStyle(name + mid);
    if (midStyle == null)
      midStyle = style;
    firstStyle = GUI.skin.FindStyle(name + first);
    if (firstStyle == null)
      firstStyle = midStyle;
    lastStyle = GUI.skin.FindStyle(name + last);
    if (lastStyle != null)
      return;
    lastStyle = midStyle;
  }

  private static Rect[] CalcMouseRects(
    Rect position,
    int count,
    int xCount,
    float elemWidth,
    float elemHeight,
    GUIStyle style,
    GUIStyle firstStyle,
    GUIStyle midStyle,
    GUIStyle lastStyle,
    bool addBorders)
  {
    int num1 = 0;
    int num2 = 0;
    float left = position.xMin;
    float top = position.yMin;
    GUIStyle guiStyle1 = style;
    Rect[] rectArray = new Rect[count];
    if (count > 1)
      guiStyle1 = firstStyle;
    for (int index = 0; index < count; ++index)
    {
      rectArray[index] = !addBorders ? new Rect(left, top, elemWidth, elemHeight) : guiStyle1.margin.Add(new Rect(left, top, elemWidth, elemHeight));
      rectArray[index].width = Mathf.Round(rectArray[index].xMax) - Mathf.Round(rectArray[index].x);
      rectArray[index].x = Mathf.Round(rectArray[index].x);
      GUIStyle guiStyle2 = midStyle;
      if (index == count - 2)
        guiStyle2 = lastStyle;
      left = left + elemWidth + (float) Mathf.Max(guiStyle1.margin.right, guiStyle2.margin.left);
      ++num2;
      if (num2 >= xCount)
      {
        ++num1;
        num2 = 0;
        top = top + elemHeight + (float) Mathf.Max(style.margin.top, style.margin.bottom);
        left = position.xMin;
      }
    }
    return rectArray;
  }

  private static int GetButtonGridMouseSelection(
    Rect[] buttonRects,
    Vector2 mousePos,
    bool findNearest)
  {
    for (int gridMouseSelection = 0; gridMouseSelection < buttonRects.Length; ++gridMouseSelection)
    {
      if (buttonRects[gridMouseSelection].Contains(mousePos))
        return gridMouseSelection;
    }
    if (!findNearest)
      return -1;
    float num = 1E+07f;
    int gridMouseSelection1 = -1;
    for (int index = 0; index < buttonRects.Length; ++index)
    {
      Rect buttonRect = buttonRects[index];
      Vector2 vector2 = new Vector2(Mathf.Clamp(mousePos.x, buttonRect.xMin, buttonRect.xMax), Mathf.Clamp(mousePos.y, buttonRect.yMin, buttonRect.yMax));
      float sqrMagnitude = (mousePos - vector2).sqrMagnitude;
      if ((double) sqrMagnitude < (double) num)
      {
        gridMouseSelection1 = index;
        num = sqrMagnitude;
      }
    }
    return gridMouseSelection1;
  }

  internal static int CalcTotalHorizSpacing(
    int xCount,
    GUIStyle style,
    GUIStyle firstStyle,
    GUIStyle midStyle,
    GUIStyle lastStyle)
  {
    if (xCount < 2)
      return 0;
    if (xCount == 2)
      return Mathf.Max(firstStyle.margin.right, lastStyle.margin.left);
    int num = Mathf.Max(midStyle.margin.left, midStyle.margin.right);
    return Mathf.Max(firstStyle.margin.right, midStyle.margin.left) + Mathf.Max(midStyle.margin.right, lastStyle.margin.left) + num * (xCount - 3);
  }
}
