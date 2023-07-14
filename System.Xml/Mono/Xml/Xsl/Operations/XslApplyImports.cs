// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslApplyImports
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace Mono.Xml.Xsl.Operations
{
  internal class XslApplyImports : XslCompiledElement
  {
    public XslApplyImports(Compiler c)
      : base(c)
    {
    }

    protected override void Compile(Compiler c)
    {
      c.CheckExtraAttributes("apply-imports");
      if (c.Debugger == null)
        return;
      c.Debugger.DebugCompile(c.Input);
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      p.ApplyImports();
    }
  }
}
