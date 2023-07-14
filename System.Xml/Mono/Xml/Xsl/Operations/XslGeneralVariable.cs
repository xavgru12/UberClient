// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslGeneralVariable
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
  internal abstract class XslGeneralVariable : XslCompiledElement, IXsltContextVariable
  {
    protected XslVariableInformation var;

    public XslGeneralVariable(Compiler c)
      : base(c)
    {
    }

    protected override void Compile(Compiler c)
    {
      if (c.Debugger != null)
        c.Debugger.DebugCompile(this.DebugInput);
      this.var = new XslVariableInformation(c);
    }

    public abstract override void Evaluate(XslTransformProcessor p);

    protected abstract object GetValue(XslTransformProcessor p);

    public object Evaluate(XsltContext xsltContext)
    {
      object obj = this.GetValue(((XsltCompiledContext) xsltContext).Processor);
      return obj is XPathNodeIterator ? (object) new WrapperIterator(((XPathNodeIterator) obj).Clone(), (IXmlNamespaceResolver) xsltContext) : obj;
    }

    public XmlQualifiedName Name => this.var.Name;

    public XPathResultType VariableType => XPathResultType.Any;

    public abstract bool IsLocal { get; }

    public abstract bool IsParam { get; }
  }
}
