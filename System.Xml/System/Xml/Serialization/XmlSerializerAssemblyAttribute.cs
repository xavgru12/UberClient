// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlSerializerAssemblyAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Serialization
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
  public sealed class XmlSerializerAssemblyAttribute : Attribute
  {
    private string _assemblyName;
    private string _codeBase;

    public XmlSerializerAssemblyAttribute()
    {
    }

    public XmlSerializerAssemblyAttribute(string assemblyName) => this._assemblyName = assemblyName;

    public XmlSerializerAssemblyAttribute(string assemblyName, string codeBase)
      : this(assemblyName)
    {
      this._codeBase = codeBase;
    }

    public string AssemblyName
    {
      get => this._assemblyName;
      set => this._assemblyName = value;
    }

    public string CodeBase
    {
      get => this._codeBase;
      set => this._codeBase = value;
    }
  }
}
