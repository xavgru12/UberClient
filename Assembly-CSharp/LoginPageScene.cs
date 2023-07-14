// Decompiled with JetBrains decompiler
// Type: LoginPageScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

public class LoginPageScene : PageScene
{
  public override PageType PageType => PageType.Login;

  protected override void OnLoad() => PanelManager.Instance.OpenPanel(PanelType.Login);

  protected override void OnUnload() => PanelManager.Instance.ClosePanel(PanelType.Login);
}
