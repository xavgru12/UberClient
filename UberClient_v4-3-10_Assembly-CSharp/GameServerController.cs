// Decompiled with JetBrains decompiler
// Type: GameServerController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;

public class GameServerController : Singleton<GameServerController>
{
  private GameServerController()
  {
  }

  public GameServerView SelectedServer { get; set; }

  public void JoinFastestServer() => MonoRoutine.Start(this.StartJoiningBestGameServer());

  public void CreateOnFastestServer() => MonoRoutine.Start(this.StartCreatingOnBestGameServer());

  [DebuggerHidden]
  private IEnumerator StartJoiningBestGameServer()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    GameServerController.\u003CStartJoiningBestGameServer\u003Ec__Iterator69 serverCIterator69 = new GameServerController.\u003CStartJoiningBestGameServer\u003Ec__Iterator69();
    return (IEnumerator) serverCIterator69;
  }

  [DebuggerHidden]
  private IEnumerator StartCreatingOnBestGameServer()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    GameServerController.\u003CStartCreatingOnBestGameServer\u003Ec__Iterator6A serverCIterator6A = new GameServerController.\u003CStartCreatingOnBestGameServer\u003Ec__Iterator6A();
    return (IEnumerator) serverCIterator6A;
  }
}
