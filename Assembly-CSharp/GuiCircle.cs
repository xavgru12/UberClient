// Decompiled with JetBrains decompiler
// Type: GuiCircle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class GuiCircle
{
  private static Vector3 TexShift = new Vector3(0.5f, 0.5f, 0.5f);
  private static Vector3 Normal = new Vector3(0.0f, 0.0f, 1f);

  public static void DrawArc(Vector2 position, float angle, float radius, Material material) => GuiCircle.DrawArc(position, 0.0f, angle, radius, material, GuiCircle.Direction.Clockwise);

  public static void DrawArc(
    Vector2 position,
    float startAngle,
    float fillAngle,
    float radius,
    Material material,
    GuiCircle.Direction dir)
  {
    if (Event.current.type != UnityEngine.EventType.Repaint)
      return;
    GL.PushMatrix();
    material.SetPass(0);
    GuiCircle.DrawSolidArc(new Vector3(position.x, position.y, 0.0f), fillAngle, radius, Quaternion.Euler(0.0f, 0.0f, startAngle), dir);
    GL.PopMatrix();
  }

  private static void DrawSolidArc(
    Vector3 center,
    float angle,
    float radius,
    Quaternion rot,
    GuiCircle.Direction dir)
  {
    Vector3 vector3 = rot * Vector3.down;
    int num1 = (int) Mathf.Clamp(angle * 0.1f, 5f, 30f);
    float num2 = 1f / (float) (num1 - 1);
    Quaternion quaternion = Quaternion.AngleAxis(angle * num2, dir != GuiCircle.Direction.Clockwise ? -GuiCircle.Normal : GuiCircle.Normal);
    Vector3 a1 = vector3 * radius;
    float num3 = (float) (1.0 / (2.0 * (double) radius));
    Vector3 b = new Vector3(num3, -num3, num3);
    GL.Begin(4);
    for (int index = 0; index < num1 - 1; ++index)
    {
      Vector3 a2 = a1;
      a1 = quaternion * a1;
      GL.TexCoord(GuiCircle.TexShift);
      GL.Vertex(center);
      if (dir == GuiCircle.Direction.Clockwise)
      {
        GL.TexCoord(GuiCircle.TexShift + rot * Vector3.Scale(a2, b));
        GL.Vertex(center + a2);
        GL.TexCoord(GuiCircle.TexShift + rot * Vector3.Scale(a1, b));
        GL.Vertex(center + a1);
      }
      else
      {
        GL.TexCoord(GuiCircle.TexShift + rot * Vector3.Scale(a1, b));
        GL.Vertex(center + a1);
        GL.TexCoord(GuiCircle.TexShift + rot * Vector3.Scale(a2, b));
        GL.Vertex(center + a2);
      }
    }
    GL.End();
  }

  public static void DrawArcLine(
    Vector2 position,
    float startAngle,
    float fillAngle,
    float radius,
    float width,
    Material material,
    GuiCircle.Direction dir)
  {
    if (Event.current.type != UnityEngine.EventType.Repaint)
      return;
    material.SetPass(0);
    GuiCircle.DrawSolidArc(new Vector3(position.x, position.y, 0.0f), fillAngle, radius, width, Quaternion.Euler(0.0f, 0.0f, startAngle) * Vector3.down, dir);
  }

  private static void DrawSolidArc(
    Vector3 center,
    float angle,
    float radius,
    float width,
    Vector3 from,
    GuiCircle.Direction dir)
  {
    if ((double) radius <= 0.0)
      return;
    int num1 = (int) Mathf.Clamp(angle * 0.1f, 5f, 30f);
    float num2 = 1f / (float) (num1 - 1);
    float num3 = 1f - Mathf.Clamp(width / radius, 1f / 1000f, 1f);
    Quaternion quaternion = Quaternion.AngleAxis(angle * num2, dir != GuiCircle.Direction.Clockwise ? -GuiCircle.Normal : GuiCircle.Normal);
    Vector3 vector3_1 = from * radius;
    GL.Begin(7);
    for (int index = 0; index < num1 - 1; ++index)
    {
      Vector3 vector3_2 = vector3_1;
      vector3_1 = quaternion * vector3_1;
      if (dir == GuiCircle.Direction.Clockwise)
      {
        GL.Vertex(center + vector3_2);
        GL.Vertex(center + vector3_1);
        GL.Vertex(center + vector3_1 * num3);
        GL.Vertex(center + vector3_2 * num3);
      }
      else
      {
        GL.Vertex(center + vector3_1);
        GL.Vertex(center + vector3_2);
        GL.Vertex(center + vector3_2 * num3);
        GL.Vertex(center + vector3_1 * num3);
      }
    }
    GL.End();
  }

  public enum Direction
  {
    Clockwise,
    CounterClockwise,
  }
}
