using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public static class StringUtils
{
	public static T ParseValue<T>(string value)
	{
		Type typeFromHandle = typeof(T);
		T result = default(T);
		try
		{
			if (!typeFromHandle.IsEnum)
			{
				if (typeFromHandle != typeof(int))
				{
					if (typeFromHandle != typeof(string))
					{
						if (typeFromHandle != typeof(DateTime))
						{
							Debug.LogError("ParseValue couldn't find a conversion of value '" + value + "' into type '" + typeFromHandle.Name + "'");
							return result;
						}
						result = (T)(object)DateTime.Parse(TextUtilities.Base64Decode(value));
						return result;
					}
					result = (T)(object)value;
					return result;
				}
				result = (T)(object)int.Parse(value);
				return result;
			}
			result = (T)Enum.Parse(typeFromHandle, value);
			return result;
		}
		catch (Exception ex)
		{
			Debug.LogError("ParseValue failed converting value '" + value + "' into type '" + typeFromHandle.Name + "': " + ex.Message);
			return result;
		}
	}
}
