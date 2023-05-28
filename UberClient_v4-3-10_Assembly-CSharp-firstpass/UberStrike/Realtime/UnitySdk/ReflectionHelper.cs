// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.ReflectionHelper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Reflection;

namespace UberStrike.Realtime.UnitySdk
{
  public static class ReflectionHelper
  {
    public const BindingFlags FieldBinder = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;
    public const BindingFlags InvokeBinder = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod;

    public static List<FieldInfo> GetAllFields(Type type, bool inherited)
    {
      List<FieldInfo> allFields = new List<FieldInfo>();
      for (; type != typeof (object); type = type.BaseType)
      {
        FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        allFields.AddRange((IEnumerable<FieldInfo>) fields);
        if (!inherited)
          break;
      }
      allFields.Sort((Comparison<FieldInfo>) ((p, q) => p.Name.CompareTo(q.Name)));
      return allFields;
    }

    public static List<PropertyInfo> GetAllProperties(Type type, bool inherited)
    {
      List<PropertyInfo> allProperties = new List<PropertyInfo>();
      for (; type != typeof (object); type = type.BaseType)
      {
        PropertyInfo[] properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        allProperties.AddRange((IEnumerable<PropertyInfo>) properties);
        if (!inherited)
          break;
      }
      allProperties.Sort((Comparison<PropertyInfo>) ((p, q) => p.Name.CompareTo(q.Name)));
      return allProperties;
    }

    public static List<MethodInfo> GetAllMethods(Type type, bool inherited)
    {
      List<MethodInfo> allMethods = new List<MethodInfo>();
      for (; type != typeof (object); type = type.BaseType)
      {
        MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        allMethods.AddRange((IEnumerable<MethodInfo>) methods);
        if (!inherited)
          break;
      }
      allMethods.Sort((Comparison<MethodInfo>) ((p, q) => p.Name.CompareTo(q.Name)));
      return allMethods;
    }

    public static void FilterByAttribute<T>(Type attribute, List<T> members) where T : MemberInfo => members.RemoveAll((Predicate<T>) (m => m.GetCustomAttributes(attribute, false).Length == 0));

    public static MethodInfo GetMethodWithParameters(
      List<MethodInfo> members,
      string name,
      params Type[] args)
    {
      MethodInfo methodWithParameters = (MethodInfo) null;
      foreach (MethodInfo methodInfo in members.FindAll((Predicate<MethodInfo>) (m => m.Name == name)))
      {
        bool flag = true;
        ParameterInfo[] parameters = methodInfo.GetParameters();
        if (parameters.Length == args.Length)
        {
          for (int index = 0; index < parameters.Length; ++index)
            flag &= parameters[index].ParameterType == args[index];
        }
        if (flag)
          methodWithParameters = methodInfo;
      }
      return methodWithParameters;
    }
  }
}
