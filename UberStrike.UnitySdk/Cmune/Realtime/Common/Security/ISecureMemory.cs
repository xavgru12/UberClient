
namespace Cmune.Realtime.Common.Security
{
  public interface ISecureMemory
  {
    void ValidateData();

    object ReadObject(bool secure);
  }
}
