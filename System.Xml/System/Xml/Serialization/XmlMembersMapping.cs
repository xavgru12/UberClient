// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlMembersMapping
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Serialization
{
  public class XmlMembersMapping : XmlMapping
  {
    private bool _hasWrapperElement;
    private XmlMemberMapping[] _mapping;

    internal XmlMembersMapping()
    {
    }

    internal XmlMembersMapping(XmlMemberMapping[] mapping)
      : this(string.Empty, (string) null, false, false, mapping)
    {
    }

    internal XmlMembersMapping(string elementName, string ns, XmlMemberMapping[] mapping)
      : this(elementName, ns, true, false, mapping)
    {
    }

    internal XmlMembersMapping(
      string elementName,
      string ns,
      bool hasWrapperElement,
      bool writeAccessors,
      XmlMemberMapping[] mapping)
      : base(elementName, ns)
    {
      this._hasWrapperElement = hasWrapperElement;
      this._mapping = mapping;
      ClassMap classMap = new ClassMap();
      classMap.IgnoreMemberNamespace = writeAccessors;
      foreach (XmlMemberMapping xmlMemberMapping in mapping)
        classMap.AddMember(xmlMemberMapping.TypeMapMember);
      this.ObjectMap = (ObjectMap) classMap;
    }

    public int Count => this._mapping.Length;

    public XmlMemberMapping this[int index] => this._mapping[index];

    public string TypeName
    {
      [MonoTODO] get => throw new NotImplementedException();
    }

    public string TypeNamespace
    {
      [MonoTODO] get => throw new NotImplementedException();
    }

    internal bool HasWrapperElement => this._hasWrapperElement;
  }
}
