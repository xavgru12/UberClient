// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdKeyEntry
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdKeyEntry
  {
    public int StartDepth;
    public int SelectorLineNumber;
    public int SelectorLinePosition;
    public bool SelectorHasLineInfo;
    public XsdKeyEntryFieldCollection KeyFields;
    public bool KeyRefFound;
    public XsdKeyTable OwnerSequence;
    private bool keyFound;

    public XsdKeyEntry(XsdKeyTable keyseq, int depth, IXmlLineInfo li) => this.Init(keyseq, depth, li);

    public bool KeyFound
    {
      get
      {
        if (this.keyFound)
          return true;
        for (int i = 0; i < this.KeyFields.Count; ++i)
        {
          if (this.KeyFields[i].FieldFound)
          {
            this.keyFound = true;
            return true;
          }
        }
        return false;
      }
    }

    private void Init(XsdKeyTable keyseq, int depth, IXmlLineInfo li)
    {
      this.OwnerSequence = keyseq;
      this.KeyFields = new XsdKeyEntryFieldCollection();
      for (int index = 0; index < keyseq.Selector.Fields.Length; ++index)
        this.KeyFields.Add(new XsdKeyEntryField(this, keyseq.Selector.Fields[index]));
      this.StartDepth = depth;
      if (li == null || !li.HasLineInfo())
        return;
      this.SelectorHasLineInfo = true;
      this.SelectorLineNumber = li.LineNumber;
      this.SelectorLinePosition = li.LinePosition;
    }

    public bool CompareIdentity(XsdKeyEntry other)
    {
      for (int i = 0; i < this.KeyFields.Count; ++i)
      {
        XsdKeyEntryField keyField1 = this.KeyFields[i];
        XsdKeyEntryField keyField2 = other.KeyFields[i];
        if (keyField1.IsXsiNil && !keyField2.IsXsiNil || !keyField1.IsXsiNil && keyField2.IsXsiNil || !XmlSchemaUtil.AreSchemaDatatypeEqual(keyField2.FieldType, keyField2.Identity, keyField1.FieldType, keyField1.Identity))
          return false;
      }
      return true;
    }

    public void ProcessMatch(
      bool isAttribute,
      ArrayList qnameStack,
      object sender,
      XmlNameTable nameTable,
      string sourceUri,
      object schemaType,
      IXmlNamespaceResolver nsResolver,
      IXmlLineInfo li,
      int depth,
      string attrName,
      string attrNS,
      object attrValue,
      bool isXsiNil,
      ArrayList currentKeyFieldConsumers)
    {
      for (int i = 0; i < this.KeyFields.Count; ++i)
      {
        XsdKeyEntryField keyField = this.KeyFields[i];
        XsdIdentityPath xsdIdentityPath = keyField.Matches(isAttribute, sender, nameTable, qnameStack, sourceUri, schemaType, nsResolver, li, depth, attrName, attrNS, attrValue);
        if (xsdIdentityPath != null)
        {
          if (keyField.FieldFound)
            keyField.Consuming = keyField.Consuming ? false : throw new XmlSchemaValidationException("Two or more matching field was found.", sender, sourceUri, (XmlSchemaObject) this.OwnerSequence.SourceSchemaIdentity, (Exception) null);
          if (!keyField.Consumed)
          {
            if (isXsiNil && !keyField.SetIdentityField((object) Guid.Empty, true, XsdAnySimpleType.Instance, depth, li))
              throw new XmlSchemaValidationException("Two or more identical field was found.", sender, sourceUri, (XmlSchemaObject) this.OwnerSequence.SourceSchemaIdentity, (Exception) null);
            if (schemaType is XmlSchemaComplexType schemaComplexType && (schemaComplexType.ContentType == XmlSchemaContentType.Empty || schemaComplexType.ContentType == XmlSchemaContentType.ElementOnly) && schemaType != XmlSchemaComplexType.AnyType)
              throw new XmlSchemaValidationException("Specified schema type is complex type, which is not allowed for identity constraints.", sender, sourceUri, (XmlSchemaObject) this.OwnerSequence.SourceSchemaIdentity, (Exception) null);
            keyField.FieldFound = true;
            keyField.FieldFoundPath = xsdIdentityPath;
            keyField.FieldFoundDepth = depth;
            keyField.Consuming = true;
            if (li != null && li.HasLineInfo())
            {
              keyField.FieldHasLineInfo = true;
              keyField.FieldLineNumber = li.LineNumber;
              keyField.FieldLinePosition = li.LinePosition;
            }
            currentKeyFieldConsumers.Add((object) keyField);
          }
        }
      }
    }
  }
}
