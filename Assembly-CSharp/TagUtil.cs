using System;
using UnityEngine;

public static class TagUtil
{
	public static string GetTag(Collider c)
	{
		string result = "Cement";
		try
		{
			if (!c)
			{
				return result;
			}
			result = c.tag;
			return result;
		}
		catch (Exception ex)
		{
			Debug.LogError("Failed to get tag from collider: " + ex.Message);
			return result;
		}
	}
}
