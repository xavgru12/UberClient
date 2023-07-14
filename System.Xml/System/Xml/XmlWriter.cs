// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlWriter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.IO;
using System.Text;
using System.Xml.XPath;

namespace System.Xml
{
  public abstract class XmlWriter : IDisposable
  {
    private XmlWriterSettings settings;

    void IDisposable.Dispose() => this.Dispose(false);

    public virtual XmlWriterSettings Settings
    {
      get
      {
        if (this.settings == null)
          this.settings = new XmlWriterSettings();
        return this.settings;
      }
    }

    public abstract WriteState WriteState { get; }

    public virtual string XmlLang => (string) null;

    public virtual XmlSpace XmlSpace => XmlSpace.None;

    public abstract void Close();

    public static XmlWriter Create(Stream stream) => XmlWriter.Create(stream, (XmlWriterSettings) null);

    public static XmlWriter Create(string file) => XmlWriter.Create(file, (XmlWriterSettings) null);

    public static XmlWriter Create(TextWriter writer) => XmlWriter.Create(writer, (XmlWriterSettings) null);

    public static XmlWriter Create(XmlWriter writer) => XmlWriter.Create(writer, (XmlWriterSettings) null);

    public static XmlWriter Create(StringBuilder builder) => XmlWriter.Create(builder, (XmlWriterSettings) null);

    public static XmlWriter Create(Stream stream, XmlWriterSettings settings)
    {
      Encoding encoding = settings == null ? Encoding.UTF8 : settings.Encoding;
      return XmlWriter.Create((TextWriter) new StreamWriter(stream, encoding), settings);
    }

    public static XmlWriter Create(string file, XmlWriterSettings settings)
    {
      Encoding encoding = settings == null ? Encoding.UTF8 : settings.Encoding;
      return XmlWriter.CreateTextWriter((TextWriter) new StreamWriter(file, false, encoding), settings, true);
    }

    public static XmlWriter Create(StringBuilder builder, XmlWriterSettings settings) => XmlWriter.Create((TextWriter) new StringWriter(builder), settings);

    public static XmlWriter Create(TextWriter writer, XmlWriterSettings settings)
    {
      if (settings == null)
        settings = new XmlWriterSettings();
      return XmlWriter.CreateTextWriter(writer, settings, settings.CloseOutput);
    }

    public static XmlWriter Create(XmlWriter writer, XmlWriterSettings settings)
    {
      if (settings == null)
        settings = new XmlWriterSettings();
      writer.settings = settings;
      return writer;
    }

    private static XmlWriter CreateTextWriter(
      TextWriter writer,
      XmlWriterSettings settings,
      bool closeOutput)
    {
      if (settings == null)
        settings = new XmlWriterSettings();
      return XmlWriter.Create((XmlWriter) new XmlTextWriter(writer, settings, closeOutput), settings);
    }

    protected virtual void Dispose(bool disposing) => this.Close();

    public abstract void Flush();

    public abstract string LookupPrefix(string ns);

    private void WriteAttribute(XmlReader reader, bool defattr)
    {
      if (!defattr && reader.IsDefault)
        return;
      this.WriteStartAttribute(reader.Prefix, reader.LocalName, reader.NamespaceURI);
      while (reader.ReadAttributeValue())
      {
        switch (reader.NodeType)
        {
          case XmlNodeType.Text:
            this.WriteString(reader.Value);
            continue;
          case XmlNodeType.EntityReference:
            this.WriteEntityRef(reader.Name);
            continue;
          default:
            continue;
        }
      }
      this.WriteEndAttribute();
    }

    public virtual void WriteAttributes(XmlReader reader, bool defattr)
    {
      if (reader == null)
        throw new ArgumentException("null XmlReader specified.", nameof (reader));
      switch (reader.NodeType)
      {
        case XmlNodeType.Element:
          if (!reader.MoveToFirstAttribute())
            break;
          goto case XmlNodeType.Attribute;
        case XmlNodeType.Attribute:
          do
          {
            this.WriteAttribute(reader, defattr);
          }
          while (reader.MoveToNextAttribute());
          reader.MoveToElement();
          break;
        case XmlNodeType.XmlDeclaration:
          this.WriteAttributeString("version", reader["version"]);
          if (reader["encoding"] != null)
            this.WriteAttributeString("encoding", reader["encoding"]);
          if (reader["standalone"] == null)
            break;
          this.WriteAttributeString("standalone", reader["standalone"]);
          break;
        default:
          throw new XmlException("NodeType is not one of Element, Attribute, nor XmlDeclaration.");
      }
    }

    public void WriteAttributeString(string localName, string value) => this.WriteAttributeString(string.Empty, localName, (string) null, value);

    public void WriteAttributeString(string localName, string ns, string value) => this.WriteAttributeString(string.Empty, localName, ns, value);

    public void WriteAttributeString(string prefix, string localName, string ns, string value)
    {
      this.WriteStartAttribute(prefix, localName, ns);
      if (value != null && value.Length > 0)
        this.WriteString(value);
      this.WriteEndAttribute();
    }

    public abstract void WriteBase64(byte[] buffer, int index, int count);

    public virtual void WriteBinHex(byte[] buffer, int index, int count)
    {
      StringWriter w = new StringWriter();
      XmlConvert.WriteBinHex(buffer, index, count, (TextWriter) w);
      this.WriteString(w.ToString());
    }

    public abstract void WriteCData(string text);

    public abstract void WriteCharEntity(char ch);

    public abstract void WriteChars(char[] buffer, int index, int count);

    public abstract void WriteComment(string text);

    public abstract void WriteDocType(string name, string pubid, string sysid, string subset);

    public void WriteElementString(string localName, string value)
    {
      this.WriteStartElement(localName);
      if (value != null && value.Length > 0)
        this.WriteString(value);
      this.WriteEndElement();
    }

    public void WriteElementString(string localName, string ns, string value)
    {
      this.WriteStartElement(localName, ns);
      if (value != null && value.Length > 0)
        this.WriteString(value);
      this.WriteEndElement();
    }

    public void WriteElementString(string prefix, string localName, string ns, string value)
    {
      this.WriteStartElement(prefix, localName, ns);
      if (value != null && value.Length > 0)
        this.WriteString(value);
      this.WriteEndElement();
    }

    public abstract void WriteEndAttribute();

    public abstract void WriteEndDocument();

    public abstract void WriteEndElement();

    public abstract void WriteEntityRef(string name);

    public abstract void WriteFullEndElement();

    public virtual void WriteName(string name) => this.WriteNameInternal(name);

    public virtual void WriteNmToken(string name) => this.WriteNmTokenInternal(name);

    public virtual void WriteQualifiedName(string localName, string ns) => this.WriteQualifiedNameInternal(localName, ns);

    internal void WriteNameInternal(string name)
    {
      switch (this.Settings.ConformanceLevel)
      {
        case ConformanceLevel.Fragment:
        case ConformanceLevel.Document:
          XmlConvert.VerifyName(name);
          break;
      }
      this.WriteString(name);
    }

    internal virtual void WriteNmTokenInternal(string name)
    {
      bool flag = true;
      switch (this.Settings.ConformanceLevel)
      {
        case ConformanceLevel.Fragment:
        case ConformanceLevel.Document:
          flag = XmlChar.IsNmToken(name);
          break;
      }
      if (!flag)
        throw new ArgumentException("Argument name is not a valid NMTOKEN.");
      this.WriteString(name);
    }

    internal void WriteQualifiedNameInternal(string localName, string ns)
    {
      if (localName == null || localName == string.Empty)
        throw new ArgumentException();
      if (ns == null)
        ns = string.Empty;
      switch (this.Settings.ConformanceLevel)
      {
        case ConformanceLevel.Fragment:
        case ConformanceLevel.Document:
          XmlConvert.VerifyNCName(localName);
          break;
      }
      string text = ns.Length <= 0 ? string.Empty : this.LookupPrefix(ns);
      if (text == null)
        throw new ArgumentException(string.Format("Namespace '{0}' is not declared.", (object) ns));
      if (text != string.Empty)
      {
        this.WriteString(text);
        this.WriteString(":");
        this.WriteString(localName);
      }
      else
        this.WriteString(localName);
    }

    public virtual void WriteNode(XPathNavigator navigator, bool defattr)
    {
      if (navigator == null)
        throw new ArgumentNullException(nameof (navigator));
      switch (navigator.NodeType)
      {
        case XPathNodeType.Root:
          if (!navigator.MoveToFirstChild())
            break;
          do
          {
            this.WriteNode(navigator, defattr);
          }
          while (navigator.MoveToNext());
          navigator.MoveToParent();
          break;
        case XPathNodeType.Element:
          this.WriteStartElement(navigator.Prefix, navigator.LocalName, navigator.NamespaceURI);
          if (navigator.MoveToFirstNamespace(XPathNamespaceScope.Local))
          {
            do
            {
              if (defattr || navigator.SchemaInfo == null || navigator.SchemaInfo.IsDefault)
                this.WriteAttributeString(navigator.Prefix, !(navigator.LocalName == string.Empty) ? navigator.LocalName : "xmlns", "http://www.w3.org/2000/xmlns/", navigator.Value);
            }
            while (navigator.MoveToNextNamespace(XPathNamespaceScope.Local));
            navigator.MoveToParent();
          }
          if (navigator.MoveToFirstAttribute())
          {
            do
            {
              if (defattr || navigator.SchemaInfo == null || navigator.SchemaInfo.IsDefault)
                this.WriteAttributeString(navigator.Prefix, navigator.LocalName, navigator.NamespaceURI, navigator.Value);
            }
            while (navigator.MoveToNextAttribute());
            navigator.MoveToParent();
          }
          if (navigator.MoveToFirstChild())
          {
            do
            {
              this.WriteNode(navigator, defattr);
            }
            while (navigator.MoveToNext());
            navigator.MoveToParent();
          }
          if (navigator.IsEmptyElement)
          {
            this.WriteEndElement();
            break;
          }
          this.WriteFullEndElement();
          break;
        case XPathNodeType.Attribute:
          break;
        case XPathNodeType.Namespace:
          break;
        case XPathNodeType.Text:
          this.WriteString(navigator.Value);
          break;
        case XPathNodeType.SignificantWhitespace:
          this.WriteWhitespace(navigator.Value);
          break;
        case XPathNodeType.Whitespace:
          this.WriteWhitespace(navigator.Value);
          break;
        case XPathNodeType.ProcessingInstruction:
          this.WriteProcessingInstruction(navigator.Name, navigator.Value);
          break;
        case XPathNodeType.Comment:
          this.WriteComment(navigator.Value);
          break;
        default:
          throw new NotSupportedException();
      }
    }

    public virtual void WriteNode(XmlReader reader, bool defattr)
    {
      if (reader == null)
        throw new ArgumentException();
      if (reader.ReadState == ReadState.Initial)
      {
        reader.Read();
        do
        {
          this.WriteNode(reader, defattr);
        }
        while (!reader.EOF);
      }
      else
      {
        switch (reader.NodeType)
        {
          case XmlNodeType.None:
          case XmlNodeType.EndEntity:
            reader.Read();
            break;
          case XmlNodeType.Element:
            this.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
            if (reader.HasAttributes)
            {
              for (int i = 0; i < reader.AttributeCount; ++i)
              {
                reader.MoveToAttribute(i);
                this.WriteAttribute(reader, defattr);
              }
              reader.MoveToElement();
            }
            if (reader.IsEmptyElement)
            {
              this.WriteEndElement();
              goto case XmlNodeType.None;
            }
            else
            {
              int depth = reader.Depth;
              reader.Read();
              if (reader.NodeType != XmlNodeType.EndElement)
              {
                do
                {
                  this.WriteNode(reader, defattr);
                }
                while (depth < reader.Depth);
              }
              this.WriteFullEndElement();
              goto case XmlNodeType.None;
            }
          case XmlNodeType.Attribute:
            break;
          case XmlNodeType.Text:
            this.WriteString(reader.Value);
            goto case XmlNodeType.None;
          case XmlNodeType.CDATA:
            this.WriteCData(reader.Value);
            goto case XmlNodeType.None;
          case XmlNodeType.EntityReference:
            this.WriteEntityRef(reader.Name);
            goto case XmlNodeType.None;
          case XmlNodeType.ProcessingInstruction:
          case XmlNodeType.XmlDeclaration:
            this.WriteProcessingInstruction(reader.Name, reader.Value);
            goto case XmlNodeType.None;
          case XmlNodeType.Comment:
            this.WriteComment(reader.Value);
            goto case XmlNodeType.None;
          case XmlNodeType.DocumentType:
            this.WriteDocType(reader.Name, reader["PUBLIC"], reader["SYSTEM"], reader.Value);
            goto case XmlNodeType.None;
          case XmlNodeType.Whitespace:
          case XmlNodeType.SignificantWhitespace:
            this.WriteWhitespace(reader.Value);
            goto case XmlNodeType.None;
          case XmlNodeType.EndElement:
            this.WriteFullEndElement();
            goto case XmlNodeType.None;
          default:
            throw new XmlException("Unexpected node " + reader.Name + " of type " + (object) reader.NodeType);
        }
      }
    }

    public abstract void WriteProcessingInstruction(string name, string text);

    public abstract void WriteRaw(string data);

    public abstract void WriteRaw(char[] buffer, int index, int count);

    public void WriteStartAttribute(string localName) => this.WriteStartAttribute((string) null, localName, (string) null);

    public void WriteStartAttribute(string localName, string ns) => this.WriteStartAttribute((string) null, localName, ns);

    public abstract void WriteStartAttribute(string prefix, string localName, string ns);

    public abstract void WriteStartDocument();

    public abstract void WriteStartDocument(bool standalone);

    public void WriteStartElement(string localName) => this.WriteStartElement((string) null, localName, (string) null);

    public void WriteStartElement(string localName, string ns) => this.WriteStartElement((string) null, localName, ns);

    public abstract void WriteStartElement(string prefix, string localName, string ns);

    public abstract void WriteString(string text);

    public abstract void WriteSurrogateCharEntity(char lowChar, char highChar);

    public abstract void WriteWhitespace(string ws);

    public virtual void WriteValue(bool value) => this.WriteString(XQueryConvert.BooleanToString(value));

    public virtual void WriteValue(DateTime value) => this.WriteString(XmlConvert.ToString(value));

    public virtual void WriteValue(Decimal value) => this.WriteString(XQueryConvert.DecimalToString(value));

    public virtual void WriteValue(double value) => this.WriteString(XQueryConvert.DoubleToString(value));

    public virtual void WriteValue(int value) => this.WriteString(XQueryConvert.IntToString(value));

    public virtual void WriteValue(long value) => this.WriteString(XQueryConvert.IntegerToString(value));

    public virtual void WriteValue(object value)
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (value));
        case string _:
          this.WriteString((string) value);
          break;
        case bool flag2:
          this.WriteValue(flag2);
          break;
        case byte _:
          this.WriteValue((int) value);
          break;
        case byte[] _:
          this.WriteBase64((byte[]) value, 0, ((byte[]) value).Length);
          break;
        case char[] _:
          this.WriteChars((char[]) value, 0, ((char[]) value).Length);
          break;
        case DateTime dateTime:
          this.WriteValue(dateTime);
          break;
        case Decimal num1:
          this.WriteValue(num1);
          break;
        case double num2:
          this.WriteValue(num2);
          break;
        case short _:
          this.WriteValue((int) value);
          break;
        case int num3:
          this.WriteValue(num3);
          break;
        case long num4:
          this.WriteValue(num4);
          break;
        case float num5:
          this.WriteValue(num5);
          break;
        case TimeSpan timeSpan:
          this.WriteString(XmlConvert.ToString(timeSpan));
          break;
        default:
          if ((object) (value as XmlQualifiedName) != null)
          {
            XmlQualifiedName xmlQualifiedName = (XmlQualifiedName) value;
            if (!xmlQualifiedName.Equals((object) XmlQualifiedName.Empty))
            {
              if (xmlQualifiedName.Namespace.Length > 0 && this.LookupPrefix(xmlQualifiedName.Namespace) == null)
                throw new InvalidCastException(string.Format("The QName '{0}' cannot be written. No corresponding prefix is declared", (object) xmlQualifiedName));
              this.WriteQualifiedName(xmlQualifiedName.Name, xmlQualifiedName.Namespace);
              break;
            }
            this.WriteString(string.Empty);
            break;
          }
          if (!(value is IEnumerable))
            throw new InvalidCastException(string.Format("Type '{0}' cannot be cast to string", (object) value.GetType()));
          bool flag1 = false;
          IEnumerator enumerator = ((IEnumerable) value).GetEnumerator();
          try
          {
            while (enumerator.MoveNext())
            {
              object current = enumerator.Current;
              if (flag1)
                this.WriteString(" ");
              else
                flag1 = true;
              this.WriteValue(current);
            }
            break;
          }
          finally
          {
            if (enumerator is IDisposable disposable)
              disposable.Dispose();
          }
      }
    }

    public virtual void WriteValue(float value) => this.WriteString(XQueryConvert.FloatToString(value));

    public virtual void WriteValue(string value) => this.WriteString(value);
  }
}
