// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Runtime.DynamicDispatching.Emitters.SetFieldEmitter
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Boo.Lang.Runtime.DynamicDispatching.Emitters
{
  internal class SetFieldEmitter : DispatcherEmitter
  {
    private readonly FieldInfo _field;
    private Type _argumentType;

    public SetFieldEmitter(FieldInfo field, Type argumentType)
      : base(field.DeclaringType, field.Name + "=")
    {
      this._field = field;
      this._argumentType = argumentType;
    }

    protected override void EmitMethodBody()
    {
      LocalBuilder localBuilder = this.DeclareLocal(this._field.FieldType);
      this.EmitLoadValue();
      this.StoreLocal(localBuilder);
      if (this._field.IsStatic)
      {
        this.LoadLocal(localBuilder);
        this._il.Emit(OpCodes.Stsfld, this._field);
      }
      else
      {
        this.EmitLoadTargetObject(this._field.DeclaringType);
        this.LoadLocal(localBuilder);
        this._il.Emit(OpCodes.Stfld, this._field);
      }
      this.LoadLocal(localBuilder);
      this.EmitReturn(this._field.FieldType);
    }

    private void EmitLoadValue()
    {
      this.EmitArgArrayElement(0);
      this.EmitCoercion(this._argumentType, this._field.FieldType, CandidateMethod.CalculateArgumentScore(this._field.FieldType, this._argumentType));
    }
  }
}
