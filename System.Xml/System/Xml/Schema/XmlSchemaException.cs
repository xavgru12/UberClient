// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaException
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Globalization;
using System.Runtime.Serialization;

namespace System.Xml.Schema
{
  [Serializable]
  public class XmlSchemaException : SystemException
  {
    private bool hasLineInfo;
    private int lineNumber;
    private int linePosition;
    private XmlSchemaObject sourceObj;
    private string sourceUri;

    public XmlSchemaException()
      : this("A schema error occured.", (Exception) null)
    {
    }

    public XmlSchemaException(string message)
      : this(message, (Exception) null)
    {
    }

    protected XmlSchemaException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.hasLineInfo = info.GetBoolean(nameof (hasLineInfo));
      this.lineNumber = info.GetInt32(nameof (lineNumber));
      this.linePosition = info.GetInt32(nameof (linePosition));
      this.sourceUri = info.GetString(nameof (sourceUri));
      this.sourceObj = info.GetValue(nameof (sourceObj), typeof (XmlSchemaObject)) as XmlSchemaObject;
    }

    public XmlSchemaException(
      string message,
      Exception innerException,
      int lineNumber,
      int linePosition)
      : this(message, lineNumber, linePosition, (XmlSchemaObject) null, (string) null, innerException)
    {
    }

    internal XmlSchemaException(
      string message,
      int lineNumber,
      int linePosition,
      XmlSchemaObject sourceObject,
      string sourceUri,
      Exception innerException)
      : base(XmlSchemaException.GetMessage(message, sourceUri, lineNumber, linePosition, sourceObject), innerException)
    {
      this.hasLineInfo = true;
      this.lineNumber = lineNumber;
      this.linePosition = linePosition;
      this.sourceObj = sourceObject;
      this.sourceUri = sourceUri;
    }

    internal XmlSchemaException(
      string message,
      object sender,
      string sourceUri,
      XmlSchemaObject sourceObject,
      Exception innerException)
      : base(XmlSchemaException.GetMessage(message, sourceUri, sender, sourceObject), innerException)
    {
      if (sender is IXmlLineInfo xmlLineInfo && xmlLineInfo.HasLineInfo())
      {
        this.hasLineInfo = true;
        this.lineNumber = xmlLineInfo.LineNumber;
        this.linePosition = xmlLineInfo.LinePosition;
      }
      this.sourceObj = sourceObject;
    }

    internal XmlSchemaException(
      string message,
      XmlSchemaObject sourceObject,
      Exception innerException)
      : base(XmlSchemaException.GetMessage(message, (string) null, 0, 0, sourceObject), innerException)
    {
      this.hasLineInfo = true;
    }

    public XmlSchemaException(string message, Exception innerException)
      : base(XmlSchemaException.GetMessage(message, (string) null, 0, 0, (XmlSchemaObject) null), innerException)
    {
    }

    public int LineNumber => this.lineNumber;

    public int LinePosition => this.linePosition;

    public XmlSchemaObject SourceSchemaObject => this.sourceObj;

    public string SourceUri => this.sourceUri;

    private static string GetMessage(
      string message,
      string sourceUri,
      object sender,
      XmlSchemaObject sourceObj)
    {
      return !(sender is IXmlLineInfo xmlLineInfo) ? XmlSchemaException.GetMessage(message, sourceUri, 0, 0, sourceObj) : XmlSchemaException.GetMessage(message, sourceUri, xmlLineInfo.LineNumber, xmlLineInfo.LinePosition, sourceObj);
    }

    private static string GetMessage(
      string message,
      string sourceUri,
      int lineNumber,
      int linePosition,
      XmlSchemaObject sourceObj)
    {
      string message1 = "XmlSchema error: " + message;
      if (lineNumber > 0)
        message1 += string.Format((IFormatProvider) CultureInfo.InvariantCulture, " XML {0} Line {1}, Position {2}.", sourceUri == null || !(sourceUri != string.Empty) ? (object) string.Empty : (object) ("URI: " + sourceUri + " ."), (object) lineNumber, (object) linePosition);
      return message1;
    }

    public override string Message => base.Message;

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("hasLineInfo", this.hasLineInfo);
      info.AddValue("lineNumber", this.lineNumber);
      info.AddValue("linePosition", this.linePosition);
      info.AddValue("sourceUri", (object) this.sourceUri);
      info.AddValue("sourceObj", (object) this.sourceObj);
    }
  }
}
