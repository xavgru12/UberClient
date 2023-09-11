
namespace Cmune.Realtime.Common
{
  public interface IByteArray
  {
    byte[] GetBytes();

    int FromBytes(byte[] bytes, int idx);
  }
}
