// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Runtime.DynamicDispatching.Emitters.GetFieldEmitter
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Boo.Lang.Runtime.DynamicDispatching.Emitters
{
  internal class GetFieldEmitter : DispatcherEmitter
  {
    protected readonly FieldInfo _field;

    public GetFieldEmitter(FieldInfo field)
      : base(field.DeclaringType, field.Name)
    {
      this._field = field;
    }

    protected override void EmitMethodBody()
    {
      if (this._field.IsStatic)
      {
        RuntimeHelpers.RunClassConstructor(this._field.DeclaringType.TypeHandle);
        this._il.Emit(OpCodes.Ldsfld, this._field);
      }
      else
      {
        this.EmitLoadTargetObject(this._field.DeclaringType);
        this._il.Emit(OpCodes.Ldfld, this._field);
      }
      this.EmitReturn(this._field.FieldType);
    }
  }
}
