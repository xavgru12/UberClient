// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.CmuneTransform
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Realtime.Common.IO;
using System.Collections.Generic;
using UnityEngine;

namespace Cmune.Realtime.Common
{
  public class CmuneTransform : IByteArray
  {
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
    public Vector3 BoundingBox;

    public CmuneTransform()
    {
      this.Position = Vector3.zero;
      this.Rotation = Quaternion.identity;
      this.Scale = Vector3.one;
      this.BoundingBox = Vector3.one;
    }

    public CmuneTransform(Vector3 p)
      : this()
    {
      this.Position = p;
    }

    public CmuneTransform(Vector3 p, Quaternion r, Vector3 s)
      : this()
    {
      this.Position = p;
      this.Rotation = r;
      this.Scale = s;
    }

    public CmuneTransform(byte[] bytes, ref int idx) => idx = this.FromBytes(bytes, idx);

    public byte[] GetBytes()
    {
      List<byte> bytes = new List<byte>(52);
      DefaultByteConverter.FromVector3(this.Position, ref bytes);
      DefaultByteConverter.FromQuaternion(this.Rotation, ref bytes);
      DefaultByteConverter.FromVector3(this.Scale, ref bytes);
      DefaultByteConverter.FromVector3(this.BoundingBox, ref bytes);
      return bytes.ToArray();
    }

    public int FromBytes(byte[] bytes, int idx)
    {
      this.Position = DefaultByteConverter.ToVector3(bytes, ref idx);
      this.Rotation = DefaultByteConverter.ToQuaternion(bytes, ref idx);
      this.Scale = DefaultByteConverter.ToVector3(bytes, ref idx);
      this.BoundingBox = DefaultByteConverter.ToVector3(bytes, ref idx);
      return idx;
    }

    public override bool Equals(object obj)
    {
      if (!(obj is CmuneTransform))
        return false;
      CmuneTransform cmuneTransform = obj as CmuneTransform;
      return this.BoundingBox.Equals((object) cmuneTransform.BoundingBox) && this.Position.Equals((object) cmuneTransform.Position) && this.Rotation.Equals((object) cmuneTransform.Rotation) && this.Scale.Equals((object) cmuneTransform.Scale);
    }

    public override int GetHashCode() => base.GetHashCode();
  }
}
