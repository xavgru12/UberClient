// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslProcessingInstruction
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Globalization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
  internal class XslProcessingInstruction : XslCompiledElement
  {
    private XslAvt name;
    private XslOperation value;

    public XslProcessingInstruction(Compiler c)
      : base(c)
    {
    }

    protected override void Compile(Compiler c)
    {
      if (c.Debugger != null)
        c.Debugger.DebugCompile(this.DebugInput);
      c.CheckExtraAttributes("processing-instruction", "name");
      this.name = c.ParseAvtAttribute("name");
      if (!c.Input.MoveToFirstChild())
        return;
      this.value = c.CompileTemplateContent(XPathNodeType.ProcessingInstruction);
      c.Input.MoveToParent();
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      string str = this.name.Evaluate(p);
      if (string.Compare(str, "xml", true, CultureInfo.InvariantCulture) == 0)
        throw new XsltException("Processing instruction name was evaluated to \"xml\"", (Exception) null, p.CurrentNode);
      if (str.IndexOf(':') >= 0)
        return;
      p.Out.WriteProcessingInstruction(str, this.value != null ? this.value.EvaluateAsString(p) : string.Empty);
    }
  }
}
