// Decompiled with JetBrains decompiler
// Type: Mono.Xml.XPath.XPathNavigatorReader
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace Mono.Xml.XPath
{
  internal class XPathNavigatorReader : XmlReader
  {
    private XPathNavigator root;
    private XPathNavigator current;
    private bool started;
    private bool closed;
    private bool endElement;
    private bool attributeValueConsumed;
    private StringBuilder readStringBuffer = new StringBuilder();
    private StringBuilder innerXmlBuilder = new StringBuilder();
    private int depth;
    private int attributeCount;
    private bool eof;

    public XPathNavigatorReader(XPathNavigator nav) => this.current = nav.Clone();

    public override bool CanReadBinaryContent => true;

    public override bool CanReadValueChunk => true;

    public override XmlNodeType NodeType
    {
      get
      {
        if (this.ReadState != ReadState.Interactive)
          return XmlNodeType.None;
        if (this.endElement)
          return XmlNodeType.EndElement;
        if (this.attributeValueConsumed)
          return XmlNodeType.Text;
        switch (this.current.NodeType)
        {
          case XPathNodeType.Root:
            return XmlNodeType.None;
          case XPathNodeType.Element:
            return XmlNodeType.Element;
          case XPathNodeType.Attribute:
          case XPathNodeType.Namespace:
            return XmlNodeType.Attribute;
          case XPathNodeType.Text:
            return XmlNodeType.Text;
          case XPathNodeType.SignificantWhitespace:
            return XmlNodeType.SignificantWhitespace;
          case XPathNodeType.Whitespace:
            return XmlNodeType.Whitespace;
          case XPathNodeType.ProcessingInstruction:
            return XmlNodeType.ProcessingInstruction;
          case XPathNodeType.Comment:
            return XmlNodeType.Comment;
          default:
            throw new InvalidOperationException(string.Format("Current XPathNavigator status is {0} which is not acceptable to XmlReader.", (object) this.current.NodeType));
        }
      }
    }

    public override string Name
    {
      get
      {
        if (this.ReadState != ReadState.Interactive || this.attributeValueConsumed)
          return string.Empty;
        if (this.current.NodeType != XPathNodeType.Namespace)
          return this.current.Name;
        return this.current.Name == string.Empty ? "xmlns" : "xmlns:" + this.current.Name;
      }
    }

    public override string LocalName
    {
      get
      {
        if (this.ReadState != ReadState.Interactive || this.attributeValueConsumed)
          return string.Empty;
        return this.current.NodeType == XPathNodeType.Namespace && this.current.LocalName == string.Empty ? "xmlns" : this.current.LocalName;
      }
    }

    public override string NamespaceURI
    {
      get
      {
        if (this.ReadState != ReadState.Interactive || this.attributeValueConsumed)
          return string.Empty;
        return this.current.NodeType == XPathNodeType.Namespace ? "http://www.w3.org/2000/xmlns/" : this.current.NamespaceURI;
      }
    }

    public override string Prefix
    {
      get
      {
        if (this.ReadState != ReadState.Interactive || this.attributeValueConsumed)
          return string.Empty;
        return this.current.NodeType == XPathNodeType.Namespace && this.current.LocalName != string.Empty ? "xmlns" : this.current.Prefix;
      }
    }

    public override bool HasValue
    {
      get
      {
        switch (this.current.NodeType)
        {
          case XPathNodeType.Attribute:
          case XPathNodeType.Namespace:
          case XPathNodeType.Text:
          case XPathNodeType.SignificantWhitespace:
          case XPathNodeType.Whitespace:
          case XPathNodeType.ProcessingInstruction:
          case XPathNodeType.Comment:
            return true;
          default:
            return false;
        }
      }
    }

    public override int Depth
    {
      get
      {
        if (this.ReadState != ReadState.Interactive)
          return 0;
        if (this.NodeType == XmlNodeType.Attribute)
          return this.depth + 1;
        return this.attributeValueConsumed ? this.depth + 2 : this.depth;
      }
    }

    public override string Value
    {
      get
      {
        if (this.ReadState != ReadState.Interactive)
          return string.Empty;
        switch (this.current.NodeType)
        {
          case XPathNodeType.Root:
          case XPathNodeType.Element:
            return string.Empty;
          case XPathNodeType.Attribute:
          case XPathNodeType.Namespace:
          case XPathNodeType.Text:
          case XPathNodeType.SignificantWhitespace:
          case XPathNodeType.Whitespace:
          case XPathNodeType.ProcessingInstruction:
          case XPathNodeType.Comment:
            return this.current.Value;
          default:
            throw new InvalidOperationException("Current XPathNavigator status is {0} which is not acceptable to XmlReader.");
        }
      }
    }

    public override string BaseURI => this.current.BaseURI;

    public override bool IsEmptyElement => this.ReadState == ReadState.Interactive && this.current.IsEmptyElement;

    public override bool IsDefault => this.current is IXmlSchemaInfo current && current.IsDefault;

    public override char QuoteChar => '"';

    public override IXmlSchemaInfo SchemaInfo => this.current.SchemaInfo;

    public override string XmlLang => this.current.XmlLang;

    public override XmlSpace XmlSpace => XmlSpace.None;

    public override int AttributeCount => this.attributeCount;

    private int GetAttributeCount()
    {
      if (this.ReadState != ReadState.Interactive)
        return 0;
      int attributeCount = 0;
      if (this.current.MoveToFirstAttribute())
      {
        do
        {
          ++attributeCount;
        }
        while (this.current.MoveToNextAttribute());
        this.current.MoveToParent();
      }
      if (this.current.MoveToFirstNamespace(XPathNamespaceScope.Local))
      {
        do
        {
          ++attributeCount;
        }
        while (this.current.MoveToNextNamespace(XPathNamespaceScope.Local));
        this.current.MoveToParent();
      }
      return attributeCount;
    }

    private void SplitName(string name, out string localName, out string ns)
    {
      if (name == "xmlns")
      {
        localName = "xmlns";
        ns = "http://www.w3.org/2000/xmlns/";
      }
      else
      {
        localName = name;
        ns = string.Empty;
        int length = name.IndexOf(':');
        if (length <= 0)
          return;
        localName = name.Substring(length + 1, name.Length - length - 1);
        ns = this.LookupNamespace(name.Substring(0, length));
        if (!(name.Substring(0, length) == "xmlns"))
          return;
        ns = "http://www.w3.org/2000/xmlns/";
      }
    }

    public override string this[string name]
    {
      get
      {
        string localName;
        string ns;
        this.SplitName(name, out localName, out ns);
        return this[localName, ns];
      }
    }

    public override string this[string localName, string namespaceURI]
    {
      get
      {
        string attribute = this.current.GetAttribute(localName, namespaceURI);
        if (attribute != string.Empty)
          return attribute;
        return this.current.Clone().MoveToAttribute(localName, namespaceURI) ? string.Empty : (string) null;
      }
    }

    public override bool EOF => this.ReadState == ReadState.EndOfFile;

    public override ReadState ReadState
    {
      get
      {
        if (this.eof)
          return ReadState.EndOfFile;
        if (this.closed)
          return ReadState.Closed;
        return !this.started ? ReadState.Initial : ReadState.Interactive;
      }
    }

    public override XmlNameTable NameTable => this.current.NameTable;

    public override string GetAttribute(string name)
    {
      string localName;
      string ns;
      this.SplitName(name, out localName, out ns);
      return this[localName, ns];
    }

    public override string GetAttribute(string localName, string namespaceURI) => this[localName, namespaceURI];

    public override string GetAttribute(int i) => this[i];

    private bool CheckAttributeMove(bool b)
    {
      if (b)
        this.attributeValueConsumed = false;
      return b;
    }

    public override bool MoveToAttribute(string name)
    {
      string localName;
      string ns;
      this.SplitName(name, out localName, out ns);
      return this.MoveToAttribute(localName, ns);
    }

    public override bool MoveToAttribute(string localName, string namespaceURI)
    {
      bool flag = namespaceURI == "http://www.w3.org/2000/xmlns/";
      XPathNavigator xpathNavigator = (XPathNavigator) null;
      switch (this.current.NodeType)
      {
        case XPathNodeType.Element:
          if (this.MoveToFirstAttribute())
          {
            do
            {
              if (!flag ? this.current.LocalName == localName && this.current.NamespaceURI == namespaceURI : (!(localName == "xmlns") ? localName == this.current.Name : this.current.Name == string.Empty))
              {
                this.attributeValueConsumed = false;
                return true;
              }
            }
            while (this.MoveToNextAttribute());
            this.MoveToElement();
            break;
          }
          break;
        case XPathNodeType.Attribute:
        case XPathNodeType.Namespace:
          xpathNavigator = this.current.Clone();
          this.MoveToElement();
          goto case XPathNodeType.Element;
      }
      if (xpathNavigator != null)
        this.current = xpathNavigator;
      return false;
    }

    public override bool MoveToFirstAttribute()
    {
      switch (this.current.NodeType)
      {
        case XPathNodeType.Element:
          return this.CheckAttributeMove(this.current.MoveToFirstNamespace(XPathNamespaceScope.Local)) || this.CheckAttributeMove(this.current.MoveToFirstAttribute());
        case XPathNodeType.Attribute:
        case XPathNodeType.Namespace:
          this.current.MoveToParent();
          goto case XPathNodeType.Element;
        default:
          return false;
      }
    }

    public override bool MoveToNextAttribute()
    {
      switch (this.current.NodeType)
      {
        case XPathNodeType.Element:
          return this.MoveToFirstAttribute();
        case XPathNodeType.Attribute:
          return this.CheckAttributeMove(this.current.MoveToNextAttribute());
        case XPathNodeType.Namespace:
          if (this.CheckAttributeMove(this.current.MoveToNextNamespace(XPathNamespaceScope.Local)))
            return true;
          XPathNavigator other = this.current.Clone();
          this.current.MoveToParent();
          if (this.CheckAttributeMove(this.current.MoveToFirstAttribute()))
            return true;
          this.current.MoveTo(other);
          return false;
        default:
          return false;
      }
    }

    public override bool MoveToElement()
    {
      if (this.current.NodeType != XPathNodeType.Attribute && this.current.NodeType != XPathNodeType.Namespace)
        return false;
      this.attributeValueConsumed = false;
      return this.current.MoveToParent();
    }

    public override void Close()
    {
      this.closed = true;
      this.eof = true;
    }

    public override bool Read()
    {
      if (this.eof)
        return false;
      if (this.Binary != null)
        this.Binary.Reset();
      switch (this.ReadState)
      {
        case ReadState.Initial:
          this.started = true;
          this.root = this.current.Clone();
          if (this.current.NodeType == XPathNodeType.Root && !this.current.MoveToFirstChild())
          {
            this.endElement = false;
            this.eof = true;
            return false;
          }
          this.attributeCount = this.GetAttributeCount();
          return true;
        case ReadState.Interactive:
          if ((this.IsEmptyElement || this.endElement) && this.root.IsSamePosition(this.current))
          {
            this.eof = true;
            return false;
          }
          break;
        case ReadState.Error:
        case ReadState.EndOfFile:
        case ReadState.Closed:
          return false;
      }
      this.MoveToElement();
      if (this.endElement || !this.current.MoveToFirstChild())
      {
        if (!this.endElement && !this.current.IsEmptyElement && this.current.NodeType == XPathNodeType.Element)
          this.endElement = true;
        else if (!this.current.MoveToNext())
        {
          this.current.MoveToParent();
          if (this.current.NodeType == XPathNodeType.Root)
          {
            this.endElement = false;
            this.eof = true;
            return false;
          }
          this.endElement = this.current.NodeType == XPathNodeType.Element;
          if (this.endElement)
            --this.depth;
        }
        else
          this.endElement = false;
      }
      else
        ++this.depth;
      this.attributeCount = this.endElement || this.current.NodeType != XPathNodeType.Element ? 0 : this.GetAttributeCount();
      return true;
    }

    public override string ReadString()
    {
      this.readStringBuffer.Length = 0;
      XmlNodeType nodeType = this.NodeType;
      switch (nodeType)
      {
        case XmlNodeType.Element:
          if (this.IsEmptyElement)
            return string.Empty;
          while (true)
          {
            this.Read();
            switch (this.NodeType)
            {
              case XmlNodeType.Text:
              case XmlNodeType.CDATA:
              case XmlNodeType.Whitespace:
              case XmlNodeType.SignificantWhitespace:
                this.readStringBuffer.Append(this.Value);
                continue;
              default:
                goto label_9;
            }
          }
        case XmlNodeType.Text:
        case XmlNodeType.CDATA:
          while (true)
          {
            switch (this.NodeType)
            {
              case XmlNodeType.Text:
              case XmlNodeType.CDATA:
              case XmlNodeType.Whitespace:
              case XmlNodeType.SignificantWhitespace:
                this.readStringBuffer.Append(this.Value);
                this.Read();
                continue;
              default:
                goto label_9;
            }
          }
        default:
          if (nodeType != XmlNodeType.Whitespace && nodeType != XmlNodeType.SignificantWhitespace)
            return string.Empty;
          goto case XmlNodeType.Text;
      }
label_9:
      string str = this.readStringBuffer.ToString();
      this.readStringBuffer.Length = 0;
      return str;
    }

    public override string LookupNamespace(string prefix)
    {
      XPathNavigator xpathNavigator = this.current.Clone();
      try
      {
        this.MoveToElement();
        if (this.current.NodeType != XPathNodeType.Element)
          this.current.MoveToParent();
        if (this.current.MoveToFirstNamespace())
        {
          while (!(this.current.LocalName == prefix))
          {
            if (!this.current.MoveToNextNamespace())
              goto label_7;
          }
          return this.current.Value;
        }
label_7:
        return (string) null;
      }
      finally
      {
        this.current = xpathNavigator;
      }
    }

    public override void ResolveEntity() => throw new InvalidOperationException();

    public override bool ReadAttributeValue()
    {
      if (this.NodeType != XmlNodeType.Attribute || this.attributeValueConsumed)
        return false;
      this.attributeValueConsumed = true;
      return true;
    }
  }
}
