// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslMessage
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
  internal class XslMessage : XslCompiledElement
  {
    private static TextWriter output;
    private bool terminate;
    private XslOperation children;

    public XslMessage(Compiler c)
      : base(c)
    {
    }

    static XslMessage()
    {
      string environmentVariable = Environment.GetEnvironmentVariable("MONO_XSLT_MESSAGE_OUTPUT");
      if (environmentVariable != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XslMessage.\u003C\u003Ef__switch\u0024mapA == null)
        {
          // ISSUE: reference to a compiler-generated field
          XslMessage.\u003C\u003Ef__switch\u0024mapA = new Dictionary<string, int>(2)
          {
            {
              "none",
              0
            },
            {
              "stderr",
              1
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XslMessage.\u003C\u003Ef__switch\u0024mapA.TryGetValue(environmentVariable, out num))
        {
          if (num != 0)
          {
            if (num == 1)
            {
              XslMessage.output = Console.Error;
              return;
            }
          }
          else
          {
            XslMessage.output = TextWriter.Null;
            return;
          }
        }
      }
      XslMessage.output = Console.Out;
    }

    protected override void Compile(Compiler c)
    {
      if (c.Debugger != null)
        c.Debugger.DebugCompile(this.DebugInput);
      c.CheckExtraAttributes("message", "terminate");
      this.terminate = c.ParseYesNoAttribute("terminate", false);
      if (!c.Input.MoveToFirstChild())
        return;
      this.children = c.CompileTemplateContent();
      c.Input.MoveToParent();
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      if (this.children != null)
        XslMessage.output.Write(this.children.EvaluateAsString(p));
      if (this.terminate)
        throw new XsltException("Transformation terminated.", (Exception) null, p.CurrentNode);
    }
  }
}
