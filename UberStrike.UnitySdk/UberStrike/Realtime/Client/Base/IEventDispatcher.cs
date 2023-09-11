
namespace Uberstrike.Realtime.Client.Base
{
  public interface IEventDispatcher
  {
    void OnEvent(byte id, byte[] data);
  }
}
