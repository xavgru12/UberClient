// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XsltExtensionFunction
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class XsltExtensionFunction : XPFuncImpl
  {
    private object extension;
    private MethodInfo method;
    private TypeCode[] typeCodes;

    public XsltExtensionFunction(object extension, MethodInfo method, XPathNavigator currentNode)
    {
      this.extension = extension;
      this.method = method;
      ParameterInfo[] parameters = method.GetParameters();
      int length1 = parameters.Length;
      int length2 = parameters.Length;
      this.typeCodes = new TypeCode[parameters.Length];
      XPathResultType[] argTypes = new XPathResultType[parameters.Length];
      bool flag = true;
      for (int index = parameters.Length - 1; 0 <= index; --index)
      {
        this.typeCodes[index] = Type.GetTypeCode(parameters[index].ParameterType);
        argTypes[index] = XPFuncImpl.GetXPathType(parameters[index].ParameterType, currentNode);
        if (flag)
        {
          if (parameters[index].IsOptional)
            --length1;
          else
            flag = false;
        }
      }
      this.Init(length1, length2, XPFuncImpl.GetXPathType(method.ReturnType, currentNode), argTypes);
    }

    public override object Invoke(
      XsltCompiledContext xsltContext,
      object[] args,
      XPathNavigator docContext)
    {
      try
      {
        ParameterInfo[] parameters1 = this.method.GetParameters();
        object[] parameters2 = new object[parameters1.Length];
        int num;
        for (int index = 0; index < args.Length; ++index)
        {
          Type parameterType = parameters1[index].ParameterType;
          string fullName = parameterType.FullName;
          if (fullName != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (XsltExtensionFunction.\u003C\u003Ef__switch\u0024map1C == null)
            {
              // ISSUE: reference to a compiler-generated field
              XsltExtensionFunction.\u003C\u003Ef__switch\u0024map1C = new Dictionary<string, int>(8)
              {
                {
                  "System.Int16",
                  0
                },
                {
                  "System.UInt16",
                  0
                },
                {
                  "System.Int32",
                  0
                },
                {
                  "System.UInt32",
                  0
                },
                {
                  "System.Int64",
                  0
                },
                {
                  "System.UInt64",
                  0
                },
                {
                  "System.Single",
                  0
                },
                {
                  "System.Decimal",
                  0
                }
              };
            }
            // ISSUE: reference to a compiler-generated field
            if (XsltExtensionFunction.\u003C\u003Ef__switch\u0024map1C.TryGetValue(fullName, out num) && num == 0)
            {
              parameters2[index] = Convert.ChangeType(args[index], parameterType);
              continue;
            }
          }
          parameters2[index] = args[index];
        }
        string fullName1 = this.method.ReturnType.FullName;
        object obj;
        if (fullName1 != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (XsltExtensionFunction.\u003C\u003Ef__switch\u0024map1D == null)
          {
            // ISSUE: reference to a compiler-generated field
            XsltExtensionFunction.\u003C\u003Ef__switch\u0024map1D = new Dictionary<string, int>(8)
            {
              {
                "System.Int16",
                0
              },
              {
                "System.UInt16",
                0
              },
              {
                "System.Int32",
                0
              },
              {
                "System.UInt32",
                0
              },
              {
                "System.Int64",
                0
              },
              {
                "System.UInt64",
                0
              },
              {
                "System.Single",
                0
              },
              {
                "System.Decimal",
                0
              }
            };
          }
          // ISSUE: reference to a compiler-generated field
          if (XsltExtensionFunction.\u003C\u003Ef__switch\u0024map1D.TryGetValue(fullName1, out num) && num == 0)
          {
            obj = Convert.ChangeType(this.method.Invoke(this.extension, parameters2), typeof (double));
            goto label_15;
          }
        }
        obj = this.method.Invoke(this.extension, parameters2);
label_15:
        return obj is IXPathNavigable xpathNavigable ? (object) xpathNavigable.CreateNavigator() : obj;
      }
      catch (Exception ex)
      {
        throw new XsltException("Custom function reported an error.", ex);
      }
    }
  }
}
