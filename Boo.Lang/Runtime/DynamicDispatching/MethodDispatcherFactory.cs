// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Runtime.DynamicDispatching.MethodDispatcherFactory
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
  public class MethodDispatcherFactory : AbstractDispatcherFactory
  {
    public MethodDispatcherFactory(
      ExtensionRegistry extensions,
      object target,
      Type type,
      string methodName,
      params object[] arguments)
      : base(extensions, target, type, methodName, arguments)
    {
    }

    public Dispatcher Create()
    {
      Type[] argumentTypes = this.GetArgumentTypes();
      CandidateMethod found = this.ResolveMethod(argumentTypes);
      return found != null ? this.EmitMethodDispatcher(found, argumentTypes) : this.ProduceExtensionDispatcher();
    }

    private Dispatcher ProduceExtensionDispatcher() => this.EmitExtensionDispatcher(this.ResolveExtensionMethod() ?? throw new MissingMethodException(this._type.FullName, this._name));

    private CandidateMethod ResolveExtensionMethod() => this.ResolveExtension(this.GetExtensionMethods());

    private CandidateMethod ResolveMethod(Type[] argumentTypes) => AbstractDispatcherFactory.ResolveMethod(argumentTypes, this.GetCandidates());

    [DebuggerHidden]
    private IEnumerable<MethodInfo> GetCandidates()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MethodDispatcherFactory.\u003CGetCandidates\u003Ec__Iterator8 candidates = new MethodDispatcherFactory.\u003CGetCandidates\u003Ec__Iterator8()
      {
        \u003C\u003Ef__this = this
      };
      // ISSUE: reference to a compiler-generated field
      candidates.\u0024PC = -2;
      return (IEnumerable<MethodInfo>) candidates;
    }

    private Dispatcher EmitMethodDispatcher(CandidateMethod found, Type[] argumentTypes) => new MethodDispatcherEmitter(this._type, found, argumentTypes).Emit();
  }
}
