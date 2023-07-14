// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlTypeMapMember
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Reflection;

namespace System.Xml.Serialization
{
  internal class XmlTypeMapMember
  {
    private const int OPTIONAL = 1;
    private const int RETURN_VALUE = 2;
    private const int IGNORE = 4;
    private string _name;
    private int _index;
    private int _globalIndex;
    private TypeData _typeData;
    private MemberInfo _member;
    private MemberInfo _specifiedMember;
    private object _defaultValue = (object) DBNull.Value;
    private string documentation;
    private int _flags;

    public string Name
    {
      get => this._name;
      set => this._name = value;
    }

    public object DefaultValue
    {
      get => this._defaultValue;
      set => this._defaultValue = value;
    }

    public string Documentation
    {
      set => this.documentation = value;
      get => this.documentation;
    }

    public bool IsReadOnly(Type type)
    {
      if (this._member == null)
        this.InitMember(type);
      return this._member is PropertyInfo && !((PropertyInfo) this._member).CanWrite;
    }

    public static object GetValue(object ob, string name)
    {
      MemberInfo[] member = ob.GetType().GetMember(name, BindingFlags.Instance | BindingFlags.Public);
      return member[0] is PropertyInfo ? ((PropertyInfo) member[0]).GetValue(ob, (object[]) null) : ((FieldInfo) member[0]).GetValue(ob);
    }

    public object GetValue(object ob)
    {
      if (this._member == null)
        this.InitMember(ob.GetType());
      return this._member is PropertyInfo ? ((PropertyInfo) this._member).GetValue(ob, (object[]) null) : ((FieldInfo) this._member).GetValue(ob);
    }

    public void SetValue(object ob, object value)
    {
      if (this._member == null)
        this.InitMember(ob.GetType());
      if (this._member is PropertyInfo)
        ((PropertyInfo) this._member).SetValue(ob, value, (object[]) null);
      else
        ((FieldInfo) this._member).SetValue(ob, value);
    }

    public static void SetValue(object ob, string name, object value)
    {
      MemberInfo[] member = ob.GetType().GetMember(name, BindingFlags.Instance | BindingFlags.Public);
      if (member[0] is PropertyInfo)
        ((PropertyInfo) member[0]).SetValue(ob, value, (object[]) null);
      else
        ((FieldInfo) member[0]).SetValue(ob, value);
    }

    private void InitMember(Type type)
    {
      this._member = type.GetMember(this._name, BindingFlags.Instance | BindingFlags.Public)[0];
      MemberInfo[] member = type.GetMember(this._name + "Specified", BindingFlags.Instance | BindingFlags.Public);
      if (member.Length > 0)
        this._specifiedMember = member[0];
      if (!(this._specifiedMember is PropertyInfo) || ((PropertyInfo) this._specifiedMember).CanWrite)
        return;
      this._specifiedMember = (MemberInfo) null;
    }

    public TypeData TypeData
    {
      get => this._typeData;
      set => this._typeData = value;
    }

    public int Index
    {
      get => this._index;
      set => this._index = value;
    }

    public int GlobalIndex
    {
      get => this._globalIndex;
      set => this._globalIndex = value;
    }

    public bool IsOptionalValueType
    {
      get => (this._flags & 1) != 0;
      set => this._flags = !value ? this._flags & -2 : this._flags | 1;
    }

    public bool IsReturnValue
    {
      get => (this._flags & 2) != 0;
      set => this._flags = !value ? this._flags & -3 : this._flags | 2;
    }

    public bool Ignore
    {
      get => (this._flags & 4) != 0;
      set => this._flags = !value ? this._flags & -5 : this._flags | 4;
    }

    public void CheckOptionalValueType(Type type)
    {
      if (this._member == null)
        this.InitMember(type);
      this.IsOptionalValueType = this._specifiedMember != null;
    }

    public bool GetValueSpecified(object ob) => this._specifiedMember is PropertyInfo ? (bool) ((PropertyInfo) this._specifiedMember).GetValue(ob, (object[]) null) : (bool) ((FieldInfo) this._specifiedMember).GetValue(ob);

    public void SetValueSpecified(object ob, bool value)
    {
      if (this._specifiedMember is PropertyInfo)
        ((PropertyInfo) this._specifiedMember).SetValue(ob, (object) value, (object[]) null);
      else
        ((FieldInfo) this._specifiedMember).SetValue(ob, (object) value);
    }

    public virtual bool RequiresNullable => false;
  }
}
