// Decompiled with JetBrains decompiler
// Type: PlayPageScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

public class PlayPageScene : PageScene
{
  public override PageType PageType => PageType.Play;

  protected override void OnLoad() => PlayPageGUI.Instance.Show();

  protected override void OnUnload() => PlayPageGUI.Instance.Hide();

  private void OnDisable() => PlayPageGUI.Instance.Hide();
}
