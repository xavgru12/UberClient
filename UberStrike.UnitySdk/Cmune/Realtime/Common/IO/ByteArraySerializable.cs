// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.IO.ByteArraySerializable
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
