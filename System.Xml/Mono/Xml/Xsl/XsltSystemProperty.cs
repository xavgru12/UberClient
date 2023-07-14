// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XsltSystemProperty
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class XsltSystemProperty : XPathFunction
  {
    private Expression arg0;
    private IStaticXsltContext ctx;

    public XsltSystemProperty(FunctionArguments args, IStaticXsltContext ctx)
      : base(args)
    {
      this.arg0 = args != null && args.Tail == null ? args.Arg : throw new XPathException("system-property takes 1 arg");
      this.ctx = ctx;
    }

    public override XPathResultType ReturnType => XPathResultType.String;

    internal override bool Peer => this.arg0.Peer;

    public override object Evaluate(BaseIterator iter)
    {
      XmlQualifiedName xmlQualifiedName = XslNameUtil.FromString(this.arg0.EvaluateString(iter), this.ctx);
      if (xmlQualifiedName.Namespace == "http://www.w3.org/1999/XSL/Transform")
      {
        string name = xmlQualifiedName.Name;
        if (name != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (XsltSystemProperty.\u003C\u003Ef__switch\u0024map1E == null)
          {
            // ISSUE: reference to a compiler-generated field
            XsltSystemProperty.\u003C\u003Ef__switch\u0024map1E = new Dictionary<string, int>(3)
            {
              {
                "version",
                0
              },
              {
                "vendor",
                1
              },
              {
                "vendor-url",
                2
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (XsltSystemProperty.\u003C\u003Ef__switch\u0024map1E.TryGetValue(name, out num))
          {
            switch (num)
            {
              case 0:
                return (object) "1.0";
              case 1:
                return (object) "Mono";
              case 2:
                return (object) "http://www.go-mono.com/";
            }
          }
        }
      }
      return (object) string.Empty;
    }
  }
}
