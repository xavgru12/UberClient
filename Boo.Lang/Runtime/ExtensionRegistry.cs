// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Runtime.ExtensionRegistry
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Boo.Lang.Runtime
{
  public class ExtensionRegistry
  {
    private Boo.Lang.List<MemberInfo> _extensions = new Boo.Lang.List<MemberInfo>();
    private object _classLock = new object();

    public void Register(Type type)
    {
      lock (this._classLock)
        this._extensions = ExtensionRegistry.AddExtensionMembers(this.CopyExtensions(), type);
    }

    public IEnumerable<MemberInfo> Extensions => (IEnumerable<MemberInfo>) this._extensions;

    public void UnRegister(Type type)
    {
      lock (this._classLock)
      {
        Boo.Lang.List<MemberInfo> list = this.CopyExtensions();
        list.RemoveAll((Predicate<MemberInfo>) (member => member.DeclaringType == type));
        this._extensions = list;
      }
    }

    private static Boo.Lang.List<MemberInfo> AddExtensionMembers(
      Boo.Lang.List<MemberInfo> extensions,
      Type type)
    {
      foreach (MemberInfo member in type.GetMembers(BindingFlags.Static | BindingFlags.Public))
      {
        if (Attribute.IsDefined(member, typeof (ExtensionAttribute)) && !extensions.Contains(member))
          extensions.Add(member);
      }
      return extensions;
    }

    private Boo.Lang.List<MemberInfo> CopyExtensions() => new Boo.Lang.List<MemberInfo>((IEnumerable) this._extensions);
  }
}
