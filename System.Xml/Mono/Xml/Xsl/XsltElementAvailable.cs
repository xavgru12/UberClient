// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XsltElementAvailable
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class XsltElementAvailable : XPathFunction
  {
    private Expression arg0;
    private IStaticXsltContext ctx;

    public XsltElementAvailable(FunctionArguments args, IStaticXsltContext ctx)
      : base(args)
    {
      this.arg0 = args != null && args.Tail == null ? args.Arg : throw new XPathException("element-available takes 1 arg");
      this.ctx = ctx;
    }

    public override XPathResultType ReturnType => XPathResultType.Boolean;

    internal override bool Peer => this.arg0.Peer;

    public override object Evaluate(BaseIterator iter)
    {
      XmlQualifiedName xmlQualifiedName = XslNameUtil.FromString(this.arg0.EvaluateString(iter), this.ctx);
      return (object) (bool) (!(xmlQualifiedName.Namespace == "http://www.w3.org/1999/XSL/Transform") ? 0 : (xmlQualifiedName.Name == "apply-imports" || xmlQualifiedName.Name == "apply-templates" || xmlQualifiedName.Name == "call-template" || xmlQualifiedName.Name == "choose" || xmlQualifiedName.Name == "comment" || xmlQualifiedName.Name == "copy" || xmlQualifiedName.Name == "copy-of" || xmlQualifiedName.Name == "element" || xmlQualifiedName.Name == "fallback" || xmlQualifiedName.Name == "for-each" || xmlQualifiedName.Name == "message" || xmlQualifiedName.Name == "number" || xmlQualifiedName.Name == "processing-instruction" || xmlQualifiedName.Name == "text" || xmlQualifiedName.Name == "value-of" ? 1 : (xmlQualifiedName.Name == "variable" ? 1 : 0)));
    }
  }
}
