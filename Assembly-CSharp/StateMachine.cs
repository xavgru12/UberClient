using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : StateMachine<int>
{
}
public class StateMachine<T> where T : struct, IConvertible
{
	public readonly EventHandler Events = new EventHandler();

	private Dictionary<T, IState> registeredStates;

	private Stack<T> stateStack;

	public T CurrentStateId
	{
		get
		{
			if (stateStack.Count > 0)
			{
				return stateStack.Peek();
			}
			return default(T);
		}
	}

	private IState CurrentState => GetState(CurrentStateId);

	public event Action<T> OnChanged;

	public StateMachine()
	{
		registeredStates = new Dictionary<T, IState>();
		stateStack = new Stack<T>();
	}

	public void SetState(T stateId)
	{
		if (ContainsState(stateId))
		{
			if (!stateId.Equals(CurrentStateId))
			{
				PopAllStates();
				stateStack.Push(stateId);
				GetState(stateId).OnEnter();
				if (this.OnChanged != null)
				{
					this.OnChanged(stateId);
				}
			}
			return;
		}
		T val = stateId;
		throw new Exception("Unsupported state of type: " + val.ToString());
	}

	public void PushState(T stateId)
	{
		if (ContainsState(stateId))
		{
			if (!stateStack.Contains(stateId))
			{
				stateStack.Push(stateId);
				GetState(stateId).OnEnter();
				if (this.OnChanged != null)
				{
					this.OnChanged(stateId);
				}
			}
		}
		else
		{
			T val = stateId;
			Debug.LogWarning("Unsupported state of type: " + val.ToString());
		}
	}

	public void PopState(bool resume = true)
	{
		if (stateStack.Count != 0)
		{
			CurrentState.OnExit();
			stateStack.Pop();
			if (resume && stateStack.Count != 0)
			{
				CurrentState.OnResume();
			}
			if (this.OnChanged != null && stateStack.Count > 0)
			{
				this.OnChanged(stateStack.Peek());
			}
		}
	}

	public void Reset()
	{
		PopAllStates();
		stateStack.Clear();
		registeredStates.Clear();
		Events.Clear();
		if (this.OnChanged != null)
		{
			this.OnChanged(default(T));
		}
	}

	public void PopAllStates()
	{
		while (stateStack.Count > 0)
		{
			PopState(resume: false);
		}
		if (this.OnChanged != null)
		{
			this.OnChanged(default(T));
		}
	}

	public void RegisterState(T stateId, IState state)
	{
		if (!registeredStates.ContainsKey(stateId))
		{
			registeredStates.Add(stateId, state);
			return;
		}
		T val = stateId;
		throw new Exception("StateMachine::RegisterState - state [" + val.ToString() + "] already exists in the current registry");
	}

	public bool ContainsState(T stateId)
	{
		return registeredStates.ContainsKey(stateId);
	}

	public void Update()
	{
		if (stateStack.Count > 0)
		{
			CurrentState.OnUpdate();
		}
	}

	public IState GetState(T stateId)
	{
		registeredStates.TryGetValue(stateId, out IState value);
		return value;
	}
}
