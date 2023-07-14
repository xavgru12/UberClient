// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdKeyTable
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdKeyTable
  {
    public readonly bool alwaysTrue = true;
    private XsdIdentitySelector selector;
    private XmlSchemaIdentityConstraint source;
    private XmlQualifiedName qname;
    private XmlQualifiedName refKeyName;
    public XsdKeyEntryCollection Entries = new XsdKeyEntryCollection();
    public XsdKeyEntryCollection FinishedEntries = new XsdKeyEntryCollection();
    public int StartDepth;
    public XsdKeyTable ReferencedKey;

    public XsdKeyTable(XmlSchemaIdentityConstraint source) => this.Reset(source);

    public XmlQualifiedName QualifiedName => this.qname;

    public XmlQualifiedName RefKeyName => this.refKeyName;

    public XmlSchemaIdentityConstraint SourceSchemaIdentity => this.source;

    public XsdIdentitySelector Selector => this.selector;

    public void Reset(XmlSchemaIdentityConstraint source)
    {
      this.source = source;
      this.selector = source.CompiledSelector;
      this.qname = source.QualifiedName;
      if (source is XmlSchemaKeyref xmlSchemaKeyref)
        this.refKeyName = xmlSchemaKeyref.Refer;
      this.StartDepth = 0;
    }

    public XsdIdentityPath SelectorMatches(ArrayList qnameStack, int depth)
    {
      for (int index1 = 0; index1 < this.Selector.Paths.Length; ++index1)
      {
        XsdIdentityPath path = this.Selector.Paths[index1];
        if (depth == this.StartDepth)
        {
          if (path.OrderedSteps.Length == 0)
            return path;
        }
        else if (depth - this.StartDepth >= path.OrderedSteps.Length - 1)
        {
          int length = path.OrderedSteps.Length;
          if (path.OrderedSteps[length - 1].IsAttribute)
            --length;
          if ((!path.Descendants || depth >= this.StartDepth + length) && (path.Descendants || depth == this.StartDepth + length))
          {
            int index2 = length - 1;
            int num = 0;
            for (; 0 <= index2; --index2)
            {
              XsdIdentityStep orderedStep = path.OrderedSteps[index2];
              if (!orderedStep.IsAnyName)
              {
                XmlQualifiedName qname = (XmlQualifiedName) qnameStack[qnameStack.Count - num - 1];
                if ((orderedStep.NsName == null || !(qname.Namespace == orderedStep.NsName)) && (!(orderedStep.Name == qname.Name) || !(orderedStep.Namespace == qname.Namespace)) && this.alwaysTrue)
                  break;
              }
              ++num;
            }
            if (index2 < 0)
              return path;
          }
        }
      }
      return (XsdIdentityPath) null;
    }
  }
}
