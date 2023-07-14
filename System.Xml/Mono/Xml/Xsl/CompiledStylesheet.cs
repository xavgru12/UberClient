// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.CompiledStylesheet
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Xsl.Operations;
using System.Collections;
using System.Xml;

namespace Mono.Xml.Xsl
{
  internal class CompiledStylesheet
  {
    private XslStylesheet style;
    private Hashtable globalVariables;
    private Hashtable attrSets;
    private XmlNamespaceManager nsMgr;
    private Hashtable keys;
    private Hashtable outputs;
    private Hashtable decimalFormats;
    private MSXslScriptManager msScripts;

    public CompiledStylesheet(
      XslStylesheet style,
      Hashtable globalVariables,
      Hashtable attrSets,
      XmlNamespaceManager nsMgr,
      Hashtable keys,
      Hashtable outputs,
      Hashtable decimalFormats,
      MSXslScriptManager msScripts)
    {
      this.style = style;
      this.globalVariables = globalVariables;
      this.attrSets = attrSets;
      this.nsMgr = nsMgr;
      this.keys = keys;
      this.outputs = outputs;
      this.decimalFormats = decimalFormats;
      this.msScripts = msScripts;
    }

    public Hashtable Variables => this.globalVariables;

    public XslStylesheet Style => this.style;

    public XmlNamespaceManager NamespaceManager => this.nsMgr;

    public Hashtable Keys => this.keys;

    public Hashtable Outputs => this.outputs;

    public MSXslScriptManager ScriptManager => this.msScripts;

    public XslDecimalFormat LookupDecimalFormat(XmlQualifiedName name) => !(this.decimalFormats[(object) name] is XslDecimalFormat decimalFormat) && name == XmlQualifiedName.Empty ? XslDecimalFormat.Default : decimalFormat;

    public XslGeneralVariable ResolveVariable(XmlQualifiedName name) => (XslGeneralVariable) this.globalVariables[(object) name];

    public ArrayList ResolveKey(XmlQualifiedName name) => (ArrayList) this.keys[(object) name];

    public XslAttributeSet ResolveAttributeSet(XmlQualifiedName name) => (XslAttributeSet) this.attrSets[(object) name];
  }
}
