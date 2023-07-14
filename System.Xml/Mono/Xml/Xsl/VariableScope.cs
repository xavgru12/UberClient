// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.VariableScope
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Xsl.Operations;
using System;
using System.Collections;
using System.Xml;

namespace Mono.Xml.Xsl
{
  internal class VariableScope
  {
    private ArrayList variableNames;
    private Hashtable variables;
    private VariableScope parent;
    private int nextSlot;
    private int highTide;

    public VariableScope(VariableScope parent)
    {
      this.parent = parent;
      if (parent == null)
        return;
      this.nextSlot = parent.nextSlot;
    }

    internal void giveHighTideToParent()
    {
      if (this.parent == null)
        return;
      this.parent.highTide = Math.Max(this.VariableHighTide, this.parent.VariableHighTide);
    }

    public int VariableHighTide => Math.Max(this.highTide, this.nextSlot);

    public VariableScope Parent => this.parent;

    public int AddVariable(XslLocalVariable v)
    {
      if (this.variables == null)
      {
        this.variableNames = new ArrayList();
        this.variables = new Hashtable();
      }
      this.variables[(object) v.Name] = (object) v;
      int num = this.variableNames.IndexOf((object) v.Name);
      if (num >= 0)
        return num;
      this.variableNames.Add((object) v.Name);
      return this.nextSlot++;
    }

    public XslLocalVariable ResolveStatic(XmlQualifiedName name)
    {
      for (VariableScope variableScope = this; variableScope != null; variableScope = variableScope.Parent)
      {
        if (variableScope.variables != null && variableScope.variables[(object) name] is XslLocalVariable variable)
          return variable;
      }
      return (XslLocalVariable) null;
    }

    public XslLocalVariable Resolve(XslTransformProcessor p, XmlQualifiedName name)
    {
      for (VariableScope variableScope = this; variableScope != null; variableScope = variableScope.Parent)
      {
        if (variableScope.variables != null && variableScope.variables[(object) name] is XslLocalVariable variable && variable.IsEvaluated(p))
          return variable;
      }
      return (XslLocalVariable) null;
    }
  }
}
