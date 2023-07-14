// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Runtime.DynamicDispatching.Emitters.DispatcherEmitter
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Boo.Lang.Runtime.DynamicDispatching.Emitters
{
  public abstract class DispatcherEmitter
  {
    private DynamicMethod _dynamicMethod;
    protected readonly ILGenerator _il;

    public DispatcherEmitter(Type owner, string dynamicMethodName)
    {
      this._dynamicMethod = new DynamicMethod(owner.Name + "$" + dynamicMethodName, typeof (object), new Type[2]
      {
        typeof (object),
        typeof (object[])
      }, owner);
      this._il = this._dynamicMethod.GetILGenerator();
    }

    public Dispatcher Emit()
    {
      this.EmitMethodBody();
      return this.CreateMethodDispatcher();
    }

    protected abstract void EmitMethodBody();

    protected Dispatcher CreateMethodDispatcher() => (Dispatcher) this._dynamicMethod.CreateDelegate(typeof (Dispatcher));

    protected bool IsStobj(OpCode code) => (int) OpCodes.Stobj.Value == (int) code.Value;

    protected void EmitCastOrUnbox(Type type)
    {
      if (type.IsValueType)
      {
        this._il.Emit(OpCodes.Unbox, type);
        this._il.Emit(OpCodes.Ldobj, type);
      }
      else
        this._il.Emit(OpCodes.Castclass, type);
    }

    protected void BoxIfNeeded(Type returnType)
    {
      if (!returnType.IsValueType)
        return;
      this._il.Emit(OpCodes.Box, returnType);
    }

    protected void EmitLoadTargetObject(Type expectedType)
    {
      this._il.Emit(OpCodes.Ldarg_0);
      if (expectedType.IsValueType)
        this._il.Emit(OpCodes.Unbox, expectedType);
      else
        this._il.Emit(OpCodes.Castclass, expectedType);
    }

    protected void EmitReturn(Type typeOnStack)
    {
      if (typeOnStack == typeof (void))
        this._il.Emit(OpCodes.Ldnull);
      else
        this.BoxIfNeeded(typeOnStack);
      this._il.Emit(OpCodes.Ret);
    }

    protected void EmitPromotion(Type expectedType, Type actualType)
    {
      this._il.Emit(OpCodes.Unbox_Any, actualType);
      this._il.Emit(DispatcherEmitter.NumericPromotionOpcodeFor(Type.GetTypeCode(expectedType), true));
    }

    private static OpCode NumericPromotionOpcodeFor(TypeCode typeCode, bool @checked)
    {
      switch (typeCode)
      {
        case TypeCode.Char:
        case TypeCode.UInt16:
          return @checked ? OpCodes.Conv_Ovf_U2 : OpCodes.Conv_U2;
        case TypeCode.SByte:
          return @checked ? OpCodes.Conv_Ovf_I1 : OpCodes.Conv_I1;
        case TypeCode.Byte:
          return @checked ? OpCodes.Conv_Ovf_U1 : OpCodes.Conv_U1;
        case TypeCode.Int16:
          return @checked ? OpCodes.Conv_Ovf_I2 : OpCodes.Conv_I2;
        case TypeCode.Int32:
          return @checked ? OpCodes.Conv_Ovf_I4 : OpCodes.Conv_I4;
        case TypeCode.UInt32:
          return @checked ? OpCodes.Conv_Ovf_U4 : OpCodes.Conv_U4;
        case TypeCode.Int64:
          return @checked ? OpCodes.Conv_Ovf_I8 : OpCodes.Conv_I8;
        case TypeCode.UInt64:
          return @checked ? OpCodes.Conv_Ovf_U8 : OpCodes.Conv_U8;
        case TypeCode.Single:
          return OpCodes.Conv_R4;
        case TypeCode.Double:
          return OpCodes.Conv_R8;
        default:
          throw new ArgumentException(typeCode.ToString());
      }
    }

    protected void EmitArgArrayElement(int argumentIndex)
    {
      this._il.Emit(OpCodes.Ldarg_1);
      this._il.Emit(OpCodes.Ldc_I4, argumentIndex);
      this._il.Emit(OpCodes.Ldelem_Ref);
    }

    private MethodInfo GetPromotionMethod(Type type) => typeof (IConvertible).GetMethod("To" + (object) Type.GetTypeCode(type));

    protected void Dup() => this._il.Emit(OpCodes.Dup);

    protected void EmitCoercion(Type actualType, Type expectedType, int score)
    {
      switch (score)
      {
        case 3:
        case 5:
          this.EmitPromotion(expectedType, actualType);
          break;
        case 4:
          this.EmitCastOrUnbox(actualType);
          this._il.Emit(OpCodes.Call, RuntimeServices.FindImplicitConversionOperator(actualType, expectedType));
          break;
        default:
          this.EmitCastOrUnbox(expectedType);
          break;
      }
    }

    protected void LoadLocal(LocalBuilder value) => this._il.Emit(OpCodes.Ldloc, value);

    protected void StoreLocal(LocalBuilder value) => this._il.Emit(OpCodes.Stloc, value);

    protected LocalBuilder DeclareLocal(Type type) => this._il.DeclareLocal(type);
  }
}
