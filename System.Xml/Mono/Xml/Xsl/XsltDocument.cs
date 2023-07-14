// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XsltDocument
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
  internal class XsltDocument : XPathFunction
  {
    private Expression arg0;
    private Expression arg1;
    private XPathNavigator doc;
    private static string VoidBaseUriFlag = "&^)(*&%*^$&$VOID!BASE!URI!";

    public XsltDocument(FunctionArguments args, Compiler c)
      : base(args)
    {
      this.arg0 = args != null && (args.Tail == null || args.Tail.Tail == null) ? args.Arg : throw new XPathException("document takes one or two args");
      if (args.Tail != null)
        this.arg1 = args.Tail.Arg;
      this.doc = c.Input.Clone();
    }

    public override XPathResultType ReturnType => XPathResultType.NodeSet;

    internal override bool Peer
    {
      get
      {
        if (!this.arg0.Peer)
          return false;
        return this.arg1 == null || this.arg1.Peer;
      }
    }

    public override object Evaluate(BaseIterator iter)
    {
      string baseUri = (string) null;
      if (this.arg1 != null)
      {
        XPathNodeIterator nodeSet = (XPathNodeIterator) this.arg1.EvaluateNodeSet(iter);
        baseUri = !nodeSet.MoveNext() ? XsltDocument.VoidBaseUriFlag : nodeSet.Current.BaseURI;
      }
      object itr = this.arg0.Evaluate(iter);
      return itr is XPathNodeIterator ? (object) this.GetDocument(iter.NamespaceManager as XsltCompiledContext, (XPathNodeIterator) itr, baseUri) : (object) this.GetDocument(iter.NamespaceManager as XsltCompiledContext, !(itr is IFormattable) ? itr.ToString() : ((IFormattable) itr).ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture), baseUri);
    }

    private Uri Resolve(string thisUri, string baseUri, XslTransformProcessor p)
    {
      XmlResolver resolver = p.Resolver;
      if (resolver == null)
        return (Uri) null;
      Uri baseUri1 = (Uri) null;
      if (!object.ReferenceEquals((object) baseUri, (object) XsltDocument.VoidBaseUriFlag) && baseUri != string.Empty)
        baseUri1 = resolver.ResolveUri((Uri) null, baseUri);
      return resolver.ResolveUri(baseUri1, thisUri);
    }

    private XPathNodeIterator GetDocument(
      XsltCompiledContext xsltContext,
      XPathNodeIterator itr,
      string baseUri)
    {
      ArrayList arrayList = new ArrayList();
      try
      {
        Hashtable hashtable = new Hashtable();
        while (itr.MoveNext())
        {
          Uri uri = this.Resolve(itr.Current.Value, baseUri == null ? this.doc.BaseURI : baseUri, xsltContext.Processor);
          if (!hashtable.ContainsKey((object) uri))
          {
            hashtable.Add((object) uri, (object) null);
            if (uri != (Uri) null && uri.ToString() == string.Empty)
            {
              XPathNavigator xpathNavigator = this.doc.Clone();
              xpathNavigator.MoveToRoot();
              arrayList.Add((object) xpathNavigator);
            }
            else
              arrayList.Add((object) xsltContext.Processor.GetDocument(uri));
          }
        }
      }
      catch (Exception ex)
      {
        arrayList.Clear();
      }
      return (XPathNodeIterator) new ListIterator((IList) arrayList, (IXmlNamespaceResolver) xsltContext);
    }

    private XPathNodeIterator GetDocument(
      XsltCompiledContext xsltContext,
      string arg0,
      string baseUri)
    {
      try
      {
        Uri uri = this.Resolve(arg0, baseUri == null ? this.doc.BaseURI : baseUri, xsltContext.Processor);
        XPathNavigator nav;
        if (uri != (Uri) null && uri.ToString() == string.Empty)
        {
          nav = this.doc.Clone();
          nav.MoveToRoot();
        }
        else
          nav = xsltContext.Processor.GetDocument(uri);
        return (XPathNodeIterator) new SelfIterator(nav, (IXmlNamespaceResolver) xsltContext);
      }
      catch (Exception ex)
      {
        return (XPathNodeIterator) new ListIterator((IList) new ArrayList(), (IXmlNamespaceResolver) xsltContext);
      }
    }

    public override string ToString() => "document(" + this.arg0.ToString() + (this.arg1 == null ? string.Empty : ",") + (this.arg1 == null ? string.Empty : this.arg1.ToString()) + ")";
  }
}
