// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslVariableInformation
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.XPath;
using System;
using System.Collections;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
  internal class XslVariableInformation
  {
    private XmlQualifiedName name;
    private XPathExpression select;
    private XslOperation content;

    public XslVariableInformation(Compiler c)
    {
      c.CheckExtraAttributes(c.Input.LocalName, nameof (name), nameof (select));
      c.AssertAttribute(nameof (name));
      this.name = c.ParseQNameAttribute(nameof (name));
      try
      {
        XmlConvert.VerifyName(this.name.Name);
      }
      catch (XmlException ex)
      {
        throw new XsltCompileException("Variable name is not qualified name", (Exception) ex, c.Input);
      }
      string attribute = c.GetAttribute(nameof (select));
      if (attribute != null && attribute != string.Empty)
      {
        this.select = (XPathExpression) c.CompileExpression(c.GetAttribute(nameof (select)));
      }
      else
      {
        if (!c.Input.MoveToFirstChild())
          return;
        this.content = c.CompileTemplateContent();
        c.Input.MoveToParent();
      }
    }

    public object Evaluate(XslTransformProcessor p)
    {
      if (this.select != null)
      {
        object obj = p.Evaluate(this.select);
        if (obj is XPathNodeIterator)
        {
          ArrayList arrayList = new ArrayList();
          XPathNodeIterator xpathNodeIterator = (XPathNodeIterator) obj;
          while (xpathNodeIterator.MoveNext())
            arrayList.Add((object) xpathNodeIterator.Current.Clone());
          obj = (object) new ListIterator((IList) arrayList, (IXmlNamespaceResolver) p.XPathContext);
        }
        return obj;
      }
      if (this.content == null)
        return (object) string.Empty;
      DTMXPathDocumentWriter2 writer = new DTMXPathDocumentWriter2(p.Root.NameTable, 200);
      Outputter newOutput = (Outputter) new GenericOutputter((XmlWriter) writer, p.Outputs, (Encoding) null, true);
      p.PushOutput(newOutput);
      if (p.CurrentNodeset.CurrentPosition == 0)
        p.NodesetMoveNext();
      this.content.Evaluate(p);
      p.PopOutput();
      return (object) writer.CreateDocument().CreateNavigator();
    }

    public XmlQualifiedName Name => this.name;

    internal XPathExpression Select => this.select;

    internal XslOperation Content => this.content;
  }
}
