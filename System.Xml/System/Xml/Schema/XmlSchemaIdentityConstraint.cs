// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaIdentityConstraint
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Schema;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaIdentityConstraint : XmlSchemaAnnotated
  {
    private XmlSchemaObjectCollection fields;
    private string name;
    private XmlQualifiedName qName;
    private XmlSchemaXPath selector;
    private XsdIdentitySelector compiledSelector;

    public XmlSchemaIdentityConstraint()
    {
      this.fields = new XmlSchemaObjectCollection();
      this.qName = XmlQualifiedName.Empty;
    }

    [XmlAttribute("name")]
    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    [XmlElement("selector", typeof (XmlSchemaXPath))]
    public XmlSchemaXPath Selector
    {
      get => this.selector;
      set => this.selector = value;
    }

    [XmlElement("field", typeof (XmlSchemaXPath))]
    public XmlSchemaObjectCollection Fields => this.fields;

    [XmlIgnore]
    public XmlQualifiedName QualifiedName => this.qName;

    internal XsdIdentitySelector CompiledSelector => this.compiledSelector;

    internal override void SetParent(XmlSchemaObject parent)
    {
      base.SetParent(parent);
      if (this.Selector != null)
        this.Selector.SetParent((XmlSchemaObject) this);
      foreach (XmlSchemaObject field in this.Fields)
        field.SetParent((XmlSchemaObject) this);
    }

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CompilationId == schema.CompilationId)
        return 0;
      if (this.Name == null)
        this.error(h, "Required attribute name must be present");
      else if (!XmlSchemaUtil.CheckNCName(this.name))
      {
        this.error(h, "attribute name must be NCName");
      }
      else
      {
        this.qName = new XmlQualifiedName(this.Name, this.AncestorSchema.TargetNamespace);
        if (schema.NamedIdentities.Contains(this.qName))
        {
          XmlSchemaIdentityConstraint namedIdentity = schema.NamedIdentities[this.qName] as XmlSchemaIdentityConstraint;
          this.error(h, string.Format("There is already same named identity constraint in this namespace. Existing item is at {0}({1},{2})", (object) namedIdentity.SourceUri, (object) namedIdentity.LineNumber, (object) namedIdentity.LinePosition));
        }
        else
          schema.NamedIdentities.Add(this.qName, (XmlSchemaObject) this);
      }
      if (this.Selector == null)
      {
        this.error(h, "selector must be present");
      }
      else
      {
        this.Selector.isSelector = true;
        this.errorCount += this.Selector.Compile(h, schema);
        if (this.selector.errorCount == 0)
          this.compiledSelector = new XsdIdentitySelector(this.Selector);
      }
      if (this.errorCount > 0)
        return this.errorCount;
      if (this.Fields.Count == 0)
      {
        this.error(h, "atleast one field value must be present");
      }
      else
      {
        for (int index = 0; index < this.Fields.Count; ++index)
        {
          if (this.Fields[index] is XmlSchemaXPath field)
          {
            this.errorCount += field.Compile(h, schema);
            if (field.errorCount == 0)
              this.compiledSelector.AddField(new XsdIdentityField(field, index));
          }
          else
            this.error(h, "Object of type " + (object) this.Fields[index].GetType() + " is invalid in the Fields Collection");
        }
      }
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }
  }
}
