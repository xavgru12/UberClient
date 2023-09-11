// Decompiled with JetBrains decompiler
// Type: Cmune.Util.ReflectionHelper
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Core.Types;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cmune.Util
{
  public static class ReflectionHelper
  {
    public const BindingFlags FieldBinder = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;
    public const BindingFlags InvokeBinder = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod;

    public static List<FieldInfo> GetAllFields(Type type, bool inherited)
    {
      List<FieldInfo> allFields = new List<FieldInfo>();
      for (; (object) type != (object) typeof (object); type = type.BaseType)
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
      for (; (object) type != (object) typeof (object); type = type.BaseType)
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
      for (; (object) type != (object) typeof (object); type = type.BaseType)
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

    public static List<MemberInfoMethod<T>> GetMethodsWithAttribute<T>(Type type, bool inherited) => ReflectionHelper.GetMethodsWithAttribute<T>(ReflectionHelper.GetAllMethods(type, inherited));

    public static List<MemberInfoMethod<T>> GetMethodsWithAttribute<T>(List<MethodInfo> members)
    {
      List<MemberInfoMethod<T>> methodsWithAttribute = new List<MemberInfoMethod<T>>(members.Count);
      foreach (MethodInfo member in members)
      {
        if (member.GetCustomAttributes(typeof (T), false) is T[] customAttributes && customAttributes.Length > 0)
          methodsWithAttribute.Add(new MemberInfoMethod<T>(member, customAttributes[0]));
      }
      return methodsWithAttribute;
    }

    public static List<MemberInfoField<T>> GetFieldsWithAttribute<T>(Type type, bool inherited) => ReflectionHelper.GetFieldsWithAttribute<T>(ReflectionHelper.GetAllFields(type, inherited));

    public static List<MemberInfoField<T>> GetFieldsWithAttribute<T>(List<FieldInfo> members)
    {
      List<MemberInfoField<T>> fieldsWithAttribute = new List<MemberInfoField<T>>(members.Count);
      foreach (FieldInfo member in members)
      {
        if (member.GetCustomAttributes(typeof (T), false) is T[] customAttributes && customAttributes.Length > 0)
          fieldsWithAttribute.Add(new MemberInfoField<T>(member, customAttributes[0]));
      }
      return fieldsWithAttribute;
    }

    public static List<MemberInfoProperty<T>> GetPropertiesWithAttribute<T>(
      Type type,
      bool inherited)
    {
      return ReflectionHelper.GetPropertiesWithAttribute<T>(ReflectionHelper.GetAllProperties(type, inherited));
    }

    public static List<MemberInfoProperty<T>> GetPropertiesWithAttribute<T>(
      List<PropertyInfo> members)
    {
      List<MemberInfoProperty<T>> propertiesWithAttribute = new List<MemberInfoProperty<T>>(members.Count);
      foreach (PropertyInfo member in members)
      {
        if (member.GetCustomAttributes(typeof (T), false) is T[] customAttributes && customAttributes.Length > 0)
          propertiesWithAttribute.Add(new MemberInfoProperty<T>(member, customAttributes[0]));
      }
      return propertiesWithAttribute;
    }

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
            flag &= (object) parameters[index].ParameterType == (object) args[index];
        }
        if (flag)
          methodWithParameters = methodInfo;
      }
      return methodWithParameters;
    }
  }
}
