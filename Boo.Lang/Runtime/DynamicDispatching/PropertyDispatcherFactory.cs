// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Runtime.DynamicDispatching.PropertyDispatcherFactory
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using Boo.Lang.Runtime.DynamicDispatching.Emitters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Boo.Lang.Runtime.DynamicDispatching
{
  public class PropertyDispatcherFactory : AbstractDispatcherFactory
  {
    public PropertyDispatcherFactory(
      ExtensionRegistry extensions,
      object target,
      Type type,
      string name,
      params object[] arguments)
      : base(extensions, target, type, name, arguments)
    {
    }

    public Dispatcher CreateSetter() => this.Create(SetOrGet.Set);

    public Dispatcher CreateGetter() => this.Create(SetOrGet.Get);

    private Dispatcher Create(SetOrGet gos)
    {
      MemberInfo[] member = this._type.GetMember(this._name, MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.OptionalParamBinding);
      if (member.Length == 0)
        return this.FindExtension(this.GetCandidateExtensions(gos));
      return member.Length <= 1 ? this.EmitDispatcherFor(member[0], gos) : throw new AmbiguousMatchException(Builtins.join((IEnumerable) member, ", "));
    }

    private Dispatcher FindExtension(IEnumerable<MethodInfo> candidates) => this.EmitExtensionDispatcher(this.ResolveExtension(candidates) ?? throw this.MissingField());

    [DebuggerHidden]
    private IEnumerable<MethodInfo> GetCandidateExtensions(SetOrGet gos)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PropertyDispatcherFactory.\u003CGetCandidateExtensions\u003Ec__Iterator9 candidateExtensions = new PropertyDispatcherFactory.\u003CGetCandidateExtensions\u003Ec__Iterator9()
      {
        gos = gos,
        \u003C\u0024\u003Egos = gos,
        \u003C\u003Ef__this = this
      };
      // ISSUE: reference to a compiler-generated field
      candidateExtensions.\u0024PC = -2;
      return (IEnumerable<MethodInfo>) candidateExtensions;
    }

    private static MethodInfo Accessor(PropertyInfo p, SetOrGet gos) => gos == SetOrGet.Get ? p.GetGetMethod(true) : p.GetSetMethod(true);

    private Dispatcher EmitDispatcherFor(MemberInfo info, SetOrGet gos) => info.MemberType == MemberTypes.Property ? this.EmitPropertyDispatcher((PropertyInfo) info, gos) : this.EmitFieldDispatcher((FieldInfo) info, gos);

    private Dispatcher EmitFieldDispatcher(FieldInfo field, SetOrGet gos) => gos == SetOrGet.Get ? new GetFieldEmitter(field).Emit() : new SetFieldEmitter(field, this.GetArgumentTypes()[0]).Emit();

    private Dispatcher EmitPropertyDispatcher(PropertyInfo property, SetOrGet gos)
    {
      Type[] argumentTypes = this.GetArgumentTypes();
      CandidateMethod found = AbstractDispatcherFactory.ResolveMethod(argumentTypes, (IEnumerable<MethodInfo>) new MethodInfo[1]
      {
        PropertyDispatcherFactory.Accessor(property, gos) ?? throw this.MissingField()
      });
      if (found == null)
        throw this.MissingField();
      return gos == SetOrGet.Get ? new MethodDispatcherEmitter(this._type, found, argumentTypes).Emit() : new SetPropertyEmitter(this._type, found, argumentTypes).Emit();
    }
  }
}
