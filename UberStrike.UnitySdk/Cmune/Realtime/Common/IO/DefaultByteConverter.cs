// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.IO.DefaultByteConverter
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Realtime.Common.Synchronization;
using Cmune.Util;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Cmune.Realtime.Common.IO
{
  public class DefaultByteConverter : IByteConverter
  {
    protected CmuneDataType _dataTypes = new CmuneDataType();

    public bool IsTypeSupported(System.Type type) => this.GetCmuneDataType(type) != (byte) 0;

    protected bool IsCmuneTypeDefined(byte type) => this._dataTypes.IsDefined(type);

    public virtual byte GetCmuneDataType(System.Type o)
    {
      if ((object) o == (object) typeof (byte))
        return 1;
      if ((object) o == (object) typeof (sbyte))
        return 2;
      if ((object) o == (object) typeof (int))
        return 6;
      if ((object) o == (object) typeof (short))
        return 4;
      if ((object) o == (object) typeof (ushort))
        return 5;
      if ((object) o == (object) typeof (long))
        return 8;
      if ((object) o == (object) typeof (float))
        return 10;
      if ((object) o == (object) typeof (string))
        return 12;
      if ((object) o == (object) typeof (bool))
        return 3;
      if ((object) o == (object) typeof (Vector3))
        return 30;
      if ((object) o == (object) typeof (Quaternion))
        return 31;
      if ((object) o == (object) typeof (Color))
        return 32;
      if ((object) o == (object) typeof (AssetType))
        return 43;
      if ((object) o == (object) typeof (SyncObject))
        return 48;
      if ((object) o == (object) typeof (CommActorInfo))
        return 46;
      if ((object) o == (object) typeof (NetworkPackage))
        return 44;
      if ((object) o == (object) typeof (RoomMetaData))
        return 40;
      if ((object) o == (object) typeof (CmuneTransform))
        return 42;
      if ((object) o == (object) typeof (CmuneRoomID))
        return 45;
      if ((object) o == (object) typeof (ServerLoadData))
        return 47;
      if (o.IsArray)
      {
        if ((object) o.GetElementType() == (object) typeof (byte))
          return 15;
        if ((object) o.GetElementType() == (object) typeof (int))
          return 20;
        if ((object) o.GetElementType() == (object) typeof (short))
          return 18;
        if ((object) o.GetElementType() == (object) typeof (float))
          return 24;
        if ((object) o.GetElementType() == (object) typeof (string))
          return 26;
        if ((object) o.GetElementType() == (object) typeof (SyncObject))
          return 53;
        if ((object) o.GetElementType() == (object) typeof (CmuneRoomID))
          return 51;
        if ((object) o.GetElementType() == (object) typeof (Quaternion))
          return 36;
        return (object) o.GetElementType() == (object) typeof (Vector3) ? (byte) 35 : (byte) 0;
      }
      if (!o.IsGenericType)
        return 0;
      if (typeof (ICollection<byte>).IsAssignableFrom(o))
        return 15;
      if (typeof (ICollection<int>).IsAssignableFrom(o))
        return 20;
      if (typeof (ICollection<short>).IsAssignableFrom(o))
        return 18;
      if (typeof (ICollection<ushort>).IsAssignableFrom(o))
        return 19;
      if (typeof (ICollection<float>).IsAssignableFrom(o))
        return 24;
      if (typeof (ICollection<string>).IsAssignableFrom(o))
        return 26;
      if (typeof (ICollection<SyncObject>).IsAssignableFrom(o))
        return 53;
      if (typeof (ICollection<CmuneRoomID>).IsAssignableFrom(o))
        return 51;
      if (typeof (ICollection<Quaternion>).IsAssignableFrom(o))
        return 36;
      return typeof (ICollection<Vector3>).IsAssignableFrom(o) ? (byte) 35 : (byte) 0;
    }

    public void FromObject(object o, ref List<byte> bytes) => this.FromObject(o, true, ref bytes);

    public bool FromObject(object o, bool encodeType, ref List<byte> bytes)
    {
      if (o == null)
        return false;
      byte cmuneDataType = this.GetCmuneDataType(o.GetType());
      return this.FromObject(o, cmuneDataType, encodeType, ref bytes);
    }

    public bool TrySerializeObject<T>(T o, bool encodeType, ref List<byte> bytes)
    {
      byte cmuneDataType = this.GetCmuneDataType(typeof (T));
      return this.FromObject((object) o, cmuneDataType, encodeType, ref bytes);
    }

    protected virtual bool FromObject(object o, byte type, bool encodeType, ref List<byte> bytes)
    {
      switch (type)
      {
        case 1:
          if (encodeType)
            bytes.Add((byte) 1);
          DefaultByteConverter.FromByte((byte) o, ref bytes);
          break;
        case 2:
          if (encodeType)
            bytes.Add((byte) 2);
          DefaultByteConverter.FromSByte((sbyte) o, ref bytes);
          break;
        case 3:
          if (encodeType)
            bytes.Add((byte) 3);
          DefaultByteConverter.FromBool((bool) o, ref bytes);
          break;
        case 4:
          if (encodeType)
            bytes.Add((byte) 4);
          DefaultByteConverter.FromShort((short) o, ref bytes);
          break;
        case 5:
          if (encodeType)
            bytes.Add((byte) 5);
          DefaultByteConverter.FromUShort((ushort) o, ref bytes);
          break;
        case 6:
          if (encodeType)
            bytes.Add((byte) 6);
          DefaultByteConverter.FromInt((int) o, ref bytes);
          break;
        case 8:
          if (encodeType)
            bytes.Add((byte) 8);
          DefaultByteConverter.FromLong((long) o, ref bytes);
          break;
        case 10:
          if (encodeType)
            bytes.Add((byte) 10);
          DefaultByteConverter.FromFloat((float) o, ref bytes);
          break;
        case 12:
          if (encodeType)
            bytes.Add((byte) 12);
          DefaultByteConverter.FromString((string) o, ref bytes);
          break;
        case 15:
          if (encodeType)
            bytes.Add((byte) 15);
          DefaultByteConverter.FromByteCollection((ICollection<byte>) o, ref bytes);
          break;
        case 18:
          if (encodeType)
            bytes.Add((byte) 18);
          DefaultByteConverter.FromShortCollection((ICollection<short>) o, ref bytes);
          break;
        case 19:
          if (encodeType)
            bytes.Add((byte) 19);
          DefaultByteConverter.FromUShortCollection((ICollection<ushort>) o, ref bytes);
          break;
        case 20:
          if (encodeType)
            bytes.Add((byte) 20);
          DefaultByteConverter.FromIntCollection((ICollection<int>) o, ref bytes);
          break;
        case 24:
          if (encodeType)
            bytes.Add((byte) 24);
          DefaultByteConverter.FromFloatCollection((ICollection<float>) o, ref bytes);
          break;
        case 26:
          if (encodeType)
            bytes.Add((byte) 26);
          DefaultByteConverter.FromCollectionString((ICollection<string>) o, ref bytes);
          break;
        case 30:
          if (encodeType)
            bytes.Add((byte) 30);
          DefaultByteConverter.FromVector3((Vector3) o, ref bytes);
          break;
        case 31:
          if (encodeType)
            bytes.Add((byte) 31);
          DefaultByteConverter.FromQuaternion((Quaternion) o, ref bytes);
          break;
        case 32:
          if (encodeType)
            bytes.Add((byte) 32);
          DefaultByteConverter.FromColor((Color) o, ref bytes);
          break;
        case 35:
          if (encodeType)
            bytes.Add((byte) 35);
          DefaultByteConverter.FromVector3Collection((ICollection<Vector3>) o, ref bytes);
          break;
        case 36:
          if (encodeType)
            bytes.Add((byte) 36);
          DefaultByteConverter.FromQuaternionCollection((ICollection<Quaternion>) o, ref bytes);
          break;
        case 40:
          if (encodeType)
            bytes.Add((byte) 40);
          bytes.AddRange((IEnumerable<byte>) ((RoomMetaData) o).GetBytes());
          break;
        case 42:
          if (encodeType)
            bytes.Add((byte) 42);
          bytes.AddRange((IEnumerable<byte>) ((CmuneTransform) o).GetBytes());
          break;
        case 43:
          if (encodeType)
            bytes.Add((byte) 43);
          DefaultByteConverter.FromByte((byte) (int) o, ref bytes);
          break;
        case 44:
          if (encodeType)
            bytes.Add((byte) 44);
          bytes.AddRange((IEnumerable<byte>) ((NetworkPackage) o).GetBytes());
          break;
        case 45:
          if (encodeType)
            bytes.Add((byte) 45);
          bytes.AddRange((IEnumerable<byte>) ((CmuneRoomID) o).GetBytes());
          break;
        case 46:
          if (encodeType)
            bytes.Add((byte) 46);
          SyncObjectBuilder.GetSyncData((CmuneDeltaSync) o, true).ToBytes(bytes);
          break;
        case 47:
          if (encodeType)
            bytes.Add((byte) 47);
          bytes.AddRange((IEnumerable<byte>) ((ServerLoadData) o).GetBytes());
          break;
        case 48:
          if (encodeType)
            bytes.Add((byte) 48);
          ((SyncObject) o).ToBytes(bytes);
          break;
        case 51:
          if (encodeType)
            bytes.Add((byte) 51);
          this.FromRoomIDCollection((ICollection<CmuneRoomID>) o, ref bytes);
          break;
        case 53:
          if (encodeType)
            bytes.Add((byte) 53);
          this.FromSyncObjectCollection((ICollection<SyncObject>) o, ref bytes);
          break;
        default:
          if (type == (byte) 0)
            CmuneDebug.LogWarning("Not supported Type '{0}' ", (object) o.GetType());
          else
            CmuneDebug.LogError("TYPE '{0}' NOT IMPLEMENTED for '{1}'", (object) type, (object) o.GetType());
          return false;
      }
      return true;
    }

    public bool TryDecodeObject(byte[] bytes, System.Type type, ref int i, out object obj)
    {
      byte cmuneDataType = this.GetCmuneDataType(type);
      return this.TryDecodeObject(bytes, cmuneDataType, ref i, out obj);
    }

    public bool TryDecodeObject(byte[] bytes, byte type, ref int i, out object obj)
    {
      if (type != (byte) 0)
      {
        obj = this.ToObject(bytes, type, ref i);
        return obj != null && i < int.MaxValue;
      }
      i = int.MaxValue;
      obj = (object) null;
      return false;
    }

    public object ToObject(byte[] bytes, ref int i)
    {
      if (i >= 0 && i < bytes.Length)
      {
        if (this.IsCmuneTypeDefined(bytes[i]))
        {
          byte type = bytes[i];
          ++i;
          return this.ToObject(bytes, type, ref i);
        }
        CmuneDebug.LogError("Decode Object failed because found unrecognized datatype {0}", (object) bytes[i]);
        i = int.MaxValue;
        return (object) null;
      }
      CmuneDebug.LogError("Trying to call ToObject but index at {0} of byte[]({1}) ", (object) i, (object) bytes.Length);
      i = int.MaxValue;
      return (object) null;
    }

    public virtual object ToObject(byte[] bytes, byte type, ref int i)
    {
      if (i >= bytes.Length)
        return (object) null;
      switch (type)
      {
        case 0:
          CmuneDebug.LogError("Bad call of ToObject with CmuneType '{0}' at index {1} and byte[] of Length {2}", (object) type, (object) i, (object) bytes.Length);
          i = int.MaxValue;
          return (object) null;
        case 1:
          return (object) DefaultByteConverter.ToByte(bytes, ref i);
        case 2:
          return (object) DefaultByteConverter.ToSByte(bytes, ref i);
        case 3:
          return (object) DefaultByteConverter.ToBool(bytes, ref i);
        case 4:
          return (object) DefaultByteConverter.ToShort(bytes, ref i);
        case 5:
          return (object) DefaultByteConverter.ToUShort(bytes, ref i);
        case 6:
          return (object) DefaultByteConverter.ToInt(bytes, ref i);
        case 8:
          return (object) DefaultByteConverter.ToLong(bytes, ref i);
        case 10:
          return (object) DefaultByteConverter.ToFloat(bytes, ref i);
        case 12:
          return (object) DefaultByteConverter.ToString(bytes, ref i);
        case 15:
          return (object) DefaultByteConverter.ToByteCollection(bytes, ref i);
        case 18:
          return (object) DefaultByteConverter.ToShortCollection(bytes, ref i);
        case 19:
          return (object) DefaultByteConverter.ToUShortCollection(bytes, ref i);
        case 20:
          return (object) DefaultByteConverter.ToIntCollection(bytes, ref i);
        case 24:
          return (object) DefaultByteConverter.ToFloatCollection(bytes, ref i);
        case 26:
          return (object) DefaultByteConverter.ToCollectionString(bytes, ref i);
        case 30:
          return (object) DefaultByteConverter.ToVector3(bytes, ref i);
        case 31:
          return (object) DefaultByteConverter.ToQuaternion(bytes, ref i);
        case 32:
          return (object) DefaultByteConverter.ToColor(bytes, ref i);
        case 35:
          return (object) DefaultByteConverter.ToVector3Collection(bytes, ref i);
        case 36:
          return (object) DefaultByteConverter.ToQuaternionCollection(bytes, ref i);
        case 40:
          return (object) new RoomMetaData(bytes, ref i);
        case 42:
          return (object) new CmuneTransform(bytes, ref i);
        case 43:
          return (object) (AssetType) DefaultByteConverter.ToByte(bytes, ref i);
        case 44:
          return (object) new NetworkPackage(bytes, ref i);
        case 45:
          return (object) new CmuneRoomID(bytes, ref i);
        case 46:
          return (object) new CommActorInfo(SyncObject.FromBytes(bytes, ref i));
        case 47:
          return (object) new ServerLoadData(bytes, ref i);
        case 48:
          return (object) SyncObject.FromBytes(bytes, ref i);
        case 51:
          return (object) this.ToRoomIDCollection(bytes, ref i);
        case 53:
          return (object) this.ToSyncObjectCollection(bytes, ref i);
        default:
          CmuneDebug.LogError("Cmune Type {0} not implemented!", (object) type);
          i = int.MaxValue;
          return (object) null;
      }
    }

    public static void FromVector3Collection(ICollection<Vector3> array, ref List<byte> bytes)
    {
      if (array.Count >= (int) byte.MaxValue)
        throw new CmuneException(string.Format("Trying to encode a ushort collection of length {0}, but only marked as SMALL (up to 255 elements))", (object) array.Count), new object[0]);
      bytes.Add((byte) array.Count);
      foreach (Vector3 v in (IEnumerable<Vector3>) array)
        DefaultByteConverter.FromVector3(v, ref bytes);
    }

    public static List<Vector3> ToVector3Collection(byte[] bytes, ref int i)
    {
      int capacity = (int) bytes[i++];
      List<Vector3> vector3Collection = new List<Vector3>(capacity);
      for (int index = 0; index < capacity; ++index)
        vector3Collection.Add(DefaultByteConverter.ToVector3(bytes, ref i));
      return vector3Collection;
    }

    public static void FromQuaternionCollection(ICollection<Quaternion> array, ref List<byte> bytes)
    {
      if (array.Count >= (int) byte.MaxValue)
        throw new CmuneException(string.Format("Trying to encode a ushort collection of length {0}, but only marked as SMALL (up to 255 elements))", (object) array.Count), new object[0]);
      bytes.Add((byte) array.Count);
      foreach (Quaternion q in (IEnumerable<Quaternion>) array)
        DefaultByteConverter.FromQuaternion(q, ref bytes);
    }

    public static List<Quaternion> ToQuaternionCollection(byte[] bytes, ref int i)
    {
      int capacity = (int) bytes[i++];
      List<Quaternion> quaternionCollection = new List<Quaternion>(capacity);
      for (int index = 0; index < capacity; ++index)
        quaternionCollection.Add(DefaultByteConverter.ToQuaternion(bytes, ref i));
      return quaternionCollection;
    }

    public static void FromUShortCollection(ICollection<ushort> array, ref List<byte> bytes)
    {
      if (array.Count >= (int) byte.MaxValue)
        throw new CmuneException(string.Format("Trying to encode a ushort collection of length {0}, but only marked as SMALL (up to 255 elements))", (object) array.Count), new object[0]);
      bytes.Add((byte) array.Count);
      foreach (ushort f in (IEnumerable<ushort>) array)
        DefaultByteConverter.FromUShort(f, ref bytes);
    }

    public static List<ushort> ToUShortCollection(byte[] bytes, ref int i)
    {
      int capacity = (int) bytes[i++];
      if (capacity >= 0 && i >= 0 && bytes.Length >= i + capacity * 2)
      {
        List<ushort> ushortCollection = new List<ushort>(capacity);
        for (int index = 0; index < capacity; ++index)
          ushortCollection.Add(DefaultByteConverter.ToUShort(bytes, ref i));
        return ushortCollection;
      }
      CmuneDebug.LogError("Trying to decode an short[] of length {0} at index {1} of byte[]({2}) ", (object) capacity, (object) i, (object) bytes.Length);
      i = int.MaxValue;
      return new List<ushort>(0);
    }

    public static void FromShortCollection(ICollection<short> array, ref List<byte> bytes)
    {
      if (array.Count >= (int) byte.MaxValue)
        throw new CmuneException(string.Format("Trying to encode a short collection of length {0}, but only marked as SMALL (up to 255 elements))", (object) array.Count), new object[0]);
      bytes.Add((byte) array.Count);
      foreach (short f in (IEnumerable<short>) array)
        DefaultByteConverter.FromShort(f, ref bytes);
    }

    public static List<short> ToShortCollection(byte[] bytes, ref int i)
    {
      int capacity = (int) bytes[i++];
      if (capacity >= 0 && i >= 0 && bytes.Length >= i + capacity * 2)
      {
        List<short> shortCollection = new List<short>(capacity);
        for (int index = 0; index < capacity; ++index)
          shortCollection.Add(DefaultByteConverter.ToShort(bytes, ref i));
        return shortCollection;
      }
      CmuneDebug.LogError("Trying to decode an short[] of length {0} at index {1} of byte[]({2}) ", (object) capacity, (object) i, (object) bytes.Length);
      i = int.MaxValue;
      return new List<short>(0);
    }

    public static void FromIntCollection(ICollection<int> array, ref List<byte> bytes)
    {
      if (array.Count >= (int) short.MaxValue)
        throw new CmuneException(string.Format("Trying to encode a int collection of length {0}, but only marked as SMALL (up to {1} elements))", (object) array.Count, (object) short.MaxValue), new object[0]);
      DefaultByteConverter.FromShort((short) array.Count, ref bytes);
      foreach (int f in (IEnumerable<int>) array)
        DefaultByteConverter.FromInt(f, ref bytes);
    }

    public static List<int> ToIntCollection(byte[] bytes, ref int i)
    {
      int capacity = (int) DefaultByteConverter.ToShort(bytes, ref i);
      if (capacity >= 0 && i >= 0 && bytes.Length >= i + capacity * 4)
      {
        List<int> intCollection = new List<int>(capacity);
        for (int index = 0; index < capacity; ++index)
          intCollection.Add(DefaultByteConverter.ToInt(bytes, ref i));
        return intCollection;
      }
      CmuneDebug.LogError("Trying to decode an int[] of length {0} at index {1} of byte[]({2}) ", (object) capacity, (object) i, (object) bytes.Length);
      i = int.MaxValue;
      return new List<int>(0);
    }

    public virtual void FromRoomIDCollection(ICollection<CmuneRoomID> array, ref List<byte> bytes)
    {
      if (array.Count >= (int) short.MaxValue)
        throw new CmuneException(string.Format("Trying to encode a CmuneRoomID collection of length {0}, but only marked as SMALL (up to {1} elements))", (object) array.Count, (object) short.MaxValue), new object[0]);
      DefaultByteConverter.FromShort((short) array.Count, ref bytes);
      foreach (CmuneRoomID cmuneRoomId in (IEnumerable<CmuneRoomID>) array)
        bytes.AddRange((IEnumerable<byte>) cmuneRoomId.GetBytes());
    }

    public virtual List<CmuneRoomID> ToRoomIDCollection(byte[] bytes, ref int i)
    {
      int capacity = (int) DefaultByteConverter.ToShort(bytes, ref i);
      if (capacity >= 0 && i >= 0 && bytes.Length >= i + capacity * 12)
      {
        List<CmuneRoomID> roomIdCollection = new List<CmuneRoomID>(capacity);
        for (int index = 0; index < capacity; ++index)
          roomIdCollection.Add(new CmuneRoomID(bytes, ref i));
        return roomIdCollection;
      }
      CmuneDebug.LogError("Trying to decode an CmuneRoomID[] of length {0} at index {1} of byte[]({2}) ", (object) capacity, (object) i, (object) bytes.Length);
      i = int.MaxValue;
      return new List<CmuneRoomID>(0);
    }

    public virtual void FromSyncObjectCollection(
      ICollection<SyncObject> array,
      ref List<byte> bytes)
    {
      if (array.Count >= (int) short.MaxValue)
        throw new CmuneException(string.Format("Trying to encode a ActorInfo collection of length {0}, but only marked as SMALL (up to {1} elements))", (object) array.Count, (object) short.MaxValue), new object[0]);
      DefaultByteConverter.FromShort((short) array.Count, ref bytes);
      foreach (SyncObject syncObject in (IEnumerable<SyncObject>) array)
        syncObject.ToBytes(bytes);
    }

    public virtual List<SyncObject> ToSyncObjectCollection(byte[] bytes, ref int i)
    {
      int capacity = (int) DefaultByteConverter.ToShort(bytes, ref i);
      if (capacity >= 0 && bytes.Length >= i + capacity * 4)
      {
        List<SyncObject> objectCollection = new List<SyncObject>(capacity);
        for (int index = 0; index < capacity; ++index)
          objectCollection.Add(SyncObject.FromBytes(bytes, ref i));
        return objectCollection;
      }
      CmuneDebug.LogError("Trying to decode an SyncObject[] of length {0} at index {1} of byte[]({2}) ", (object) capacity, (object) i, (object) bytes.Length);
      i = int.MaxValue;
      return new List<SyncObject>(0);
    }

    public static void FromFloatCollection(ICollection<float> array, ref List<byte> bytes)
    {
      if (array.Count >= (int) byte.MaxValue)
        throw new CmuneException(string.Format("Trying to encode a float collection of length {0}, but only marked as SMALL (up to {1} elements))", (object) array.Count, (object) byte.MaxValue), new object[0]);
      bytes.Add((byte) array.Count);
      foreach (float f in (IEnumerable<float>) array)
        DefaultByteConverter.FromFloat(f, ref bytes);
    }

    public static List<float> ToFloatCollection(byte[] bytes, ref int i)
    {
      int capacity = (int) bytes[i++];
      if (capacity >= 0 && i >= 0 && bytes.Length >= i + capacity * 4)
      {
        List<float> floatCollection = new List<float>(capacity);
        for (int index = 0; index < capacity; ++index)
          floatCollection.Add(DefaultByteConverter.ToFloat(bytes, ref i));
        return floatCollection;
      }
      CmuneDebug.LogError("Trying to decode an float[] of length {0} at index {1} of byte[]({2}) ", (object) capacity, (object) i, (object) bytes.Length);
      i = int.MaxValue;
      return new List<float>(0);
    }

    public static void FromByteCollection(ICollection<byte> array, ref List<byte> bytes)
    {
      if (array.Count < int.MaxValue)
      {
        DefaultByteConverter.FromInt(array.Count, ref bytes);
        bytes.AddRange((IEnumerable<byte>) array);
      }
      else
        throw new CmuneException("Trying to encode a byte collection of length {0}, but only marked as SMALL (up to {1} elements))", new object[2]
        {
          (object) array.Count,
          (object) int.MaxValue
        });
    }

    public static List<byte> ToByteCollection(byte[] bytes, ref int i)
    {
      int capacity = DefaultByteConverter.ToInt(bytes, ref i);
      if (capacity >= 0 && i >= 0 && bytes.Length >= i + capacity)
      {
        List<byte> byteCollection = new List<byte>(capacity);
        for (int index = 0; index < capacity; ++index)
          byteCollection.Add(bytes[i + index]);
        i += capacity;
        return byteCollection;
      }
      CmuneDebug.LogError("Trying to decode an byte[] of length {0} at index {1} of byte[]({2}) ", (object) capacity, (object) i, (object) bytes.Length);
      i = int.MaxValue;
      return new List<byte>(0);
    }

    public static void FromCollectionString(ICollection<string> array, ref List<byte> bytes)
    {
      if (array.Count >= (int) byte.MaxValue)
        throw new CmuneException(string.Format("Trying to encode a string collection of length {0}, but only marked as SMALL (up to 255 elements))", (object) array.Count), new object[0]);
      bytes.Add((byte) array.Count);
      foreach (string s in (IEnumerable<string>) array)
        DefaultByteConverter.FromString(s, ref bytes);
    }

    public static List<string> ToCollectionString(byte[] bytes, ref int i)
    {
      int capacity = (int) bytes[i++];
      if (capacity >= 0 && i >= 0 && bytes.Length >= i + capacity)
      {
        List<string> collectionString = new List<string>(capacity);
        for (int index = 0; index < capacity; ++index)
          collectionString.Add(DefaultByteConverter.ToString(bytes, ref i));
        return collectionString;
      }
      CmuneDebug.LogError("Trying to decode an string[] of length {0} at index {1} of byte[]({2}) ", (object) capacity, (object) i, (object) bytes.Length);
      i = int.MaxValue;
      return new List<string>(0);
    }

    public static void FromLong(long l, ref List<byte> bytes) => bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.CheckEndian(BitConverter.GetBytes(l)));

    public static long ToLong(byte[] bytes, ref int i)
    {
      if (i + 8 <= bytes.Length)
      {
        int num = i;
        i += 8;
        return BitConverter.ToInt64(DefaultByteConverter.CheckEndian(bytes, num, 8), num);
      }
      i = int.MaxValue;
      return 0;
    }

    public static void FromFloat(float f, ref List<byte> bytes) => bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.CheckEndian(BitConverter.GetBytes(f)));

    public static float ToFloat(byte[] bytes, ref int i)
    {
      if (i + 4 <= bytes.Length)
      {
        int num = i;
        i += 4;
        return BitConverter.ToSingle(DefaultByteConverter.CheckEndian(bytes, num, 4), num);
      }
      i = int.MaxValue;
      return 0.0f;
    }

    public static byte[] FromShort(short f) => DefaultByteConverter.CheckEndian(BitConverter.GetBytes(f));

    public static void FromShort(short f, ref List<byte> bytes) => bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.CheckEndian(BitConverter.GetBytes(f)));

    public static short ToShort(byte[] bytes, ref int i)
    {
      int num = i;
      i += 2;
      if (num >= 0 && num + 2 <= bytes.Length)
        return BitConverter.ToInt16(DefaultByteConverter.CheckEndian(bytes, num, 2), num);
      i = int.MaxValue;
      return 0;
    }

    public static void FromUShort(ushort f, ref List<byte> bytes) => bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.CheckEndian(BitConverter.GetBytes(f)));

    public static ushort ToUShort(byte[] bytes, ref int i)
    {
      int num = i;
      i += 2;
      if (num >= 0 && num + 2 <= bytes.Length)
        return BitConverter.ToUInt16(DefaultByteConverter.CheckEndian(bytes, num, 2), num);
      i = int.MaxValue;
      return 0;
    }

    public static void FromInt(int f, ref List<byte> bytes) => bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.CheckEndian(BitConverter.GetBytes(f)));

    public static byte[] FromInt(int f) => DefaultByteConverter.CheckEndian(BitConverter.GetBytes(f));

    public static int ToInt(byte[] bytes, ref int i)
    {
      int num = i;
      i += 4;
      if (num >= 0 && num + 4 <= bytes.Length)
        return BitConverter.ToInt32(DefaultByteConverter.CheckEndian(bytes, num, 4), num);
      i = int.MaxValue;
      return 0;
    }

    public static void FromBool(bool b, ref List<byte> bytes) => bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.CheckEndian(BitConverter.GetBytes(b)));

    public static bool ToBool(byte[] bytes, ref int i)
    {
      int num = i;
      ++i;
      return BitConverter.ToBoolean(DefaultByteConverter.CheckEndian(bytes, num, 1), num);
    }

    public static void FromSByte(sbyte sb, ref List<byte> bytes) => bytes.Add((byte) ((uint) sb + (uint) sbyte.MaxValue));

    public static sbyte ToSByte(byte[] bytes, ref int i) => (sbyte) ((int) bytes[i++] - (int) sbyte.MaxValue);

    public static void FromByte(byte b, ref List<byte> bytes) => bytes.Add(b);

    public static byte ToByte(byte[] bytes, ref int i) => bytes[i++];

    public static void FromChar(char c, ref List<byte> bytes) => bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.CheckEndian(BitConverter.GetBytes(c)));

    public static char ToChar(byte[] bytes, ref int i)
    {
      int num = i;
      i += 2;
      return BitConverter.ToChar(DefaultByteConverter.CheckEndian(bytes, num, 2), num);
    }

    public static void FromString(string s, ref List<byte> bytes) => DefaultByteConverter.FromString(s, ref bytes, false);

    public static void FromString(string s, ref List<byte> bytes, bool small)
    {
      if (string.IsNullOrEmpty(s))
      {
        if (small)
          bytes.Add((byte) 0);
        else
          bytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes((short) 0));
      }
      else
      {
        int num = small ? (int) sbyte.MaxValue : 16383;
        if (s.Length > num)
          s = s.Substring(0, s.Length - num);
        if (small)
          bytes.Add((byte) s.Length);
        else
          bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.CheckEndian(BitConverter.GetBytes((short) s.Length)));
        bytes.AddRange((IEnumerable<byte>) Encoding.Unicode.GetBytes(s));
      }
    }

    public static string ToString(byte[] bytes, ref int i) => DefaultByteConverter.ToString(bytes, ref i, false);

    public static string ToString(byte[] bytes, ref int i, bool small)
    {
      string empty = string.Empty;
      int num = -1;
      if (small)
      {
        if (i >= 0 && bytes.Length >= i + 1)
        {
          num = (int) bytes[i];
          ++i;
        }
      }
      else if (i >= 0 && bytes.Length >= i + 2)
      {
        num = (int) BitConverter.ToInt16(DefaultByteConverter.CheckEndian(bytes, i, 2), i);
        i += 2;
      }
      if (num >= 0 && bytes.Length >= i + num * 2)
      {
        if (num > 0)
        {
          empty = Encoding.Unicode.GetString(bytes, i, num * 2);
          i += num * 2;
        }
      }
      else
      {
        if (CmuneDebug.IsWarningEnabled)
          CmuneDebug.LogWarning("Error in ToString! Array size {0} not long enough for {1} bytes, starting from {2}", (object) bytes.Length, (object) (num * 2), (object) i);
        i = int.MaxValue;
      }
      return empty;
    }

    public static void FromQuaternion(Quaternion q, ref List<byte> bytes)
    {
      bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.CheckEndian(BitConverter.GetBytes(q[0])));
      bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.CheckEndian(BitConverter.GetBytes(q[1])));
      bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.CheckEndian(BitConverter.GetBytes(q[2])));
      bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.CheckEndian(BitConverter.GetBytes(q[3])));
    }

    public static Quaternion ToQuaternion(byte[] bytes, ref int i)
    {
      Quaternion quaternion = new Quaternion(BitConverter.ToSingle(DefaultByteConverter.CheckEndian(bytes, i, 4), i), BitConverter.ToSingle(DefaultByteConverter.CheckEndian(bytes, i + 4, 4), i + 4), BitConverter.ToSingle(DefaultByteConverter.CheckEndian(bytes, i + 8, 4), i + 8), BitConverter.ToSingle(DefaultByteConverter.CheckEndian(bytes, i + 12, 4), i + 12));
      i += 16;
      return quaternion;
    }

    public static void FromVector3(Vector3 v, ref List<byte> bytes)
    {
      bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.CheckEndian(BitConverter.GetBytes(v[0])));
      bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.CheckEndian(BitConverter.GetBytes(v[1])));
      bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.CheckEndian(BitConverter.GetBytes(v[2])));
    }

    public static Vector3 ToVector3(byte[] bytes, ref int i)
    {
      Vector3 vector3 = new Vector3(BitConverter.ToSingle(DefaultByteConverter.CheckEndian(bytes, i, 4), i), BitConverter.ToSingle(DefaultByteConverter.CheckEndian(bytes, i + 4, 4), i + 4), BitConverter.ToSingle(DefaultByteConverter.CheckEndian(bytes, i + 8, 4), i + 8));
      i += 12;
      return vector3;
    }

    public static void FromColor(Color v, ref List<byte> bytes)
    {
      bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.CheckEndian(BitConverter.GetBytes(v[0])));
      bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.CheckEndian(BitConverter.GetBytes(v[1])));
      bytes.AddRange((IEnumerable<byte>) DefaultByteConverter.CheckEndian(BitConverter.GetBytes(v[2])));
    }

    public static Color ToColor(byte[] bytes, ref int i)
    {
      Color color = new Color(BitConverter.ToSingle(DefaultByteConverter.CheckEndian(bytes, i, 4), i), BitConverter.ToSingle(DefaultByteConverter.CheckEndian(bytes, i + 4, 4), i + 4), BitConverter.ToSingle(DefaultByteConverter.CheckEndian(bytes, i + 8, 4), i + 8));
      i += 12;
      return color;
    }

    private static byte[] CheckEndian(byte[] bytes, int start = 0, int length = -1) => BitConverter.IsLittleEndian ? bytes : DefaultByteConverter.ReverseArray(bytes, start, length);

    public static byte[] ReverseArray(byte[] bytes, int start, int length)
    {
      if (length < 0 || length > 1)
      {
        start = Mathf.Clamp(start, 0, bytes.Length);
        length = length > 0 ? Mathf.Clamp(length, 0, bytes.Length - start) : bytes.Length - start;
        int num1 = start + length;
        int num2 = length >> 1;
        for (int index = 0; index < num2; ++index)
        {
          byte num3 = bytes[start + index];
          bytes[start + index] = bytes[num1 - index - 1];
          bytes[num1 - index - 1] = num3;
        }
      }
      return bytes;
    }

    public static byte ReverseByte(byte inByte)
    {
      byte num = 0;
      for (byte index = 128; Convert.ToInt32(index) > 0; index >>= 1)
      {
        num >>= 1;
        if ((byte) ((uint) inByte & (uint) index) != (byte) 0)
          num |= (byte) 128;
      }
      return num;
    }
  }
}
