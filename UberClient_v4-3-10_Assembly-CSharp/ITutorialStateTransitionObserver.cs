// Decompiled with JetBrains decompiler
// Type: ITutorialStateTransitionObserver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

public interface ITutorialStateTransitionObserver
{
  void TransitFromInitToAirlockCameraFollowOnWaitForSeconds();

  void TransitFromAirlockCameraFollowToAirlockWelcomeOnWaitForSeconds();

  void TransitFromAirlockWelcomeToAirlockMouseLookOnWaitForSeconds();

  void TransitFromAirlockMouseLookToAirlockWASDWalkOnMouseLookAirlock();

  void TransitFromAirlockWASDWalkToAirlockDoorOpenOnWASDWalkAirlock();

  void TransitFromAirlockDoorOpenToTunnelOnEnterArmory();

  void TransitFromTunnelToArmoryOnEnterArmory();

  void TransitFromArmoryToHeadingToFightOnLeaveArmory();

  void TransitFromHeadingToFightToArenaFieldOnEnterPlayground();

  void TransitFromArenaFieldToFiniOnAllBotsKilled();
}
