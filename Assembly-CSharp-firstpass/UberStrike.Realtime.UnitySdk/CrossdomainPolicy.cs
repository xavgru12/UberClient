using System;
using System.Collections;
using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
{
	public static class CrossdomainPolicy
	{
		private static Dictionary<string, bool?> _dict = new Dictionary<string, bool?>(20);

		public static Func<string, Action, IEnumerator> CheckPolicyRoutine = Default;

		private static IEnumerator Default(string address, Action callback)
		{
			SetPolicyValue(address, b: true);
			callback();
			yield break;
		}

		public static bool HasValidPolicy(string address)
		{
			bool? value;
			lock (_dict)
			{
				if (!_dict.TryGetValue(address, out value))
				{
					return false;
				}
			}
			if (value.HasValue)
			{
				return value.Value;
			}
			return false;
		}

		public static bool HasPolicyEntry(string address)
		{
			bool? value;
			lock (_dict)
			{
				_dict.TryGetValue(address, out value);
			}
			return value.HasValue;
		}

		public static void RemovePolicyEntry(string address)
		{
			lock (_dict)
			{
				_dict.Remove(address);
			}
		}

		public static void SetPolicyValue(string address, bool b)
		{
			lock (_dict)
			{
				_dict[address] = b;
			}
		}
	}
}
