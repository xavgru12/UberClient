// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctions
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Globalization;

namespace System.Xml.XPath
{
  internal class XPathFunctions
  {
    public static bool ToBoolean(object arg)
    {
      switch (arg)
      {
        case null:
          throw new ArgumentNullException();
        case bool boolean:
          return boolean;
        case double d:
          return d != 0.0 && !double.IsNaN(d);
        case string _:
          return ((string) arg).Length != 0;
        case XPathNodeIterator _:
          return ((XPathNodeIterator) arg).MoveNext();
        case XPathNavigator _:
          return XPathFunctions.ToBoolean((object) ((XPathNavigator) arg).SelectChildren(XPathNodeType.All));
        default:
          throw new ArgumentException();
      }
    }

    public static bool ToBoolean(bool b) => b;

    public static bool ToBoolean(double d) => d != 0.0 && !double.IsNaN(d);

    public static bool ToBoolean(string s) => s != null && s.Length > 0;

    public static bool ToBoolean(BaseIterator iter) => iter != null && iter.MoveNext();

    public static string ToString(object arg)
    {
      switch (arg)
      {
        case null:
          throw new ArgumentNullException();
        case string _:
          return (string) arg;
        case bool flag:
          return flag ? "true" : "false";
        case double d:
          return XPathFunctions.ToString(d);
        case XPathNodeIterator _:
          XPathNodeIterator xpathNodeIterator = (XPathNodeIterator) arg;
          return !xpathNodeIterator.MoveNext() ? string.Empty : xpathNodeIterator.Current.Value;
        case XPathNavigator _:
          return ((XPathItem) arg).Value;
        default:
          throw new ArgumentException();
      }
    }

    public static string ToString(double d)
    {
      if (d == double.NegativeInfinity)
        return "-Infinity";
      return d == double.PositiveInfinity ? "Infinity" : d.ToString("R", (IFormatProvider) NumberFormatInfo.InvariantInfo);
    }

    public static double ToNumber(object arg)
    {
      if (arg == null)
        throw new ArgumentNullException();
      if (arg is BaseIterator || arg is XPathNavigator)
        arg = (object) XPathFunctions.ToString(arg);
      if (arg is string)
        return XPathFunctions.ToNumber(arg as string);
      if (arg is double number)
        return number;
      return arg is bool flag ? Convert.ToDouble(flag) : throw new ArgumentException();
    }

    public static double ToNumber(string arg)
    {
      string s = arg != null ? arg.Trim(XmlChar.WhitespaceChars) : throw new ArgumentNullException();
      if (s.Length == 0)
        return double.NaN;
      try
      {
        if (s[0] == '.')
          s = '.'.ToString() + s;
        return double.Parse(s, NumberStyles.Integer | NumberStyles.AllowDecimalPoint, (IFormatProvider) NumberFormatInfo.InvariantInfo);
      }
      catch (OverflowException ex)
      {
        return double.NaN;
      }
      catch (FormatException ex)
      {
        return double.NaN;
      }
    }
  }
}
