// Decompiled with JetBrains decompiler
// Type: ItemConfigurationUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Globalization;
using System.Reflection;
using UberStrike.Core.Models.Views;
using UberStrike.Realtime.UnitySdk;

public static class ItemConfigurationUtil
{
  public static void CopyProperties<T>(T config, BaseUberStrikeItemView item) where T : BaseUberStrikeItemView
  {
    CloneUtil.CopyAllFields<BaseUberStrikeItemView>((BaseUberStrikeItemView) config, item);
    foreach (FieldInfo allField in ReflectionHelper.GetAllFields(config.GetType(), true))
    {
      string customPropertyName = ItemConfigurationUtil.GetCustomPropertyName(allField);
      if (!string.IsNullOrEmpty(customPropertyName) && item.CustomProperties != null && item.CustomProperties.ContainsKey(customPropertyName))
        allField.SetValue((object) config, ItemConfigurationUtil.Convert(item.CustomProperties[customPropertyName], allField.FieldType));
    }
  }

  private static string GetCustomPropertyName(FieldInfo info)
  {
    object[] customAttributes = info.GetCustomAttributes(typeof (CustomPropertyAttribute), true);
    return customAttributes.Length > 0 ? ((CustomPropertyAttribute) customAttributes[0]).Name : string.Empty;
  }

  private static object Convert(string value, Type type)
  {
    if (type == typeof (string))
      return (object) value;
    if (type.IsEnum || type == typeof (int))
      return (object) int.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
    if (type == typeof (float))
      return (object) float.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
    if (type == typeof (bool))
      return (object) bool.Parse(value);
    throw new NotSupportedException("ConfigurableItem has unsupported property of type: " + (object) type);
  }
}
