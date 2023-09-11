// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.NetworkMethodAttribute
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;

namespace Cmune.Realtime.Common
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
  public class NetworkMethodAttribute : Attribute, IAttributeID<byte>
  {
    private byte? _id;

    public NetworkMethodAttribute()
    {
    }

    public NetworkMethodAttribute(byte id) => this._id = new byte?(id);

    public bool HasID => this._id.HasValue;

    public byte ID => !this._id.HasValue ? (byte) 0 : this._id.Value;
  }
}
