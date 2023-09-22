
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
