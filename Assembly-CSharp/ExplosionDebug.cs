using System.Collections.Generic;
using UnityEngine;

public class ExplosionDebug : AutoMonoBehaviour<ExplosionDebug>
{
	public struct Line
	{
		public Vector3 Start;

		public Vector3 End;

		public Line(Vector3 start, Vector3 end)
		{
			Start = start;
			End = end;
		}
	}

	public Vector3 ImpactPoint;

	public Vector3 TestPoint;

	public float Radius;

	public List<Vector3> Hits = new List<Vector3>();

	public List<Line> Protections = new List<Line>();

	public void Reset()
	{
		Hits.Clear();
		Protections.Clear();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(ImpactPoint, Radius);
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(TestPoint, 0.1f);
		for (int i = 0; i < Hits.Count; i++)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(Hits[i], 0.1f);
		}
		for (int j = 0; j < Protections.Count; j++)
		{
			Gizmos.color = Color.green;
			Line line = Protections[j];
			Vector3 start = line.Start;
			Line line2 = Protections[j];
			Gizmos.DrawLine(start, line2.End);
		}
	}
}
