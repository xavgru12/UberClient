// Decompiled with JetBrains decompiler
// Type: RectExtentions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class RectExtentions
{
  public static Rect FullExtends(this Rect r) => new Rect(0.0f, 0.0f, r.width, r.height);

  public static Rect Lerp(this Rect r, Rect target, float time) => new Rect(Mathf.Lerp(r.x, target.x, time), Mathf.Lerp(r.y, target.y, time), Mathf.Lerp(r.width, target.width, time), Mathf.Lerp(r.height, target.height, time));

  public static Rect Expand(this Rect r, int width, int height) => new Rect(r.x, r.y, r.width + (float) width, r.height + (float) height);

  public static Rect Contract(this Rect r, int horizontalBorder, int verticalBorder) => new Rect(r.x + (float) horizontalBorder, r.y + (float) verticalBorder, r.width - (float) horizontalBorder * 2f, r.height - (float) verticalBorder * 2f);

  public static Rect OffsetBy(this Rect r, Vector2 offset) => new Rect(r.x + offset.x, r.y + offset.y, r.width, r.height);

  public static Rect OffsetBy(this Rect r, float x, float y) => new Rect(r.x + x, r.y + y, r.width, r.height);

  public static Rect Add(this Rect r1, Rect r2) => new Rect(r1.x + r2.x, r1.y + r2.y, r1.width + r2.width, r1.height + r2.height);

  public static Rect Center(this Rect r) => new Rect((float) (((double) Screen.width - (double) r.width) * 0.5), (float) (((double) Screen.height - (double) r.height) * 0.5), r.width, r.height);

  public static Rect Center(this Rect r, float width, float height) => new Rect((float) (((double) r.width - (double) width) * 0.5), (float) (((double) r.height - (double) height) * 0.5), width, height);

  public static Rect CenterHorizontally(this Rect r, float y, float width, float height) => new Rect((float) (((double) r.width - (double) width) * 0.5), y, width, height);

  public static Rect CenterVertically(this Rect r, float x, float width, float height) => new Rect(x, (float) (((double) r.height - (double) height) * 0.5), width, height);

  public static float HalfWidth(this Rect r) => r.width * 0.5f;

  public static float HalfHeight(this Rect r) => r.height * 0.5f;

  public static bool ContainsTouch(this Rect rect, Vector2 touchPosition)
  {
    Vector2 point = new Vector2(touchPosition.x, (float) Screen.height - touchPosition.y);
    return rect.Contains(point);
  }
}
