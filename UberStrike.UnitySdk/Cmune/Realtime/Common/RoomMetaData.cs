// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.RoomMetaData
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using Cmune.Realtime.Common.IO;
using System.Collections.Generic;
using System.Text;

namespace Cmune.Realtime.Common
{
  public class RoomMetaData : IRoomMetaData, IByteArray
  {
    protected string _roomName = string.Empty;
    protected string _password = string.Empty;
    protected int _maxPlayers;
    protected int _inPlayers;
    protected byte _tag;
    private CmuneRoomID _roomID = CmuneRoomID.Empty;

    public static RoomMetaData Empty => new RoomMetaData();

    protected RoomMetaData()
    {
    }

    public RoomMetaData(int roomNumber, string roomName, string server)
    {
      this._roomID.Number = roomNumber;
      this._roomID.Server = server;
      this._roomName = roomName;
    }

    public RoomMetaData(byte[] t, ref int idx) => idx = this.FromBytes(t, idx);

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("ID: {0}\n", (object) this._roomID);
      stringBuilder.AppendFormat("Name: {0}\n", (object) this._roomName);
      stringBuilder.AppendFormat("Password: {0}\n", (object) this._password);
      stringBuilder.AppendFormat("Players: {0}/{1}\n", (object) this._inPlayers, (object) this._maxPlayers);
      stringBuilder.AppendFormat("Tag: {0}\n", (object) this.Tag);
      stringBuilder.AppendFormat("IP: {0}\n", (object) this._roomID.Server);
      return stringBuilder.ToString();
    }

    public float RoomJoinValue
    {
      get
      {
        float num = (float) (this.MaxPlayers - this.ConnectedPlayers);
        return ((double) num > 2.0 ? 1f / num : num * 0.5f) * (float) this.MaxPlayers * (float) this.MaxPlayers;
      }
    }

    public virtual string RoomName
    {
      get => this._roomName;
      set => this._roomName = value;
    }

    public int RoomNumber => this._roomID.Number;

    public CmuneRoomID RoomID
    {
      get => this._roomID;
      set => this._roomID = value;
    }

    public virtual int ConnectedPlayers
    {
      get => this._inPlayers;
      set => this._inPlayers = value;
    }

    public virtual int MaxPlayers
    {
      get => this._maxPlayers;
      set => this._maxPlayers = value;
    }

    public virtual string Password
    {
      get => this._password;
      set => this._password = value;
    }

    public virtual bool IsPublic => string.IsNullOrEmpty(this.Password);

    public bool IsFull => this.ConnectedPlayers >= this.MaxPlayers;

    public string ServerConnection
    {
      get => this._roomID.Server;
      set => this._roomID.Server = value;
    }

    public PhotonUsageType Tag
    {
      get => (PhotonUsageType) this._tag;
      set => this._tag = (byte) value;
    }

    public virtual byte[] GetBytes()
    {
      List<byte> bytes = new List<byte>();
      DefaultByteConverter.FromString(this._roomName, ref bytes, true);
      DefaultByteConverter.FromString(this._password, ref bytes, true);
      DefaultByteConverter.FromByte((byte) this._maxPlayers, ref bytes);
      DefaultByteConverter.FromByte((byte) this._inPlayers, ref bytes);
      DefaultByteConverter.FromByte(this._tag, ref bytes);
      bytes.AddRange((IEnumerable<byte>) this._roomID.GetBytes());
      return bytes.ToArray();
    }

    public virtual int FromBytes(byte[] bytes, int idx)
    {
      this._roomName = DefaultByteConverter.ToString(bytes, ref idx, true);
      this._password = DefaultByteConverter.ToString(bytes, ref idx, true);
      this._maxPlayers = (int) DefaultByteConverter.ToByte(bytes, ref idx);
      this._inPlayers = (int) DefaultByteConverter.ToByte(bytes, ref idx);
      this._tag = DefaultByteConverter.ToByte(bytes, ref idx);
      idx = this._roomID.FromBytes(bytes, idx);
      return idx;
    }
  }
}
