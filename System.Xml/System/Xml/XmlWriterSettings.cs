// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlWriterSettings
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;

namespace System.Xml
{
  public sealed class XmlWriterSettings
  {
    private bool checkCharacters;
    private bool closeOutput;
    private ConformanceLevel conformance;
    private Encoding encoding;
    private bool indent;
    private string indentChars;
    private string newLineChars;
    private bool newLineOnAttributes;
    private NewLineHandling newLineHandling;
    private bool omitXmlDeclaration;
    private XmlOutputMethod outputMethod;

    public XmlWriterSettings() => this.Reset();

    public XmlWriterSettings Clone() => (XmlWriterSettings) this.MemberwiseClone();

    public void Reset()
    {
      this.checkCharacters = true;
      this.closeOutput = false;
      this.conformance = ConformanceLevel.Document;
      this.encoding = Encoding.UTF8;
      this.indent = false;
      this.indentChars = "  ";
      this.newLineChars = Environment.NewLine;
      this.newLineOnAttributes = false;
      this.newLineHandling = NewLineHandling.None;
      this.omitXmlDeclaration = false;
      this.outputMethod = XmlOutputMethod.AutoDetect;
    }

    public bool CheckCharacters
    {
      get => this.checkCharacters;
      set => this.checkCharacters = value;
    }

    public bool CloseOutput
    {
      get => this.closeOutput;
      set => this.closeOutput = value;
    }

    public ConformanceLevel ConformanceLevel
    {
      get => this.conformance;
      set => this.conformance = value;
    }

    public Encoding Encoding
    {
      get => this.encoding;
      set => this.encoding = value;
    }

    public bool Indent
    {
      get => this.indent;
      set => this.indent = value;
    }

    public string IndentChars
    {
      get => this.indentChars;
      set => this.indentChars = value;
    }

    public string NewLineChars
    {
      get => this.newLineChars;
      set => this.newLineChars = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    public bool NewLineOnAttributes
    {
      get => this.newLineOnAttributes;
      set => this.newLineOnAttributes = value;
    }

    public NewLineHandling NewLineHandling
    {
      get => this.newLineHandling;
      set => this.newLineHandling = value;
    }

    public bool OmitXmlDeclaration
    {
      get => this.omitXmlDeclaration;
      set => this.omitXmlDeclaration = value;
    }

    public XmlOutputMethod OutputMethod => this.outputMethod;

    internal NamespaceHandling NamespaceHandling { get; set; }
  }
}
