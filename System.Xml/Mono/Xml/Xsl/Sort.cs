// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Sort
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Xsl.Operations;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
  internal class Sort
  {
    private string lang;
    private XmlDataType dataType;
    private XmlSortOrder order;
    private XmlCaseOrder caseOrder;
    private XslAvt langAvt;
    private XslAvt dataTypeAvt;
    private XslAvt orderAvt;
    private XslAvt caseOrderAvt;
    private XPathExpression expr;

    public Sort(Compiler c)
    {
      c.CheckExtraAttributes("sort", "select", nameof (lang), "data-type", nameof (order), "case-order");
      this.expr = (XPathExpression) c.CompileExpression(c.GetAttribute("select"));
      if (this.expr == null)
        this.expr = (XPathExpression) c.CompileExpression("string(.)");
      this.langAvt = c.ParseAvtAttribute(nameof (lang));
      this.dataTypeAvt = c.ParseAvtAttribute("data-type");
      this.orderAvt = c.ParseAvtAttribute(nameof (order));
      this.caseOrderAvt = c.ParseAvtAttribute("case-order");
      this.lang = this.ParseLang(XslAvt.AttemptPreCalc(ref this.langAvt));
      this.dataType = this.ParseDataType(XslAvt.AttemptPreCalc(ref this.dataTypeAvt));
      this.order = this.ParseOrder(XslAvt.AttemptPreCalc(ref this.orderAvt));
      this.caseOrder = this.ParseCaseOrder(XslAvt.AttemptPreCalc(ref this.caseOrderAvt));
    }

    public bool IsContextDependent => this.orderAvt != null || this.caseOrderAvt != null || this.langAvt != null || this.dataTypeAvt != null;

    private string ParseLang(string value) => value;

    private XmlDataType ParseDataType(string value)
    {
      string key = value;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (Sort.\u003C\u003Ef__switch\u0024map10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Sort.\u003C\u003Ef__switch\u0024map10 = new Dictionary<string, int>(2)
          {
            {
              "number",
              0
            },
            {
              "text",
              1
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (Sort.\u003C\u003Ef__switch\u0024map10.TryGetValue(key, out num))
        {
          if (num == 0)
            return XmlDataType.Number;
          if (num == 1)
            ;
        }
      }
      return XmlDataType.Text;
    }

    private XmlSortOrder ParseOrder(string value)
    {
      string key = value;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (Sort.\u003C\u003Ef__switch\u0024map11 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Sort.\u003C\u003Ef__switch\u0024map11 = new Dictionary<string, int>(2)
          {
            {
              "descending",
              0
            },
            {
              "ascending",
              1
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (Sort.\u003C\u003Ef__switch\u0024map11.TryGetValue(key, out num))
        {
          if (num == 0)
            return XmlSortOrder.Descending;
          if (num == 1)
            ;
        }
      }
      return XmlSortOrder.Ascending;
    }

    private XmlCaseOrder ParseCaseOrder(string value)
    {
      string key = value;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (Sort.\u003C\u003Ef__switch\u0024map12 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Sort.\u003C\u003Ef__switch\u0024map12 = new Dictionary<string, int>(2)
          {
            {
              "upper-first",
              0
            },
            {
              "lower-first",
              1
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (Sort.\u003C\u003Ef__switch\u0024map12.TryGetValue(key, out num))
        {
          if (num == 0)
            return XmlCaseOrder.UpperFirst;
          if (num == 1)
            return XmlCaseOrder.LowerFirst;
        }
      }
      return XmlCaseOrder.None;
    }

    public void AddToExpr(XPathExpression e, XslTransformProcessor p) => e.AddSort((object) this.expr, this.orderAvt != null ? this.ParseOrder(this.orderAvt.Evaluate(p)) : this.order, this.caseOrderAvt != null ? this.ParseCaseOrder(this.caseOrderAvt.Evaluate(p)) : this.caseOrder, this.langAvt != null ? this.ParseLang(this.langAvt.Evaluate(p)) : this.lang, this.dataTypeAvt != null ? this.ParseDataType(this.dataTypeAvt.Evaluate(p)) : this.dataType);

    public XPathSorter ToXPathSorter(XslTransformProcessor p) => new XPathSorter((object) this.expr, this.orderAvt != null ? this.ParseOrder(this.orderAvt.Evaluate(p)) : this.order, this.caseOrderAvt != null ? this.ParseCaseOrder(this.caseOrderAvt.Evaluate(p)) : this.caseOrder, this.langAvt != null ? this.ParseLang(this.langAvt.Evaluate(p)) : this.lang, this.dataTypeAvt != null ? this.ParseDataType(this.dataTypeAvt.Evaluate(p)) : this.dataType);
  }
}
