// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslText
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.XPath;

namespace Mono.Xml.Xsl.Operations
{
  internal class XslText : XslCompiledElement
  {
    private bool disableOutputEscaping;
    private string text = string.Empty;
    private bool isWhitespace;

    public XslText(Compiler c, bool isWhitespace)
      : base(c)
    {
      this.isWhitespace = isWhitespace;
    }

    protected override void Compile(Compiler c)
    {
      if (c.Debugger != null)
        c.Debugger.DebugCompile(this.DebugInput);
      c.CheckExtraAttributes("text", "disable-output-escaping");
      this.text = c.Input.Value;
      if (c.Input.NodeType != XPathNodeType.Element)
        return;
      this.disableOutputEscaping = c.ParseYesNoAttribute("disable-output-escaping", false);
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      if (!this.disableOutputEscaping)
      {
        if (this.isWhitespace)
          p.Out.WriteWhitespace(this.text);
        else
          p.Out.WriteString(this.text);
      }
      else
        p.Out.WriteRaw(this.text);
    }
  }
}
