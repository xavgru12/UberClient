// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdWildcard
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdWildcard
  {
    private XmlSchemaObject xsobj;
    public XmlSchemaContentProcessing ResolvedProcessing;
    public string TargetNamespace;
    public bool SkipCompile;
    public bool HasValueAny;
    public bool HasValueLocal;
    public bool HasValueOther;
    public bool HasValueTargetNamespace;
    public StringCollection ResolvedNamespaces;

    public XsdWildcard(XmlSchemaObject wildcard) => this.xsobj = wildcard;

    private void Reset()
    {
      this.HasValueAny = false;
      this.HasValueLocal = false;
      this.HasValueOther = false;
      this.HasValueTargetNamespace = false;
      this.ResolvedNamespaces = new StringCollection();
    }

    public void Compile(string nss, ValidationEventHandler h, XmlSchema schema)
    {
      if (this.SkipCompile)
        return;
      this.Reset();
      int num1 = 0;
      foreach (string split in XmlSchemaUtil.SplitList(nss != null ? nss : "##any"))
      {
        string key = split;
        if (key != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (XsdWildcard.\u003C\u003Ef__switch\u0024map6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            XsdWildcard.\u003C\u003Ef__switch\u0024map6 = new Dictionary<string, int>(4)
            {
              {
                "##any",
                0
              },
              {
                "##other",
                1
              },
              {
                "##targetNamespace",
                2
              },
              {
                "##local",
                3
              }
            };
          }
          int num2;
          // ISSUE: reference to a compiler-generated field
          if (XsdWildcard.\u003C\u003Ef__switch\u0024map6.TryGetValue(key, out num2))
          {
            switch (num2)
            {
              case 0:
                if (this.HasValueAny)
                  this.xsobj.error(h, "Multiple specification of ##any was found.");
                num1 |= 1;
                this.HasValueAny = true;
                continue;
              case 1:
                if (this.HasValueOther)
                  this.xsobj.error(h, "Multiple specification of ##other was found.");
                num1 |= 2;
                this.HasValueOther = true;
                continue;
              case 2:
                if (this.HasValueTargetNamespace)
                  this.xsobj.error(h, "Multiple specification of ##targetNamespace was found.");
                num1 |= 4;
                this.HasValueTargetNamespace = true;
                continue;
              case 3:
                if (this.HasValueLocal)
                  this.xsobj.error(h, "Multiple specification of ##local was found.");
                num1 |= 8;
                this.HasValueLocal = true;
                continue;
            }
          }
        }
        if (!XmlSchemaUtil.CheckAnyUri(split))
          this.xsobj.error(h, "the namespace is not a valid anyURI");
        else if (this.ResolvedNamespaces.Contains(split))
        {
          this.xsobj.error(h, "Multiple specification of '" + split + "' was found.");
        }
        else
        {
          num1 |= 16;
          this.ResolvedNamespaces.Add(split);
        }
      }
      if ((num1 & 1) == 1 && num1 != 1)
        this.xsobj.error(h, "##any if present must be the only namespace attribute");
      if ((num1 & 2) != 2 || num1 == 2)
        return;
      this.xsobj.error(h, "##other if present must be the only namespace attribute");
    }

    public bool ExamineAttributeWildcardIntersection(
      XmlSchemaAny other,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      if (this.HasValueAny == other.HasValueAny && this.HasValueLocal == other.HasValueLocal && this.HasValueOther == other.HasValueOther && this.HasValueTargetNamespace == other.HasValueTargetNamespace && this.ResolvedProcessing == other.ResolvedProcessContents)
      {
        bool flag = false;
        for (int index = 0; index < this.ResolvedNamespaces.Count; ++index)
        {
          if (!other.ResolvedNamespaces.Contains(this.ResolvedNamespaces[index]))
            flag = true;
        }
        if (!flag)
          return false;
      }
      if (this.HasValueAny)
        return !other.HasValueAny && !other.HasValueLocal && !other.HasValueOther && !other.HasValueTargetNamespace && other.ResolvedNamespaces.Count == 0;
      if (other.HasValueAny)
        return !this.HasValueAny && !this.HasValueLocal && !this.HasValueOther && !this.HasValueTargetNamespace && this.ResolvedNamespaces.Count == 0;
      if (this.HasValueOther && other.HasValueOther && this.TargetNamespace != other.TargetNamespace)
        return false;
      if (this.HasValueOther)
        return (!other.HasValueLocal || !(this.TargetNamespace != string.Empty)) && (!other.HasValueTargetNamespace || !(this.TargetNamespace != other.TargetNamespace)) && other.ValidateWildcardAllowsNamespaceName(this.TargetNamespace, h, schema, false);
      if (other.HasValueOther)
        return (!this.HasValueLocal || !(other.TargetNamespace != string.Empty)) && (!this.HasValueTargetNamespace || !(other.TargetNamespace != this.TargetNamespace)) && this.ValidateWildcardAllowsNamespaceName(other.TargetNamespace, h, schema, false);
      if (this.ResolvedNamespaces.Count > 0)
      {
        for (int index = 0; index < this.ResolvedNamespaces.Count; ++index)
        {
          if (other.ResolvedNamespaces.Contains(this.ResolvedNamespaces[index]))
            return false;
        }
      }
      return true;
    }

    public bool ValidateWildcardAllowsNamespaceName(
      string ns,
      ValidationEventHandler h,
      XmlSchema schema,
      bool raiseError)
    {
      if (this.HasValueAny || this.HasValueOther && ns != this.TargetNamespace || this.HasValueTargetNamespace && ns == this.TargetNamespace || this.HasValueLocal && ns == string.Empty)
        return true;
      for (int index = 0; index < this.ResolvedNamespaces.Count; ++index)
      {
        if (ns == this.ResolvedNamespaces[index])
          return true;
      }
      if (raiseError)
        this.xsobj.error(h, "This wildcard does not allow the namespace: " + ns);
      return false;
    }

    internal void ValidateWildcardSubset(
      XsdWildcard other,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      this.ValidateWildcardSubset(other, h, schema, true);
    }

    internal bool ValidateWildcardSubset(
      XsdWildcard other,
      ValidationEventHandler h,
      XmlSchema schema,
      bool raiseError)
    {
      if (other.HasValueAny || this.HasValueOther && other.HasValueOther && (this.TargetNamespace == other.TargetNamespace || other.TargetNamespace == null || other.TargetNamespace == string.Empty))
        return true;
      if (this.HasValueAny)
      {
        if (raiseError)
          this.xsobj.error(h, "Invalid wildcard subset was found.");
        return false;
      }
      if (other.HasValueOther)
      {
        if (this.HasValueTargetNamespace && other.TargetNamespace == this.TargetNamespace || this.HasValueLocal && (other.TargetNamespace == null || other.TargetNamespace.Length == 0))
        {
          if (raiseError)
            this.xsobj.error(h, "Invalid wildcard subset was found.");
          return false;
        }
        for (int index = 0; index < this.ResolvedNamespaces.Count; ++index)
        {
          if (this.ResolvedNamespaces[index] == other.TargetNamespace)
          {
            if (raiseError)
              this.xsobj.error(h, "Invalid wildcard subset was found.");
            return false;
          }
        }
      }
      else
      {
        if (this.HasValueLocal && !other.HasValueLocal || this.HasValueTargetNamespace && !other.HasValueTargetNamespace)
        {
          if (raiseError)
            this.xsobj.error(h, "Invalid wildcard subset was found.");
          return false;
        }
        if (this.HasValueOther)
        {
          if (raiseError)
            this.xsobj.error(h, "Invalid wildcard subset was found.");
          return false;
        }
        for (int index = 0; index < this.ResolvedNamespaces.Count; ++index)
        {
          if (!other.ResolvedNamespaces.Contains(this.ResolvedNamespaces[index]))
          {
            if (raiseError)
              this.xsobj.error(h, "Invalid wildcard subset was found.");
            return false;
          }
        }
      }
      return true;
    }
  }
}
