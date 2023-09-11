
using System.IO;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization
{
  public static class ClanInvitationAnswerViewModelProxy
  {
    public static void Serialize(Stream stream, ClanInvitationAnswerViewModel instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.GroupInvitationId);
          BooleanProxy.Serialize((Stream) bytes, instance.IsInvitationAccepted);
          Int32Proxy.Serialize((Stream) bytes, instance.ReturnValue);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static ClanInvitationAnswerViewModel Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ClanInvitationAnswerViewModel invitationAnswerViewModel = (ClanInvitationAnswerViewModel) null;
      if (num != 0)
      {
        invitationAnswerViewModel = new ClanInvitationAnswerViewModel();
        invitationAnswerViewModel.GroupInvitationId = Int32Proxy.Deserialize(bytes);
        invitationAnswerViewModel.IsInvitationAccepted = BooleanProxy.Deserialize(bytes);
        invitationAnswerViewModel.ReturnValue = Int32Proxy.Deserialize(bytes);
      }
      return invitationAnswerViewModel;
    }
  }
}
