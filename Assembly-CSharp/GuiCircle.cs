using UnityEngine;

public static class GuiCircle
{
	public enum Direction
	{
		Clockwise,
		CounterClockwise
	}

	private static Vector3 TexShift = new Vector3(0.5f, 0.5f, 0.5f);

	private static Vector3 Normal = new Vector3(0f, 0f, 1f);

	public static void DrawArc(Vector2 position, float angle, float radius, Material material)
	{
		DrawArc(position, 0f, angle, radius, material, Direction.Clockwise);
	}

	public static void DrawArc(Vector2 position, float startAngle, float fillAngle, float radius, Material material, Direction dir)
	{
		if (Event.current.type == EventType.Repaint)
		{
			GL.PushMatrix();
			material.SetPass(0);
			DrawSolidArc(new Vector3(position.x, position.y, 0f), fillAngle, radius, Quaternion.Euler(0f, 0f, startAngle), dir);
			GL.PopMatrix();
		}
	}

	private static void DrawSolidArc(Vector3 center, float angle, float radius, Quaternion rot, Direction dir)
	{
		Vector3 a = rot * Vector3.down;
		int num = (int)Mathf.Clamp(angle * 0.1f, 5f, 30f);
		float num2 = 1f / (float)(num - 1);
		Quaternion rotation = Quaternion.AngleAxis(angle * num2, (dir != 0) ? (-Normal) : Normal);
		Vector3 vector = a * radius;
		float num3 = 1f / (2f * radius);
		Vector3 b = new Vector3(num3, 0f - num3, num3);
		GL.Begin(4);
		for (int i = 0; i < num - 1; i++)
		{
			Vector3 vector2 = vector;
			vector = rotation * vector;
			GL.TexCoord(TexShift);
			GL.Vertex(center);
			if (dir == Direction.Clockwise)
			{
				GL.TexCoord(TexShift + rot * Vector3.Scale(vector2, b));
				GL.Vertex(center + vector2);
				GL.TexCoord(TexShift + rot * Vector3.Scale(vector, b));
				GL.Vertex(center + vector);
			}
			else
			{
				GL.TexCoord(TexShift + rot * Vector3.Scale(vector, b));
				GL.Vertex(center + vector);
				GL.TexCoord(TexShift + rot * Vector3.Scale(vector2, b));
				GL.Vertex(center + vector2);
			}
		}
		GL.End();
	}

	public static void DrawArcLine(Vector2 position, float startAngle, float fillAngle, float radius, float width, Material material, Direction dir)
	{
		if (Event.current.type == EventType.Repaint)
		{
			material.SetPass(0);
			DrawSolidArc(new Vector3(position.x, position.y, 0f), fillAngle, radius, width, Quaternion.Euler(0f, 0f, startAngle) * Vector3.down, dir);
		}
	}

	private static void DrawSolidArc(Vector3 center, float angle, float radius, float width, Vector3 from, Direction dir)
	{
		if (!(radius > 0f))
		{
			return;
		}
		int num = (int)Mathf.Clamp(angle * 0.1f, 5f, 30f);
		float num2 = 1f / (float)(num - 1);
		float d = 1f - Mathf.Clamp(width / radius, 0.001f, 1f);
		Quaternion rotation = Quaternion.AngleAxis(angle * num2, (dir != 0) ? (-Normal) : Normal);
		Vector3 vector = from * radius;
		GL.Begin(7);
		for (int i = 0; i < num - 1; i++)
		{
			Vector3 vector2 = vector;
			vector = rotation * vector;
			if (dir == Direction.Clockwise)
			{
				GL.Vertex(center + vector2);
				GL.Vertex(center + vector);
				GL.Vertex(center + vector * d);
				GL.Vertex(center + vector2 * d);
			}
			else
			{
				GL.Vertex(center + vector);
				GL.Vertex(center + vector2);
				GL.Vertex(center + vector2 * d);
				GL.Vertex(center + vector * d);
			}
		}
		GL.End();
	}
}
