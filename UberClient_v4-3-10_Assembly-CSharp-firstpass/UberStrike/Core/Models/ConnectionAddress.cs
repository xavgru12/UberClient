// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.ConnectionAddress
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace UberStrike.Core.Models
{
  [Serializable]
  public class ConnectionAddress
  {
    public int Ipv4 { get; set; }

    public ushort Port { get; set; }

    public string ConnectionString => string.Format("{0}:{1}", (object) ConnectionAddress.ToString(this.Ipv4), (object) this.Port);

    public string IpAddress => ConnectionAddress.ToString(this.Ipv4);

    public ConnectionAddress()
    {
    }

    public ConnectionAddress(string connection)
    {
      try
      {
        string[] strArray = connection.Split(':');
        this.Ipv4 = ConnectionAddress.ToInteger(strArray[0]);
        this.Port = ushort.Parse(strArray[1]);
      }
      catch
      {
      }
    }

    public ConnectionAddress(string ipAddress, ushort port)
    {
      this.Ipv4 = ConnectionAddress.ToInteger(ipAddress);
      this.Port = port;
    }

    public static string ToString(int ipv4) => string.Format("{0}.{1}.{2}.{3}", (object) (ipv4 >> 24 & (int) byte.MaxValue), (object) (ipv4 >> 16 & (int) byte.MaxValue), (object) (ipv4 >> 8 & (int) byte.MaxValue), (object) (ipv4 & (int) byte.MaxValue));

    public static int ToInteger(string ipAddress)
    {
      int integer = 0;
      string[] strArray = ipAddress.Split('.');
      if (strArray.Length == 4)
      {
        for (int index = 0; index < strArray.Length; ++index)
          integer |= int.Parse(strArray[index]) << (3 - index) * 8;
      }
      return integer;
    }
  }
}
