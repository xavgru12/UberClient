// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.SoapIncludeAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Serialization
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = true)]
  public class SoapIncludeAttribute : Attribute
  {
    private Type type;

    public SoapIncludeAttribute(Type type) => this.type = type;

    public Type Type
    {
      get => this.type;
      set => this.type = value;
    }
  }
}
