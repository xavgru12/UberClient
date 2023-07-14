// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.ValidationHandler
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Schema
{
  internal class ValidationHandler
  {
    public static void RaiseValidationEvent(
      ValidationEventHandler handle,
      Exception innerException,
      string message,
      XmlSchemaObject xsobj,
      object sender,
      string sourceUri,
      XmlSeverityType severity)
    {
      ValidationEventArgs e = new ValidationEventArgs(new XmlSchemaException(message, sender, sourceUri, xsobj, innerException), message, severity);
      if (handle == null)
      {
        if (e.Severity == XmlSeverityType.Error)
          throw e.Exception;
      }
      else
        handle(sender, e);
    }
  }
}
