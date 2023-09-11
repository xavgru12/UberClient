// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.CmuneGuid
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;

namespace Cmune.Realtime.Common
{
  public class CmuneGuid : IByteArray
  {
    private Guid _guid;
    private string _stringID;

    public CmuneGuid()
    {
      this._guid = new Guid();
      this._stringID = this._guid.ToString();
    }

    public CmuneGuid(string id)
    {
      this._guid = new Guid(id);
      this._stringID = this._guid.ToString();
    }

    public string ID
    {
      get => this._stringID;
      private set => this._stringID = value;
    }

    public byte[] GetBytes() => this._guid.ToByteArray();

    public int FromBytes(byte[] bytes, int idx)
    {
      byte[] numArray = new byte[16];
      Array.Copy((Array) bytes, idx, (Array) numArray, 0, 16);
      this._guid = new Guid(numArray);
      this._stringID = this._guid.ToString();
      idx += 16;
      return idx;
    }
  }
}
