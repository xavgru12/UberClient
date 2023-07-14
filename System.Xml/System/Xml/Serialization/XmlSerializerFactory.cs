// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlSerializerFactory
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Security.Policy;

namespace System.Xml.Serialization
{
  public class XmlSerializerFactory
  {
    private static Hashtable serializersBySource = new Hashtable();

    public XmlSerializer CreateSerializer(Type type) => this.CreateSerializer(type, (XmlAttributeOverrides) null, (Type[]) null, (XmlRootAttribute) null, (string) null);

    public XmlSerializer CreateSerializer(XmlTypeMapping xmlTypeMapping)
    {
      lock (XmlSerializerFactory.serializersBySource)
      {
        XmlSerializer serializer = (XmlSerializer) XmlSerializerFactory.serializersBySource[(object) xmlTypeMapping.Source];
        if (serializer == null)
        {
          serializer = new XmlSerializer(xmlTypeMapping);
          XmlSerializerFactory.serializersBySource[(object) xmlTypeMapping.Source] = (object) serializer;
        }
        return serializer;
      }
    }

    public XmlSerializer CreateSerializer(Type type, string defaultNamespace) => this.CreateSerializer(type, (XmlAttributeOverrides) null, (Type[]) null, (XmlRootAttribute) null, defaultNamespace);

    public XmlSerializer CreateSerializer(Type type, Type[] extraTypes) => this.CreateSerializer(type, (XmlAttributeOverrides) null, extraTypes, (XmlRootAttribute) null, (string) null);

    public XmlSerializer CreateSerializer(Type type, XmlAttributeOverrides overrides) => this.CreateSerializer(type, overrides, (Type[]) null, (XmlRootAttribute) null, (string) null);

    public XmlSerializer CreateSerializer(Type type, XmlRootAttribute root) => this.CreateSerializer(type, (XmlAttributeOverrides) null, (Type[]) null, root, (string) null);

    public XmlSerializer CreateSerializer(
      Type type,
      XmlAttributeOverrides overrides,
      Type[] extraTypes,
      XmlRootAttribute root,
      string defaultNamespace)
    {
      XmlTypeSerializationSource key = new XmlTypeSerializationSource(type, root, overrides, defaultNamespace, extraTypes);
      lock (XmlSerializerFactory.serializersBySource)
      {
        XmlSerializer serializer = (XmlSerializer) XmlSerializerFactory.serializersBySource[(object) key];
        if (serializer == null)
        {
          serializer = new XmlSerializer(type, overrides, extraTypes, root, defaultNamespace);
          XmlSerializerFactory.serializersBySource[(object) serializer.Mapping.Source] = (object) serializer;
        }
        return serializer;
      }
    }

    [MonoTODO]
    public XmlSerializer CreateSerializer(
      Type type,
      XmlAttributeOverrides overrides,
      Type[] extraTypes,
      XmlRootAttribute root,
      string defaultNamespace,
      string location,
      Evidence evidence)
    {
      throw new NotImplementedException();
    }
  }
}
