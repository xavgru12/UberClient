// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.SyncObject
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
{
  public class SyncObject
  {
    public readonly Dictionary<int, object> Data;

    private SyncObject()
    {
      this.Id = 0;
      this.DeltaCode = 0;
      this.Data = new Dictionary<int, object>(1);
    }

    public SyncObject(int id, Dictionary<int, object> data)
    {
      this.Id = id;
      this.DeltaCode = 0;
      this.Data = data;
      foreach (KeyValuePair<int, object> keyValuePair in data)
      {
        if (keyValuePair.Value != null)
          this.DeltaCode |= keyValuePair.Key;
      }
    }

    public byte[] ToBytes()
    {
      List<byte> bytes = new List<byte>();
      this.ToBytes(bytes);
      return bytes.ToArray();
    }

    public void ToBytes(List<byte> bytes)
    {
      bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.FromInt(this.DeltaCode));
      if (this.DeltaCode == 0)
        return;
      bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.FromInt(this.Id));
      for (int index = 0; index <= 32; ++index)
      {
        object obj;
        if (this.Data.TryGetValue(1 << index, out obj))
          RealtimeSerialization.ToBytes(true, ref bytes, obj);
      }
    }

    public static SyncObject FromBytes(byte[] bytes, ref int idx)
    {
      SyncObject syncObject = new SyncObject();
      syncObject.DeltaCode = DefaultByteConverter.ToInt(bytes, ref idx);
      if (syncObject.DeltaCode != 0)
      {
        syncObject.Id = DefaultByteConverter.ToInt(bytes, ref idx);
        for (int index = 0; index <= 32 && idx < bytes.Length && idx < int.MaxValue; ++index)
        {
          int key = 1 << index;
          if ((syncObject.DeltaCode & key) != 0)
          {
            byte type = bytes[idx++];
            object obj;
            if (RealtimeSerialization.TryDecodeObject(bytes, ref idx, type, out obj))
              syncObject.Data[key] = obj;
            else
              CmuneDebug.LogError("Error deserializing Field with Bit Tag '{0}' and of CmuneDataType {1}", (object) index, (object) type);
          }
        }
      }
      return syncObject;
    }

    public bool Contains(int deltaCode) => (this.DeltaCode & deltaCode) != 0;

    public override int GetHashCode() => this.Id ^ this.DeltaCode;

    public override bool Equals(object obj) => !object.ReferenceEquals(obj, (object) null) && obj is SyncObject syncObject && this.DeltaCode == syncObject.DeltaCode && (this.DeltaCode == 0 || this.Id == syncObject.Id) && Comparison.IsEqual((object) this.Data.Values, (object) syncObject.Data.Values);

    public int DeltaCode { get; set; }

    public bool IsEmpty => this.DeltaCode == 0;

    public int Id { get; private set; }
  }
}
