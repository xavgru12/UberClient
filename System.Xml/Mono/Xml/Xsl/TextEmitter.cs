// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.TextEmitter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.IO;
using System.Text;

namespace Mono.Xml.Xsl
{
  internal class TextEmitter : Emitter
  {
    private TextWriter writer;

    public TextEmitter(TextWriter writer) => this.writer = writer;

    public override void WriteStartDocument(Encoding encoding, StandaloneType standalone)
    {
    }

    public override void WriteEndDocument()
    {
    }

    public override void WriteDocType(string type, string publicId, string systemId)
    {
    }

    public override void WriteStartElement(string prefix, string localName, string nsURI)
    {
    }

    public override void WriteEndElement()
    {
    }

    public override void WriteAttributeString(
      string prefix,
      string localName,
      string nsURI,
      string value)
    {
    }

    public override void WriteComment(string text)
    {
    }

    public override void WriteProcessingInstruction(string name, string text)
    {
    }

    public override void WriteString(string text) => this.writer.Write(text);

    public override void WriteRaw(string data) => this.writer.Write(data);

    public override void WriteCDataSection(string text) => this.writer.Write(text);

    public override void Done()
    {
    }
  }
}
