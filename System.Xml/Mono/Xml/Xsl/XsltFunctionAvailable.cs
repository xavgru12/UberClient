// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XsltFunctionAvailable
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class XsltFunctionAvailable : XPathFunction
  {
    private Expression arg0;
    private IStaticXsltContext ctx;

    public XsltFunctionAvailable(FunctionArguments args, IStaticXsltContext ctx)
      : base(args)
    {
      this.arg0 = args != null && args.Tail == null ? args.Arg : throw new XPathException("element-available takes 1 arg");
      this.ctx = ctx;
    }

    public override XPathResultType ReturnType => XPathResultType.Boolean;

    internal override bool Peer => this.arg0.Peer;

    public override object Evaluate(BaseIterator iter)
    {
      string name = this.arg0.EvaluateString(iter);
      return name.IndexOf(':') > 0 ? (object) ((iter.NamespaceManager as XsltCompiledContext).ResolveFunction(XslNameUtil.FromString(name, this.ctx), (XPathResultType[]) null) != null) : (object) (bool) (name == "boolean" || name == "ceiling" || name == "concat" || name == "contains" || name == "count" || name == "false" || name == "floor" || name == "id" || name == "lang" || name == "last" || name == "local-name" || name == "name" || name == "namespace-uri" || name == "normalize-space" || name == "not" || name == "number" || name == "position" || name == "round" || name == "starts-with" || name == "string" || name == "string-length" || name == "substring" || name == "substring-after" || name == "substring-before" || name == "sum" || name == "translate" || name == "true" || name == "document" || name == "format-number" || name == "function-available" || name == "generate-id" || name == "key" || name == "current" || name == "unparsed-entity-uri" || name == "element-available" ? 1 : (name == "system-property" ? 1 : 0));
    }
  }
}
