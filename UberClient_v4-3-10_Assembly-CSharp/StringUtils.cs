// Decompiled with JetBrains decompiler
// Type: StringUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public static class StringUtils
{
  public static T ParseValue<T>(string value)
  {
    System.Type enumType = typeof (T);
    T obj = default (T);
    try
    {
      if (enumType.IsEnum)
        obj = (T) Enum.Parse(enumType, value);
      else if (enumType == typeof (int))
        obj = (T) (ValueType) int.Parse(value);
      else if (enumType == typeof (string))
        obj = (T) value;
      else if (enumType == typeof (DateTime))
        obj = (T) (ValueType) DateTime.Parse(TextUtilities.Base64Decode(value));
      else
        Debug.LogError((object) ("ParseValue couldn't find a conversion of value '" + value + "' into type '" + enumType.Name + "'"));
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("ParseValue failed converting value '" + value + "' into type '" + enumType.Name + "': " + ex.Message));
    }
    return obj;
  }
}
