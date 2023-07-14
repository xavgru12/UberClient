// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.CodeIdentifier
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Globalization;

namespace System.Xml.Serialization
{
  public class CodeIdentifier
  {
    [Obsolete("Design mistake. It only contains static methods.")]
    public CodeIdentifier()
    {
    }

    public static string MakeCamel(string identifier)
    {
      string str = CodeIdentifier.MakeValid(identifier);
      return char.ToLower(str[0], CultureInfo.InvariantCulture).ToString() + str.Substring(1);
    }

    public static string MakePascal(string identifier)
    {
      string str = CodeIdentifier.MakeValid(identifier);
      return char.ToUpper(str[0], CultureInfo.InvariantCulture).ToString() + str.Substring(1);
    }

    public static string MakeValid(string identifier)
    {
      switch (identifier)
      {
        case null:
          throw new NullReferenceException();
        case "":
          return "Item";
        default:
          string str = string.Empty;
          if (!char.IsLetter(identifier[0]) && identifier[0] != '_')
            str = "Item";
          foreach (char c in identifier)
          {
            if (char.IsLetterOrDigit(c) || c == '_')
              str += (string) (object) c;
          }
          if (str.Length > 400)
            str = str.Substring(0, 400);
          return str;
      }
    }
  }
}
