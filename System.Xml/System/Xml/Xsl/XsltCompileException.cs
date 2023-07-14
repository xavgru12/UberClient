// Decompiled with JetBrains decompiler
// Type: System.Xml.Xsl.XsltCompileException
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Runtime.Serialization;
using System.Xml.XPath;

namespace System.Xml.Xsl
{
  [Serializable]
  public class XsltCompileException : XsltException
  {
    public XsltCompileException()
    {
    }

    public XsltCompileException(string message)
      : base(message)
    {
    }

    public XsltCompileException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected XsltCompileException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public XsltCompileException(
      Exception inner,
      string sourceUri,
      int lineNumber,
      int linePosition)
      : base(lineNumber == 0 ? "{0}." : "{0} at {1}({2},{3}). See InnerException for details.", "XSLT compile error", inner, lineNumber, linePosition, sourceUri)
    {
    }

    internal XsltCompileException(string message, Exception innerException, XPathNavigator nav)
      : base(message, innerException, nav)
    {
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);
  }
}
