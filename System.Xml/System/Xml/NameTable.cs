// Decompiled with JetBrains decompiler
// Type: System.Xml.NameTable
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml
{
  public class NameTable : XmlNameTable
  {
    private const int INITIAL_BUCKETS = 128;
    private int count = 128;
    private NameTable.Entry[] buckets = new NameTable.Entry[128];
    private int size;

    public override string Add(char[] key, int start, int len)
    {
      if (0 > start && start >= key.Length || 0 > len && len >= key.Length - len)
        throw new IndexOutOfRangeException("The Index is out of range.");
      if (len == 0)
        return string.Empty;
      int num1 = 0;
      int num2 = start + len;
      for (int index = start; index < num2; ++index)
        num1 = (num1 << 5) - num1 + (int) key[index];
      int hash = num1 & int.MaxValue;
      for (NameTable.Entry entry = this.buckets[hash % this.count]; entry != null; entry = entry.next)
      {
        if (entry.hash == hash && entry.len == len && NameTable.StrEqArray(entry.str, key, start))
          return entry.str;
      }
      return this.AddEntry(new string(key, start, len), hash);
    }

    public override string Add(string key)
    {
      int num1 = key != null ? key.Length : throw new ArgumentNullException(nameof (key));
      if (num1 == 0)
        return string.Empty;
      int num2 = 0;
      for (int index = 0; index < num1; ++index)
        num2 = (num2 << 5) - num2 + (int) key[index];
      int hash = num2 & int.MaxValue;
      for (NameTable.Entry entry = this.buckets[hash % this.count]; entry != null; entry = entry.next)
      {
        if (entry.hash == hash && entry.len == key.Length && entry.str == key)
          return entry.str;
      }
      return this.AddEntry(key, hash);
    }

    public override string Get(char[] key, int start, int len)
    {
      if (0 > start && start >= key.Length || 0 > len && len >= key.Length - len)
        throw new IndexOutOfRangeException("The Index is out of range.");
      if (len == 0)
        return string.Empty;
      int num1 = 0;
      int num2 = start + len;
      for (int index = start; index < num2; ++index)
        num1 = (num1 << 5) - num1 + (int) key[index];
      int num3 = num1 & int.MaxValue;
      for (NameTable.Entry entry = this.buckets[num3 % this.count]; entry != null; entry = entry.next)
      {
        if (entry.hash == num3 && entry.len == len && NameTable.StrEqArray(entry.str, key, start))
          return entry.str;
      }
      return (string) null;
    }

    public override string Get(string value)
    {
      int num1 = value != null ? value.Length : throw new ArgumentNullException(nameof (value));
      if (num1 == 0)
        return string.Empty;
      int num2 = 0;
      for (int index = 0; index < num1; ++index)
        num2 = (num2 << 5) - num2 + (int) value[index];
      int num3 = num2 & int.MaxValue;
      for (NameTable.Entry entry = this.buckets[num3 % this.count]; entry != null; entry = entry.next)
      {
        if (entry.hash == num3 && entry.len == value.Length && entry.str == value)
          return entry.str;
      }
      return (string) null;
    }

    private string AddEntry(string str, int hash)
    {
      int index1 = hash % this.count;
      this.buckets[index1] = new NameTable.Entry(str, hash, this.buckets[index1]);
      if (this.size++ == this.count)
      {
        this.count <<= 1;
        int num = this.count - 1;
        NameTable.Entry[] entryArray = new NameTable.Entry[this.count];
        NameTable.Entry next;
        for (int index2 = 0; index2 < this.buckets.Length; ++index2)
        {
          for (NameTable.Entry entry = this.buckets[index2]; entry != null; entry = next)
          {
            int index3 = entry.hash & num;
            next = entry.next;
            entry.next = entryArray[index3];
            entryArray[index3] = entry;
          }
        }
        this.buckets = entryArray;
      }
      return str;
    }

    private static bool StrEqArray(string str, char[] str2, int start)
    {
      int index = str.Length - 1;
      start += index;
      while ((int) str[index] == (int) str2[start])
      {
        --index;
        --start;
        if (index < 0)
          return true;
      }
      return false;
    }

    private class Entry
    {
      public string str;
      public int hash;
      public int len;
      public NameTable.Entry next;

      public Entry(string str, int hash, NameTable.Entry next)
      {
        this.str = str;
        this.len = str.Length;
        this.hash = hash;
        this.next = next;
      }
    }
  }
}
