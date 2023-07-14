// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XPathVariableBinding
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
  internal class XPathVariableBinding : Expression
  {
    private XslGeneralVariable v;

    public XPathVariableBinding(XslGeneralVariable v) => this.v = v;

    public override string ToString() => "$" + this.v.Name.ToString();

    public override XPathResultType ReturnType => XPathResultType.Any;

    public override XPathResultType GetReturnType(BaseIterator iter) => XPathResultType.Any;

    public override object Evaluate(BaseIterator iter) => this.v.Evaluate(iter.NamespaceManager as XsltContext);
  }
}
