
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
