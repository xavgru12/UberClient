// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslTemplateContent
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
  internal class XslTemplateContent : XslCompiledElementBase
  {
    private ArrayList content = new ArrayList();
    private bool hasStack;
    private int stackSize;
    private XPathNodeType parentType;
    private bool xslForEach;

    public XslTemplateContent(Compiler c, XPathNodeType parentType, bool xslForEach)
      : base(c)
    {
      this.parentType = parentType;
      this.xslForEach = xslForEach;
      this.Compile(c);
    }

    public XPathNodeType ParentType => this.parentType;

    protected override void Compile(Compiler c)
    {
      if (c.Debugger != null)
        c.Debugger.DebugCompile(this.DebugInput);
      this.hasStack = c.CurrentVariableScope == null;
      c.PushScope();
      do
      {
        XPathNavigator input = c.Input;
        switch (input.NodeType)
        {
          case XPathNodeType.Element:
            string namespaceUri = input.NamespaceURI;
            if (namespaceUri != null)
            {
              // ISSUE: reference to a compiler-generated field
              if (XslTemplateContent.\u003C\u003Ef__switch\u0024mapD == null)
              {
                // ISSUE: reference to a compiler-generated field
                XslTemplateContent.\u003C\u003Ef__switch\u0024mapD = new Dictionary<string, int>(1)
                {
                  {
                    "http://www.w3.org/1999/XSL/Transform",
                    0
                  }
                };
              }
              int num1;
              // ISSUE: reference to a compiler-generated field
              if (XslTemplateContent.\u003C\u003Ef__switch\u0024mapD.TryGetValue(namespaceUri, out num1) && num1 == 0)
              {
                string localName = input.LocalName;
                if (localName != null)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (XslTemplateContent.\u003C\u003Ef__switch\u0024mapC == null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    XslTemplateContent.\u003C\u003Ef__switch\u0024mapC = new Dictionary<string, int>(19)
                    {
                      {
                        "apply-imports",
                        0
                      },
                      {
                        "apply-templates",
                        1
                      },
                      {
                        "attribute",
                        2
                      },
                      {
                        "call-template",
                        3
                      },
                      {
                        "choose",
                        4
                      },
                      {
                        "comment",
                        5
                      },
                      {
                        "copy",
                        6
                      },
                      {
                        "copy-of",
                        7
                      },
                      {
                        "element",
                        8
                      },
                      {
                        "fallback",
                        9
                      },
                      {
                        "for-each",
                        10
                      },
                      {
                        "if",
                        11
                      },
                      {
                        "message",
                        12
                      },
                      {
                        "number",
                        13
                      },
                      {
                        "processing-instruction",
                        14
                      },
                      {
                        "text",
                        15
                      },
                      {
                        "value-of",
                        16
                      },
                      {
                        "variable",
                        17
                      },
                      {
                        "sort",
                        18
                      }
                    };
                  }
                  int num2;
                  // ISSUE: reference to a compiler-generated field
                  if (XslTemplateContent.\u003C\u003Ef__switch\u0024mapC.TryGetValue(localName, out num2))
                  {
                    switch (num2)
                    {
                      case 0:
                        this.content.Add((object) new XslApplyImports(c));
                        goto label_46;
                      case 1:
                        this.content.Add((object) new XslApplyTemplates(c));
                        goto label_46;
                      case 2:
                        if (this.ParentType == XPathNodeType.All || this.ParentType == XPathNodeType.Element)
                        {
                          this.content.Add((object) new XslAttribute(c));
                          goto label_46;
                        }
                        else
                          goto label_46;
                      case 3:
                        this.content.Add((object) new XslCallTemplate(c));
                        goto label_46;
                      case 4:
                        this.content.Add((object) new XslChoose(c));
                        goto label_46;
                      case 5:
                        if (this.ParentType == XPathNodeType.All || this.ParentType == XPathNodeType.Element)
                        {
                          this.content.Add((object) new XslComment(c));
                          goto label_46;
                        }
                        else
                          goto label_46;
                      case 6:
                        this.content.Add((object) new XslCopy(c));
                        goto label_46;
                      case 7:
                        this.content.Add((object) new XslCopyOf(c));
                        goto label_46;
                      case 8:
                        if (this.ParentType == XPathNodeType.All || this.ParentType == XPathNodeType.Element)
                        {
                          this.content.Add((object) new XslElement(c));
                          goto label_46;
                        }
                        else
                          goto label_46;
                      case 9:
                        goto label_46;
                      case 10:
                        this.content.Add((object) new XslForEach(c));
                        goto label_46;
                      case 11:
                        this.content.Add((object) new XslIf(c));
                        goto label_46;
                      case 12:
                        this.content.Add((object) new XslMessage(c));
                        goto label_46;
                      case 13:
                        this.content.Add((object) new XslNumber(c));
                        goto label_46;
                      case 14:
                        if (this.ParentType == XPathNodeType.All || this.ParentType == XPathNodeType.Element)
                        {
                          this.content.Add((object) new XslProcessingInstruction(c));
                          goto label_46;
                        }
                        else
                          goto label_46;
                      case 15:
                        this.content.Add((object) new XslText(c, false));
                        goto label_46;
                      case 16:
                        this.content.Add((object) new XslValueOf(c));
                        goto label_46;
                      case 17:
                        this.content.Add((object) new XslLocalVariable(c));
                        goto label_46;
                      case 18:
                        if (!this.xslForEach)
                          throw new XsltCompileException("'sort' element is not allowed here as a templete content", (Exception) null, input);
                        goto label_46;
                    }
                  }
                }
                this.content.Add((object) new XslNotSupportedOperation(c));
                break;
              }
            }
            if (!c.IsExtensionNamespace(input.NamespaceURI))
            {
              this.content.Add((object) new XslLiteralElement(c));
              break;
            }
            if (input.MoveToFirstChild())
            {
              do
              {
                if (input.NamespaceURI == "http://www.w3.org/1999/XSL/Transform" && input.LocalName == "fallback")
                  this.content.Add((object) new XslFallback(c));
              }
              while (input.MoveToNext());
              input.MoveToParent();
              break;
            }
            break;
          case XPathNodeType.Text:
            this.content.Add((object) new XslText(c, false));
            break;
          case XPathNodeType.SignificantWhitespace:
            this.content.Add((object) new XslText(c, true));
            break;
        }
label_46:;
      }
      while (c.Input.MoveToNext());
      if (this.hasStack)
      {
        this.stackSize = c.PopScope().VariableHighTide;
        this.hasStack = this.stackSize > 0;
      }
      else
        c.PopScope();
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      if (this.hasStack)
        p.PushStack(this.stackSize);
      int count = this.content.Count;
      for (int index = 0; index < count; ++index)
        ((XslOperation) this.content[index]).Evaluate(p);
      if (!this.hasStack)
        return;
      p.PopStack();
    }
  }
}
