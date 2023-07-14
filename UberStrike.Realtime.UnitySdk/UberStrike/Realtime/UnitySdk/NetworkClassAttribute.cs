// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.NetworkClassAttribute
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;

namespace UberStrike.Realtime.UnitySdk
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public class NetworkClassAttribute : Attribute, IAttributeID<short>
  {
    private short? _id;

    public NetworkClassAttribute(short id) => this._id = new short?(id);

    public bool HasID => this._id.HasValue;

    public short ID => this._id.HasValue ? this._id.Value : (short) -1;
  }
}
