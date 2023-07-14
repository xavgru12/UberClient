// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Runtime.DynamicDispatching.SliceDispatcherFactory
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using Boo.Lang.Runtime.DynamicDispatching.Emitters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Boo.Lang.Runtime.DynamicDispatching
{
  internal class SliceDispatcherFactory : AbstractDispatcherFactory
  {
    public SliceDispatcherFactory(
      ExtensionRegistry extensions,
      object target,
      Type type,
      string name,
      params object[] arguments)
      : base(extensions, target, type, name.Length != 0 ? name : RuntimeServices.GetDefaultMemberName(type), arguments)
    {
    }

    public Dispatcher CreateGetter()
    {
      MemberInfo[] candidates = this.ResolveMember();
      return candidates.Length == 1 ? this.CreateGetter(candidates[0]) : this.EmitMethodDispatcher(this.Getters(candidates));
    }

    [DebuggerHidden]
    private IEnumerable<MethodInfo> Getters(MemberInfo[] candidates)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SliceDispatcherFactory.\u003CGetters\u003Ec__IteratorA gettersCIteratorA = new SliceDispatcherFactory.\u003CGetters\u003Ec__IteratorA()
      {
        candidates = candidates,
        \u003C\u0024\u003Ecandidates = candidates
      };
      // ISSUE: reference to a compiler-generated field
      gettersCIteratorA.\u0024PC = -2;
      return (IEnumerable<MethodInfo>) gettersCIteratorA;
    }

    private Dispatcher CreateGetter(MemberInfo member)
    {
      switch (member.MemberType)
      {
        case MemberTypes.Field:
          FieldInfo field = (FieldInfo) member;
          return (Dispatcher) ((o, arguments) => RuntimeServices.GetSlice(field.GetValue(o), string.Empty, arguments));
        case MemberTypes.Property:
          MethodInfo getter = ((PropertyInfo) member).GetGetMethod(true);
          if (getter == null)
            throw this.MissingField();
          return getter.GetParameters().Length > 0 ? this.EmitMethodDispatcher(getter) : (Dispatcher) ((o, arguments) => RuntimeServices.GetSlice(getter.Invoke(o, (object[]) null), string.Empty, arguments));
        default:
          throw this.MissingField();
      }
    }

    private Dispatcher EmitMethodDispatcher(MethodInfo candidate) => this.EmitMethodDispatcher((IEnumerable<MethodInfo>) new MethodInfo[1]
    {
      candidate
    });

    private Dispatcher EmitMethodDispatcher(IEnumerable<MethodInfo> candidates) => new MethodDispatcherEmitter(this._type, AbstractDispatcherFactory.ResolveMethod(this.GetArgumentTypes(), candidates) ?? throw this.MissingField(), this.GetArgumentTypes()).Emit();

    private MemberInfo[] ResolveMember()
    {
      MemberInfo[] member = this._type.GetMember(this._name, MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.OptionalParamBinding);
      return member.Length != 0 ? member : throw this.MissingField();
    }

    public Dispatcher CreateSetter()
    {
      MemberInfo[] candidates = this.ResolveMember();
      return candidates.Length == 1 ? this.CreateSetter(candidates[0]) : this.EmitMethodDispatcher(this.Setters(candidates));
    }

    [DebuggerHidden]
    private IEnumerable<MethodInfo> Setters(MemberInfo[] candidates)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SliceDispatcherFactory.\u003CSetters\u003Ec__IteratorB settersCIteratorB = new SliceDispatcherFactory.\u003CSetters\u003Ec__IteratorB()
      {
        candidates = candidates,
        \u003C\u0024\u003Ecandidates = candidates
      };
      // ISSUE: reference to a compiler-generated field
      settersCIteratorB.\u0024PC = -2;
      return (IEnumerable<MethodInfo>) settersCIteratorB;
    }

    private Dispatcher CreateSetter(MemberInfo member)
    {
      switch (member.MemberType)
      {
        case MemberTypes.Field:
          FieldInfo field = (FieldInfo) member;
          return (Dispatcher) ((o, arguments) => RuntimeServices.SetSlice(field.GetValue(o), string.Empty, arguments));
        case MemberTypes.Property:
          PropertyInfo propertyInfo = (PropertyInfo) member;
          if (propertyInfo.GetIndexParameters().Length <= 0)
            return (Dispatcher) ((o, arguments) => RuntimeServices.SetSlice(RuntimeServices.GetProperty(o, this._name), string.Empty, arguments));
          return this.EmitMethodDispatcher(propertyInfo.GetSetMethod(true) ?? throw this.MissingField());
        default:
          throw this.MissingField();
      }
    }
  }
}
