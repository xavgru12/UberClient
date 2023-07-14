// Decompiled with JetBrains decompiler
// Type: System.Xml.Xsl.XslCompiledTransform
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Xsl;
using System.IO;
using System.Security.Policy;
using System.Text;
using System.Xml.XPath;

namespace System.Xml.Xsl
{
  [MonoTODO]
  public sealed class XslCompiledTransform
  {
    private bool enable_debug;
    private object debugger;
    private CompiledStylesheet s;
    private XmlWriterSettings output_settings = new XmlWriterSettings();

    public XslCompiledTransform()
      : this(false)
    {
    }

    public XslCompiledTransform(bool enableDebug)
    {
      this.enable_debug = enableDebug;
      if (this.enable_debug)
        this.debugger = (object) new NoOperationDebugger();
      this.output_settings.ConformanceLevel = ConformanceLevel.Fragment;
    }

    [MonoTODO]
    public XmlWriterSettings OutputSettings => this.output_settings;

    public void Transform(string inputfile, string outputfile)
    {
      using (Stream output = (Stream) File.Create(outputfile))
        this.Transform((IXPathNavigable) new XPathDocument(inputfile, XmlSpace.Preserve), (XsltArgumentList) null, output);
    }

    public void Transform(string inputfile, XmlWriter output) => this.Transform(inputfile, (XsltArgumentList) null, output);

    public void Transform(string inputfile, XsltArgumentList args, Stream output) => this.Transform((IXPathNavigable) new XPathDocument(inputfile, XmlSpace.Preserve), args, output);

    public void Transform(string inputfile, XsltArgumentList args, TextWriter output) => this.Transform((IXPathNavigable) new XPathDocument(inputfile, XmlSpace.Preserve), args, output);

    public void Transform(string inputfile, XsltArgumentList args, XmlWriter output) => this.Transform((IXPathNavigable) new XPathDocument(inputfile, XmlSpace.Preserve), args, output);

    public void Transform(XmlReader reader, XmlWriter output) => this.Transform(reader, (XsltArgumentList) null, output);

    public void Transform(XmlReader reader, XsltArgumentList args, Stream output) => this.Transform((IXPathNavigable) new XPathDocument(reader, XmlSpace.Preserve), args, output);

    public void Transform(XmlReader reader, XsltArgumentList args, TextWriter output) => this.Transform((IXPathNavigable) new XPathDocument(reader, XmlSpace.Preserve), args, output);

    public void Transform(XmlReader reader, XsltArgumentList args, XmlWriter output) => this.Transform(reader, args, output, (XmlResolver) null);

    public void Transform(IXPathNavigable input, XsltArgumentList args, TextWriter output) => this.Transform(input.CreateNavigator(), args, output);

    public void Transform(IXPathNavigable input, XsltArgumentList args, Stream output) => this.Transform(input.CreateNavigator(), args, output);

    public void Transform(IXPathNavigable input, XmlWriter output) => this.Transform(input, (XsltArgumentList) null, output);

    public void Transform(IXPathNavigable input, XsltArgumentList args, XmlWriter output) => this.Transform(input.CreateNavigator(), args, output, (XmlResolver) null);

    public void Transform(
      XmlReader input,
      XsltArgumentList args,
      XmlWriter output,
      XmlResolver resolver)
    {
      this.Transform(new XPathDocument(input, XmlSpace.Preserve).CreateNavigator(), args, output, resolver);
    }

    private void Transform(
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

    private void Transform(XPathNavigator input, XsltArgumentList args, Stream output)
    {
      XslOutput output1 = (XslOutput) this.s.Outputs[(object) string.Empty];
      this.Transform(input, args, (TextWriter) new StreamWriter(output, output1.Encoding));
    }

    private void Transform(XPathNavigator input, XsltArgumentList args, TextWriter output)
    {
      if (this.s == null)
        throw new XsltException("No stylesheet was loaded.", (Exception) null);
      Outputter outputtter = (Outputter) new GenericOutputter(output, this.s.Outputs, output.Encoding);
      new XslTransformProcessor(this.s, this.debugger).Process(input, outputtter, args, (XmlResolver) null);
      outputtter.Done();
      output.Flush();
    }

    private XmlReader GetXmlReader(string url)
    {
      XmlResolver xmlResolver = (XmlResolver) new XmlUrlResolver();
      Uri absoluteUri = xmlResolver.ResolveUri((Uri) null, url);
      Stream entity = xmlResolver.GetEntity(absoluteUri, (string) null, typeof (Stream)) as Stream;
      return (XmlReader) new XmlValidatingReader((XmlReader) new XmlTextReader(absoluteUri.ToString(), entity)
      {
        XmlResolver = xmlResolver
      })
      {
        XmlResolver = xmlResolver,
        ValidationType = ValidationType.None
      };
    }

    public void Load(string url)
    {
      using (XmlReader xmlReader = this.GetXmlReader(url))
        this.Load(xmlReader);
    }

    public void Load(XmlReader stylesheet) => this.Load(stylesheet, (XsltSettings) null, (XmlResolver) null);

    public void Load(IXPathNavigable stylesheet) => this.Load(stylesheet.CreateNavigator(), (XsltSettings) null, (XmlResolver) null);

    public void Load(IXPathNavigable stylesheet, XsltSettings settings, XmlResolver resolver) => this.Load(stylesheet.CreateNavigator(), settings, resolver);

    public void Load(XmlReader stylesheet, XsltSettings settings, XmlResolver resolver) => this.Load(new XPathDocument(stylesheet, XmlSpace.Preserve).CreateNavigator(), settings, resolver);

    public void Load(string stylesheet, XsltSettings settings, XmlResolver resolver) => this.Load(new XPathDocument(stylesheet, XmlSpace.Preserve).CreateNavigator(), settings, resolver);

    private void Load(XPathNavigator stylesheet, XsltSettings settings, XmlResolver resolver) => this.s = new Compiler(this.debugger).Compile(stylesheet, resolver, (Evidence) null);
  }
}
