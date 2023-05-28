// Decompiled with JetBrains decompiler
// Type: StateMachine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class StateMachine
{
  private Dictionary<int, IState> _statesGroup;
  private Stack<int> _curStates;

  public StateMachine()
  {
    this._statesGroup = new Dictionary<int, IState>();
    this._curStates = new Stack<int>();
  }

  public void SetState(int stateId)
  {
    if (!this.ContainsState(stateId))
      throw new Exception("ChangeState failed - cannot found state [" + (object) stateId + "] from state machine");
    this.PopAllStates();
    this._curStates.Push(stateId);
    this.GetState(stateId).OnEnter();
  }

  public void PushState(int stateId)
  {
    if (!this.ContainsState(stateId))
      throw new Exception("PushState failed - cannot found state[" + (object) stateId + "] from state machine");
    this._curStates.Push(stateId);
    this.GetState(stateId).OnEnter();
  }

  public void PopState()
  {
    if (this._curStates.Count == 0)
      return;
    this.CurrentState.OnExit();
    this._curStates.Pop();
  }

  public void PopAllStates()
  {
    while (this._curStates.Count > 0)
      this.PopState();
  }

  public void RegisterState(int stateId, IState state)
  {
    if (this._statesGroup.ContainsKey(stateId))
      throw new Exception("StateMachine::RegisterState - state [" + (object) stateId + "] already exists in the current registry");
    this._statesGroup.Add(stateId, state);
  }

  public bool ContainsState(int stateId) => this._statesGroup.ContainsKey(stateId);

  public void Update()
  {
    if (this._curStates.Count <= 0)
      return;
    this.CurrentState.OnUpdate();
  }

  public void OnGUI()
  {
    if (this._curStates.Count <= 0)
      return;
    this.CurrentState.OnGUI();
  }

  public int CurrentStateId => this._curStates.Count > 0 ? this._curStates.Peek() : -1;

  public bool IsRunning => this._curStates.Count > 0;

  public IState GetState(int stateId)
  {
    IState state = (IState) null;
    this._statesGroup.TryGetValue(stateId, out state);
    return state;
  }

  private IState CurrentState => this.GetState(this.CurrentStateId);
}
