// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.CodeIdentifiers
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Globalization;

namespace System.Xml.Serialization
{
  public class CodeIdentifiers
  {
    private bool useCamelCasing;
    private Hashtable table;
    private Hashtable reserved;

    public CodeIdentifiers()
      : this(true)
    {
    }

    public CodeIdentifiers(bool caseSensitive)
    {
      StringComparer stringComparer = !caseSensitive ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
      this.table = new Hashtable((IEqualityComparer) stringComparer);
      this.reserved = new Hashtable((IEqualityComparer) stringComparer);
    }

    public bool UseCamelCasing
    {
      get => this.useCamelCasing;
      set => this.useCamelCasing = value;
    }

    public void Add(string identifier, object value) => this.table.Add((object) identifier, value);

    public void AddReserved(string identifier) => this.reserved.Add((object) identifier, (object) identifier);

    public string AddUnique(string identifier, object value)
    {
      string identifier1 = this.MakeUnique(identifier);
      this.Add(identifier1, value);
      return identifier1;
    }

    public void Clear() => this.table.Clear();

    public bool IsInUse(string identifier) => this.table.ContainsKey((object) identifier) || this.reserved.ContainsKey((object) identifier);

    public string MakeRightCase(string identifier) => this.UseCamelCasing ? CodeIdentifier.MakeCamel(identifier) : CodeIdentifier.MakePascal(identifier);

    public string MakeUnique(string identifier)
    {
      string identifier1 = identifier;
      int num = 1;
      while (this.IsInUse(identifier1))
      {
        identifier1 = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}{1}", (object) identifier, (object) num);
        ++num;
      }
      return identifier1;
    }

    public void Remove(string identifier) => this.table.Remove((object) identifier);

    public void RemoveReserved(string identifier) => this.reserved.Remove((object) identifier);

    public object ToArray(Type type)
    {
      Array instance = Array.CreateInstance(type, this.table.Count);
      this.table.CopyTo(instance, 0);
      return (object) instance;
    }
  }
}
