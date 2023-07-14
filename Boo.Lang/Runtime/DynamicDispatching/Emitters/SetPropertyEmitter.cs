// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Runtime.DynamicDispatching.Emitters.SetPropertyEmitter
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Boo.Lang.Runtime.DynamicDispatching.Emitters
{
  internal class SetPropertyEmitter : MethodDispatcherEmitter
  {
    public SetPropertyEmitter(Type type, CandidateMethod found, Type[] argumentTypes)
      : base(type, found, argumentTypes)
    {
    }

    protected override void EmitMethodBody()
    {
      Type valueType = this.GetValueType();
      LocalBuilder localBuilder = this.DeclareLocal(valueType);
      this.EmitLoadTargetObject();
      this.EmitMethodArguments();
      this.Dup();
      this.StoreLocal(localBuilder);
      this.EmitMethodCall();
      this.LoadLocal(localBuilder);
      this.EmitReturn(valueType);
    }

    private Type GetValueType()
    {
      ParameterInfo[] parameters = this._found.Parameters;
      return parameters[parameters.Length - 1].ParameterType;
    }
  }
}
