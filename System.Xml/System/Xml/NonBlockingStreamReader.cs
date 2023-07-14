// Decompiled with JetBrains decompiler
// Type: System.Xml.NonBlockingStreamReader
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Xml
{
  internal class NonBlockingStreamReader : TextReader
  {
    private const int DefaultBufferSize = 1024;
    private const int DefaultFileBufferSize = 4096;
    private const int MinimumBufferSize = 128;
    private byte[] input_buffer;
    private char[] decoded_buffer;
    private int decoded_count;
    private int pos;
    private int buffer_size;
    private Encoding encoding;
    private Decoder decoder;
    private Stream base_stream;
    private bool mayBlock;
    private StringBuilder line_builder;
    private bool foundCR;

    public NonBlockingStreamReader(Stream stream, Encoding encoding)
    {
      int byteCount = 1024;
      this.base_stream = stream;
      this.input_buffer = new byte[byteCount];
      this.buffer_size = byteCount;
      this.encoding = encoding;
      this.decoder = encoding.GetDecoder();
      this.decoded_buffer = new char[encoding.GetMaxCharCount(byteCount)];
      this.decoded_count = 0;
      this.pos = 0;
    }

    public Encoding Encoding => this.encoding;

    public override void Close() => this.Dispose(true);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.base_stream != null)
        this.base_stream.Close();
      this.input_buffer = (byte[]) null;
      this.decoded_buffer = (char[]) null;
      this.encoding = (Encoding) null;
      this.decoder = (Decoder) null;
      this.base_stream = (Stream) null;
      base.Dispose(disposing);
    }

    public void DiscardBufferedData()
    {
      this.pos = this.decoded_count = 0;
      this.mayBlock = false;
      this.decoder.Reset();
    }

    private int ReadBuffer()
    {
      this.pos = 0;
      this.decoded_count = 0;
      int byteIndex = 0;
      do
      {
        int byteCount = this.base_stream.Read(this.input_buffer, 0, this.buffer_size);
        if (byteCount == 0)
          return 0;
        this.mayBlock = byteCount < this.buffer_size;
        this.decoded_count += this.decoder.GetChars(this.input_buffer, byteIndex, byteCount, this.decoded_buffer, 0);
        byteIndex = 0;
      }
      while (this.decoded_count == 0);
      return this.decoded_count;
    }

    public override int Peek()
    {
      if (this.base_stream == null)
        throw new ObjectDisposedException("StreamReader", "Cannot read from a closed StreamReader");
      return this.pos >= this.decoded_count && (this.mayBlock || this.ReadBuffer() == 0) ? -1 : (int) this.decoded_buffer[this.pos];
    }

    public override int Read()
    {
      if (this.base_stream == null)
        throw new ObjectDisposedException("StreamReader", "Cannot read from a closed StreamReader");
      return this.pos >= this.decoded_count && this.ReadBuffer() == 0 ? -1 : (int) this.decoded_buffer[this.pos++];
    }

    public override int Read([In, Out] char[] dest_buffer, int index, int count)
    {
      if (this.base_stream == null)
        throw new ObjectDisposedException("StreamReader", "Cannot read from a closed StreamReader");
      if (dest_buffer == null)
        throw new ArgumentNullException(nameof (dest_buffer));
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index), "< 0");
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count), "< 0");
      if (index > dest_buffer.Length - count)
        throw new ArgumentException("index + count > dest_buffer.Length");
      int num = 0;
      if (this.pos >= this.decoded_count && this.ReadBuffer() == 0)
        return num > 0 ? num : 0;
      int length = Math.Min(this.decoded_count - this.pos, count);
      Array.Copy((Array) this.decoded_buffer, this.pos, (Array) dest_buffer, index, length);
      this.pos += length;
      index += length;
      count -= length;
      return num + length;
    }

    private int FindNextEOL()
    {
      for (; this.pos < this.decoded_count; ++this.pos)
      {
        char ch = this.decoded_buffer[this.pos];
        if (ch == '\n')
        {
          ++this.pos;
          int nextEol = !this.foundCR ? this.pos - 1 : this.pos - 2;
          if (nextEol < 0)
            nextEol = 0;
          this.foundCR = false;
          return nextEol;
        }
        if (this.foundCR)
        {
          this.foundCR = false;
          return this.pos - 1;
        }
        this.foundCR = ch == '\r';
      }
      return -1;
    }

    public override string ReadLine()
    {
      if (this.base_stream == null)
        throw new ObjectDisposedException("StreamReader", "Cannot read from a closed StreamReader");
      if (this.pos >= this.decoded_count && this.ReadBuffer() == 0)
        return (string) null;
      int pos = this.pos;
      int nextEol1 = this.FindNextEOL();
      if (nextEol1 < this.decoded_count && nextEol1 >= pos)
        return new string(this.decoded_buffer, pos, nextEol1 - pos);
      if (this.line_builder == null)
        this.line_builder = new StringBuilder();
      else
        this.line_builder.Length = 0;
      int nextEol2;
      do
      {
        if (this.foundCR)
          --this.decoded_count;
        this.line_builder.Append(new string(this.decoded_buffer, pos, this.decoded_count - pos));
        if (this.ReadBuffer() == 0)
        {
          if (this.line_builder.Capacity <= 32768)
            return this.line_builder.ToString(0, this.line_builder.Length);
          StringBuilder lineBuilder = this.line_builder;
          this.line_builder = (StringBuilder) null;
          return lineBuilder.ToString(0, lineBuilder.Length);
        }
        pos = this.pos;
        nextEol2 = this.FindNextEOL();
      }
      while (nextEol2 >= this.decoded_count || nextEol2 < pos);
      this.line_builder.Append(new string(this.decoded_buffer, pos, nextEol2 - pos));
      if (this.line_builder.Capacity <= 32768)
        return this.line_builder.ToString(0, this.line_builder.Length);
      StringBuilder lineBuilder1 = this.line_builder;
      this.line_builder = (StringBuilder) null;
      return lineBuilder1.ToString(0, lineBuilder1.Length);
    }

    public override string ReadToEnd()
    {
      if (this.base_stream == null)
        throw new ObjectDisposedException("StreamReader", "Cannot read from a closed StreamReader");
      StringBuilder stringBuilder = new StringBuilder();
      int length = this.decoded_buffer.Length;
      char[] dest_buffer = new char[length];
      int charCount;
      while ((charCount = this.Read(dest_buffer, 0, length)) != 0)
        stringBuilder.Append(dest_buffer, 0, charCount);
      return stringBuilder.ToString();
    }
  }
}
