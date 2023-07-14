// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XslDefaultNodeTemplate
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
  internal class XslDefaultNodeTemplate : XslTemplate
  {
    private XmlQualifiedName mode;
    private static XslDefaultNodeTemplate instance = new XslDefaultNodeTemplate(XmlQualifiedName.Empty);

    public XslDefaultNodeTemplate(XmlQualifiedName mode)
      : base((Compiler) null)
    {
      this.mode = mode;
    }

    public static XslTemplate Instance => (XslTemplate) XslDefaultNodeTemplate.instance;

    public override void Evaluate(XslTransformProcessor p, Hashtable withParams) => p.ApplyTemplates(p.CurrentNode.SelectChildren(XPathNodeType.All), this.mode, (ArrayList) null);
  }
}
