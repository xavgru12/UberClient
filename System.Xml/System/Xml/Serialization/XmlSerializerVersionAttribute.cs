// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlSerializerVersionAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Serialization
{
  [AttributeUsage(AttributeTargets.Assembly)]
  public sealed class XmlSerializerVersionAttribute : Attribute
  {
    private string _namespace;
    private string _parentAssemblyId;
    private Type _type;
    private string _version;

    public XmlSerializerVersionAttribute()
    {
    }

    public XmlSerializerVersionAttribute(Type type) => this._type = type;

    public string Namespace
    {
      get => this._namespace;
      set => this._namespace = value;
    }

    public string ParentAssemblyId
    {
      get => this._parentAssemblyId;
      set => this._parentAssemblyId = value;
    }

    public Type Type
    {
      get => this._type;
      set => this._type = value;
    }

    public string Version
    {
      get => this._version;
      set => this._version = value;
    }
  }
}
