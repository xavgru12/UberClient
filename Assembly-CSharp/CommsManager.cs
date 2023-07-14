// Decompiled with JetBrains decompiler
// Type: CommsManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UberStrike.Realtime.UnitySdk;
using UberStrike.WebService.Unity;

public class CommsManager : Singleton<CommsManager>
{
  private CommsManager()
  {
  }

  public float NextFriendsRefresh { get; private set; }

  public void SendFriendRequest(int cmid, string message)
  {
    message = TextUtilities.ShortenText(TextUtilities.Trim(message), 140, false);
    RelationshipWebServiceClient.SendContactRequest(PlayerDataManager.CmidSecure, cmid, message, (Action<int>) (ev =>
    {
      CommActorInfo info;
      if (!CommConnectionManager.CommCenter.TryGetActorWithCmid(cmid, out info))
        return;
      CommConnectionManager.CommCenter.UpdateInboxRequest(info.ActorId);
    }), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace)));
  }

  [DebuggerHidden]
  public IEnumerator GetContactsByGroups() => (IEnumerator) new CommsManager.\u003CGetContactsByGroups\u003Ec__Iterator68()
  {
    \u003C\u003Ef__this = this
  };

  public void UpdateCommunicator()
  {
    CommConnectionManager.CommCenter.SendContactList();
    Singleton<ChatManager>.Instance.UpdateFriendSection();
  }
}
