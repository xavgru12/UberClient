
namespace Cmune.Realtime.Common
{
  public interface INetworkSynchronizable
  {
    bool IsSynchronizationEnabled { get; }

    void SyncChanges();
  }
}
