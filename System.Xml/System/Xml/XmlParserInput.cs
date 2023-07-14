// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlParserInput
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml;
using System.Collections;
using System.Globalization;
using System.IO;

namespace System.Xml
{
  internal class XmlParserInput
  {
    private Stack sourceStack = new Stack();
    private XmlParserInput.XmlParserInputSource source;
    private bool has_peek;
    private int peek_char;
    private bool allowTextDecl = true;

    public XmlParserInput(TextReader reader, string baseURI)
      : this(reader, baseURI, 1, 0)
    {
    }

    public XmlParserInput(TextReader reader, string baseURI, int line, int column) => this.source = new XmlParserInput.XmlParserInputSource(reader, baseURI, false, line, column);

    public void Close()
    {
      while (this.sourceStack.Count > 0)
        ((XmlParserInput.XmlParserInputSource) this.sourceStack.Pop()).Close();
      this.source.Close();
    }

    public void Expect(int expected)
    {
      int num = this.ReadChar();
      if (num != expected)
        throw this.ReaderError(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "expected '{0}' ({1:X}) but found '{2}' ({3:X})", (object) (char) expected, (object) expected, (object) (char) num, (object) num));
    }

    public void Expect(string expected)
    {
      int length = expected.Length;
      for (int index = 0; index < length; ++index)
        this.Expect((int) expected[index]);
    }

    public void PushPEBuffer(DTDParameterEntityDeclaration pe)
    {
      this.sourceStack.Push((object) this.source);
      this.source = new XmlParserInput.XmlParserInputSource((TextReader) new StringReader(pe.ReplacementText), pe.ActualUri, true, 1, 0);
    }

    private int ReadSourceChar()
    {
      int num;
      for (num = this.source.Read(); num < 0 && this.sourceStack.Count > 0; num = this.source.Read())
        this.source = this.sourceStack.Pop() as XmlParserInput.XmlParserInputSource;
      return num;
    }

    public int PeekChar()
    {
      if (this.has_peek)
        return this.peek_char;
      this.peek_char = this.ReadSourceChar();
      if (this.peek_char >= 55296 && this.peek_char <= 56319)
      {
        this.peek_char = 65536 + (this.peek_char - 55296 << 10);
        int num = this.ReadSourceChar();
        if (num >= 56320 && num <= 57343)
          this.peek_char += num - 56320;
      }
      this.has_peek = true;
      return this.peek_char;
    }

    public int ReadChar()
    {
      int num = this.PeekChar();
      this.has_peek = false;
      return num;
    }

    public string BaseURI => this.source.BaseURI;

    public bool HasPEBuffer => this.sourceStack.Count > 0;

    public int LineNumber => this.source.LineNumber;

    public int LinePosition => this.source.LinePosition;

    public bool AllowTextDecl
    {
      get => this.allowTextDecl;
      set => this.allowTextDecl = value;
    }

    private XmlException ReaderError(string message) => new XmlException(message, (Exception) null, this.LineNumber, this.LinePosition);

    private class XmlParserInputSource
    {
      public readonly string BaseURI;
      private readonly TextReader reader;
      public int state;
      public bool isPE;
      private int line;
      private int column;

      public XmlParserInputSource(
        TextReader reader,
        string baseUri,
        bool pe,
        int line,
        int column)
      {
        this.BaseURI = baseUri;
        this.reader = reader;
        this.isPE = pe;
        this.line = line;
        this.column = column;
      }

      public int LineNumber => this.line;

      public int LinePosition => this.column;

      public void Close() => this.reader.Close();

      public int Read()
      {
        if (this.state == 2)
          return -1;
        if (this.isPE && this.state == 0)
        {
          this.state = 1;
          return 32;
        }
        int num = this.reader.Read();
        if (num == 10)
        {
          ++this.line;
          this.column = 1;
        }
        else if (num >= 0)
          ++this.column;
        if (num >= 0 || this.state != 1)
          return num;
        this.state = 2;
        return 32;
      }
    }
  }
}
