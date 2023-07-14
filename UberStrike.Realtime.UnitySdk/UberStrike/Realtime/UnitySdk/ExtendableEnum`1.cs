// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.ExtendableEnum`1
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System.Collections.Generic;
using System.Reflection;

namespace UberStrike.Realtime.UnitySdk
{
  public abstract class ExtendableEnum<T>
  {
    private List<T> _allMembers;
    private List<T> _allDeclaredMembers;

    public ExtendableEnum()
    {
      this._allMembers = new List<T>();
      this._allDeclaredMembers = new List<T>();
      this.PopulateList(this._allMembers, this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy));
      this.PopulateList(this._allDeclaredMembers, this.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy));
    }

    public IEnumerable<T> AllValues => (IEnumerable<T>) this._allMembers;

    public IEnumerable<T> AllDeclaredValues => (IEnumerable<T>) this._allDeclaredMembers;

    public bool IsDefined(T b) => this._allMembers.Contains(b);

    private void PopulateList(List<T> list, FieldInfo[] fields)
    {
      foreach (FieldInfo field in fields)
      {
        if (field.FieldType == typeof (T))
        {
          T obj = (T) field.GetValue((object) this);
          list.Add(obj);
        }
      }
    }
  }
}
