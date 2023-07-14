// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslComment
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.XPath;

namespace Mono.Xml.Xsl.Operations
{
  internal class XslComment : XslCompiledElement
  {
    private XslOperation value;

    public XslComment(Compiler c)
      : base(c)
    {
    }

    protected override void Compile(Compiler c)
    {
      if (c.Debugger != null)
        c.Debugger.DebugCompile(c.Input);
      c.CheckExtraAttributes("comment");
      if (!c.Input.MoveToFirstChild())
        return;
      this.value = c.CompileTemplateContent(XPathNodeType.Comment);
      c.Input.MoveToParent();
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      p.Out.WriteComment(this.value != null ? this.value.EvaluateAsString(p) : string.Empty);
    }
  }
}
