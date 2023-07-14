// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlException
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Globalization;
using System.Runtime.Serialization;

namespace System.Xml
{
  [Serializable]
  public class XmlException : SystemException
  {
    private const string Xml_DefaultException = "Xml_DefaultException";
    private const string Xml_UserException = "Xml_UserException";
    private int lineNumber;
    private int linePosition;
    private string sourceUri;
    private string res;
    private string[] messages;

    public XmlException()
    {
      this.res = nameof (Xml_DefaultException);
      this.messages = new string[1];
    }

    public XmlException(string message, Exception innerException)
      : base(message, innerException)
    {
      this.res = nameof (Xml_UserException);
      this.messages = new string[1]{ message };
    }

    protected XmlException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.lineNumber = info.GetInt32(nameof (lineNumber));
      this.linePosition = info.GetInt32(nameof (linePosition));
      this.res = info.GetString(nameof (res));
      this.messages = (string[]) info.GetValue("args", typeof (string[]));
      this.sourceUri = info.GetString(nameof (sourceUri));
    }

    public XmlException(string message)
      : base(message)
    {
      this.res = nameof (Xml_UserException);
      this.messages = new string[1]{ message };
    }

    internal XmlException(IXmlLineInfo li, string sourceUri, string message)
      : this(li, (Exception) null, sourceUri, message)
    {
    }

    internal XmlException(
      IXmlLineInfo li,
      Exception innerException,
      string sourceUri,
      string message)
      : this(message, innerException)
    {
      if (li != null)
      {
        this.lineNumber = li.LineNumber;
        this.linePosition = li.LinePosition;
      }
      this.sourceUri = sourceUri;
    }

    public XmlException(
      string message,
      Exception innerException,
      int lineNumber,
      int linePosition)
      : this(message, innerException)
    {
      this.lineNumber = lineNumber;
      this.linePosition = linePosition;
    }

    internal XmlException(
      string message,
      int lineNumber,
      int linePosition,
      object sourceObject,
      string sourceUri,
      Exception innerException)
      : base(XmlException.GetMessage(message, sourceUri, lineNumber, linePosition, sourceObject), innerException)
    {
      this.lineNumber = lineNumber;
      this.linePosition = linePosition;
      this.sourceUri = sourceUri;
    }

    private static string GetMessage(
      string message,
      string sourceUri,
      int lineNumber,
      int linePosition,
      object sourceObj)
    {
      string message1 = "XmlSchema error: " + message;
      if (lineNumber > 0)
        message1 += string.Format((IFormatProvider) CultureInfo.InvariantCulture, " XML {0} Line {1}, Position {2}.", sourceUri == null || !(sourceUri != string.Empty) ? (object) string.Empty : (object) ("URI: " + sourceUri + " ."), (object) lineNumber, (object) linePosition);
      return message1;
    }

    public int LineNumber => this.lineNumber;

    public int LinePosition => this.linePosition;

    public string SourceUri => this.sourceUri;

    public override string Message
    {
      get
      {
        if (this.lineNumber == 0)
          return base.Message;
        return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} {3} Line {1}, position {2}.", (object) base.Message, (object) this.lineNumber, (object) this.linePosition, (object) this.sourceUri);
      }
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("lineNumber", this.lineNumber);
      info.AddValue("linePosition", this.linePosition);
      info.AddValue("res", (object) this.res);
      info.AddValue("args", (object) this.messages);
      info.AddValue("sourceUri", (object) this.sourceUri);
    }
  }
}
