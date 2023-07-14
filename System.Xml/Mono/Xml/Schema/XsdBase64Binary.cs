// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdBase64Binary
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdBase64Binary : XsdString
  {
    private static string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
    private static byte[] decodeTable;

    internal XsdBase64Binary()
    {
    }

    static XsdBase64Binary()
    {
      int length = XsdBase64Binary.ALPHABET.Length;
      XsdBase64Binary.decodeTable = new byte[123];
      for (int index = 0; index < XsdBase64Binary.decodeTable.Length; ++index)
        XsdBase64Binary.decodeTable[index] = byte.MaxValue;
      for (int index1 = 0; index1 < length; ++index1)
      {
        char index2 = XsdBase64Binary.ALPHABET[index1];
        XsdBase64Binary.decodeTable[(int) index2] = (byte) index1;
      }
    }

    public override XmlTypeCode TypeCode => XmlTypeCode.Base64Binary;

    public override Type ValueType => typeof (byte[]);

    public override object ParseValue(
      string s,
      XmlNameTable nameTable,
      IXmlNamespaceResolver nsmgr)
    {
      byte[] bytes = new ASCIIEncoding().GetBytes(s);
      return (object) new FromBase64Transform().TransformFinalBlock(bytes, 0, bytes.Length);
    }

    internal override int Length(string s)
    {
      int num1 = 0;
      int num2 = 0;
      int length = s.Length;
      for (int index = 0; index < length; ++index)
      {
        char ch = s[index];
        if (!char.IsWhiteSpace(ch))
        {
          if (XsdBase64Binary.isData(ch))
          {
            ++num1;
          }
          else
          {
            if (!XsdBase64Binary.isPad(ch))
              return -1;
            ++num2;
          }
        }
      }
      if (num2 > 2)
        return -1;
      if (num2 > 0)
        num2 = 3 - num2;
      return num1 / 4 * 3 + num2;
    }

    protected static bool isPad(char octect) => octect == '=';

    protected static bool isData(char octect) => octect <= 'z' && XsdBase64Binary.decodeTable[(int) octect] != byte.MaxValue;

    internal override System.ValueType ParseValueType(
      string s,
      XmlNameTable nameTable,
      IXmlNamespaceResolver nsmgr)
    {
      return (System.ValueType) new StringValueType(this.ParseValue(s, nameTable, nsmgr) as string);
    }
  }
}
