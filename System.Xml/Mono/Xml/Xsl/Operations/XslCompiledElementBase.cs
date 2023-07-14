// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslCompiledElementBase
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl.Operations
{
  internal abstract class XslCompiledElementBase : XslOperation
  {
    private int lineNumber;
    private int linePosition;
    private XPathNavigator debugInput;

    public XslCompiledElementBase(Compiler c)
    {
      if (c.Input is IXmlLineInfo input)
      {
        this.lineNumber = input.LineNumber;
        this.linePosition = input.LinePosition;
      }
      if (c.Debugger == null)
        return;
      this.debugInput = c.Input.Clone();
    }

    public XPathNavigator DebugInput => this.debugInput;

    public int LineNumber => this.lineNumber;

    public int LinePosition => this.linePosition;

    protected abstract void Compile(Compiler c);
  }
}
