// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XsltUnparsedEntityUri
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
  internal class XsltUnparsedEntityUri : XPathFunction
  {
    private Expression arg0;

    public XsltUnparsedEntityUri(FunctionArguments args)
      : base(args)
    {
      this.arg0 = args != null && args.Tail == null ? args.Arg : throw new XPathException("unparsed-entity-uri takes 1 arg");
    }

    public override XPathResultType ReturnType => XPathResultType.String;

    internal override bool Peer => this.arg0.Peer;

    public override object Evaluate(BaseIterator iter)
    {
      if (!(iter.Current is IHasXmlNode current))
        return (object) string.Empty;
      XmlNode node = current.GetNode();
      if (node.OwnerDocument == null)
        return (object) string.Empty;
      XmlDocumentType documentType = node.OwnerDocument.DocumentType;
      if (documentType == null)
        return (object) string.Empty;
      if (!(documentType.Entities.GetNamedItem(this.arg0.EvaluateString(iter)) is XmlEntity namedItem))
        return (object) string.Empty;
      return namedItem.SystemId != null ? (object) namedItem.SystemId : (object) string.Empty;
    }
  }
}
