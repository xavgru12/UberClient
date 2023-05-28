// Decompiled with JetBrains decompiler
// Type: ITutorialCinematicSequenceListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

public interface ITutorialCinematicSequenceListener
{
  void OnAirlockCameraZoomIn();

  void OnAirlockWelcome();

  void OnAirlockMouseLookSubtitle();

  void OnAirlockWasdSubtitle();

  void OnAirlockDoorOpen();

  void OnTutorialEnd();
}
