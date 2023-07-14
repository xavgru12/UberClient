// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaParticle
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Globalization;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public abstract class XmlSchemaParticle : XmlSchemaAnnotated
  {
    private Decimal minOccurs;
    private Decimal maxOccurs;
    private string minstr;
    private string maxstr;
    private static XmlSchemaParticle empty;
    private Decimal validatedMinOccurs = 1M;
    private Decimal validatedMaxOccurs = 1M;
    internal int recursionDepth = -1;
    private Decimal minEffectiveTotalRange = -1M;
    internal bool parentIsGroupDefinition;
    internal XmlSchemaParticle OptimizedParticle;

    protected XmlSchemaParticle()
    {
      this.minOccurs = 1M;
      this.maxOccurs = 1M;
    }

    internal static XmlSchemaParticle Empty
    {
      get
      {
        if (XmlSchemaParticle.empty == null)
          XmlSchemaParticle.empty = (XmlSchemaParticle) new XmlSchemaParticle.EmptyParticle();
        return XmlSchemaParticle.empty;
      }
    }

    [XmlAttribute("minOccurs")]
    public string MinOccursString
    {
      get => this.minstr;
      set
      {
        if (value == null)
        {
          this.minOccurs = 1M;
          this.minstr = value;
        }
        else
        {
          Decimal d = Decimal.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
          this.minOccurs = d >= 0M && d == Decimal.Truncate(d) ? d : throw new XmlSchemaException("MinOccursString must be a non-negative number", (Exception) null);
          this.minstr = d.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }
      }
    }

    [XmlAttribute("maxOccurs")]
    public string MaxOccursString
    {
      get => this.maxstr;
      set
      {
        if (value == "unbounded")
        {
          this.maxstr = value;
          this.maxOccurs = Decimal.MaxValue;
        }
        else
        {
          Decimal d = Decimal.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
          this.maxOccurs = d >= 0M && d == Decimal.Truncate(d) ? d : throw new XmlSchemaException("MaxOccurs must be a non-negative integer", (Exception) null);
          this.maxstr = d.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          if (!(d == 0M) || this.minstr != null)
            return;
          this.minOccurs = 0M;
        }
      }
    }

    [XmlIgnore]
    public Decimal MinOccurs
    {
      get => this.minOccurs;
      set => this.MinOccursString = value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    [XmlIgnore]
    public Decimal MaxOccurs
    {
      get => this.maxOccurs;
      set
      {
        if (value == Decimal.MaxValue)
          this.MaxOccursString = "unbounded";
        else
          this.MaxOccursString = value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      }
    }

    internal Decimal ValidatedMinOccurs => this.validatedMinOccurs;

    internal Decimal ValidatedMaxOccurs => this.validatedMaxOccurs;

    internal virtual XmlSchemaParticle GetOptimizedParticle(bool isTop) => (XmlSchemaParticle) null;

    internal XmlSchemaParticle GetShallowClone() => (XmlSchemaParticle) this.MemberwiseClone();

    internal void CompileOccurence(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.MinOccurs > this.MaxOccurs && (!(this.MaxOccurs == 0M) || this.MinOccursString != null))
      {
        this.error(h, "minOccurs must be less than or equal to maxOccurs");
      }
      else
      {
        this.validatedMaxOccurs = !(this.MaxOccursString == "unbounded") ? this.maxOccurs : Decimal.MaxValue;
        if (this.validatedMaxOccurs == 0M)
          this.validatedMinOccurs = 0M;
        else
          this.validatedMinOccurs = this.minOccurs;
      }
    }

    internal override void CopyInfo(XmlSchemaParticle obj)
    {
      base.CopyInfo(obj);
      obj.maxOccurs = !(this.MaxOccursString == "unbounded") ? (obj.validatedMaxOccurs = this.ValidatedMaxOccurs) : (obj.validatedMaxOccurs = Decimal.MaxValue);
      obj.minOccurs = !(this.MaxOccurs == 0M) ? (obj.validatedMinOccurs = this.ValidatedMinOccurs) : (obj.validatedMinOccurs = 0M);
      if (this.MinOccursString != null)
        obj.MinOccursString = this.MinOccursString;
      if (this.MaxOccursString == null)
        return;
      obj.MaxOccursString = this.MaxOccursString;
    }

    internal virtual bool ValidateOccurenceRangeOK(
      XmlSchemaParticle other,
      ValidationEventHandler h,
      XmlSchema schema,
      bool raiseError)
    {
      if (!(this.ValidatedMinOccurs < other.ValidatedMinOccurs) && (!(other.ValidatedMaxOccurs != Decimal.MaxValue) || !(this.ValidatedMaxOccurs > other.ValidatedMaxOccurs)))
        return true;
      if (raiseError)
        this.error(h, "Invalid derivation occurence range was found.");
      return false;
    }

    internal virtual Decimal GetMinEffectiveTotalRange() => this.ValidatedMinOccurs;

    internal Decimal GetMinEffectiveTotalRangeAllAndSequence()
    {
      if (this.minEffectiveTotalRange >= 0M)
        return this.minEffectiveTotalRange;
      Decimal rangeAllAndSequence = 0M;
      foreach (XmlSchemaParticle xmlSchemaParticle in !(this is XmlSchemaAll) ? ((XmlSchemaSequence) this).Items : ((XmlSchemaAll) this).Items)
        rangeAllAndSequence += xmlSchemaParticle.GetMinEffectiveTotalRange();
      this.minEffectiveTotalRange = rangeAllAndSequence;
      return rangeAllAndSequence;
    }

    internal virtual bool ValidateIsEmptiable() => this.validatedMinOccurs == 0M || this.GetMinEffectiveTotalRange() == 0M;

    internal virtual bool ValidateDerivationByRestriction(
      XmlSchemaParticle baseParticle,
      ValidationEventHandler h,
      XmlSchema schema,
      bool raiseError)
    {
      return false;
    }

    internal virtual void ValidateUniqueParticleAttribution(
      XmlSchemaObjectTable qnames,
      ArrayList nsNames,
      ValidationEventHandler h,
      XmlSchema schema)
    {
    }

    internal virtual void ValidateUniqueTypeAttribution(
      XmlSchemaObjectTable labels,
      ValidationEventHandler h,
      XmlSchema schema)
    {
    }

    internal virtual void CheckRecursion(int depth, ValidationEventHandler h, XmlSchema schema)
    {
    }

    internal virtual bool ParticleEquals(XmlSchemaParticle other) => false;

    internal class EmptyParticle : XmlSchemaParticle
    {
      internal EmptyParticle()
      {
      }

      internal override XmlSchemaParticle GetOptimizedParticle(bool isTop) => (XmlSchemaParticle) this;

      internal override bool ParticleEquals(XmlSchemaParticle other) => other == this || other == XmlSchemaParticle.Empty;

      internal override bool ValidateDerivationByRestriction(
        XmlSchemaParticle baseParticle,
        ValidationEventHandler h,
        XmlSchema schema,
        bool raiseError)
      {
        return true;
      }

      internal override void CheckRecursion(int depth, ValidationEventHandler h, XmlSchema schema)
      {
      }

      internal override void ValidateUniqueParticleAttribution(
        XmlSchemaObjectTable qnames,
        ArrayList nsNames,
        ValidationEventHandler h,
        XmlSchema schema)
      {
      }

      internal override void ValidateUniqueTypeAttribution(
        XmlSchemaObjectTable labels,
        ValidationEventHandler h,
        XmlSchema schema)
      {
      }
    }
  }
}
