// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Runtime.DynamicDispatching.Emitters.ExtensionMethodDispatcherEmitter
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;
using System.Reflection.Emit;

namespace Boo.Lang.Runtime.DynamicDispatching.Emitters
{
  internal class ExtensionMethodDispatcherEmitter : MethodDispatcherEmitter
  {
    public ExtensionMethodDispatcherEmitter(CandidateMethod found, Type[] argumentTypes)
      : base(found, argumentTypes)
    {
    }

    protected override void EmitLoadTargetObject()
    {
      this._il.Emit(OpCodes.Ldarg_0);
      this.EmitCastOrUnbox(this._found.GetParameterType(0));
    }

    protected override int FixedArgumentOffset => 1;
  }
}
