﻿// Decompiled with JetBrains decompiler
// Type: Cmune.Core.Types.ExtendableEnum`1
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System.Collections.Generic;
using System.Reflection;

namespace Cmune.Core.Types
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
        if ((object) field.FieldType == (object) typeof (T))
        {
          T obj = (T) field.GetValue((object) this);
          list.Add(obj);
        }
      }
    }
  }
}
