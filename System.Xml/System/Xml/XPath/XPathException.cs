// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathException
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Runtime.Serialization;

namespace System.Xml.XPath
{
  [Serializable]
  public class XPathException : SystemException
  {
    public XPathException()
      : base(string.Empty)
    {
    }

    protected XPathException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public XPathException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public XPathException(string message)
      : base(message, (Exception) null)
    {
    }

    public override string Message => base.Message;

    public override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);
  }
}
