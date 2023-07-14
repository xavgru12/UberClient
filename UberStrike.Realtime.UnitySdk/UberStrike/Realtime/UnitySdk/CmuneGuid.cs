// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.CmuneGuid
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;

namespace UberStrike.Realtime.UnitySdk
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
