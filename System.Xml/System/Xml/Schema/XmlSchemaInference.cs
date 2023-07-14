// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaInference
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Schema
{
  [MonoTODO]
  public sealed class XmlSchemaInference
  {
    private XmlSchemaInference.InferenceOption occurrence;
    private XmlSchemaInference.InferenceOption typeInference;

    public XmlSchemaInference.InferenceOption Occurrence
    {
      get => this.occurrence;
      set => this.occurrence = value;
    }

    public XmlSchemaInference.InferenceOption TypeInference
    {
      get => this.typeInference;
      set => this.typeInference = value;
    }

    public XmlSchemaSet InferSchema(XmlReader xmlReader) => this.InferSchema(xmlReader, new XmlSchemaSet());

    public XmlSchemaSet InferSchema(XmlReader xmlReader, XmlSchemaSet schemas) => XsdInference.Process(xmlReader, schemas, this.occurrence == XmlSchemaInference.InferenceOption.Relaxed, this.typeInference == XmlSchemaInference.InferenceOption.Relaxed);

    public enum InferenceOption
    {
      Restricted,
      Relaxed,
    }
  }
}
