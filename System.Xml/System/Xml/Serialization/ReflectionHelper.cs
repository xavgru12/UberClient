// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.ReflectionHelper
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Reflection;

namespace System.Xml.Serialization
{
  internal class ReflectionHelper
  {
    private Hashtable _clrTypes = new Hashtable();
    private Hashtable _schemaTypes = new Hashtable();
    private static readonly ParameterModifier[] empty_modifiers = new ParameterModifier[0];

    public void RegisterSchemaType(XmlTypeMapping map, string xmlType, string ns)
    {
      string key = xmlType + "/" + ns;
      if (this._schemaTypes.ContainsKey((object) key))
        return;
      this._schemaTypes.Add((object) key, (object) map);
    }

    public XmlTypeMapping GetRegisteredSchemaType(string xmlType, string ns) => this._schemaTypes[(object) (xmlType + "/" + ns)] as XmlTypeMapping;

    public void RegisterClrType(XmlTypeMapping map, Type type, string ns)
    {
      if (type == typeof (object))
        ns = string.Empty;
      string key = type.FullName + "/" + ns;
      if (this._clrTypes.ContainsKey((object) key))
        return;
      this._clrTypes.Add((object) key, (object) map);
    }

    public XmlTypeMapping GetRegisteredClrType(Type type, string ns)
    {
      if (type == typeof (object))
        ns = string.Empty;
      return this._clrTypes[(object) (type.FullName + "/" + ns)] as XmlTypeMapping;
    }

    public Exception CreateError(XmlTypeMapping map, string message) => (Exception) new InvalidOperationException("There was an error reflecting '" + map.TypeFullName + "': " + message);

    public static void CheckSerializableType(Type type, bool allowPrivateConstructors)
    {
      if (type.IsArray)
        return;
      if (!allowPrivateConstructors && type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Type.EmptyTypes, ReflectionHelper.empty_modifiers) == null && !type.IsAbstract && !type.IsValueType)
        throw new InvalidOperationException(type.FullName + " cannot be serialized because it does not have a default public constructor");
      if (type.IsInterface && !TypeTranslator.GetTypeData(type).IsListType)
        throw new InvalidOperationException(type.FullName + " cannot be serialized because it is an interface");
      Type type1 = type;
      while (type1.IsPublic || type1.IsNestedPublic)
      {
        Type type2 = type1;
        type1 = type1.DeclaringType;
        if (type1 == null || type1 == type2)
          return;
      }
      throw new InvalidOperationException(type.FullName + " is inaccessible due to its protection level. Only public types can be processed");
    }

    public static string BuildMapKey(Type type) => type.FullName + "::";

    public static string BuildMapKey(MethodInfo method, string tag)
    {
      string str1 = method.DeclaringType.FullName + ":" + method.ReturnType.FullName + " " + method.Name + "(";
      ParameterInfo[] parameters = method.GetParameters();
      for (int index = 0; index < parameters.Length; ++index)
      {
        if (index > 0)
          str1 += ", ";
        str1 += parameters[index].ParameterType.FullName;
      }
      string str2 = str1 + ")";
      if (tag != null)
        str2 = str2 + ":" + tag;
      return str2;
    }
  }
}
