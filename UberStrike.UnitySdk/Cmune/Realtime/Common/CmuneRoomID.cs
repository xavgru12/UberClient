
using Cmune.Realtime.Common.IO;
using System;
using System.Collections.Generic;

namespace Cmune.Realtime.Common
{
  public struct CmuneRoomID : IByteArray, IComparable<CmuneRoomID>
  {
    private byte _major;
    private byte _minor;
    private ConnectionAddress _server;
    private int _number;
    private string _uniqueID;
    private int _hashCode;

    private CmuneRoomID(int number)
    {
      this._major = (byte) 1;
      this._minor = (byte) 7;
      this._uniqueID = string.Empty;
      this._hashCode = 0;
      this._server = ConnectionAddress.Empty;
      this._number = number;
    }

    public CmuneRoomID(int number, string server)
      : this(number)
    {
      this._server.ConnectionString = server;
      this.UpdateID();
    }

    public CmuneRoomID(byte[] bytes, ref int idx)
      : this(0)
    {
      idx = this.FromBytes(bytes, idx);
      this.UpdateID();
    }

    public CmuneRoomID(byte[] bytes)
      : this(0)
    {
      this.FromBytes(bytes, 0);
      this.UpdateID();
    }

    private void UpdateID()
    {
      this._uniqueID = string.Format("{0}.{1}.{2}:{3}", (object) this._major, (object) this._minor, (object) this.Number, (object) this.Server);
      this._hashCode = this._uniqueID.GetHashCode();
    }

    public override string ToString() => this._uniqueID;

    public byte[] GetBytes()
    {
      List<byte> bytes = new List<byte>(12);
      bytes.Add(this._major);
      bytes.Add(this._minor);
      DefaultByteConverter.FromInt(this._number, ref bytes);
      bytes.AddRange((IEnumerable<byte>) this._server.GetBytes());
      return bytes.ToArray();
    }

    public int FromBytes(byte[] bytes, int idx)
    {
      this._major = bytes[idx++];
      this._minor = bytes[idx++];
      this._number = DefaultByteConverter.ToInt(bytes, ref idx);
      idx = this._server.FromBytes(bytes, idx);
      this.UpdateID();
      return idx;
    }

    public int CompareTo(CmuneRoomID other) => !object.ReferenceEquals((object) other, (object) null) ? this.ID.CompareTo(other.ID) : 0;

    public override bool Equals(object obj) => !object.ReferenceEquals(obj, (object) null) && obj is CmuneRoomID cmuneRoomId && this.ID == cmuneRoomId.ID;

    public static bool operator ==(CmuneRoomID a, CmuneRoomID b) => !object.ReferenceEquals((object) a, (object) null) && !object.ReferenceEquals((object) b, (object) null) ? a.ID == b.ID : object.ReferenceEquals((object) a, (object) b);

    public static bool operator !=(CmuneRoomID a, CmuneRoomID b) => !object.ReferenceEquals((object) a, (object) null) && !object.ReferenceEquals((object) b, (object) null) ? a.ID != b.ID : !object.ReferenceEquals((object) a, (object) b);

    public override int GetHashCode() => this._hashCode;

    public static CmuneRoomID Empty
    {
      get
      {
        CmuneRoomID empty = new CmuneRoomID(0);
        empty.UpdateID();
        return empty;
      }
    }

    public bool IsEmpty => this.Number == 0;

    public bool CanConnectToServer => this._server.IsValid;

    public bool IsVersionCompatible => this._minor == (byte) 7 && this._major == (byte) 1;

    public string ID => this._uniqueID;

    public string Server
    {
      set
      {
        this._server.ConnectionString = value;
        this.UpdateID();
      }
      get => this._server.ConnectionString;
    }

    public int Number
    {
      set
      {
        this._number = value;
        this.UpdateID();
      }
      get => this._number;
    }
  }
}
