// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathDocument
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.XPath;
using System.IO;

namespace System.Xml.XPath
{
  public class XPathDocument : IXPathNavigable
  {
    private IXPathNavigable document;

    public XPathDocument(Stream stream) => this.Initialize((XmlReader) new XmlValidatingReader((XmlReader) new XmlTextReader(stream))
    {
      ValidationType = ValidationType.None
    }, XmlSpace.None);

    public XPathDocument(string uri)
      : this(uri, XmlSpace.None)
    {
    }

    public XPathDocument(TextReader reader) => this.Initialize((XmlReader) new XmlValidatingReader((XmlReader) new XmlTextReader(reader))
    {
      ValidationType = ValidationType.None
    }, XmlSpace.None);

    public XPathDocument(XmlReader reader)
      : this(reader, XmlSpace.None)
    {
    }

    public XPathDocument(string uri, XmlSpace space)
    {
      XmlValidatingReader reader = (XmlValidatingReader) null;
      try
      {
        reader = new XmlValidatingReader((XmlReader) new XmlTextReader(uri));
        reader.ValidationType = ValidationType.None;
        this.Initialize((XmlReader) reader, space);
      }
      finally
      {
        reader?.Close();
      }
    }

    public XPathDocument(XmlReader reader, XmlSpace space) => this.Initialize(reader, space);

    private void Initialize(XmlReader reader, XmlSpace space) => this.document = (IXPathNavigable) new DTMXPathDocumentBuilder2(reader, space).CreateDocument();

    public XPathNavigator CreateNavigator() => this.document.CreateNavigator();
  }
}
