// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Runtime.DynamicDispatching.AbstractDispatcherFactory
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
  public abstract class AbstractDispatcherFactory
  {
    private readonly ExtensionRegistry _extensions;
    private readonly object _target;
    protected readonly Type _type;
    protected readonly string _name;
    private readonly object[] _arguments;

    public AbstractDispatcherFactory(
      ExtensionRegistry extensions,
      object target,
      Type type,
      string name,
      params object[] arguments)
    {
      this._extensions = extensions;
      this._target = target;
      this._type = type;
      this._name = name;
      this._arguments = arguments;
    }

    protected IEnumerable<MemberInfo> Extensions => this._extensions.Extensions;

    protected object[] GetExtensionArgs() => AbstractDispatcherFactory.AdjustExtensionArgs(this._target, this._arguments);

    private static object[] AdjustExtensionArgs(object target, object[] originalArguments)
    {
      if (originalArguments == null)
        return new object[1]{ target };
      object[] destinationArray = new object[originalArguments.Length + 1];
      destinationArray[0] = target;
      Array.Copy((Array) originalArguments, 0, (Array) destinationArray, 1, originalArguments.Length);
      return destinationArray;
    }

    protected Type[] GetArgumentTypes() => MethodResolver.GetArgumentTypes(this._arguments);

    protected Type[] GetExtensionArgumentTypes() => MethodResolver.GetArgumentTypes(this.GetExtensionArgs());

    protected Dispatcher EmitExtensionDispatcher(CandidateMethod found) => new ExtensionMethodDispatcherEmitter(found, this.GetArgumentTypes()).Emit();

    protected CandidateMethod ResolveExtension(IEnumerable<MethodInfo> candidates) => new MethodResolver(this.GetExtensionArgumentTypes()).ResolveMethod(candidates);

    protected IEnumerable<MethodInfo> GetExtensionMethods() => this.GetExtensions<MethodInfo>(MemberTypes.Method);

    [DebuggerHidden]
    protected IEnumerable<T> GetExtensions<T>(MemberTypes memberTypes) where T : MemberInfo
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AbstractDispatcherFactory.\u003CGetExtensions\u003Ec__Iterator7<T> extensions = new AbstractDispatcherFactory.\u003CGetExtensions\u003Ec__Iterator7<T>()
      {
        memberTypes = memberTypes,
        \u003C\u0024\u003EmemberTypes = memberTypes,
        \u003C\u003Ef__this = this
      };
      // ISSUE: reference to a compiler-generated field
      extensions.\u0024PC = -2;
      return (IEnumerable<T>) extensions;
    }

    protected static CandidateMethod ResolveMethod(
      Type[] argumentTypes,
      IEnumerable<MethodInfo> candidates)
    {
      return new MethodResolver(argumentTypes).ResolveMethod(candidates);
    }

    protected MissingFieldException MissingField() => new MissingFieldException(this._type.FullName, this._name);
  }
}
