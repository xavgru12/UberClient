// Decompiled with JetBrains decompiler
// Type: System.Xml.Xsl.SimpleXsltDebugger
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.XPath;

namespace System.Xml.Xsl
{
  internal class SimpleXsltDebugger
  {
    public void OnCompile(XPathNavigator style)
    {
      Console.Write("Compiling: ");
      this.PrintXPathNavigator(style);
      Console.WriteLine();
    }

    public void OnExecute(
      XPathNodeIterator currentNodeSet,
      XPathNavigator style,
      XsltContext xsltContext)
    {
      Console.Write("Executing: ");
      this.PrintXPathNavigator(style);
      Console.WriteLine(" / NodeSet: (type {1}) {0} / XsltContext: {2}", (object) currentNodeSet, (object) currentNodeSet.GetType(), (object) xsltContext);
    }

    private void PrintXPathNavigator(XPathNavigator nav)
    {
      IXmlLineInfo xmlLineInfo1 = !(nav is IXmlLineInfo xmlLineInfo2) || !xmlLineInfo2.HasLineInfo() ? (IXmlLineInfo) null : xmlLineInfo2;
      Console.Write("({0}, {1}) element {2}", (object) (xmlLineInfo1 == null ? 0 : xmlLineInfo1.LineNumber), (object) (xmlLineInfo1 == null ? 0 : xmlLineInfo1.LinePosition), (object) nav.Name);
    }
  }
}
