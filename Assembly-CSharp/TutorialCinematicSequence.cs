// Decompiled with JetBrains decompiler
// Type: TutorialCinematicSequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;

public class TutorialCinematicSequence
{
  private TutorialCinematicSequence.TutorialState state;
  private ITutorialCinematicSequenceListener listener;

  public TutorialCinematicSequence(ITutorialCinematicSequenceListener l) => this.listener = l;

  public TutorialCinematicSequence.TutorialState State => this.state;

  [DebuggerHidden]
  public IEnumerator StartSequences() => (IEnumerator) new TutorialCinematicSequence.\u003CStartSequences\u003Ec__Iterator26()
  {
    \u003C\u003Ef__this = this
  };

  public void OnAirlockCameraReady() => this.state = TutorialCinematicSequence.TutorialState.AirlockCameraReady;

  public void OnAirlockMouseLook() => this.state = TutorialCinematicSequence.TutorialState.AirlockMouseLook;

  public void OnAirlockWasdWalk() => this.state = TutorialCinematicSequence.TutorialState.AirlockWasdWalk;

  public void OnArmoryEnter() => this.state = TutorialCinematicSequence.TutorialState.ArmoryEnter;

  public void OnArmoryPickupMG() => this.state = TutorialCinematicSequence.TutorialState.ArmoryPickupMG;

  public void OnShootingRangeGameOver() => this.state = TutorialCinematicSequence.TutorialState.ShootingRangeGameOver;

  public enum TutorialState
  {
    None,
    AirlockCameraZoomIn,
    AirlockCameraReady,
    AirlockWelcome,
    AirlockMouseLookSubtitle,
    AirlockMouseLook,
    AirlockWasdSubtitle,
    AirlockWasdWalk,
    AirlockDoorOpen,
    ArmoryEnter,
    ArmoryPickupMG,
    ShootingRangeGameOver,
    TutorialEnd,
  }
}
