// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathItem
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Schema;

namespace System.Xml.XPath
{
  public abstract class XPathItem
  {
    public virtual object ValueAs(Type type) => this.ValueAs(type, (IXmlNamespaceResolver) null);

    public abstract object ValueAs(Type type, IXmlNamespaceResolver nsResolver);

    public abstract bool IsNode { get; }

    public abstract object TypedValue { get; }

    public abstract string Value { get; }

    public abstract bool ValueAsBoolean { get; }

    public abstract DateTime ValueAsDateTime { get; }

    public abstract double ValueAsDouble { get; }

    public abstract int ValueAsInt { get; }

    public abstract long ValueAsLong { get; }

    public abstract Type ValueType { get; }

    public abstract XmlSchemaType XmlType { get; }
  }
}
