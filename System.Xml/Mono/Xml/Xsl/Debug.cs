// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Debug
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class Debug
  {
    [Conditional("_DEBUG")]
    internal static void TraceContext(XPathNavigator context)
    {
      string str1 = "(null)";
      if (context == null)
        return;
      context = context.Clone();
      if (context.NodeType != XPathNodeType.Element)
        return;
      string str2 = string.Format("<{0}:{1}", (object) context.Prefix, (object) context.LocalName);
      for (bool flag = context.MoveToFirstAttribute(); flag; flag = context.MoveToNextAttribute())
        str2 += string.Format((IFormatProvider) CultureInfo.InvariantCulture, " {0}:{1}={2}", (object) context.Prefix, (object) context.LocalName, (object) context.Value);
      str1 = str2 + ">";
    }

    [Conditional("DEBUG")]
    internal static void Assert(bool condition, string message)
    {
      if (!condition)
        throw new XsltException(message, (Exception) null);
    }

    [Conditional("_DEBUG")]
    internal static void WriteLine(object value) => Console.Error.WriteLine(value);

    [Conditional("_DEBUG")]
    internal static void WriteLine(string message) => Console.Error.WriteLine(message);

    [Conditional("DEBUG")]
    internal static void EnterNavigator(Compiler c)
    {
    }

    [Conditional("DEBUG")]
    internal static void ExitNavigator(Compiler c)
    {
    }
  }
}
