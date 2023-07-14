// Decompiled with JetBrains decompiler
// Type: Mono.Xml.XPath.yydebug.yyDebugSimple
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;

namespace Mono.Xml.XPath.yydebug
{
  internal class yyDebugSimple : yyDebug
  {
    private void println(string s) => Console.Error.WriteLine(s);

    public void push(int state, object value) => this.println("push\tstate " + (object) state + "\tvalue " + value);

    public void lex(int state, int token, string name, object value) => this.println("lex\tstate " + (object) state + "\treading " + name + "\tvalue " + value);

    public void shift(int from, int to, int errorFlag)
    {
      switch (errorFlag)
      {
        case 0:
        case 1:
        case 2:
          this.println("shift\tfrom state " + (object) from + " to " + (object) to + "\t" + (object) errorFlag + " left to recover");
          break;
        case 3:
          this.println("shift\tfrom state " + (object) from + " to " + (object) to + "\ton error");
          break;
        default:
          this.println("shift\tfrom state " + (object) from + " to " + (object) to);
          break;
      }
    }

    public void pop(int state) => this.println("pop\tstate " + (object) state + "\ton error");

    public void discard(int state, int token, string name, object value) => this.println("discard\tstate " + (object) state + "\ttoken " + name + "\tvalue " + value);

    public void reduce(int from, int to, int rule, string text, int len) => this.println("reduce\tstate " + (object) from + "\tuncover " + (object) to + "\trule (" + (object) rule + ") " + text);

    public void shift(int from, int to) => this.println("goto\tfrom state " + (object) from + " to " + (object) to);

    public void accept(object value) => this.println("accept\tvalue " + value);

    public void error(string message) => this.println("error\t" + message);

    public void reject() => this.println(nameof (reject));
  }
}
