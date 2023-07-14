// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.ExprKeyContainer
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
  internal class ExprKeyContainer : Expression
  {
    private Expression expr;

    public ExprKeyContainer(Expression expr) => this.expr = expr;

    public Expression BodyExpression => this.expr;

    public override object Evaluate(BaseIterator iter) => this.expr.Evaluate(iter);

    internal override XPathNodeType EvaluatedNodeType => this.expr.EvaluatedNodeType;

    public override XPathResultType ReturnType => this.expr.ReturnType;
  }
}
