// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlStreamReader
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.IO;
using System.Runtime.InteropServices;

namespace System.Xml
{
  internal class XmlStreamReader : NonBlockingStreamReader
  {
    private XmlInputStream input;
    private static XmlException invalidDataException = new XmlException("invalid data.");

    private XmlStreamReader(XmlInputStream input)
      : base((Stream) input, input.ActualEncoding == null ? XmlInputStream.StrictUTF8 : input.ActualEncoding)
    {
      this.input = input;
    }

    public XmlStreamReader(Stream input)
      : this(new XmlInputStream(input))
    {
    }

    public override void Close() => this.input.Close();

    public override int Read([In, Out] char[] dest_buffer, int index, int count)
    {
      try
      {
        return base.Read(dest_buffer, index, count);
      }
      catch (ArgumentException ex)
      {
        throw XmlStreamReader.invalidDataException;
      }
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (!disposing)
        return;
      this.Close();
    }
  }
}
