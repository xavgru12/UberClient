// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.ByteArraySerializable
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

namespace UberStrike.Realtime.UnitySdk
{
  public abstract class ByteArraySerializable : IByteArray
  {
    private int _index = 0;

    public ByteArraySerializable()
    {
    }

    public ByteArraySerializable(byte[] bytes, int index) => this._index = this.FromBytes(bytes, index);

    public int Index => this._index;

    public abstract byte[] GetBytes();

    public abstract int FromBytes(byte[] bytes, int idx);
  }
}
