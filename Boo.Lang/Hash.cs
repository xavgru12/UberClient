// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Hash
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using Boo.Lang.Runtime;
using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Boo.Lang
{
  [EnumeratorItemType(typeof (DictionaryEntry))]
  [Serializable]
  public class Hash : Hashtable, IEquatable<Hash>
  {
    public Hash()
      : base(BooHashCodeProvider.Default)
    {
    }

    public Hash(IDictionary other)
      : this()
    {
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      foreach (DictionaryEntry dictionaryEntry in other)
        this.Add(dictionaryEntry.Key, dictionaryEntry.Value);
    }

    public Hash(IEnumerable enumerable)
      : this()
    {
      if (enumerable == null)
        throw new ArgumentNullException(nameof (enumerable));
      foreach (Array array in enumerable)
        this.Add(array.GetValue(0), array.GetValue(1));
    }

    public Hash(bool caseInsensitive)
      : base((IEqualityComparer) StringComparer.InvariantCultureIgnoreCase)
    {
    }

    public Hash(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public override object Clone() => (object) new Hash((IDictionary) this);

    public override bool Equals(object other)
    {
      if (other == null)
        return false;
      return this == other || this.Equals(other as Hash);
    }

    public bool Equals(Hash other)
    {
      if (other == null)
        return false;
      if (this == other)
        return true;
      if (this.Count != other.Count)
        return false;
      foreach (DictionaryEntry dictionaryEntry in (Hashtable) other)
      {
        if (!this.ContainsKey(dictionaryEntry.Key) || !RuntimeServices.EqualityOperator(dictionaryEntry.Value, this[dictionaryEntry.Key]))
          return false;
      }
      return true;
    }

    public override int GetHashCode()
    {
      int hashCode = 0;
      foreach (object key in (Hashtable) this)
        hashCode ^= this.GetHash(key);
      return hashCode;
    }
  }
}
