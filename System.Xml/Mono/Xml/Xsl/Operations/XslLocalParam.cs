// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslLocalParam
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace Mono.Xml.Xsl.Operations
{
  internal class XslLocalParam : XslLocalVariable
  {
    public XslLocalParam(Compiler c)
      : base(c)
    {
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      if (p.GetStackItem(this.slot) != null)
        return;
      if (p.Arguments != null && this.var.Select == null && this.var.Content == null)
      {
        object paramVal = p.Arguments.GetParam(this.Name.Name, this.Name.Namespace);
        if (paramVal != null)
        {
          this.Override(p, paramVal);
          return;
        }
      }
      base.Evaluate(p);
    }

    public void Override(XslTransformProcessor p, object paramVal) => p.SetStackItem(this.slot, paramVal);

    public override bool IsParam => true;
  }
}
