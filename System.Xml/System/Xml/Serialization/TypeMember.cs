// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.TypeMember
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Serialization
{
  internal sealed class TypeMember
  {
    private Type type;
    private string member;

    internal TypeMember(Type type, string member)
    {
      this.type = type;
      this.member = member;
    }

    public override int GetHashCode() => this.type.GetHashCode() + this.member.GetHashCode();

    public override bool Equals(object obj) => (object) (obj as TypeMember) != null && TypeMember.Equals(this, (TypeMember) obj);

    public static bool Equals(TypeMember tm1, TypeMember tm2) => object.ReferenceEquals((object) tm1, (object) tm2) || !object.ReferenceEquals((object) tm1, (object) null) && !object.ReferenceEquals((object) tm2, (object) null) && tm1.type == tm2.type && tm1.member == tm2.member;

    public override string ToString() => this.type.ToString() + " " + this.member;

    public static bool operator ==(TypeMember tm1, TypeMember tm2) => TypeMember.Equals(tm1, tm2);

    public static bool operator !=(TypeMember tm1, TypeMember tm2) => !TypeMember.Equals(tm1, tm2);
  }
}
