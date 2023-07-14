// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslLocalVariable
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace Mono.Xml.Xsl.Operations
{
  internal class XslLocalVariable : XslGeneralVariable
  {
    protected int slot;

    public XslLocalVariable(Compiler c)
      : base(c)
    {
      this.slot = c.AddVariable(this);
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      p.SetStackItem(this.slot, this.var.Evaluate(p));
    }

    protected override object GetValue(XslTransformProcessor p) => p.GetStackItem(this.slot);

    public bool IsEvaluated(XslTransformProcessor p) => p.GetStackItem(this.slot) != null;

    public override bool IsLocal => true;

    public override bool IsParam => false;
  }
}
