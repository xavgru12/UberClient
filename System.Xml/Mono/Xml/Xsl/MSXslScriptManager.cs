// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.MSXslScriptManager
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Policy;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class MSXslScriptManager
  {
    private Hashtable scripts = new Hashtable();

    public void AddScript(Compiler c)
    {
      MSXslScriptManager.MSXslScript msXslScript = new MSXslScriptManager.MSXslScript(c.Input, c.Evidence);
      this.scripts.Add((object) (c.Input.GetNamespace(msXslScript.ImplementsPrefix) ?? throw new XsltCompileException("Specified prefix for msxsl:script was not found: " + msXslScript.ImplementsPrefix, (Exception) null, c.Input)), msXslScript.Compile(c.Input));
    }

    public object GetExtensionObject(string ns) => !this.scripts.ContainsKey((object) ns) ? (object) null : Activator.CreateInstance((Type) this.scripts[(object) ns]);

    private enum ScriptingLanguage
    {
      JScript,
      VisualBasic,
      CSharp,
    }

    private class MSXslScript
    {
      private MSXslScriptManager.ScriptingLanguage language;
      private string implementsPrefix;
      private string code;
      private Evidence evidence;

      public MSXslScript(XPathNavigator nav, Evidence evidence)
      {
        this.evidence = evidence;
        this.code = nav.Value;
        if (nav.MoveToFirstAttribute())
        {
          do
          {
            string localName = nav.LocalName;
            if (localName != null)
            {
              // ISSUE: reference to a compiler-generated field
              if (MSXslScriptManager.MSXslScript.\u003C\u003Ef__switch\u0024map1A == null)
              {
                // ISSUE: reference to a compiler-generated field
                MSXslScriptManager.MSXslScript.\u003C\u003Ef__switch\u0024map1A = new Dictionary<string, int>(2)
                {
                  {
                    nameof (language),
                    0
                  },
                  {
                    "implements-prefix",
                    1
                  }
                };
              }
              int num1;
              // ISSUE: reference to a compiler-generated field
              if (MSXslScriptManager.MSXslScript.\u003C\u003Ef__switch\u0024map1A.TryGetValue(localName, out num1))
              {
                switch (num1)
                {
                  case 0:
                    string lower = nav.Value.ToLower(CultureInfo.InvariantCulture);
                    if (lower != null)
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (MSXslScriptManager.MSXslScript.\u003C\u003Ef__switch\u0024map19 == null)
                      {
                        // ISSUE: reference to a compiler-generated field
                        MSXslScriptManager.MSXslScript.\u003C\u003Ef__switch\u0024map19 = new Dictionary<string, int>(6)
                        {
                          {
                            "jscript",
                            0
                          },
                          {
                            "javascript",
                            0
                          },
                          {
                            "vb",
                            1
                          },
                          {
                            "visualbasic",
                            1
                          },
                          {
                            "c#",
                            2
                          },
                          {
                            "csharp",
                            2
                          }
                        };
                      }
                      int num2;
                      // ISSUE: reference to a compiler-generated field
                      if (MSXslScriptManager.MSXslScript.\u003C\u003Ef__switch\u0024map19.TryGetValue(lower, out num2))
                      {
                        switch (num2)
                        {
                          case 0:
                            this.language = MSXslScriptManager.ScriptingLanguage.JScript;
                            goto label_17;
                          case 1:
                            this.language = MSXslScriptManager.ScriptingLanguage.VisualBasic;
                            goto label_17;
                          case 2:
                            this.language = MSXslScriptManager.ScriptingLanguage.CSharp;
                            goto label_17;
                        }
                      }
                    }
                    throw new XsltException("Invalid scripting language!", (Exception) null);
                  case 1:
                    this.implementsPrefix = nav.Value;
                    break;
                }
              }
            }
label_17:;
          }
          while (nav.MoveToNextAttribute());
          nav.MoveToParent();
        }
        if (this.implementsPrefix == null)
          throw new XsltException("need implements-prefix attr", (Exception) null);
      }

      public MSXslScriptManager.ScriptingLanguage Language => this.language;

      public string ImplementsPrefix => this.implementsPrefix;

      public string Code => this.code;

      public object Compile(XPathNavigator node) => throw new NotImplementedException();
    }
  }
}
