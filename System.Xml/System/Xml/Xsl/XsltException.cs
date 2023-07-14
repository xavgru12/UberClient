// Decompiled with JetBrains decompiler
// Type: System.Xml.Xsl.XsltException
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.XPath;

namespace System.Xml.Xsl
{
  [Serializable]
  public class XsltException : SystemException
  {
    private int lineNumber;
    private int linePosition;
    private string sourceUri;
    private string templateFrames;

    public XsltException()
      : this(string.Empty, (Exception) null)
    {
    }

    public XsltException(string message)
      : this(message, (Exception) null)
    {
    }

    public XsltException(string message, Exception innerException)
      : this("{0}", message, innerException, 0, 0, (string) null)
    {
    }

    protected XsltException(SerializationInfo info, StreamingContext context)
    {
      this.lineNumber = info.GetInt32(nameof (lineNumber));
      this.linePosition = info.GetInt32(nameof (linePosition));
      this.sourceUri = info.GetString(nameof (sourceUri));
      this.templateFrames = info.GetString(nameof (templateFrames));
    }

    internal XsltException(
      string msgFormat,
      string message,
      Exception innerException,
      int lineNumber,
      int linePosition,
      string sourceUri)
      : base(XsltException.CreateMessage(msgFormat, message, lineNumber, linePosition, sourceUri), innerException)
    {
      this.lineNumber = lineNumber;
      this.linePosition = linePosition;
      this.sourceUri = sourceUri;
    }

    internal XsltException(string message, Exception innerException, XPathNavigator nav)
      : base(XsltException.CreateMessage(message, nav), innerException)
    {
      this.lineNumber = !(nav is IXmlLineInfo xmlLineInfo) ? 0 : xmlLineInfo.LineNumber;
      this.linePosition = xmlLineInfo == null ? 0 : xmlLineInfo.LinePosition;
      this.sourceUri = nav == null ? string.Empty : nav.BaseURI;
    }

    private static string CreateMessage(string message, XPathNavigator nav)
    {
      int lineNumber = !(nav is IXmlLineInfo xmlLineInfo) ? 0 : xmlLineInfo.LineNumber;
      int linePosition = xmlLineInfo == null ? 0 : xmlLineInfo.LinePosition;
      string sourceUri = nav == null ? string.Empty : nav.BaseURI;
      return lineNumber != 0 ? XsltException.CreateMessage("{0} at {1}({2},{3}).", message, lineNumber, linePosition, sourceUri) : XsltException.CreateMessage("{0}.", message, lineNumber, linePosition, sourceUri);
    }

    private static string CreateMessage(
      string msgFormat,
      string message,
      int lineNumber,
      int linePosition,
      string sourceUri)
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, msgFormat, (object) message, (object) sourceUri, (object) lineNumber.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) linePosition.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public virtual int LineNumber => this.lineNumber;

    public virtual int LinePosition => this.linePosition;

    public override string Message => this.templateFrames != null ? base.Message + this.templateFrames : base.Message;

    public virtual string SourceUri => this.sourceUri;

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("lineNumber", this.lineNumber);
      info.AddValue("linePosition", this.linePosition);
      info.AddValue("sourceUri", (object) this.sourceUri);
      info.AddValue("templateFrames", (object) this.templateFrames);
    }

    internal void AddTemplateFrame(string frame) => this.templateFrames += frame;
  }
}
