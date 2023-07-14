// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlQualifiedName
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml
{
  [Serializable]
  public class XmlQualifiedName
  {
    public static readonly XmlQualifiedName Empty = new XmlQualifiedName();
    private readonly string name;
    private readonly string ns;
    private readonly int hash;

    public XmlQualifiedName()
      : this(string.Empty, string.Empty)
    {
    }

    public XmlQualifiedName(string name)
      : this(name, string.Empty)
    {
    }

    public XmlQualifiedName(string name, string ns)
    {
      this.name = name != null ? name : string.Empty;
      this.ns = ns != null ? ns : string.Empty;
      this.hash = this.name.GetHashCode() ^ this.ns.GetHashCode();
    }

    public bool IsEmpty => this.name.Length == 0 && this.ns.Length == 0;

    public string Name => this.name;

    public string Namespace => this.ns;

    public override bool Equals(object other) => this == other as XmlQualifiedName;

    public override int GetHashCode() => this.hash;

    public override string ToString() => this.ns == string.Empty ? this.name : this.ns + ":" + this.name;

    public static string ToString(string name, string ns) => ns == string.Empty ? name : ns + ":" + name;

    internal static XmlQualifiedName Parse(string name, IXmlNamespaceResolver resolver) => XmlQualifiedName.Parse(name, resolver, false);

    internal static XmlQualifiedName Parse(
      string name,
      IXmlNamespaceResolver resolver,
      bool considerDefaultNamespace)
    {
      int length = name.IndexOf(':');
      if (length < 0 && !considerDefaultNamespace)
        return new XmlQualifiedName(name);
      string prefix = length >= 0 ? name.Substring(0, length) : string.Empty;
      string name1 = length >= 0 ? name.Substring(length + 1) : name;
      return new XmlQualifiedName(name1, resolver.LookupNamespace(prefix) ?? throw new ArgumentException("Invalid qualified name."));
    }

    internal static XmlQualifiedName Parse(string name, XmlReader reader)
    {
      int length = name.IndexOf(':');
      if (length < 0)
        return new XmlQualifiedName(name);
      return new XmlQualifiedName(name.Substring(length + 1), reader.LookupNamespace(name.Substring(0, length)) ?? throw new ArgumentException("Invalid qualified name."));
    }

    public static bool operator ==(XmlQualifiedName a, XmlQualifiedName b)
    {
      if ((object) a == (object) b)
        return true;
      return (object) a != null && (object) b != null && a.hash == b.hash && a.name == b.name && a.ns == b.ns;
    }

    public static bool operator !=(XmlQualifiedName a, XmlQualifiedName b) => !(a == b);
  }
}
