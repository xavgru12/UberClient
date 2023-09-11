
namespace Cmune.Realtime.Photon.Client.Events
{
  public class PlayerBanEvent
  {
    public string Message;

    public PlayerBanEvent(string message) => this.Message = message;
  }
}
