// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.NetworkMethodAttribute
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;

namespace UberStrike.Realtime.UnitySdk
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

    public byte ID => this._id.HasValue ? this._id.Value : (byte) 0;
  }
}
