// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XslKey
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.XPath;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
  internal class XslKey
  {
    private XmlQualifiedName name;
    private CompiledExpression useExpr;
    private Pattern matchPattern;

    public XslKey(Compiler c)
    {
      this.name = c.ParseQNameAttribute(nameof (name));
      c.KeyCompilationMode = true;
      this.useExpr = c.CompileExpression(c.GetAttribute("use"));
      if (this.useExpr == null)
        this.useExpr = c.CompileExpression(".");
      c.AssertAttribute("match");
      string attribute = c.GetAttribute("match");
      this.matchPattern = c.CompilePattern(attribute, c.Input);
      c.KeyCompilationMode = false;
    }

    public XmlQualifiedName Name => this.name;

    internal CompiledExpression Use => this.useExpr;

    internal Pattern Match => this.matchPattern;
  }
}
