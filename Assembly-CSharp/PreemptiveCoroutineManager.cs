using System.Collections;
using System.Collections.Generic;

public class PreemptiveCoroutineManager : Singleton<PreemptiveCoroutineManager>
{
	public delegate IEnumerator CoroutineFunction();

	private Dictionary<CoroutineFunction, int> coroutineFuncIds;

	private PreemptiveCoroutineManager()
	{
		coroutineFuncIds = new Dictionary<CoroutineFunction, int>();
	}

	public int IncrementId(CoroutineFunction func)
	{
		if (coroutineFuncIds.ContainsKey(func))
		{
			Dictionary<CoroutineFunction, int> dictionary;
			Dictionary<CoroutineFunction, int> dictionary2 = dictionary = coroutineFuncIds;
			CoroutineFunction key;
			CoroutineFunction key2 = key = func;
			int num = dictionary[key];
			return dictionary2[key2] = num + 1;
		}
		return ResetCoroutineId(func);
	}

	public bool IsCurrent(CoroutineFunction func, int coroutineId)
	{
		if (coroutineFuncIds.ContainsKey(func))
		{
			return coroutineFuncIds[func] == coroutineId;
		}
		return false;
	}

	public int ResetCoroutineId(CoroutineFunction func)
	{
		coroutineFuncIds[func] = 0;
		return 0;
	}
}
