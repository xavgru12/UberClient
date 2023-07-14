// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslGlobalVariable
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
  internal class XslGlobalVariable : XslGeneralVariable
  {
    private static object busyObject = new object();

    public XslGlobalVariable(Compiler c)
      : base(c)
    {
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      Hashtable globalVariableTable = p.globalVariableTable;
      if (globalVariableTable.Contains((object) this))
      {
        if (globalVariableTable[(object) this] == XslGlobalVariable.busyObject)
          throw new XsltException("Circular dependency was detected", (Exception) null, p.CurrentNode);
      }
      else
      {
        globalVariableTable[(object) this] = XslGlobalVariable.busyObject;
        globalVariableTable[(object) this] = this.var.Evaluate(p);
      }
    }

    protected override object GetValue(XslTransformProcessor p)
    {
      this.Evaluate(p);
      return p.globalVariableTable[(object) this];
    }

    public override bool IsLocal => false;

    public override bool IsParam => false;
  }
}
