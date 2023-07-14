// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaInferenceException
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Runtime.Serialization;

namespace System.Xml.Schema
{
  [Serializable]
  public class XmlSchemaInferenceException : XmlSchemaException
  {
    public XmlSchemaInferenceException()
    {
    }

    public XmlSchemaInferenceException(string message)
      : base(message)
    {
    }

    protected XmlSchemaInferenceException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public XmlSchemaInferenceException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public XmlSchemaInferenceException(
      string message,
      Exception innerException,
      int line,
      int column)
      : base(message, innerException, line, column)
    {
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);
  }
}
