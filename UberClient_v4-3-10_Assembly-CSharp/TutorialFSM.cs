// Decompiled with JetBrains decompiler
// Type: TutorialFSM
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class TutorialFSM
{
  private int cur_state;
  private List<ITutorialStateTransitionObserver> observers = new List<ITutorialStateTransitionObserver>();

  public TutorialFSM.State CurrentState => (TutorialFSM.State) this.cur_state;

  public void AddObserver(ITutorialStateTransitionObserver ob)
  {
    if (ob == null || this.observers.Contains(ob))
      return;
    this.observers.Add(ob);
  }

  public void RemoveObserver(ITutorialStateTransitionObserver ob)
  {
    if (ob == null || !this.observers.Contains(ob))
      return;
    this.observers.Remove(ob);
  }

  public void OnWaitForSeconds()
  {
    switch (this.cur_state)
    {
      case 0:
        this.cur_state = 1;
        using (List<ITutorialStateTransitionObserver>.Enumerator enumerator = this.observers.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current?.TransitFromInitToAirlockCameraFollowOnWaitForSeconds();
          break;
        }
      case 1:
        this.cur_state = 2;
        using (List<ITutorialStateTransitionObserver>.Enumerator enumerator = this.observers.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current?.TransitFromAirlockCameraFollowToAirlockWelcomeOnWaitForSeconds();
          break;
        }
      case 2:
        this.cur_state = 3;
        using (List<ITutorialStateTransitionObserver>.Enumerator enumerator = this.observers.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current?.TransitFromAirlockWelcomeToAirlockMouseLookOnWaitForSeconds();
          break;
        }
    }
  }

  public void OnWelcomeAirlock()
  {
  }

  public void OnLookAirlock()
  {
    if (this.cur_state != 3)
      return;
    this.cur_state = 4;
    foreach (ITutorialStateTransitionObserver observer in this.observers)
      observer?.TransitFromAirlockMouseLookToAirlockWASDWalkOnMouseLookAirlock();
  }

  public void OnWASDWalkAirlock()
  {
    if (this.cur_state != 4)
      return;
    this.cur_state = 5;
    foreach (ITutorialStateTransitionObserver observer in this.observers)
      observer?.TransitFromAirlockWASDWalkToAirlockDoorOpenOnWASDWalkAirlock();
  }

  public void OnEnterArmory()
  {
    switch (this.cur_state)
    {
      case 5:
        this.cur_state = 6;
        using (List<ITutorialStateTransitionObserver>.Enumerator enumerator = this.observers.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current?.TransitFromAirlockDoorOpenToTunnelOnEnterArmory();
          break;
        }
      case 6:
        this.cur_state = 7;
        using (List<ITutorialStateTransitionObserver>.Enumerator enumerator = this.observers.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current?.TransitFromTunnelToArmoryOnEnterArmory();
          break;
        }
    }
  }

  public void OnLeaveArmory()
  {
    if (this.cur_state != 7)
      return;
    this.cur_state = 9;
    foreach (ITutorialStateTransitionObserver observer in this.observers)
      observer?.TransitFromArmoryToHeadingToFightOnLeaveArmory();
  }

  public void OnEnterPlayground()
  {
    if (this.cur_state != 9)
      return;
    this.cur_state = 10;
    foreach (ITutorialStateTransitionObserver observer in this.observers)
      observer?.TransitFromHeadingToFightToArenaFieldOnEnterPlayground();
  }

  public void OnAllBotsKilled()
  {
    if (this.cur_state != 10)
      return;
    this.cur_state = 11;
    foreach (ITutorialStateTransitionObserver observer in this.observers)
      observer?.TransitFromArenaFieldToFiniOnAllBotsKilled();
  }

  public enum State
  {
    Init,
    AirlockCameraFollow,
    AirlockWelcome,
    AirlockMouseLook,
    AirlockWASDWalk,
    AirlockDoorOpen,
    Tunnel,
    Armory,
    ShootingArea,
    HeadingToFight,
    ArenaField,
    Fini,
  }
}
