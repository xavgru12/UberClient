// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.ClassMap
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.Serialization
{
  internal class ClassMap : ObjectMap
  {
    private Hashtable _elements = new Hashtable();
    private ArrayList _elementMembers;
    private Hashtable _attributeMembers;
    private XmlTypeMapMemberAttribute[] _attributeMembersArray;
    private XmlTypeMapElementInfo[] _elementsByIndex;
    private ArrayList _flatLists;
    private ArrayList _allMembers = new ArrayList();
    private ArrayList _membersWithDefault;
    private ArrayList _listMembers;
    private XmlTypeMapMemberAnyElement _defaultAnyElement;
    private XmlTypeMapMemberAnyAttribute _defaultAnyAttribute;
    private XmlTypeMapMemberNamespaces _namespaceDeclarations;
    private XmlTypeMapMember _xmlTextCollector;
    private XmlTypeMapMember _returnMember;
    private bool _ignoreMemberNamespace;
    private bool _canBeSimpleType = true;

    public void AddMember(XmlTypeMapMember member)
    {
      member.GlobalIndex = this._allMembers.Count;
      this._allMembers.Add((object) member);
      if (!(member.DefaultValue is DBNull) && member.DefaultValue != null)
      {
        if (this._membersWithDefault == null)
          this._membersWithDefault = new ArrayList();
        this._membersWithDefault.Add((object) member);
      }
      if (member.IsReturnValue)
        this._returnMember = member;
      if (member is XmlTypeMapMemberAttribute)
      {
        XmlTypeMapMemberAttribute mapMemberAttribute = (XmlTypeMapMemberAttribute) member;
        if (this._attributeMembers == null)
          this._attributeMembers = new Hashtable();
        string key = this.BuildKey(mapMemberAttribute.AttributeName, mapMemberAttribute.Namespace);
        member.Index = !this._attributeMembers.ContainsKey((object) key) ? this._attributeMembers.Count : throw new InvalidOperationException("The XML attribute named '" + mapMemberAttribute.AttributeName + "' from namespace '" + mapMemberAttribute.Namespace + "' is already present in the current scope. Use XML attributes to specify another XML name or namespace for the attribute.");
        this._attributeMembers.Add((object) key, (object) member);
      }
      else
      {
        if (member is XmlTypeMapMemberFlatList)
          this.RegisterFlatList((XmlTypeMapMemberExpandable) member);
        else if (member is XmlTypeMapMemberAnyElement)
        {
          XmlTypeMapMemberAnyElement member1 = (XmlTypeMapMemberAnyElement) member;
          if (member1.IsDefaultAny)
            this._defaultAnyElement = member1;
          if (member1.TypeData.IsListType)
            this.RegisterFlatList((XmlTypeMapMemberExpandable) member1);
        }
        else
        {
          if (member is XmlTypeMapMemberAnyAttribute)
          {
            this._defaultAnyAttribute = (XmlTypeMapMemberAnyAttribute) member;
            return;
          }
          if (member is XmlTypeMapMemberNamespaces)
          {
            this._namespaceDeclarations = (XmlTypeMapMemberNamespaces) member;
            return;
          }
        }
        if (member is XmlTypeMapMemberElement && ((XmlTypeMapMemberElement) member).IsXmlTextCollector)
          this._xmlTextCollector = this._xmlTextCollector == null ? member : throw new InvalidOperationException("XmlTextAttribute can only be applied once in a class");
        if (this._elementMembers == null)
        {
          this._elementMembers = new ArrayList();
          this._elements = new Hashtable();
        }
        member.Index = this._elementMembers.Count;
        this._elementMembers.Add((object) member);
        foreach (XmlTypeMapElementInfo typeMapElementInfo in (IEnumerable) ((XmlTypeMapMemberElement) member).ElementInfo)
        {
          string key = this.BuildKey(typeMapElementInfo.ElementName, typeMapElementInfo.Namespace);
          if (this._elements.ContainsKey((object) key))
            throw new InvalidOperationException("The XML element named '" + typeMapElementInfo.ElementName + "' from namespace '" + typeMapElementInfo.Namespace + "' is already present in the current scope. Use XML attributes to specify another XML name or namespace for the element.");
          this._elements.Add((object) key, (object) typeMapElementInfo);
        }
        if (!member.TypeData.IsListType || member.TypeData.Type == null || member.TypeData.Type.IsArray)
          return;
        if (this._listMembers == null)
          this._listMembers = new ArrayList();
        this._listMembers.Add((object) member);
      }
    }

    private void RegisterFlatList(XmlTypeMapMemberExpandable member)
    {
      if (this._flatLists == null)
        this._flatLists = new ArrayList();
      member.FlatArrayIndex = this._flatLists.Count;
      this._flatLists.Add((object) member);
    }

    public XmlTypeMapMemberAttribute GetAttribute(string name, string ns) => this._attributeMembers == null ? (XmlTypeMapMemberAttribute) null : (XmlTypeMapMemberAttribute) this._attributeMembers[(object) this.BuildKey(name, ns)];

    public XmlTypeMapElementInfo GetElement(string name, string ns) => this._elements == null ? (XmlTypeMapElementInfo) null : (XmlTypeMapElementInfo) this._elements[(object) this.BuildKey(name, ns)];

    public XmlTypeMapElementInfo GetElement(int index)
    {
      if (this._elements == null)
        return (XmlTypeMapElementInfo) null;
      if (this._elementsByIndex == null)
      {
        this._elementsByIndex = new XmlTypeMapElementInfo[this._elementMembers.Count];
        foreach (XmlTypeMapMemberElement elementMember in this._elementMembers)
        {
          if (elementMember.ElementInfo.Count != 1)
            throw new InvalidOperationException("Read by order only possible for encoded/bare format");
          this._elementsByIndex[elementMember.Index] = (XmlTypeMapElementInfo) elementMember.ElementInfo[0];
        }
      }
      return this._elementsByIndex[index];
    }

    private string BuildKey(string name, string ns) => this._ignoreMemberNamespace ? name : name + " / " + ns;

    public ICollection AllElementInfos => this._elements.Values;

    public bool IgnoreMemberNamespace
    {
      get => this._ignoreMemberNamespace;
      set => this._ignoreMemberNamespace = value;
    }

    public XmlTypeMapMember FindMember(string name)
    {
      for (int index = 0; index < this._allMembers.Count; ++index)
      {
        if (((XmlTypeMapMember) this._allMembers[index]).Name == name)
          return (XmlTypeMapMember) this._allMembers[index];
      }
      return (XmlTypeMapMember) null;
    }

    public XmlTypeMapMemberAnyElement DefaultAnyElementMember => this._defaultAnyElement;

    public XmlTypeMapMemberAnyAttribute DefaultAnyAttributeMember => this._defaultAnyAttribute;

    public XmlTypeMapMemberNamespaces NamespaceDeclarations => this._namespaceDeclarations;

    public ICollection AttributeMembers
    {
      get
      {
        if (this._attributeMembers == null)
          return (ICollection) null;
        if (this._attributeMembersArray != null)
          return (ICollection) this._attributeMembersArray;
        this._attributeMembersArray = new XmlTypeMapMemberAttribute[this._attributeMembers.Count];
        foreach (XmlTypeMapMemberAttribute mapMemberAttribute in (IEnumerable) this._attributeMembers.Values)
          this._attributeMembersArray[mapMemberAttribute.Index] = mapMemberAttribute;
        return (ICollection) this._attributeMembersArray;
      }
    }

    public ICollection ElementMembers => (ICollection) this._elementMembers;

    public ArrayList AllMembers => this._allMembers;

    public ArrayList FlatLists => this._flatLists;

    public ArrayList MembersWithDefault => this._membersWithDefault;

    public ArrayList ListMembers => this._listMembers;

    public XmlTypeMapMember XmlTextCollector => this._xmlTextCollector;

    public XmlTypeMapMember ReturnMember => this._returnMember;

    public XmlQualifiedName SimpleContentBaseType
    {
      get
      {
        if (!this._canBeSimpleType || this._elementMembers == null || this._elementMembers.Count != 1)
          return (XmlQualifiedName) null;
        XmlTypeMapMemberElement elementMember = (XmlTypeMapMemberElement) this._elementMembers[0];
        if (elementMember.ElementInfo.Count != 1)
          return (XmlQualifiedName) null;
        XmlTypeMapElementInfo typeMapElementInfo = (XmlTypeMapElementInfo) elementMember.ElementInfo[0];
        if (!typeMapElementInfo.IsTextElement)
          return (XmlQualifiedName) null;
        return elementMember.TypeData.SchemaType == SchemaTypes.Primitive || elementMember.TypeData.SchemaType == SchemaTypes.Enum ? new XmlQualifiedName(typeMapElementInfo.TypeData.XmlType, typeMapElementInfo.DataTypeNamespace) : (XmlQualifiedName) null;
      }
    }

    public void SetCanBeSimpleType(bool can) => this._canBeSimpleType = can;

    public bool HasSimpleContent => this.SimpleContentBaseType != (XmlQualifiedName) null;
  }
}
