// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.ValidationEventArgs
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Schema
{
  public class ValidationEventArgs : EventArgs
  {
    private XmlSchemaException exception;
    private string message;
    private XmlSeverityType severity;

    private ValidationEventArgs()
    {
    }

    internal ValidationEventArgs(XmlSchemaException ex, string message, XmlSeverityType severity)
    {
      this.exception = ex;
      this.message = message;
      this.severity = severity;
    }

    public XmlSchemaException Exception => this.exception;

    public string Message => this.message;

    public XmlSeverityType Severity => this.severity;
  }
}
