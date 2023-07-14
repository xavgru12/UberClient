// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.KeyHelper
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Globalization;
using System.Text;

namespace System.Xml.Serialization
{
  internal class KeyHelper
  {
    public static void AddField(StringBuilder sb, int n, string val) => KeyHelper.AddField(sb, n, val, (string) null);

    public static void AddField(StringBuilder sb, int n, string val, string def)
    {
      if (!(val != def))
        return;
      sb.Append(n.ToString());
      sb.Append(val.Length.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      sb.Append(val);
    }

    public static void AddField(StringBuilder sb, int n, bool val) => KeyHelper.AddField(sb, n, val, false);

    public static void AddField(StringBuilder sb, int n, bool val, bool def)
    {
      if (val == def)
        return;
      sb.Append(n.ToString());
    }

    public static void AddField(StringBuilder sb, int n, int val, int def)
    {
      if (val == def)
        return;
      sb.Append(n.ToString());
      sb.Append(val.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static void AddField(StringBuilder sb, int n, Type val)
    {
      if (val == null)
        return;
      sb.Append(n.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      sb.Append(val.ToString());
    }
  }
}
