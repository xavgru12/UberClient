// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdKeyEntryField
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdKeyEntryField
  {
    private XsdKeyEntry entry;
    private XsdIdentityField field;
    public bool FieldFound;
    public int FieldLineNumber;
    public int FieldLinePosition;
    public bool FieldHasLineInfo;
    public XsdAnySimpleType FieldType;
    public object Identity;
    public bool IsXsiNil;
    public int FieldFoundDepth;
    public XsdIdentityPath FieldFoundPath;
    public bool Consuming;
    public bool Consumed;

    public XsdKeyEntryField(XsdKeyEntry entry, XsdIdentityField field)
    {
      this.entry = entry;
      this.field = field;
    }

    public XsdIdentityField Field => this.field;

    public bool SetIdentityField(
      object identity,
      bool isXsiNil,
      XsdAnySimpleType type,
      int depth,
      IXmlLineInfo li)
    {
      this.FieldFoundDepth = depth;
      this.Identity = identity;
      this.IsXsiNil = isXsiNil;
      this.FieldFound |= isXsiNil;
      this.FieldType = type;
      this.Consuming = false;
      this.Consumed = true;
      if (li != null && li.HasLineInfo())
      {
        this.FieldHasLineInfo = true;
        this.FieldLineNumber = li.LineNumber;
        this.FieldLinePosition = li.LinePosition;
      }
      if (!(this.entry.OwnerSequence.SourceSchemaIdentity is XmlSchemaKeyref))
      {
        for (int i = 0; i < this.entry.OwnerSequence.FinishedEntries.Count; ++i)
        {
          if (this.entry.CompareIdentity(this.entry.OwnerSequence.FinishedEntries[i]))
            return false;
        }
      }
      return true;
    }

    internal XsdIdentityPath Matches(
      bool matchesAttr,
      object sender,
      XmlNameTable nameTable,
      ArrayList qnameStack,
      string sourceUri,
      object schemaType,
      IXmlNamespaceResolver nsResolver,
      IXmlLineInfo lineInfo,
      int depth,
      string attrName,
      string attrNS,
      object attrValue)
    {
      XsdIdentityPath xsdIdentityPath = (XsdIdentityPath) null;
      for (int index1 = 0; index1 < this.field.Paths.Length; ++index1)
      {
        XsdIdentityPath path = this.field.Paths[index1];
        bool isAttribute = path.IsAttribute;
        if (matchesAttr == isAttribute)
        {
          if (path.IsAttribute)
          {
            XsdIdentityStep orderedStep = path.OrderedSteps[path.OrderedSteps.Length - 1];
            bool flag = false;
            if (orderedStep.IsAnyName || orderedStep.NsName != null)
            {
              if (orderedStep.IsAnyName || attrNS == orderedStep.NsName)
                flag = true;
            }
            else if (orderedStep.Name == attrName && orderedStep.Namespace == attrNS)
              flag = true;
            if (flag && this.entry.StartDepth + (path.OrderedSteps.Length - 1) == depth - 1)
              xsdIdentityPath = path;
            else
              continue;
          }
          if (!this.FieldFound || depth <= this.FieldFoundDepth || this.FieldFoundPath != path)
          {
            if (path.OrderedSteps.Length == 0)
            {
              if (depth == this.entry.StartDepth)
                return path;
            }
            else if (depth - this.entry.StartDepth >= path.OrderedSteps.Length - 1)
            {
              int length = path.OrderedSteps.Length;
              if (isAttribute)
                --length;
              if ((!path.Descendants || depth >= this.entry.StartDepth + length) && (path.Descendants || depth == this.entry.StartDepth + length))
              {
                int index2;
                for (index2 = length - 1; index2 >= 0; --index2)
                {
                  XsdIdentityStep orderedStep = path.OrderedSteps[index2];
                  if (!orderedStep.IsCurrent && !orderedStep.IsAnyName)
                  {
                    XmlQualifiedName qname = (XmlQualifiedName) qnameStack[this.entry.StartDepth + index2 + (!isAttribute ? 1 : 0)];
                    if ((orderedStep.NsName == null || !(qname.Namespace == orderedStep.NsName)) && (!(orderedStep.Name == "*") && !(orderedStep.Name == qname.Name) || !(orderedStep.Namespace == qname.Namespace)))
                      break;
                  }
                }
                if (index2 < 0 && !matchesAttr)
                  return path;
              }
            }
          }
        }
      }
      if (xsdIdentityPath != null)
      {
        this.FillAttributeFieldValue(sender, nameTable, sourceUri, schemaType, nsResolver, attrValue, lineInfo, depth);
        if (this.Identity != null)
          return xsdIdentityPath;
      }
      return (XsdIdentityPath) null;
    }

    private void FillAttributeFieldValue(
      object sender,
      XmlNameTable nameTable,
      string sourceUri,
      object schemaType,
      IXmlNamespaceResolver nsResolver,
      object identity,
      IXmlLineInfo lineInfo,
      int depth)
    {
      if (this.FieldFound)
      {
        object identity1 = this.Identity;
        string str;
        if (this.FieldHasLineInfo)
          str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, " at line {0}, position {1}", (object) this.FieldLineNumber, (object) this.FieldLinePosition);
        else
          str = string.Empty;
        throw new XmlSchemaValidationException(string.Format("The key value was already found as '{0}'{1}.", identity1, (object) str), sender, sourceUri, (XmlSchemaObject) this.entry.OwnerSequence.SourceSchemaIdentity, (Exception) null);
      }
      XmlSchemaDatatype type = schemaType as XmlSchemaDatatype;
      XmlSchemaSimpleType schemaSimpleType = schemaType as XmlSchemaSimpleType;
      if (type == null)
      {
        if (schemaSimpleType != null)
          type = schemaSimpleType.Datatype;
      }
      try
      {
        if (!this.SetIdentityField(identity, false, type as XsdAnySimpleType, depth, lineInfo))
          throw new XmlSchemaValidationException("Two or more identical field was found.", sender, sourceUri, (XmlSchemaObject) this.entry.OwnerSequence.SourceSchemaIdentity, (Exception) null);
        this.Consuming = true;
        this.FieldFound = true;
      }
      catch (Exception ex)
      {
        throw new XmlSchemaValidationException("Failed to read typed value.", sender, sourceUri, (XmlSchemaObject) this.entry.OwnerSequence.SourceSchemaIdentity, ex);
      }
    }
  }
}
