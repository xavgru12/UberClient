using System.Collections.Generic;

public class PopupStack<T>
{
	private List<T> items = new List<T>();

	public int Count => items.Count;

	public T Peek()
	{
		if (items.Count > 0)
		{
			return items[items.Count - 1];
		}
		return default(T);
	}

	public void Push(T item)
	{
		items.Add(item);
	}

	public T Pop()
	{
		if (items.Count > 0)
		{
			T val = items[items.Count - 1];
			items.Remove(val);
			return val;
		}
		return default(T);
	}

	public void Remove(int itemAtPosition)
	{
		items.RemoveAt(itemAtPosition);
	}

	public void Remove(T item)
	{
		items.Remove(item);
	}

	public void Clear()
	{
		items.Clear();
	}
}
