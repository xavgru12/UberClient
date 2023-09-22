
using Cmune.Realtime.Common.IO;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Cmune.Realtime.Common
{
  public class NetworkPackage : IByteArray
  {
    public short netID = -1;
    public int playerID = -1;
    public NetworkPackage.PackageState state;
    public Vector3 position = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;
    public Vector3 velocity = Vector3.zero;
    public Vector3 angularVelocity = Vector3.zero;
    public short timeStamp;

    public NetworkPackage(byte[] bytes, ref int idx) => idx = this.FromBytes(bytes, idx);

    public NetworkPackage(short netID, Vector3 pos, Quaternion rot)
    {
      this.state = NetworkPackage.PackageState.PAO;
      this.netID = netID;
      this.position = pos;
      this.rotation = rot;
    }

    public NetworkPackage(
      short netID,
      Vector3 pos,
      Quaternion rot,
      short time,
      int playerID,
      Vector3 vel,
      Vector3 avel)
      : this(netID, pos, rot)
    {
      this.state = NetworkPackage.PackageState.DCO;
      this.playerID = playerID;
      this.velocity = vel;
      this.angularVelocity = avel;
      this.timeStamp = time;
    }

    public virtual byte[] GetBytes()
    {
      List<byte> bytes = new List<byte>();
      DefaultByteConverter.FromShort(this.netID, ref bytes);
      DefaultByteConverter.FromByte((byte) this.state, ref bytes);
      DefaultByteConverter.FromVector3(this.position, ref bytes);
      DefaultByteConverter.FromQuaternion(this.rotation, ref bytes);
      if (this.state != NetworkPackage.PackageState.PAO)
      {
        DefaultByteConverter.FromShort(this.timeStamp, ref bytes);
        DefaultByteConverter.FromInt(this.playerID, ref bytes);
        DefaultByteConverter.FromVector3(this.velocity, ref bytes);
        DefaultByteConverter.FromVector3(this.angularVelocity, ref bytes);
      }
      return bytes.ToArray();
    }

    public virtual int FromBytes(byte[] bytes, int idx)
    {
      this.netID = DefaultByteConverter.ToShort(bytes, ref idx);
      this.state = (NetworkPackage.PackageState) DefaultByteConverter.ToByte(bytes, ref idx);
      this.position = DefaultByteConverter.ToVector3(bytes, ref idx);
      this.rotation = DefaultByteConverter.ToQuaternion(bytes, ref idx);
      if (this.state != NetworkPackage.PackageState.PAO)
      {
        this.timeStamp = DefaultByteConverter.ToShort(bytes, ref idx);
        this.playerID = DefaultByteConverter.ToInt(bytes, ref idx);
        this.velocity = DefaultByteConverter.ToVector3(bytes, ref idx);
        this.angularVelocity = DefaultByteConverter.ToVector3(bytes, ref idx);
      }
      return idx;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder("NetPackage\n");
      stringBuilder.AppendLine(this.state.ToString());
      stringBuilder.AppendLine(this.position.ToString());
      stringBuilder.AppendLine(this.rotation.ToString());
      stringBuilder.AppendLine(this.velocity.ToString());
      stringBuilder.AppendLine(this.angularVelocity.ToString());
      return stringBuilder.ToString();
    }

    public enum PackageState
    {
      PAO,
      DAO,
      DCO,
    }
  }
}
