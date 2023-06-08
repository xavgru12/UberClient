using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
{
	public class CmunePairList<T1, T2> : List<KeyValuePair<T1, T2>>
	{
		public ICollection<T1> Keys
		{
			get
			{
				List<T1> i = new List<T1>(base.Count);
				ForEach(delegate(KeyValuePair<T1, T2> p)
				{
					i.Add(p.Key);
				});
				return i;
			}
		}

		public ICollection<T2> Values
		{
			get
			{
				List<T2> i = new List<T2>(base.Count);
				ForEach(delegate(KeyValuePair<T1, T2> p)
				{
					i.Add(p.Value);
				});
				return i;
			}
		}

		public CmunePairList()
		{
		}

		public CmunePairList(int capacity)
			: base(capacity)
		{
		}

		public CmunePairList(IEnumerable<KeyValuePair<T1, T2>> collection)
			: base(collection)
		{
		}

		public CmunePairList(IEnumerable<T1> collection1, IEnumerable<T2> collection2)
		{
			IEnumerator<T1> enumerator = collection1.GetEnumerator();
			IEnumerator<T2> enumerator2 = collection2.GetEnumerator();
			while (enumerator.MoveNext() && enumerator2.MoveNext())
			{
				Add(new KeyValuePair<T1, T2>(enumerator.Current, enumerator2.Current));
			}
		}

		public ICollection<KeyValuePair<T1, T2>> GetPairsWithKey(T1 key)
		{
			return FindAll((KeyValuePair<T1, T2> p) => p.Key.Equals(key));
		}

		public ICollection<KeyValuePair<T1, T2>> GetPairsWithValue(T2 value)
		{
			return FindAll((KeyValuePair<T1, T2> p) => p.Value.Equals(value));
		}

		public void Add(T1 first, T2 second)
		{
			Add(new KeyValuePair<T1, T2>(first, second));
		}

		public void Clamp(int max)
		{
			if (base.Count > max)
			{
				RemoveRange(max, base.Count - max);
			}
		}
	}
}
