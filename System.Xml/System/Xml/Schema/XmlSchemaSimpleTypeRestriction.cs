// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaSimpleTypeRestriction
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Schema;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaSimpleTypeRestriction : XmlSchemaSimpleTypeContent
  {
    private const string xmlname = "restriction";
    private XmlSchemaSimpleType baseType;
    private XmlQualifiedName baseTypeName;
    private XmlSchemaObjectCollection facets;
    private string[] enumarationFacetValues;
    private string[] patternFacetValues;
    private Regex[] rexPatterns;
    private Decimal lengthFacet;
    private Decimal maxLengthFacet;
    private Decimal minLengthFacet;
    private Decimal fractionDigitsFacet;
    private Decimal totalDigitsFacet;
    private object maxInclusiveFacet;
    private object maxExclusiveFacet;
    private object minInclusiveFacet;
    private object minExclusiveFacet;
    private XmlSchemaFacet.Facet fixedFacets;
    private static NumberStyles lengthStyle = NumberStyles.Integer;
    private static readonly XmlSchemaFacet.Facet listFacets = XmlSchemaFacet.Facet.length | XmlSchemaFacet.Facet.minLength | XmlSchemaFacet.Facet.maxLength | XmlSchemaFacet.Facet.pattern | XmlSchemaFacet.Facet.enumeration | XmlSchemaFacet.Facet.whiteSpace;

    public XmlSchemaSimpleTypeRestriction()
    {
      this.baseTypeName = XmlQualifiedName.Empty;
      this.facets = new XmlSchemaObjectCollection();
    }

    [XmlAttribute("base")]
    public XmlQualifiedName BaseTypeName
    {
      get => this.baseTypeName;
      set => this.baseTypeName = value;
    }

    [XmlElement("simpleType", Type = typeof (XmlSchemaSimpleType))]
    public XmlSchemaSimpleType BaseType
    {
      get => this.baseType;
      set => this.baseType = value;
    }

    [XmlElement("length", typeof (XmlSchemaLengthFacet))]
    [XmlElement("maxLength", typeof (XmlSchemaMaxLengthFacet))]
    [XmlElement("pattern", typeof (XmlSchemaPatternFacet))]
    [XmlElement("whiteSpace", typeof (XmlSchemaWhiteSpaceFacet))]
    [XmlElement("minExclusive", typeof (XmlSchemaMinExclusiveFacet))]
    [XmlElement("minInclusive", typeof (XmlSchemaMinInclusiveFacet))]
    [XmlElement("maxExclusive", typeof (XmlSchemaMaxExclusiveFacet))]
    [XmlElement("enumeration", typeof (XmlSchemaEnumerationFacet))]
    [XmlElement("maxInclusive", typeof (XmlSchemaMaxInclusiveFacet))]
    [XmlElement("totalDigits", typeof (XmlSchemaTotalDigitsFacet))]
    [XmlElement("fractionDigits", typeof (XmlSchemaFractionDigitsFacet))]
    [XmlElement("minLength", typeof (XmlSchemaMinLengthFacet))]
    public XmlSchemaObjectCollection Facets => this.facets;

    internal override void SetParent(XmlSchemaObject parent)
    {
      base.SetParent(parent);
      if (this.BaseType != null)
        this.BaseType.SetParent((XmlSchemaObject) this);
      foreach (XmlSchemaObject facet in this.Facets)
        facet.SetParent((XmlSchemaObject) this);
    }

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CompilationId == schema.CompilationId)
        return 0;
      this.errorCount = 0;
      if (this.baseType != null && !this.BaseTypeName.IsEmpty)
        this.error(h, "both base and simpletype can't be set");
      if (this.baseType == null && this.BaseTypeName.IsEmpty)
        this.error(h, "one of basetype or simpletype must be present");
      if (this.baseType != null)
        this.errorCount += this.baseType.Compile(h, schema);
      if (!XmlSchemaUtil.CheckQName(this.BaseTypeName))
        this.error(h, "BaseTypeName must be a XmlQualifiedName");
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      for (int index = 0; index < this.Facets.Count; ++index)
      {
        if (!(this.Facets[index] is XmlSchemaFacet))
          this.error(h, "Only XmlSchemaFacet objects are allowed for Facets property");
      }
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    private bool IsAllowedFacet(XmlSchemaFacet xsf)
    {
      if (this.ActualBaseSchemaType is XsdAnySimpleType actualBaseSchemaType)
        return actualBaseSchemaType.AllowsFacet(xsf);
      switch (((XmlSchemaSimpleType) this.ActualBaseSchemaType).Content)
      {
        case XmlSchemaSimpleTypeRestriction simpleTypeRestriction when simpleTypeRestriction != this:
          return simpleTypeRestriction.IsAllowedFacet(xsf);
        case XmlSchemaSimpleTypeList _:
          return (xsf.ThisFacet & XmlSchemaSimpleTypeRestriction.listFacets) != XmlSchemaFacet.Facet.None;
        case XmlSchemaSimpleTypeUnion _:
          return xsf is XmlSchemaPatternFacet || xsf is XmlSchemaEnumerationFacet;
        default:
          return false;
      }
    }

    internal override int Validate(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.IsValidated(schema.ValidationId))
        return this.errorCount;
      this.ValidateActualType(h, schema);
      this.lengthFacet = this.maxLengthFacet = this.minLengthFacet = this.fractionDigitsFacet = this.totalDigitsFacet = -1M;
      XmlSchemaSimpleTypeRestriction simpleTypeRestriction = (XmlSchemaSimpleTypeRestriction) null;
      if (this.ActualBaseSchemaType is XmlSchemaSimpleType)
        simpleTypeRestriction = ((XmlSchemaSimpleType) this.ActualBaseSchemaType).Content as XmlSchemaSimpleTypeRestriction;
      if (simpleTypeRestriction != null)
      {
        this.fixedFacets = simpleTypeRestriction.fixedFacets;
        this.lengthFacet = simpleTypeRestriction.lengthFacet;
        this.maxLengthFacet = simpleTypeRestriction.maxLengthFacet;
        this.minLengthFacet = simpleTypeRestriction.minLengthFacet;
        this.fractionDigitsFacet = simpleTypeRestriction.fractionDigitsFacet;
        this.totalDigitsFacet = simpleTypeRestriction.totalDigitsFacet;
        this.maxInclusiveFacet = simpleTypeRestriction.maxInclusiveFacet;
        this.maxExclusiveFacet = simpleTypeRestriction.maxExclusiveFacet;
        this.minInclusiveFacet = simpleTypeRestriction.minInclusiveFacet;
        this.minExclusiveFacet = simpleTypeRestriction.minExclusiveFacet;
      }
      this.enumarationFacetValues = this.patternFacetValues = (string[]) null;
      this.rexPatterns = (Regex[]) null;
      XmlSchemaFacet.Facet facetsDefined = XmlSchemaFacet.Facet.None;
      ArrayList arrayList1 = (ArrayList) null;
      ArrayList arrayList2 = (ArrayList) null;
      for (int index = 0; index < this.facets.Count; ++index)
      {
        if (this.facets[index] is XmlSchemaFacet facet3)
        {
          if (!this.IsAllowedFacet(facet3))
            facet3.error(h, facet3.ThisFacet.ToString() + " is not a valid facet for this type");
          else if (this.facets[index] is XmlSchemaEnumerationFacet facet2)
          {
            if (arrayList1 == null)
              arrayList1 = new ArrayList();
            arrayList1.Add((object) facet2.Value);
          }
          else if (this.facets[index] is XmlSchemaPatternFacet facet1)
          {
            if (arrayList2 == null)
              arrayList2 = new ArrayList();
            arrayList2.Add((object) facet1.Value);
          }
          else if ((facetsDefined & facet3.ThisFacet) != XmlSchemaFacet.Facet.None)
          {
            facet3.error(h, "This is a duplicate '" + (object) facet3.ThisFacet + "' facet.");
          }
          else
          {
            facetsDefined |= facet3.ThisFacet;
            switch (facet3)
            {
              case XmlSchemaLengthFacet _:
                this.checkLengthFacet((XmlSchemaLengthFacet) facet3, facetsDefined, h);
                break;
              case XmlSchemaMaxLengthFacet _:
                this.checkMaxLengthFacet((XmlSchemaMaxLengthFacet) facet3, facetsDefined, h);
                break;
              case XmlSchemaMinLengthFacet _:
                this.checkMinLengthFacet((XmlSchemaMinLengthFacet) facet3, facetsDefined, h);
                break;
              case XmlSchemaMinInclusiveFacet _:
                this.checkMinMaxFacet(facet3, ref this.minInclusiveFacet, h);
                break;
              case XmlSchemaMaxInclusiveFacet _:
                this.checkMinMaxFacet(facet3, ref this.maxInclusiveFacet, h);
                break;
              case XmlSchemaMinExclusiveFacet _:
                this.checkMinMaxFacet(facet3, ref this.minExclusiveFacet, h);
                break;
              case XmlSchemaMaxExclusiveFacet _:
                this.checkMinMaxFacet(facet3, ref this.maxExclusiveFacet, h);
                break;
              case XmlSchemaFractionDigitsFacet _:
                this.checkFractionDigitsFacet((XmlSchemaFractionDigitsFacet) facet3, h);
                break;
              case XmlSchemaTotalDigitsFacet _:
                this.checkTotalDigitsFacet((XmlSchemaTotalDigitsFacet) facet3, h);
                break;
            }
            if (facet3.IsFixed)
              this.fixedFacets |= facet3.ThisFacet;
          }
        }
      }
      if (arrayList1 != null)
        this.enumarationFacetValues = arrayList1.ToArray(typeof (string)) as string[];
      if (arrayList2 != null)
      {
        this.patternFacetValues = arrayList2.ToArray(typeof (string)) as string[];
        this.rexPatterns = new Regex[arrayList2.Count];
        for (int index1 = 0; index1 < this.patternFacetValues.Length; ++index1)
        {
          try
          {
            string patternFacetValue = this.patternFacetValues[index1];
            StringBuilder stringBuilder = (StringBuilder) null;
            int startIndex = 0;
            for (int index2 = 0; index2 < patternFacetValue.Length; ++index2)
            {
              if (patternFacetValue[index2] == '\\' && patternFacetValue.Length > index1 + 1)
              {
                string str = (string) null;
                switch (patternFacetValue[index2 + 1])
                {
                  case 'C':
                    str = "[^\\p{L}\\p{N}_\\.\\-:]";
                    break;
                  case 'I':
                    str = "[^\\p{L}_]";
                    break;
                  case 'c':
                    str = "[\\p{L}\\p{N}_\\.\\-:]";
                    break;
                  case 'i':
                    str = "[\\p{L}_]";
                    break;
                }
                if (str != null)
                {
                  if (stringBuilder == null)
                    stringBuilder = new StringBuilder();
                  stringBuilder.Append(patternFacetValue, startIndex, index2 - startIndex);
                  stringBuilder.Append(str);
                  startIndex = index2 + 2;
                }
              }
            }
            if (stringBuilder != null)
            {
              stringBuilder.Append(patternFacetValue, startIndex, patternFacetValue.Length - startIndex);
              patternFacetValue = stringBuilder.ToString();
            }
            Regex regex = new Regex("^" + patternFacetValue + "$");
            this.rexPatterns[index1] = regex;
          }
          catch (Exception ex)
          {
            XmlSchemaObject.error(h, "Invalid regular expression pattern was specified.", ex);
          }
        }
      }
      this.ValidationId = schema.ValidationId;
      return this.errorCount;
    }

    internal void ValidateActualType(ValidationEventHandler h, XmlSchema schema) => this.GetActualType(h, schema, true);

    internal object GetActualType(ValidationEventHandler h, XmlSchema schema, bool validate)
    {
      object actualType = (object) null;
      XmlSchemaSimpleType schemaSimpleType = this.baseType ?? schema.FindSchemaType(this.baseTypeName) as XmlSchemaSimpleType;
      if (schemaSimpleType != null)
      {
        if (validate)
          this.errorCount += schemaSimpleType.Validate(h, schema);
        actualType = (object) schemaSimpleType;
      }
      else if (this.baseTypeName == XmlSchemaComplexType.AnyTypeName)
        actualType = (object) XmlSchemaSimpleType.AnySimpleType;
      else if (this.baseTypeName.Namespace == "http://www.w3.org/2001/XMLSchema" || this.baseTypeName.Namespace == "http://www.w3.org/2003/11/xpath-datatypes")
      {
        actualType = (object) XmlSchemaDatatype.FromName(this.baseTypeName);
        if (actualType == null && validate)
          this.error(h, "Invalid schema type name was specified: " + (object) this.baseTypeName);
      }
      else if (!schema.IsNamespaceAbsent(this.baseTypeName.Namespace) && validate)
        this.error(h, "Referenced base schema type " + (object) this.baseTypeName + " was not found in the corresponding schema.");
      return actualType;
    }

    private void checkTotalDigitsFacet(XmlSchemaTotalDigitsFacet totf, ValidationEventHandler h)
    {
      if (totf == null)
        return;
      try
      {
        Decimal num = Decimal.Parse(totf.Value.Trim(), XmlSchemaSimpleTypeRestriction.lengthStyle, (IFormatProvider) CultureInfo.InvariantCulture);
        if (num <= 0M)
          totf.error(h, string.Format((IFormatProvider) CultureInfo.InvariantCulture, "The value '{0}' is an invalid totalDigits value", (object) num));
        if (this.totalDigitsFacet > 0M && num > this.totalDigitsFacet)
          totf.error(h, string.Format((IFormatProvider) CultureInfo.InvariantCulture, "The value '{0}' is not a valid restriction of the base totalDigits facet '{1}'", (object) num, (object) this.totalDigitsFacet));
        this.totalDigitsFacet = num;
      }
      catch (FormatException ex)
      {
        totf.error(h, string.Format("The value '{0}' is an invalid totalDigits facet specification", (object) totf.Value.Trim()));
      }
    }

    private void checkFractionDigitsFacet(
      XmlSchemaFractionDigitsFacet fracf,
      ValidationEventHandler h)
    {
      if (fracf == null)
        return;
      try
      {
        Decimal num = Decimal.Parse(fracf.Value.Trim(), XmlSchemaSimpleTypeRestriction.lengthStyle, (IFormatProvider) CultureInfo.InvariantCulture);
        if (num < 0M)
          fracf.error(h, string.Format((IFormatProvider) CultureInfo.InvariantCulture, "The value '{0}' is an invalid fractionDigits value", (object) num));
        if (this.fractionDigitsFacet >= 0M && num > this.fractionDigitsFacet)
          fracf.error(h, string.Format((IFormatProvider) CultureInfo.InvariantCulture, "The value '{0}' is not a valid restriction of the base fractionDigits facet '{1}'", (object) num, (object) this.fractionDigitsFacet));
        this.fractionDigitsFacet = num;
      }
      catch (FormatException ex)
      {
        fracf.error(h, string.Format("The value '{0}' is an invalid fractionDigits facet specification", (object) fracf.Value.Trim()));
      }
    }

    private void checkMinMaxFacet(
      XmlSchemaFacet facet,
      ref object baseFacet,
      ValidationEventHandler h)
    {
      object x = this.ValidateValueWithDatatype(facet.Value);
      if (x != null)
      {
        if ((this.fixedFacets & facet.ThisFacet) != XmlSchemaFacet.Facet.None && baseFacet != null && this.getDatatype().Compare(x, baseFacet) != XsdOrdering.Equal)
          facet.error(h, string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} is not the same as fixed parent {1} facet.", (object) facet.Value, (object) facet.ThisFacet));
        baseFacet = x;
      }
      else
        facet.error(h, string.Format("The value '{0}' is not valid against the base type.", (object) facet.Value));
    }

    private void checkLengthFacet(
      XmlSchemaLengthFacet lf,
      XmlSchemaFacet.Facet facetsDefined,
      ValidationEventHandler h)
    {
      if (lf == null)
        return;
      try
      {
        if ((facetsDefined & (XmlSchemaFacet.Facet.minLength | XmlSchemaFacet.Facet.maxLength)) != XmlSchemaFacet.Facet.None)
        {
          lf.error(h, "It is an error for both length and minLength or maxLength to be present.");
        }
        else
        {
          this.lengthFacet = Decimal.Parse(lf.Value.Trim(), XmlSchemaSimpleTypeRestriction.lengthStyle, (IFormatProvider) CultureInfo.InvariantCulture);
          if (!(this.lengthFacet < 0M))
            return;
          lf.error(h, "The value '" + (object) this.lengthFacet + "' is an invalid length");
        }
      }
      catch (FormatException ex)
      {
        lf.error(h, "The value '" + lf.Value + "' is an invalid length facet specification");
      }
    }

    private void checkMaxLengthFacet(
      XmlSchemaMaxLengthFacet maxlf,
      XmlSchemaFacet.Facet facetsDefined,
      ValidationEventHandler h)
    {
      if (maxlf == null)
        return;
      try
      {
        if ((facetsDefined & XmlSchemaFacet.Facet.length) != XmlSchemaFacet.Facet.None)
        {
          maxlf.error(h, "It is an error for both length and minLength or maxLength to be present.");
        }
        else
        {
          Decimal num = Decimal.Parse(maxlf.Value.Trim(), XmlSchemaSimpleTypeRestriction.lengthStyle, (IFormatProvider) CultureInfo.InvariantCulture);
          if ((this.fixedFacets & XmlSchemaFacet.Facet.maxLength) != XmlSchemaFacet.Facet.None && num != this.maxLengthFacet)
            maxlf.error(h, string.Format((IFormatProvider) CultureInfo.InvariantCulture, "The value '{0}' is not the same as the fixed value '{1}' on the base type", (object) maxlf.Value.Trim(), (object) this.maxLengthFacet));
          if (this.maxLengthFacet > 0M && num > this.maxLengthFacet)
            maxlf.error(h, string.Format((IFormatProvider) CultureInfo.InvariantCulture, "The value '{0}' is not a valid restriction of the value '{1}' on the base maxLength facet", (object) maxlf.Value.Trim(), (object) this.maxLengthFacet));
          else
            this.maxLengthFacet = num;
          if (this.maxLengthFacet < 0M)
            maxlf.error(h, "The value '" + (object) this.maxLengthFacet + "' is an invalid maxLength");
          if (!(this.minLengthFacet >= 0M) || !(this.minLengthFacet > this.maxLengthFacet))
            return;
          maxlf.error(h, "minLength is greater than maxLength.");
        }
      }
      catch (FormatException ex)
      {
        maxlf.error(h, "The value '" + maxlf.Value + "' is an invalid maxLength facet specification");
      }
    }

    private void checkMinLengthFacet(
      XmlSchemaMinLengthFacet minlf,
      XmlSchemaFacet.Facet facetsDefined,
      ValidationEventHandler h)
    {
      if (minlf == null)
        return;
      try
      {
        if (this.lengthFacet >= 0M)
        {
          minlf.error(h, "It is an error for both length and minLength or maxLength to be present.");
        }
        else
        {
          Decimal num = Decimal.Parse(minlf.Value.Trim(), XmlSchemaSimpleTypeRestriction.lengthStyle, (IFormatProvider) CultureInfo.InvariantCulture);
          if ((this.fixedFacets & XmlSchemaFacet.Facet.minLength) != XmlSchemaFacet.Facet.None && num != this.minLengthFacet)
            minlf.error(h, string.Format((IFormatProvider) CultureInfo.InvariantCulture, "The value '{0}' is not the same as the fixed value '{1}' on the base type", (object) minlf.Value.Trim(), (object) this.minLengthFacet));
          if (num < this.minLengthFacet)
            minlf.error(h, string.Format((IFormatProvider) CultureInfo.InvariantCulture, "The value '{0}' is not a valid restriction of the value '{1}' on the base minLength facet", (object) minlf.Value.Trim(), (object) this.minLengthFacet));
          else
            this.minLengthFacet = num;
          if (this.minLengthFacet < 0M)
            minlf.error(h, "The value '" + (object) this.minLengthFacet + "' is an invalid minLength");
          if (!(this.maxLengthFacet >= 0M) || !(this.minLengthFacet > this.maxLengthFacet))
            return;
          minlf.error(h, "minLength is greater than maxLength.");
        }
      }
      catch (FormatException ex)
      {
        minlf.error(h, "The value '" + minlf.Value + "' is an invalid minLength facet specification");
      }
    }

    private XsdAnySimpleType getDatatype()
    {
      if (this.ActualBaseSchemaType is XsdAnySimpleType actualBaseSchemaType)
        return actualBaseSchemaType;
      XmlSchemaSimpleTypeContent content = ((XmlSchemaSimpleType) this.ActualBaseSchemaType).Content;
      switch (content)
      {
        case XmlSchemaSimpleTypeRestriction _:
          return ((XmlSchemaSimpleTypeRestriction) content).getDatatype();
        case XmlSchemaSimpleTypeList _:
        case XmlSchemaSimpleTypeUnion _:
          return (XsdAnySimpleType) null;
        default:
          return (XsdAnySimpleType) null;
      }
    }

    private object ValidateValueWithDatatype(string value)
    {
      XsdAnySimpleType datatype = this.getDatatype();
      object obj = (object) null;
      if (datatype != null)
      {
        try
        {
          obj = datatype.ParseValue(value, (XmlNameTable) null, (IXmlNamespaceResolver) null);
          if (this.ActualBaseSchemaType is XmlSchemaSimpleType)
          {
            XmlSchemaSimpleTypeContent content = ((XmlSchemaSimpleType) this.ActualBaseSchemaType).Content;
            if (content is XmlSchemaSimpleTypeRestriction)
              return ((XmlSchemaSimpleTypeRestriction) content).ValidateValueWithFacets(value, (XmlNameTable) null, (IXmlNamespaceResolver) null) ? obj : (object) null;
          }
        }
        catch (Exception ex)
        {
          return (object) null;
        }
      }
      return obj;
    }

    internal bool ValidateValueWithFacets(
      string value,
      XmlNameTable nt,
      IXmlNamespaceResolver nsmgr)
    {
      return (!(this.ActualBaseSchemaType is XmlSchemaSimpleType actualBaseSchemaType) ? (XmlSchemaSimpleTypeList) null : actualBaseSchemaType.Content as XmlSchemaSimpleTypeList) != null ? this.ValidateListValueWithFacets(value, nt, nsmgr) : this.ValidateNonListValueWithFacets(value, nt, nsmgr);
    }

    private bool ValidateListValueWithFacets(
      string value,
      XmlNameTable nt,
      IXmlNamespaceResolver nsmgr)
    {
      try
      {
        return this.ValidateListValueWithFacetsCore(value, nt, nsmgr);
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    private bool ValidateListValueWithFacetsCore(
      string value,
      XmlNameTable nt,
      IXmlNamespaceResolver nsmgr)
    {
      string[] listValue = ((XsdAnySimpleType) XmlSchemaDatatype.FromName("anySimpleType", "http://www.w3.org/2001/XMLSchema")).ParseListValue(value, nt);
      if (this.patternFacetValues != null)
      {
        for (int index1 = 0; index1 < listValue.Length; ++index1)
        {
          for (int index2 = 0; index2 < this.patternFacetValues.Length; ++index2)
          {
            if (this.rexPatterns[index2] != null && !this.rexPatterns[index2].IsMatch(listValue[index1]))
              return false;
          }
        }
      }
      bool flag = false;
      if (this.enumarationFacetValues != null)
      {
        for (int index3 = 0; index3 < listValue.Length; ++index3)
        {
          for (int index4 = 0; index4 < this.enumarationFacetValues.Length; ++index4)
          {
            if (listValue[index3] == this.enumarationFacetValues[index4])
            {
              flag = true;
              break;
            }
          }
        }
      }
      if (!flag && this.enumarationFacetValues != null)
      {
        for (int index5 = 0; index5 < listValue.Length; ++index5)
        {
          XsdAnySimpleType xsdAnySimpleType = this.getDatatype() ?? (XsdAnySimpleType) XmlSchemaDatatype.FromName("anySimpleType", "http://www.w3.org/2001/XMLSchema");
          object v1 = xsdAnySimpleType.ParseValue(listValue[index5], nt, nsmgr);
          for (int index6 = 0; index6 < this.enumarationFacetValues.Length; ++index6)
          {
            if (XmlSchemaUtil.AreSchemaDatatypeEqual(xsdAnySimpleType, v1, xsdAnySimpleType, xsdAnySimpleType.ParseValue(this.enumarationFacetValues[index6], nt, nsmgr)))
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            return false;
        }
      }
      return (!(this.lengthFacet >= 0M) || !((Decimal) listValue.Length != this.lengthFacet)) && (!(this.maxLengthFacet >= 0M) || !((Decimal) listValue.Length > this.maxLengthFacet)) && (!(this.minLengthFacet >= 0M) || !((Decimal) listValue.Length < this.minLengthFacet));
    }

    private bool ValidateNonListValueWithFacets(
      string value,
      XmlNameTable nt,
      IXmlNamespaceResolver nsmgr)
    {
      try
      {
        return this.ValidateNonListValueWithFacetsCore(value, nt, nsmgr);
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    private bool ValidateNonListValueWithFacetsCore(
      string value,
      XmlNameTable nt,
      IXmlNamespaceResolver nsmgr)
    {
      if (this.patternFacetValues != null)
      {
        bool flag = false;
        for (int index = 0; index < this.patternFacetValues.Length; ++index)
        {
          if (this.rexPatterns[index] != null && this.rexPatterns[index].IsMatch(value))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return false;
      }
      XsdAnySimpleType datatype = this.getDatatype();
      bool flag1 = false;
      if (this.enumarationFacetValues != null)
      {
        for (int index = 0; index < this.enumarationFacetValues.Length; ++index)
        {
          if (value == this.enumarationFacetValues[index])
          {
            flag1 = true;
            break;
          }
        }
      }
      if (!flag1 && this.enumarationFacetValues != null)
      {
        XsdAnySimpleType xsdAnySimpleType = datatype ?? (XsdAnySimpleType) XmlSchemaDatatype.FromName("anySimpleType", "http://www.w3.org/2001/XMLSchema");
        object v1 = xsdAnySimpleType.ParseValue(value, nt, nsmgr);
        for (int index = 0; index < this.enumarationFacetValues.Length; ++index)
        {
          if (XmlSchemaUtil.AreSchemaDatatypeEqual(xsdAnySimpleType, v1, xsdAnySimpleType, xsdAnySimpleType.ParseValue(this.enumarationFacetValues[index], nt, nsmgr)))
          {
            flag1 = true;
            break;
          }
        }
        if (!flag1)
          return false;
      }
      if (!(datatype is XsdQName) && !(datatype is XsdNotation) && (!(this.lengthFacet == -1M) || !(this.maxLengthFacet == -1M) || !(this.minLengthFacet == -1M)))
      {
        int num = datatype.Length(value);
        if (this.lengthFacet >= 0M && (Decimal) num != this.lengthFacet || this.maxLengthFacet >= 0M && (Decimal) num > this.maxLengthFacet || this.minLengthFacet >= 0M && (Decimal) num < this.minLengthFacet)
          return false;
      }
      if (this.totalDigitsFacet >= 0M || this.fractionDigitsFacet >= 0M)
      {
        string str = value.Trim('+', '-', '0', '.');
        int num1 = 0;
        int length = str.Length;
        int num2 = str.IndexOf(".");
        if (num2 != -1)
        {
          --length;
          num1 = str.Length - num2 - 1;
        }
        if (this.totalDigitsFacet >= 0M && (Decimal) length > this.totalDigitsFacet || this.fractionDigitsFacet >= 0M && (Decimal) num1 > this.fractionDigitsFacet)
          return false;
      }
      if (this.maxInclusiveFacet != null || this.maxExclusiveFacet != null || this.minInclusiveFacet != null || this.minExclusiveFacet != null)
      {
        if (datatype != null)
        {
          object x;
          try
          {
            x = datatype.ParseValue(value, nt, (IXmlNamespaceResolver) null);
          }
          catch (OverflowException ex)
          {
            return false;
          }
          catch (FormatException ex)
          {
            return false;
          }
          if (this.maxInclusiveFacet != null)
          {
            switch (datatype.Compare(x, this.maxInclusiveFacet))
            {
              case XsdOrdering.LessThan:
              case XsdOrdering.Equal:
                break;
              default:
                return false;
            }
          }
          if (this.maxExclusiveFacet != null && datatype.Compare(x, this.maxExclusiveFacet) != XsdOrdering.LessThan)
            return false;
          if (this.minInclusiveFacet != null)
          {
            switch (datatype.Compare(x, this.minInclusiveFacet))
            {
              case XsdOrdering.Equal:
              case XsdOrdering.GreaterThan:
                break;
              default:
                return false;
            }
          }
          if (this.minExclusiveFacet != null && datatype.Compare(x, this.minExclusiveFacet) != XsdOrdering.GreaterThan)
            return false;
        }
      }
      return true;
    }

    internal static XmlSchemaSimpleTypeRestriction Read(
      XmlSchemaReader reader,
      ValidationEventHandler h)
    {
      XmlSchemaSimpleTypeRestriction xso = new XmlSchemaSimpleTypeRestriction();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "restriction")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaSimpleTypeRestriction.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaSimpleTypeRestriction) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "base")
        {
          Exception innerEx;
          xso.baseTypeName = XmlSchemaUtil.ReadQNameAttribute((XmlReader) reader, out innerEx);
          if (innerEx != null)
            XmlSchemaObject.error(h, reader.Value + " is not a valid value for base attribute", innerEx);
        }
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for restriction", (Exception) null);
        else
          XmlSchemaUtil.ReadUnhandledAttribute((XmlReader) reader, (XmlSchemaObject) xso);
      }
      reader.MoveToElement();
      if (reader.IsEmptyElement)
        return xso;
      int num = 1;
      while (reader.ReadNextElement())
      {
        if (reader.NodeType == XmlNodeType.EndElement)
        {
          if (reader.LocalName != "restriction")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaSimpleTypeRestriction.Read, name=" + reader.Name, (Exception) null);
            break;
          }
          break;
        }
        if (num <= 1 && reader.LocalName == "annotation")
        {
          num = 2;
          XmlSchemaAnnotation schemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
          if (schemaAnnotation != null)
            xso.Annotation = schemaAnnotation;
        }
        else if (num <= 2 && reader.LocalName == "simpleType")
        {
          num = 3;
          XmlSchemaSimpleType schemaSimpleType = XmlSchemaSimpleType.Read(reader, h);
          if (schemaSimpleType != null)
            xso.baseType = schemaSimpleType;
        }
        else
        {
          if (num <= 3)
          {
            if (reader.LocalName == "minExclusive")
            {
              num = 3;
              XmlSchemaMinExclusiveFacet minExclusiveFacet = XmlSchemaMinExclusiveFacet.Read(reader, h);
              if (minExclusiveFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) minExclusiveFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "minInclusive")
            {
              num = 3;
              XmlSchemaMinInclusiveFacet minInclusiveFacet = XmlSchemaMinInclusiveFacet.Read(reader, h);
              if (minInclusiveFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) minInclusiveFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "maxExclusive")
            {
              num = 3;
              XmlSchemaMaxExclusiveFacet maxExclusiveFacet = XmlSchemaMaxExclusiveFacet.Read(reader, h);
              if (maxExclusiveFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) maxExclusiveFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "maxInclusive")
            {
              num = 3;
              XmlSchemaMaxInclusiveFacet maxInclusiveFacet = XmlSchemaMaxInclusiveFacet.Read(reader, h);
              if (maxInclusiveFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) maxInclusiveFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "totalDigits")
            {
              num = 3;
              XmlSchemaTotalDigitsFacet totalDigitsFacet = XmlSchemaTotalDigitsFacet.Read(reader, h);
              if (totalDigitsFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) totalDigitsFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "fractionDigits")
            {
              num = 3;
              XmlSchemaFractionDigitsFacet fractionDigitsFacet = XmlSchemaFractionDigitsFacet.Read(reader, h);
              if (fractionDigitsFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) fractionDigitsFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "length")
            {
              num = 3;
              XmlSchemaLengthFacet schemaLengthFacet = XmlSchemaLengthFacet.Read(reader, h);
              if (schemaLengthFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) schemaLengthFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "minLength")
            {
              num = 3;
              XmlSchemaMinLengthFacet schemaMinLengthFacet = XmlSchemaMinLengthFacet.Read(reader, h);
              if (schemaMinLengthFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) schemaMinLengthFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "maxLength")
            {
              num = 3;
              XmlSchemaMaxLengthFacet schemaMaxLengthFacet = XmlSchemaMaxLengthFacet.Read(reader, h);
              if (schemaMaxLengthFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) schemaMaxLengthFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "enumeration")
            {
              num = 3;
              XmlSchemaEnumerationFacet enumerationFacet = XmlSchemaEnumerationFacet.Read(reader, h);
              if (enumerationFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) enumerationFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "whiteSpace")
            {
              num = 3;
              XmlSchemaWhiteSpaceFacet schemaWhiteSpaceFacet = XmlSchemaWhiteSpaceFacet.Read(reader, h);
              if (schemaWhiteSpaceFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) schemaWhiteSpaceFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "pattern")
            {
              num = 3;
              XmlSchemaPatternFacet schemaPatternFacet = XmlSchemaPatternFacet.Read(reader, h);
              if (schemaPatternFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) schemaPatternFacet);
                continue;
              }
              continue;
            }
          }
          reader.RaiseInvalidElementError();
        }
      }
      return xso;
    }
  }
}
