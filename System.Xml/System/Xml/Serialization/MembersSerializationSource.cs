// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.MembersSerializationSource
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Globalization;
using System.Text;

namespace System.Xml.Serialization
{
  internal class MembersSerializationSource : SerializationSource
  {
    private string elementName;
    private bool hasWrapperElement;
    private string membersHash;
    private bool writeAccessors;
    private bool literalFormat;

    public MembersSerializationSource(
      string elementName,
      bool hasWrapperElement,
      XmlReflectionMember[] members,
      bool writeAccessors,
      bool literalFormat,
      string namspace,
      Type[] includedTypes)
      : base(namspace, includedTypes)
    {
      this.elementName = elementName;
      this.hasWrapperElement = hasWrapperElement;
      this.writeAccessors = writeAccessors;
      this.literalFormat = literalFormat;
      StringBuilder sb = new StringBuilder();
      sb.Append(members.Length.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      foreach (XmlReflectionMember member in members)
        member.AddKeyHash(sb);
      this.membersHash = sb.ToString();
    }

    public override bool Equals(object o) => o is MembersSerializationSource other && !(this.literalFormat = other.literalFormat) && !(this.elementName != other.elementName) && this.hasWrapperElement == other.hasWrapperElement && !(this.membersHash != other.membersHash) && this.BaseEquals((SerializationSource) other);

    public override int GetHashCode() => this.membersHash.GetHashCode();
  }
}
