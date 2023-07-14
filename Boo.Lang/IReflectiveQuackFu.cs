// Decompiled with JetBrains decompiler
// Type: Boo.Lang.IReflectiveQuackFu
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System.Collections.Generic;

namespace Boo.Lang
{
  public interface IReflectiveQuackFu : IQuackFu
  {
    IEnumerable<IQuackFuMember> QuackGetMembers();
  }
}
