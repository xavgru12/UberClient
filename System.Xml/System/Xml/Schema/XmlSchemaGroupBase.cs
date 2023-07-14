// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaGroupBase
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public abstract class XmlSchemaGroupBase : XmlSchemaParticle
  {
    private XmlSchemaObjectCollection compiledItems;

    protected XmlSchemaGroupBase() => this.compiledItems = new XmlSchemaObjectCollection();

    [XmlIgnore]
    public abstract XmlSchemaObjectCollection Items { get; }

    internal XmlSchemaObjectCollection CompiledItems => this.compiledItems;

    internal void CopyOptimizedItems(XmlSchemaGroupBase gb)
    {
      for (int index = 0; index < this.Items.Count; ++index)
      {
        XmlSchemaParticle optimizedParticle = (this.Items[index] as XmlSchemaParticle).GetOptimizedParticle(false);
        if (optimizedParticle != XmlSchemaParticle.Empty)
        {
          gb.Items.Add((XmlSchemaObject) optimizedParticle);
          gb.CompiledItems.Add((XmlSchemaObject) optimizedParticle);
        }
      }
    }

    internal override bool ParticleEquals(XmlSchemaParticle other)
    {
      if (!(other is XmlSchemaGroupBase xmlSchemaGroupBase) || this.GetType() != xmlSchemaGroupBase.GetType() || this.ValidatedMaxOccurs != xmlSchemaGroupBase.ValidatedMaxOccurs || this.ValidatedMinOccurs != xmlSchemaGroupBase.ValidatedMinOccurs || this.CompiledItems.Count != xmlSchemaGroupBase.CompiledItems.Count)
        return false;
      for (int index = 0; index < this.CompiledItems.Count; ++index)
      {
        if (!(this.CompiledItems[index] as XmlSchemaParticle).ParticleEquals(xmlSchemaGroupBase.CompiledItems[index] as XmlSchemaParticle))
          return false;
      }
      return true;
    }

    internal override void CheckRecursion(int depth, ValidationEventHandler h, XmlSchema schema)
    {
      foreach (XmlSchemaParticle xmlSchemaParticle in this.Items)
        xmlSchemaParticle.CheckRecursion(depth, h, schema);
    }

    internal bool ValidateNSRecurseCheckCardinality(
      XmlSchemaAny any,
      ValidationEventHandler h,
      XmlSchema schema,
      bool raiseError)
    {
      foreach (XmlSchemaParticle xmlSchemaParticle in this.Items)
      {
        if (!xmlSchemaParticle.ValidateDerivationByRestriction((XmlSchemaParticle) any, h, schema, raiseError))
          return false;
      }
      return this.ValidateOccurenceRangeOK((XmlSchemaParticle) any, h, schema, raiseError);
    }

    internal bool ValidateRecurse(
      XmlSchemaGroupBase baseGroup,
      ValidationEventHandler h,
      XmlSchema schema,
      bool raiseError)
    {
      return this.ValidateSeqRecurseMapSumCommon(baseGroup, h, schema, false, false, raiseError);
    }

    internal bool ValidateSeqRecurseMapSumCommon(
      XmlSchemaGroupBase baseGroup,
      ValidationEventHandler h,
      XmlSchema schema,
      bool isLax,
      bool isMapAndSum,
      bool raiseError)
    {
      int index1 = 0;
      int index2 = 0;
      Decimal num = 0M;
      if (baseGroup.CompiledItems.Count == 0 && this.CompiledItems.Count > 0)
      {
        if (raiseError)
          this.error(h, "Invalid particle derivation by restriction was found. base particle does not contain particles.");
        return false;
      }
      for (int index3 = 0; index3 < this.CompiledItems.Count; ++index3)
      {
        XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle) null;
        for (; this.CompiledItems.Count > index1; ++index1)
        {
          xmlSchemaParticle = (XmlSchemaParticle) this.CompiledItems[index1];
          if (xmlSchemaParticle != XmlSchemaParticle.Empty)
            break;
        }
        if (index1 >= this.CompiledItems.Count)
        {
          if (raiseError)
            this.error(h, "Invalid particle derivation by restriction was found. Cannot be mapped to base particle.");
          return false;
        }
        while (baseGroup.CompiledItems.Count > index2)
        {
          XmlSchemaParticle compiledItem = (XmlSchemaParticle) baseGroup.CompiledItems[index2];
          if (compiledItem != XmlSchemaParticle.Empty || !(compiledItem.ValidatedMaxOccurs > 0M))
          {
            if (!xmlSchemaParticle.ValidateDerivationByRestriction(compiledItem, h, schema, false))
            {
              if (!isLax && !isMapAndSum && compiledItem.MinOccurs > num && !compiledItem.ValidateIsEmptiable())
              {
                if (raiseError)
                  this.error(h, "Invalid particle derivation by restriction was found. Invalid sub-particle derivation was found.");
                return false;
              }
              num = 0M;
              ++index2;
            }
            else
            {
              num += compiledItem.ValidatedMinOccurs;
              if (num >= baseGroup.ValidatedMaxOccurs)
              {
                num = 0M;
                ++index2;
              }
              ++index1;
              break;
            }
          }
        }
      }
      if (this.CompiledItems.Count > 0 && index1 != this.CompiledItems.Count)
      {
        if (raiseError)
          this.error(h, "Invalid particle derivation by restriction was found. Extraneous derived particle was found.");
        return false;
      }
      if (!isLax && !isMapAndSum)
      {
        if (num > 0M)
          ++index2;
        for (int index4 = index2; index4 < baseGroup.CompiledItems.Count; ++index4)
        {
          if (!(baseGroup.CompiledItems[index4] as XmlSchemaParticle).ValidateIsEmptiable())
          {
            if (raiseError)
              this.error(h, "Invalid particle derivation by restriction was found. There is a base particle which does not have mapped derived particle and is not emptiable.");
            return false;
          }
        }
      }
      return true;
    }
  }
}
