// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Common.IO.UberStrikeByteConverter
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Realtime.Common;
using Cmune.Realtime.Common.IO;
using Cmune.Realtime.Common.Synchronization;
using Cmune.Util;
using System;
using System.Collections.Generic;

namespace UberStrike.Realtime.Common.IO
{
  public class UberStrikeByteConverter : DefaultByteConverter
  {
    public UberStrikeByteConverter() => this._dataTypes = (CmuneDataType) new UberStrikeDataType();

    public override byte GetCmuneDataType(Type type)
    {
      if ((object) type == (object) typeof (CharacterInfo))
        return 101;
      if ((object) type == (object) typeof (ShortVector3))
        return 102;
      if ((object) type == (object) typeof (DamageEvent))
        return 107;
      if ((object) type == (object) typeof (GameMetaData))
        return 103;
      if ((object) type == (object) typeof (ArmorInfo))
        return 104;
      if ((object) type == (object) typeof (StatsInfo))
        return 108;
      if ((object) type == (object) typeof (EndOfMatchData))
        return 109;
      if ((object) type == (object) typeof (WeaponInfo))
        return 105;
      if ((object) type == (object) typeof (StatsCollection))
        return 106;
      return typeof (ICollection<RoomMetaData>).IsAssignableFrom(type) ? (byte) 111 : base.GetCmuneDataType(type);
    }

    protected override bool FromObject(object o, byte type, bool encodeType, ref List<byte> bytes)
    {
      switch (type)
      {
        case 101:
          if (encodeType)
            bytes.Add((byte) 101);
          SyncObjectBuilder.GetSyncData((CmuneDeltaSync) o, true).ToBytes(bytes);
          break;
        case 102:
          if (encodeType)
            bytes.Add((byte) 102);
          bytes.AddRange((IEnumerable<byte>) ((ShortVector3) o).GetBytes());
          break;
        case 103:
          if (encodeType)
            bytes.Add((byte) 103);
          bytes.AddRange((IEnumerable<byte>) ((RoomMetaData) o).GetBytes());
          break;
        case 104:
          if (encodeType)
            bytes.Add((byte) 104);
          bytes.AddRange((IEnumerable<byte>) ((ArmorInfo) o).GetBytes());
          break;
        case 105:
          if (encodeType)
            bytes.Add((byte) 105);
          bytes.AddRange((IEnumerable<byte>) ((WeaponInfo) o).GetBytes());
          break;
        case 106:
          if (encodeType)
            bytes.Add((byte) 106);
          bytes.AddRange((IEnumerable<byte>) ((StatsCollection) o).GetBytes());
          break;
        case 107:
          if (encodeType)
            bytes.Add((byte) 107);
          bytes.AddRange((IEnumerable<byte>) ((DamageEvent) o).GetBytes());
          break;
        case 108:
          if (encodeType)
            bytes.Add((byte) 108);
          bytes.AddRange((IEnumerable<byte>) ((StatsInfo) o).GetBytes());
          break;
        case 109:
          if (encodeType)
            bytes.Add((byte) 109);
          bytes.AddRange((IEnumerable<byte>) ((EndOfMatchData) o).GetBytes());
          break;
        case 111:
          if (encodeType)
            bytes.Add((byte) 111);
          this.FromRoomDataCollection((ICollection<RoomMetaData>) o, ref bytes);
          break;
        default:
          return base.FromObject(o, type, encodeType, ref bytes);
      }
      return true;
    }

    public override object ToObject(byte[] bytes, byte type, ref int i)
    {
      object obj = (object) null;
      if (i < bytes.Length)
      {
        switch (type)
        {
          case 101:
            obj = (object) new CharacterInfo(SyncObject.FromBytes(bytes, ref i));
            break;
          case 102:
            obj = (object) new ShortVector3(bytes, ref i);
            break;
          case 103:
            obj = (object) new GameMetaData(bytes, ref i);
            break;
          case 104:
            obj = (object) new ArmorInfo(bytes, ref i);
            break;
          case 105:
            obj = (object) new WeaponInfo(bytes, ref i);
            break;
          case 106:
            obj = (object) new StatsCollection(bytes, ref i);
            break;
          case 107:
            obj = (object) DamageEvent.FromBytes(bytes, ref i);
            break;
          case 108:
            obj = (object) new StatsInfo(bytes, ref i);
            break;
          case 109:
            obj = (object) new EndOfMatchData(bytes, ref i);
            break;
          case 111:
            obj = (object) this.ToRoomDataCollection(bytes, ref i);
            break;
          default:
            obj = base.ToObject(bytes, type, ref i);
            break;
        }
      }
      return obj;
    }

    public void FromRoomDataCollection(ICollection<RoomMetaData> array, ref List<byte> bytes)
    {
      if (array.Count >= (int) short.MaxValue)
        throw new CmuneException(string.Format("Trying to encode a RoomMetaData collection of length {0}, but only marked as SMALL (up to {1} elements))", (object) array.Count, (object) short.MaxValue), new object[0]);
      DefaultByteConverter.FromShort((short) array.Count, ref bytes);
      foreach (RoomMetaData roomMetaData in (IEnumerable<RoomMetaData>) array)
      {
        bytes.Add(this.GetCmuneDataType(roomMetaData.GetType()));
        bytes.AddRange((IEnumerable<byte>) roomMetaData.GetBytes());
      }
    }

    public List<RoomMetaData> ToRoomDataCollection(byte[] bytes, ref int i)
    {
      int capacity = (int) DefaultByteConverter.ToShort(bytes, ref i);
      List<RoomMetaData> roomDataCollection = new List<RoomMetaData>(capacity);
      for (int index = 0; index < capacity; ++index)
      {
        switch (bytes[i++])
        {
          case 40:
            roomDataCollection.Add(new RoomMetaData(bytes, ref i));
            break;
          case 103:
            roomDataCollection.Add((RoomMetaData) new GameMetaData(bytes, ref i));
            break;
        }
      }
      return roomDataCollection;
    }
  }
}
