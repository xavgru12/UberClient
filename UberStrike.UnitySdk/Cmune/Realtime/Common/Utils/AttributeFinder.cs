
using Cmune.Core.Types;
using Cmune.Util;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cmune.Realtime.Common.Utils
{
  public static class AttributeFinder
  {
    public static void FindNetworkMethods(
      Type type,
      ref Dictionary<string, byte> _lookupNameIndex,
      ref Dictionary<byte, MethodInfo> _lookupIndexMethod)
    {
      AttributeFinder.FindNetworkMethods<NetworkMethodAttribute>(type, ref _lookupNameIndex, ref _lookupIndexMethod);
    }

    public static void FindNetworkMethods<T>(
      Type type,
      ref Dictionary<string, byte> _lookupNameIndex,
      ref Dictionary<byte, MethodInfo> _lookupIndexMethod)
      where T : IAttributeID<byte>
    {
      List<MemberInfoMethod<T>> methods = AttributeFinder.GetMethods<T>(type);
      byte num = 0;
      List<byte> byteList = new List<byte>();
      foreach (MemberInfoMethod<T> memberInfoMethod in methods)
      {
        if (memberInfoMethod.Attribute.HasID)
        {
          byte id = memberInfoMethod.Attribute.ID;
          if (!byteList.Contains(id))
            byteList.Add(id);
          else
            throw CmuneDebug.Exception("Reflection.FindNetworkMethods Detected a Collision of ID {0} in {1}", (object) id, (object) type.Name);
        }
      }
      foreach (MemberInfoMethod<T> memberInfoMethod in methods)
      {
        byte key;
        if (memberInfoMethod.Attribute.HasID)
        {
          key = memberInfoMethod.Attribute.ID;
        }
        else
        {
          while (byteList.Contains(num))
            ++num;
          byteList.Add(num);
          key = num;
        }
        try
        {
          _lookupIndexMethod.Add(key, memberInfoMethod.Method);
        }
        catch
        {
          throw CmuneDebug.Exception("Failed registering network function with name {0}! Static ID {1} already existing and reserved by function {2}!", (object) memberInfoMethod.Method.Name, (object) key, (object) _lookupIndexMethod[key].Name);
        }
        if (!_lookupNameIndex.ContainsKey(memberInfoMethod.Method.Name))
          _lookupNameIndex.Add(memberInfoMethod.Method.Name, key);
        else if (CmuneDebug.IsWarningEnabled)
          CmuneDebug.LogWarning(string.Format("Network function with name {0} can't be called by name because ambigous! Use ID {1} instead or Rename!", (object) memberInfoMethod.Method.Name, (object) key), new object[0]);
      }
    }

    public static List<MemberInfoMethod<T>> GetMethods<T>(Type type)
    {
      List<MemberInfoMethod<T>> methods = new List<MemberInfoMethod<T>>();
      foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        object[] customAttributes = method.GetCustomAttributes(typeof (T), true);
        if (customAttributes.Length > 0)
          methods.Add(new MemberInfoMethod<T>(method, (T) customAttributes[0]));
      }
      try
      {
        methods.Sort((Comparison<MemberInfoMethod<T>>) ((p, q) => p.Name.CompareTo(q.Name)));
      }
      catch (Exception ex)
      {
        CmuneDebug.LogError("GetMethods - Exception in ReflectionHelper: {0}", (object) ex.Message);
        methods.Clear();
      }
      return methods;
    }
  }
}
