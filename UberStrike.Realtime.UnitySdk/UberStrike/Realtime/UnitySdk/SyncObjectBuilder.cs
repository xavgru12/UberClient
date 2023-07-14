// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.SyncObjectBuilder
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;
using System.Collections.Generic;
using System.Reflection;

namespace UberStrike.Realtime.UnitySdk
{
  public static class SyncObjectBuilder
  {
    private static Dictionary<Type, Dictionary<int, FieldInfo>> _indexFields = new Dictionary<Type, Dictionary<int, FieldInfo>>(5);

    public static void ReadSyncData(
      SyncObject syncData,
      bool updateCache,
      int mask,
      CmuneDeltaSync obj)
    {
      Dictionary<int, FieldInfo> fieldInfoLookup = SyncObjectBuilder.GetFieldInfoLookup(obj.GetType());
      obj.InstanceId = syncData.Id;
      foreach (KeyValuePair<int, object> keyValuePair in syncData.Data)
      {
        FieldInfo fieldInfo;
        if ((keyValuePair.Key & mask) != 0 && fieldInfoLookup.TryGetValue(keyValuePair.Key, out fieldInfo))
        {
          try
          {
            fieldInfo.SetValue((object) obj, keyValuePair.Value);
            if (updateCache)
              obj.Cache[keyValuePair.Key] = CmunePrint.GetHashCode(keyValuePair.Value);
          }
          catch (Exception ex)
          {
            throw CmuneDebug.Exception("Error in ReadSyncData at key {0} with {1}", (object) keyValuePair.Key, (object) ex.Message);
          }
        }
      }
      if (syncData.IsEmpty)
        return;
      ++obj.VersionID;
    }

    public static List<SyncObject> GetSyncData<T>(IEnumerable<T> list, bool full) where T : CmuneDeltaSync
    {
      List<SyncObject> syncData = new List<SyncObject>();
      foreach (T obj in list)
        syncData.Add(SyncObjectBuilder.GetSyncData((CmuneDeltaSync) obj, full));
      return syncData;
    }

    public static SyncObject GetSyncData(CmuneDeltaSync obj, bool full)
    {
      Dictionary<int, FieldInfo> fieldInfoLookup = SyncObjectBuilder.GetFieldInfoLookup(obj.GetType());
      Dictionary<int, object> data = new Dictionary<int, object>(full ? 32 : 1);
      foreach (KeyValuePair<int, FieldInfo> keyValuePair in fieldInfoLookup)
      {
        object obj1 = keyValuePair.Value.GetValue((object) obj);
        int num = 0;
        int hashCode = CmunePrint.GetHashCode(obj1);
        if (full || !obj.Cache.TryGetValue(keyValuePair.Key, out num) || num != hashCode)
        {
          data[keyValuePair.Key] = obj1;
          obj.Cache[keyValuePair.Key] = hashCode;
        }
      }
      return new SyncObject(obj.InstanceId, data);
    }

    private static Dictionary<int, FieldInfo> CreateFieldInfoLookup(Type type)
    {
      List<MemberInfoField<CMUNESYNC>> fieldsWithAttribute = ReflectionHelper.GetFieldsWithAttribute<CMUNESYNC>(type, true);
      CmuneDebug.Assert(fieldsWithAttribute.Count < 32, "CmuneDeltaSync has more than 31 synchronizable fields!");
      fieldsWithAttribute.Sort((Comparison<MemberInfoField<CMUNESYNC>>) ((p, q) => p.Attribute.TagId.CompareTo(q.Attribute.TagId)));
      Dictionary<int, FieldInfo> fieldInfoLookup = new Dictionary<int, FieldInfo>(32);
      Dictionary<int, CMUNESYNC> dictionary = new Dictionary<int, CMUNESYNC>(32);
      for (int index = 0; index < fieldsWithAttribute.Count; ++index)
      {
        MemberInfoField<CMUNESYNC> memberInfoField = fieldsWithAttribute[index];
        if (RealtimeSerialization.IsTypeSupported(memberInfoField.Field.FieldType))
        {
          if (memberInfoField.Attribute.IsTagged)
          {
            try
            {
              fieldInfoLookup.Add(memberInfoField.Attribute.TagId, memberInfoField.Field);
              dictionary.Add(memberInfoField.Attribute.TagId, memberInfoField.Attribute);
            }
            catch
            {
              throw CmuneDebug.Exception("Sync table Exception at field ({0}) because ID {1} is already in use", (object) memberInfoField.Field.Name, (object) memberInfoField.Attribute.TagId);
            }
          }
        }
        else
          throw CmuneDebug.Exception("CmuneDeltaSync can't sync field ({0}) of type {1}", (object) memberInfoField.Field.Name, (object) memberInfoField.Field.FieldType);
      }
      return fieldInfoLookup;
    }

    private static Dictionary<int, FieldInfo> GetFieldInfoLookup(Type type)
    {
      Dictionary<int, FieldInfo> fieldInfoLookup;
      if (!SyncObjectBuilder._indexFields.TryGetValue(type, out fieldInfoLookup))
      {
        fieldInfoLookup = SyncObjectBuilder.CreateFieldInfoLookup(type);
        SyncObjectBuilder._indexFields[type] = fieldInfoLookup;
      }
      return fieldInfoLookup;
    }
  }
}
