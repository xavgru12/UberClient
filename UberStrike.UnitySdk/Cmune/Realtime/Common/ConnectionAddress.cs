
using Cmune.Realtime.Common.IO;
using Cmune.Util;
using System;
using System.Collections.Generic;

namespace Cmune.Realtime.Common
{
  public struct ConnectionAddress : IByteArray
  {
    private byte[] _connectionBytes;
    private string _connectionString;

    public static ConnectionAddress Empty => new ConnectionAddress(0);

    private ConnectionAddress(int dummy)
    {
      this._connectionString = string.Empty;
      this._connectionBytes = new byte[6];
      this.UpdateConnectionString();
    }

    public ConnectionAddress(string ipAddress, short port)
    {
      this._connectionString = string.Empty;
      this._connectionBytes = new byte[6];
      this.FromString(string.Format("{0}:{1}", (object) ipAddress, (object) port));
    }

    public ConnectionAddress(string connectionString)
    {
      this._connectionString = string.Empty;
      this._connectionBytes = new byte[6];
      this.FromString(connectionString);
    }

    public bool IsValid => this._connectionBytes[0] > (byte) 0;

    public string ServerIP => string.Format("{0}.{1}.{2}.{3}", (object) this._connectionBytes[0], (object) this._connectionBytes[1], (object) this._connectionBytes[2], (object) this._connectionBytes[3]);

    public string ServerPort
    {
      get
      {
        int i = 4;
        return DefaultByteConverter.ToUShort(this._connectionBytes, ref i).ToString();
      }
    }

    public string ConnectionString
    {
      get => this._connectionString;
      set => this.FromString(value);
    }

    public override bool Equals(object obj) => obj is ConnectionAddress connectionAddress && this == connectionAddress;

    public override int GetHashCode() => this._connectionString.GetHashCode();

    public static bool operator ==(ConnectionAddress a, ConnectionAddress b)
    {
      if (object.ReferenceEquals((object) a, (object) b))
        return true;
      return (ValueType) a != null && (ValueType) b != null && a.ConnectionString == b.ConnectionString;
    }

    public static bool operator !=(ConnectionAddress a, ConnectionAddress b) => !(a == b);

    private void UpdateConnectionString()
    {
      int i = 4;
      this._connectionString = string.Format("{0}.{1}.{2}.{3}:{4}", (object) this._connectionBytes[0], (object) this._connectionBytes[1], (object) this._connectionBytes[2], (object) this._connectionBytes[3], (object) DefaultByteConverter.ToUShort(this._connectionBytes, ref i));
    }

    private void FromString(string connection)
    {
      string[] strArray1 = connection.Split(':');
      if (strArray1.Length == 2)
      {
        if (strArray1[0].Equals("localhost", StringComparison.InvariantCultureIgnoreCase))
          strArray1[0] = "127.0.0.1";
        string[] strArray2 = strArray1[0].Split('.');
        ushort result = 0;
        if (!byte.TryParse(strArray2[0], out this._connectionBytes[0]) || !byte.TryParse(strArray2[1], out this._connectionBytes[1]) || !byte.TryParse(strArray2[2], out this._connectionBytes[2]) || !byte.TryParse(strArray2[3], out this._connectionBytes[3]) || !ushort.TryParse(strArray1[1], out result))
        {
          if (CmuneDebug.IsWarningEnabled)
            CmuneDebug.LogWarning("The Server Connection string '{0}' is not a combination of IP addess and port. Use the format <xxx.xxx.xxx.xxx:port>", (object) connection);
        }
        else
        {
          List<byte> bytes = new List<byte>(2);
          DefaultByteConverter.FromUShort(result, ref bytes);
          this._connectionBytes[4] = bytes[0];
          this._connectionBytes[5] = bytes[1];
        }
      }
      else if (CmuneDebug.IsWarningEnabled)
        CmuneDebug.LogWarning("The Server Connection string '{0}' is not a combination of IP addess and port. Use the format <xxx.xxx.xxx.xxx:port>", (object) connection);
      this.UpdateConnectionString();
    }

    public override string ToString() => this._connectionString;

    public byte[] GetBytes() => this._connectionBytes;

    public int FromBytes(byte[] bytes, int index)
    {
      Array.Copy((Array) bytes, index, (Array) this._connectionBytes, 0, 6);
      int i = 4;
      this._connectionString = string.Format("{0}.{1}.{2}.{3}:{4}", (object) this._connectionBytes[0], (object) this._connectionBytes[1], (object) this._connectionBytes[2], (object) this._connectionBytes[3], (object) DefaultByteConverter.ToUShort(this._connectionBytes, ref i));
      return index + 6;
    }
  }
}
