// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlSerializerNamespaces
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Collections.Specialized;

namespace System.Xml.Serialization
{
  public class XmlSerializerNamespaces
  {
    private ListDictionary namespaces;

    public XmlSerializerNamespaces() => this.namespaces = new ListDictionary();

    public XmlSerializerNamespaces(XmlQualifiedName[] namespaces)
      : this()
    {
      foreach (XmlQualifiedName xmlQualifiedName in namespaces)
        this.namespaces[(object) xmlQualifiedName.Name] = (object) xmlQualifiedName;
    }

    public XmlSerializerNamespaces(XmlSerializerNamespaces namespaces)
      : this(namespaces.ToArray())
    {
    }

    public void Add(string prefix, string ns)
    {
      XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(prefix, ns);
      this.namespaces[(object) xmlQualifiedName.Name] = (object) xmlQualifiedName;
    }

    public XmlQualifiedName[] ToArray()
    {
      XmlQualifiedName[] array = new XmlQualifiedName[this.namespaces.Count];
      this.namespaces.Values.CopyTo((Array) array, 0);
      return array;
    }

    public int Count => this.namespaces.Count;

    internal string GetPrefix(string Ns)
    {
      foreach (string key in (IEnumerable) this.namespaces.Keys)
      {
        if (Ns == ((XmlQualifiedName) this.namespaces[(object) key]).Namespace)
          return key;
      }
      return (string) null;
    }

    internal ListDictionary Namespaces => this.namespaces;
  }
}
