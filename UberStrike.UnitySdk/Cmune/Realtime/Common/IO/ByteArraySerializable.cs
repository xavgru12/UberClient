
namespace Cmune.Realtime.Common.IO
{
  public abstract class ByteArraySerializable : IByteArray
  {
    private int _index;

    public ByteArraySerializable()
    {
    }

    public ByteArraySerializable(byte[] bytes, int index) => this._index = this.FromBytes(bytes, index);

    public int Index => this._index;

    public abstract byte[] GetBytes();

    public abstract int FromBytes(byte[] bytes, int idx);
  }
}
