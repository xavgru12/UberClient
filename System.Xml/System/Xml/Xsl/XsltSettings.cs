// Decompiled with JetBrains decompiler
// Type: System.Xml.Xsl.XsltSettings
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Xsl
{
  public sealed class XsltSettings
  {
    private static readonly XsltSettings defaultSettings = new XsltSettings(true);
    private static readonly XsltSettings trustedXslt = new XsltSettings(true);
    private bool readOnly;
    private bool enableDocument;
    private bool enableScript;

    public XsltSettings()
    {
    }

    public XsltSettings(bool enableDocumentFunction, bool enableScript)
    {
      this.enableDocument = enableDocumentFunction;
      this.enableScript = enableScript;
    }

    private XsltSettings(bool readOnly) => this.readOnly = readOnly;

    static XsltSettings()
    {
      XsltSettings.trustedXslt.enableDocument = true;
      XsltSettings.trustedXslt.enableScript = true;
    }

    public static XsltSettings Default => XsltSettings.defaultSettings;

    public static XsltSettings TrustedXslt => XsltSettings.trustedXslt;

    public bool EnableDocumentFunction
    {
      get => this.enableDocument;
      set
      {
        if (this.readOnly)
          return;
        this.enableDocument = value;
      }
    }

    public bool EnableScript
    {
      get => this.enableScript;
      set
      {
        if (this.readOnly)
          return;
        this.enableScript = value;
      }
    }
  }
}
