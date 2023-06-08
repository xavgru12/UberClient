using System;
using System.Collections.Generic;

public class EventHandler
{
	private interface IEventContainer
	{
		void CastEvent(object m);
	}

	private class EventContainer<T> : IEventContainer
	{
		private Dictionary<string, Action<T>> _dictionary = new Dictionary<string, Action<T>>();

		public event Action<T> Sender;

		public void AddCallbackMethod(Action<T> callback)
		{
			string callbackMethodId = GetCallbackMethodId(callback);
			if (!_dictionary.ContainsKey(callbackMethodId))
			{
				_dictionary.Add(callbackMethodId, callback);
				this.Sender = (Action<T>)Delegate.Combine(this.Sender, callback);
			}
		}

		public void RemoveCallbackMethod(Action<T> callback)
		{
			if (_dictionary.Remove(GetCallbackMethodId(callback)))
			{
				this.Sender = (Action<T>)Delegate.Remove(this.Sender, callback);
			}
		}

		private string GetCallbackMethodId(Action<T> callback)
		{
			string text = callback.Method.DeclaringType.FullName + callback.Method.Name;
			if (callback.Target != null)
			{
				text += callback.Target.GetHashCode().ToString();
			}
			return text;
		}

		public void CastEvent(object m)
		{
			if (this.Sender != null)
			{
				this.Sender((T)m);
			}
		}
	}

	public static readonly EventHandler Global = new EventHandler();

	private Dictionary<Type, IEventContainer> eventContainer = new Dictionary<Type, IEventContainer>();

	public void Clear()
	{
		eventContainer.Clear();
	}

	public void AddListener<T>(Action<T> callback)
	{
		if (!eventContainer.TryGetValue(typeof(T), out IEventContainer value))
		{
			value = new EventContainer<T>();
			eventContainer.Add(typeof(T), value);
		}
		(value as EventContainer<T>)?.AddCallbackMethod(callback);
	}

	public void RemoveListener<T>(Action<T> callback)
	{
		if (eventContainer.TryGetValue(typeof(T), out IEventContainer value))
		{
			(value as EventContainer<T>)?.RemoveCallbackMethod(callback);
		}
	}

	public void Fire(object message)
	{
		if (eventContainer.TryGetValue(message.GetType(), out IEventContainer value))
		{
			value.CastEvent(message);
		}
	}
}
