// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlSerializableMapping
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Reflection;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
  internal class XmlSerializableMapping : XmlTypeMapping
  {
    private XmlSchema _schema;
    private XmlSchemaComplexType _schemaType;
    private XmlQualifiedName _schemaTypeName;

    internal XmlSerializableMapping(
      string elementName,
      string ns,
      TypeData typeData,
      string xmlType,
      string xmlTypeNamespace)
      : base(elementName, ns, typeData, xmlType, xmlTypeNamespace)
    {
      XmlSchemaProviderAttribute customAttribute = (XmlSchemaProviderAttribute) Attribute.GetCustomAttribute((MemberInfo) typeData.Type, typeof (XmlSchemaProviderAttribute));
      if (customAttribute != null)
      {
        string methodName = customAttribute.MethodName;
        MethodInfo method = typeData.Type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
        if (method == null)
          throw new InvalidOperationException(string.Format("Type '{0}' must implement public static method '{1}'", (object) typeData.Type, (object) methodName));
        if (!typeof (XmlQualifiedName).IsAssignableFrom(method.ReturnType) && !typeof (XmlSchemaComplexType).IsAssignableFrom(method.ReturnType))
          throw new InvalidOperationException(string.Format("Method '{0}' indicated by XmlSchemaProviderAttribute must have its return type as XmlQualifiedName", (object) methodName));
        XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
        object obj = method.Invoke((object) null, new object[1]
        {
          (object) xmlSchemaSet
        });
        this._schemaTypeName = XmlQualifiedName.Empty;
        if (obj == null)
          return;
        if (obj is XmlSchemaComplexType)
        {
          this._schemaType = (XmlSchemaComplexType) obj;
          this._schemaTypeName = this._schemaType.QualifiedName.IsEmpty ? new XmlQualifiedName(xmlType, xmlTypeNamespace) : this._schemaType.QualifiedName;
        }
        else
          this._schemaTypeName = (object) (obj as XmlQualifiedName) != null ? (XmlQualifiedName) obj : throw new InvalidOperationException(string.Format("Method {0}.{1}() specified by XmlSchemaProviderAttribute has invalid signature: return type must be compatible with System.Xml.XmlQualifiedName.", (object) typeData.Type.Name, (object) methodName));
        this.UpdateRoot(new XmlQualifiedName(this._schemaTypeName.Name, this.Namespace ?? this._schemaTypeName.Namespace));
        this.XmlTypeNamespace = this._schemaTypeName.Namespace;
        this.XmlType = this._schemaTypeName.Name;
        if (this._schemaTypeName.IsEmpty || xmlSchemaSet.Count <= 0)
          return;
        XmlSchema[] array = new XmlSchema[xmlSchemaSet.Count];
        xmlSchemaSet.CopyTo(array, 0);
        this._schema = array[0];
      }
      else
      {
        IXmlSerializable instance = (IXmlSerializable) Activator.CreateInstance(typeData.Type, true);
        try
        {
          this._schema = instance.GetSchema();
        }
        catch (Exception ex)
        {
        }
        if (this._schema != null && (this._schema.Id == null || this._schema.Id.Length == 0))
          throw new InvalidOperationException("Schema Id is missing. The schema returned from " + typeData.Type.FullName + ".GetSchema() must have an Id.");
      }
    }

    internal XmlSchema Schema => this._schema;

    internal XmlSchemaType SchemaType => (XmlSchemaType) this._schemaType;

    internal XmlQualifiedName SchemaTypeName => this._schemaTypeName;
  }
}
