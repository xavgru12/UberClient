
namespace Cmune.Realtime.Photon.Client
{
  public interface IClientNetworkClass
  {
    void SetMessageInterface(NetworkMessenger sender);

    void SetClassRegistration(RemoteMethodInterface register);
  }
}
