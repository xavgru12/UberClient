// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.FunctionArguments
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.XPath
{
  internal class FunctionArguments
  {
    protected Expression _arg;
    protected FunctionArguments _tail;

    public FunctionArguments(Expression arg, FunctionArguments tail)
    {
      this._arg = arg;
      this._tail = tail;
    }

    public Expression Arg => this._arg;

    public FunctionArguments Tail => this._tail;

    public void ToArrayList(ArrayList a)
    {
      FunctionArguments functionArguments = this;
      do
      {
        a.Add((object) functionArguments._arg);
        functionArguments = functionArguments._tail;
      }
      while (functionArguments != null);
    }
  }
}
