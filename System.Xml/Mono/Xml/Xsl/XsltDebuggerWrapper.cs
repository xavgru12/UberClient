// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XsltDebuggerWrapper
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Reflection;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
  internal class XsltDebuggerWrapper
  {
    private readonly MethodInfo on_compile;
    private readonly MethodInfo on_execute;
    private readonly object impl;

    public XsltDebuggerWrapper(object impl)
    {
      this.impl = impl;
      this.on_compile = impl.GetType().GetMethod("OnCompile", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (this.on_compile == null)
        throw new InvalidOperationException("INTERNAL ERROR: the debugger does not look like what System.Xml.dll expects. OnCompile method was not found");
      this.on_execute = impl.GetType().GetMethod("OnExecute", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (this.on_execute == null)
        throw new InvalidOperationException("INTERNAL ERROR: the debugger does not look like what System.Xml.dll expects. OnExecute method was not found");
    }

    public void DebugCompile(XPathNavigator style) => this.on_compile.Invoke(this.impl, new object[1]
    {
      (object) style.Clone()
    });

    public void DebugExecute(XslTransformProcessor p, XPathNavigator style) => this.on_execute.Invoke(this.impl, new object[3]
    {
      (object) p.CurrentNodeset.Clone(),
      (object) style.Clone(),
      (object) p.XPathContext
    });
  }
}
