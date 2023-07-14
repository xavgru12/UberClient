// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Runtime.DynamicDispatching.Emitters.MethodDispatcherEmitter
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;
using System.Collections;
using System.Reflection.Emit;

namespace Boo.Lang.Runtime.DynamicDispatching.Emitters
{
  public class MethodDispatcherEmitter : DispatcherEmitter
  {
    protected readonly CandidateMethod _found;
    protected readonly Type[] _argumentTypes;

    public MethodDispatcherEmitter(CandidateMethod found, params Type[] argumentTypes)
      : this(found.Method.DeclaringType, found, argumentTypes)
    {
    }

    public MethodDispatcherEmitter(Type owner, CandidateMethod found, Type[] argumentTypes)
      : base(owner, found.Method.Name + "$" + Builtins.join((IEnumerable) argumentTypes, "$"))
    {
      this._found = found;
      this._argumentTypes = argumentTypes;
    }

    protected override void EmitMethodBody()
    {
      this.EmitInvocation();
      this.EmitMethodReturn();
    }

    protected void EmitInvocation()
    {
      this.EmitLoadTargetObject();
      this.EmitMethodArguments();
      this.EmitMethodCall();
    }

    protected void EmitMethodCall() => this._il.Emit(!this._found.Method.IsStatic ? OpCodes.Callvirt : OpCodes.Call, this._found.Method);

    protected void EmitMethodArguments()
    {
      this.EmitFixedMethodArguments();
      if (!this._found.VarArgs)
        return;
      this.EmitVarArgsMethodArguments();
    }

    private void EmitFixedMethodArguments()
    {
      int fixedArgumentOffset = this.FixedArgumentOffset;
      int num = this.MinimumArgumentCount();
      for (int argumentIndex = 0; argumentIndex < num; ++argumentIndex)
      {
        Type parameterType = this._found.GetParameterType(argumentIndex + fixedArgumentOffset);
        this.EmitMethodArgument(argumentIndex, parameterType);
      }
    }

    protected virtual int FixedArgumentOffset => 0;

    private void EmitVarArgsMethodArguments()
    {
      int num = this._argumentTypes.Length - this.MinimumArgumentCount();
      Type argsParameterType = this._found.VarArgsParameterType;
      OpCode storeElementOpCode = MethodDispatcherEmitter.GetStoreElementOpCode(argsParameterType);
      this._il.Emit(OpCodes.Ldc_I4, num);
      this._il.Emit(OpCodes.Newarr, argsParameterType);
      for (int index = 0; index < num; ++index)
      {
        this.Dup();
        this._il.Emit(OpCodes.Ldc_I4, index);
        if (this.IsStobj(storeElementOpCode))
        {
          this._il.Emit(OpCodes.Ldelema, argsParameterType);
          this.EmitMethodArgument(this.MinimumArgumentCount() + index, argsParameterType);
          this._il.Emit(storeElementOpCode, argsParameterType);
        }
        else
        {
          this.EmitMethodArgument(this.MinimumArgumentCount() + index, argsParameterType);
          this._il.Emit(storeElementOpCode);
        }
      }
    }

    private int MinimumArgumentCount() => this._found.MinimumArgumentCount - this.FixedArgumentOffset;

    private static OpCode GetStoreElementOpCode(Type type)
    {
      if (!type.IsValueType)
        return OpCodes.Stelem_Ref;
      if (type.IsEnum)
        return OpCodes.Stelem_I4;
      switch (Type.GetTypeCode(type))
      {
        case TypeCode.Byte:
          return OpCodes.Stelem_I1;
        case TypeCode.Int16:
          return OpCodes.Stelem_I2;
        case TypeCode.Int32:
          return OpCodes.Stelem_I4;
        case TypeCode.Int64:
          return OpCodes.Stelem_I8;
        case TypeCode.Single:
          return OpCodes.Stelem_R4;
        case TypeCode.Double:
          return OpCodes.Stelem_R8;
        default:
          return OpCodes.Stobj;
      }
    }

    protected void EmitMethodArgument(int argumentIndex, Type expectedType)
    {
      this.EmitArgArrayElement(argumentIndex);
      this.EmitCoercion(argumentIndex, expectedType, this._found.ArgumentScores[argumentIndex]);
    }

    private void EmitCoercion(int argumentIndex, Type expectedType, int score) => this.EmitCoercion(this._argumentTypes[argumentIndex], expectedType, score);

    protected virtual void EmitLoadTargetObject()
    {
      if (this._found.Method.IsStatic)
        return;
      this.EmitLoadTargetObject(this._found.Method.DeclaringType);
    }

    private void EmitMethodReturn() => this.EmitReturn(this._found.Method.ReturnType);
  }
}
