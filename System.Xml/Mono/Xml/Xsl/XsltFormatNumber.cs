// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XsltFormatNumber
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class XsltFormatNumber : XPathFunction
  {
    private Expression arg0;
    private Expression arg1;
    private Expression arg2;
    private IStaticXsltContext ctx;

    public XsltFormatNumber(FunctionArguments args, IStaticXsltContext ctx)
      : base(args)
    {
      this.arg0 = args != null && args.Tail != null && (args.Tail.Tail == null || args.Tail.Tail.Tail == null) ? args.Arg : throw new XPathException("format-number takes 2 or 3 args");
      this.arg1 = args.Tail.Arg;
      if (args.Tail.Tail == null)
        return;
      this.arg2 = args.Tail.Tail.Arg;
      this.ctx = ctx;
    }

    public override XPathResultType ReturnType => XPathResultType.String;

    internal override bool Peer
    {
      get
      {
        if (!this.arg0.Peer || !this.arg1.Peer)
          return false;
        return this.arg2 == null || this.arg2.Peer;
      }
    }

    public override object Evaluate(BaseIterator iter)
    {
      double number = this.arg0.EvaluateNumber(iter);
      string pattern = this.arg1.EvaluateString(iter);
      XmlQualifiedName name = XmlQualifiedName.Empty;
      if (this.arg2 != null)
        name = XslNameUtil.FromString(this.arg2.EvaluateString(iter), this.ctx);
      try
      {
        return (object) ((XsltCompiledContext) iter.NamespaceManager).Processor.CompiledStyle.LookupDecimalFormat(name).FormatNumber(number, pattern);
      }
      catch (ArgumentException ex)
      {
        throw new XsltException(ex.Message, (Exception) ex, iter.Current);
      }
    }
  }
}
