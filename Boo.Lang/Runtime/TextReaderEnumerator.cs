// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Runtime.TextReaderEnumerator
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Boo.Lang.Runtime
{
  public class TextReaderEnumerator
  {
    [DebuggerHidden]
    public static IEnumerable<string> lines(TextReader reader)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TextReaderEnumerator.\u003Clines\u003Ec__IteratorD linesCIteratorD = new TextReaderEnumerator.\u003Clines\u003Ec__IteratorD()
      {
        reader = reader,
        \u003C\u0024\u003Ereader = reader
      };
      // ISSUE: reference to a compiler-generated field
      linesCIteratorD.\u0024PC = -2;
      return (IEnumerable<string>) linesCIteratorD;
    }
  }
}
