// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.SerializationSource
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Serialization
{
  internal abstract class SerializationSource
  {
    private Type[] includedTypes;
    private string namspace;
    private bool canBeGenerated = true;

    public SerializationSource(string namspace, Type[] includedTypes)
    {
      this.namspace = namspace;
      this.includedTypes = includedTypes;
    }

    protected bool BaseEquals(SerializationSource other)
    {
      if (this.namspace != other.namspace || this.canBeGenerated != other.canBeGenerated)
        return false;
      if (this.includedTypes == null)
        return other.includedTypes == null;
      if (other.includedTypes == null || this.includedTypes.Length != other.includedTypes.Length)
        return false;
      for (int index = 0; index < this.includedTypes.Length; ++index)
      {
        if (!this.includedTypes[index].Equals(other.includedTypes[index]))
          return false;
      }
      return true;
    }

    public virtual bool CanBeGenerated
    {
      get => this.canBeGenerated;
      set => this.canBeGenerated = value;
    }
  }
}
