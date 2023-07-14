// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlSchemaProviderAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Serialization
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
  public sealed class XmlSchemaProviderAttribute : Attribute
  {
    private string _methodName;
    private bool _isAny;

    public XmlSchemaProviderAttribute(string methodName) => this._methodName = methodName;

    public string MethodName => this._methodName;

    public bool IsAny
    {
      get => this._isAny;
      set => this._isAny = value;
    }
  }
}
