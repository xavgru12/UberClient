// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaValidationException
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Runtime.Serialization;

namespace System.Xml.Schema
{
  [Serializable]
  public class XmlSchemaValidationException : XmlSchemaException
  {
    private object source_object;

    public XmlSchemaValidationException()
    {
    }

    public XmlSchemaValidationException(string message)
      : base(message)
    {
    }

    protected XmlSchemaValidationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public XmlSchemaValidationException(
      string message,
      Exception innerException,
      int lineNumber,
      int linePosition)
      : base(message, lineNumber, linePosition, (XmlSchemaObject) null, (string) null, innerException)
    {
    }

    internal XmlSchemaValidationException(
      string message,
      int lineNumber,
      int linePosition,
      XmlSchemaObject sourceObject,
      string sourceUri,
      Exception innerException)
      : base(message, lineNumber, linePosition, sourceObject, sourceUri, innerException)
    {
    }

    internal XmlSchemaValidationException(
      string message,
      object sender,
      string sourceUri,
      XmlSchemaObject sourceObject,
      Exception innerException)
      : base(message, sender, sourceUri, sourceObject, innerException)
    {
    }

    internal XmlSchemaValidationException(
      string message,
      XmlSchemaObject sourceObject,
      Exception innerException)
      : base(message, sourceObject, innerException)
    {
    }

    public XmlSchemaValidationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);

    protected internal void SetSourceObject(object o) => this.source_object = o;

    public object SourceObject => this.source_object;
  }
}
