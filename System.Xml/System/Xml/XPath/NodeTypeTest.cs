// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.NodeTypeTest
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class NodeTypeTest : NodeTest
  {
    public readonly XPathNodeType type;
    protected string _param;

    public NodeTypeTest(Axes axis)
      : base(axis)
    {
      this.type = this._axis.NodeType;
    }

    public NodeTypeTest(Axes axis, XPathNodeType type)
      : base(axis)
    {
      this.type = type;
    }

    public NodeTypeTest(Axes axis, XPathNodeType type, string param)
      : base(axis)
    {
      this.type = type;
      this._param = param;
      if (param != null && type != XPathNodeType.ProcessingInstruction)
        throw new XPathException("No argument allowed for " + NodeTypeTest.ToString(type) + "() test");
    }

    internal NodeTypeTest(NodeTypeTest other, Axes axis)
      : base(axis)
    {
      this.type = other.type;
      this._param = other._param;
    }

    public override string ToString()
    {
      string str1 = NodeTypeTest.ToString(this.type);
      string str2 = this.type != XPathNodeType.ProcessingInstruction || this._param == null ? str1 + "()" : str1 + "('" + this._param + "')";
      return this._axis.ToString() + "::" + str2;
    }

    private static string ToString(XPathNodeType type)
    {
      switch (type)
      {
        case XPathNodeType.Element:
        case XPathNodeType.Attribute:
        case XPathNodeType.Namespace:
        case XPathNodeType.All:
          return "node";
        case XPathNodeType.Text:
          return "text";
        case XPathNodeType.ProcessingInstruction:
          return "processing-instruction";
        case XPathNodeType.Comment:
          return "comment";
        default:
          return "node-type [" + type.ToString() + "]";
      }
    }

    public override bool Match(IXmlNamespaceResolver nsm, XPathNavigator nav)
    {
      XPathNodeType nodeType = nav.NodeType;
      switch (this.type)
      {
        case XPathNodeType.Text:
          switch (nodeType)
          {
            case XPathNodeType.Text:
            case XPathNodeType.SignificantWhitespace:
            case XPathNodeType.Whitespace:
              return true;
            default:
              return false;
          }
        case XPathNodeType.ProcessingInstruction:
          return nodeType == XPathNodeType.ProcessingInstruction && (this._param == null || !(nav.Name != this._param));
        case XPathNodeType.All:
          return true;
        default:
          return this.type == nodeType;
      }
    }

    public override void GetInfo(
      out string name,
      out string ns,
      out XPathNodeType nodetype,
      IXmlNamespaceResolver nsm)
    {
      name = this._param;
      ns = (string) null;
      nodetype = this.type;
    }
  }
}
