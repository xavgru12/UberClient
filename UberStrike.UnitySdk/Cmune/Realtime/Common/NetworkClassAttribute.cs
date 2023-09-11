// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.NetworkClassAttribute
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;

namespace Cmune.Realtime.Common
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public class NetworkClassAttribute : Attribute, IAttributeID<short>
  {
    private short? _id;

    public NetworkClassAttribute(short id) => this._id = new short?(id);

    public bool HasID => this._id.HasValue;

    public short ID => !this._id.HasValue ? (short) -1 : this._id.Value;
  }
}
