// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ClanInvitationAnswerViewModelProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization
{
  public static class ClanInvitationAnswerViewModelProxy
  {
    public static void Serialize(Stream stream, ClanInvitationAnswerViewModel instance)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.GroupInvitationId);
        BooleanProxy.Serialize((Stream) bytes, instance.IsInvitationAccepted);
        Int32Proxy.Serialize((Stream) bytes, instance.ReturnValue);
        bytes.WriteTo(stream);
      }
    }

    public static ClanInvitationAnswerViewModel Deserialize(Stream bytes) => new ClanInvitationAnswerViewModel()
    {
      GroupInvitationId = Int32Proxy.Deserialize(bytes),
      IsInvitationAccepted = BooleanProxy.Deserialize(bytes),
      ReturnValue = Int32Proxy.Deserialize(bytes)
    };
  }
}
