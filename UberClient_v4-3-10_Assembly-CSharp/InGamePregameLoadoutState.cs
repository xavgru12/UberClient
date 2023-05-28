// Decompiled with JetBrains decompiler
// Type: InGamePregameLoadoutState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

internal class InGamePregameLoadoutState : IState
{
  public void OnEnter()
  {
    GamePageManager.Instance.LoadPage(PageType.PreGame);
    Singleton<HudDrawFlagGroup>.Instance.BaseDrawFlag = HudDrawFlags.XpPoints | HudDrawFlags.InGameChat;
    HudController.Instance.XpPtsHud.DisplayPermanently();
    HudController.Instance.XpPtsHud.ResetXp();
    HudController.Instance.XpPtsHud.IsXpPtsTextVisible = false;
    HudController.Instance.XpPtsHud.ResetTransform();
  }

  public void OnExit() => GamePageManager.Instance.UnloadCurrentPage();

  public void OnUpdate()
  {
  }

  public void OnGUI()
  {
  }
}
