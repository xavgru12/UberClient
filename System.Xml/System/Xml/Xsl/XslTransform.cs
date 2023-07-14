// Decompiled with JetBrains decompiler
// Type: System.Xml.Xsl.XslTransform
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Xsl;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using System.Text;
using System.Xml.XPath;

namespace System.Xml.Xsl
{
  public sealed class XslTransform
  {
    internal static readonly bool TemplateStackFrameError;
    internal static readonly TextWriter TemplateStackFrameOutput;
    private object debugger;
    private CompiledStylesheet s;
    private XmlResolver xmlResolver = (XmlResolver) new XmlUrlResolver();

    public XslTransform()
      : this(XslTransform.GetDefaultDebugger())
    {
    }

    internal XslTransform(object debugger) => this.debugger = debugger;

    static XslTransform()
    {
      string environmentVariable = Environment.GetEnvironmentVariable("MONO_XSLT_STACK_FRAME");
      if (environmentVariable == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (XslTransform.\u003C\u003Ef__switch\u0024map42 == null)
      {
        // ISSUE: reference to a compiler-generated field
        XslTransform.\u003C\u003Ef__switch\u0024map42 = new Dictionary<string, int>(3)
        {
          {
            "stdout",
            0
          },
          {
            "stderr",
            1
          },
          {
            "error",
            2
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!XslTransform.\u003C\u003Ef__switch\u0024map42.TryGetValue(environmentVariable, out num))
        return;
      switch (num)
      {
        case 0:
          XslTransform.TemplateStackFrameOutput = Console.Out;
          break;
        case 1:
          XslTransform.TemplateStackFrameOutput = Console.Error;
          break;
        case 2:
          XslTransform.TemplateStackFrameError = true;
          break;
      }
    }

    private static object GetDefaultDebugger()
    {
      string typeName = (string) null;
      try
      {
        typeName = Environment.GetEnvironmentVariable("MONO_XSLT_DEBUGGER");
      }
      catch (Exception ex)
      {
      }
      switch (typeName)
      {
        case null:
          return (object) null;
        case "simple":
          return (object) new SimpleXsltDebugger();
        default:
          return Activator.CreateInstance(Type.GetType(typeName));
      }
    }

    [MonoTODO]
    public XmlResolver XmlResolver
    {
      set => this.xmlResolver = value;
    }

    public XmlReader Transform(IXPathNavigable input, XsltArgumentList args) => this.Transform(input.CreateNavigator(), args, this.xmlResolver);

    public XmlReader Transform(IXPathNavigable input, XsltArgumentList args, XmlResolver resolver) => this.Transform(input.CreateNavigator(), args, resolver);

    public XmlReader Transform(XPathNavigator input, XsltArgumentList args) => this.Transform(input, args, this.xmlResolver);

    public XmlReader Transform(XPathNavigator input, XsltArgumentList args, XmlResolver resolver)
    {
      MemoryStream xmlFragment = new MemoryStream();
      this.Transform(input, args, (XmlWriter) new XmlTextWriter((Stream) xmlFragment, (Encoding) null), resolver);
      xmlFragment.Position = 0L;
      return (XmlReader) new XmlTextReader((Stream) xmlFragment, XmlNodeType.Element, (XmlParserContext) null);
    }

    public void Transform(IXPathNavigable input, XsltArgumentList args, TextWriter output) => this.Transform(input.CreateNavigator(), args, output, this.xmlResolver);

    public void Transform(
      IXPathNavigable input,
      XsltArgumentList args,
      TextWriter output,
      XmlResolver resolver)
    {
      this.Transform(input.CreateNavigator(), args, output, resolver);
    }

    public void Transform(IXPathNavigable input, XsltArgumentList args, Stream output) => this.Transform(input.CreateNavigator(), args, output, this.xmlResolver);

    public void Transform(
      IXPathNavigable input,
      XsltArgumentList args,
      Stream output,
      XmlResolver resolver)
    {
      this.Transform(input.CreateNavigator(), args, output, resolver);
    }

    public void Transform(IXPathNavigable input, XsltArgumentList args, XmlWriter output) => this.Transform(input.CreateNavigator(), args, output, this.xmlResolver);

    public void Transform(
      IXPathNavigable input,
      XsltArgumentList args,
      XmlWriter output,
      XmlResolver resolver)
    {
      this.Transform(input.CreateNavigator(), args, output, resolver);
    }

    public void Transform(XPathNavigator input, XsltArgumentList args, XmlWriter output) => this.Transform(input, args, output, this.xmlResolver);

    public void Transform(
      XPathNavigator input,
      XsltArgumentList args,
      XmlWriter output,
      XmlResolver resolver)
    {
      if (this.s == null)
        throw new XsltException("No stylesheet was loaded.", (Exception) null);
      Outputter outputtter = (Outputter) new GenericOutputter(output, this.s.Outputs, (Encoding) null);
      new XslTransformProcessor(this.s, this.debugger).Process(input, outputtter, args, resolver);
      output.Flush();
    }

    public void Transform(XPathNavigator input, XsltArgumentList args, Stream output) => this.Transform(input, args, output, this.xmlResolver);

    public void Transform(
      XPathNavigator input,
      XsltArgumentList args,
      Stream output,
      XmlResolver resolver)
    {
      XslOutput output1 = (XslOutput) this.s.Outputs[(object) string.Empty];
      this.Transform(input, args, (TextWriter) new StreamWriter(output, output1.Encoding), resolver);
    }

    public void Transform(XPathNavigator input, XsltArgumentList args, TextWriter output) => this.Transform(input, args, output, this.xmlResolver);

    public void Transform(
      XPathNavigator input,
      XsltArgumentList args,
      TextWriter output,
      XmlResolver resolver)
    {
      if (this.s == null)
        throw new XsltException("No stylesheet was loaded.", (Exception) null);
      Outputter outputtter = (Outputter) new GenericOutputter(output, this.s.Outputs, output.Encoding);
      new XslTransformProcessor(this.s, this.debugger).Process(input, outputtter, args, resolver);
      outputtter.Done();
      output.Flush();
    }

    public void Transform(string inputfile, string outputfile) => this.Transform(inputfile, outputfile, this.xmlResolver);

    public void Transform(string inputfile, string outputfile, XmlResolver resolver)
    {
      using (Stream output = (Stream) new FileStream(outputfile, FileMode.Create, FileAccess.ReadWrite))
        this.Transform(new XPathDocument(inputfile).CreateNavigator(), (XsltArgumentList) null, output, resolver);
    }

    public void Load(string url) => this.Load(url, (XmlResolver) null);

    public void Load(string url, XmlResolver resolver)
    {
      XmlResolver xmlResolver = resolver ?? (XmlResolver) new XmlUrlResolver();
      Uri absoluteUri = xmlResolver.ResolveUri((Uri) null, url);
      using (Stream entity = xmlResolver.GetEntity(absoluteUri, (string) null, typeof (Stream)) as Stream)
        this.Load(new XPathDocument((XmlReader) new XmlValidatingReader((XmlReader) new XmlTextReader(absoluteUri.ToString(), entity)
        {
          XmlResolver = xmlResolver
        })
        {
          XmlResolver = xmlResolver,
          ValidationType = ValidationType.None
        }, XmlSpace.Preserve).CreateNavigator(), resolver, (Evidence) null);
    }

    public void Load(XmlReader stylesheet) => this.Load(stylesheet, (XmlResolver) null, (Evidence) null);

    public void Load(XmlReader stylesheet, XmlResolver resolver) => this.Load(stylesheet, resolver, (Evidence) null);

    public void Load(XPathNavigator stylesheet) => this.Load(stylesheet, (XmlResolver) null, (Evidence) null);

    public void Load(XPathNavigator stylesheet, XmlResolver resolver) => this.Load(stylesheet, resolver, (Evidence) null);

    public void Load(IXPathNavigable stylesheet) => this.Load(stylesheet.CreateNavigator(), (XmlResolver) null);

    public void Load(IXPathNavigable stylesheet, XmlResolver resolver) => this.Load(stylesheet.CreateNavigator(), resolver);

    public void Load(IXPathNavigable stylesheet, XmlResolver resolver, Evidence evidence) => this.Load(stylesheet.CreateNavigator(), resolver, evidence);

    public void Load(XPathNavigator stylesheet, XmlResolver resolver, Evidence evidence) => this.s = new Compiler(this.debugger).Compile(stylesheet, resolver, evidence);

    public void Load(XmlReader stylesheet, XmlResolver resolver, Evidence evidence) => this.Load(new XPathDocument(stylesheet, XmlSpace.Preserve).CreateNavigator(), resolver, evidence);
  }
}
