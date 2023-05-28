// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.StatsCollectionHelper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace UberStrike.Core.Models
{
  public static class StatsCollectionHelper
  {
    private static List<PropertyInfo> properties = new List<PropertyInfo>();

    static StatsCollectionHelper()
    {
      foreach (PropertyInfo property in typeof (StatsCollection).GetProperties(BindingFlags.Instance | BindingFlags.Public))
      {
        if (property.PropertyType == typeof (int) && property.CanRead && property.CanWrite)
          StatsCollectionHelper.properties.Add(property);
      }
    }

    public static string ToString(StatsCollection instance)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (PropertyInfo property in StatsCollectionHelper.properties)
        stringBuilder.AppendFormat("{0}:{1}\n", (object) property.Name, property.GetValue((object) instance, (object[]) null));
      return stringBuilder.ToString();
    }

    public static void Reset(StatsCollection instance)
    {
      foreach (PropertyInfo property in StatsCollectionHelper.properties)
        property.SetValue((object) instance, (object) 0, (object[]) null);
    }

    public static void TakeBestValues(StatsCollection instance, StatsCollection that)
    {
      foreach (PropertyInfo property in StatsCollectionHelper.properties)
      {
        int num1 = (int) property.GetValue((object) instance, (object[]) null);
        int num2 = (int) property.GetValue((object) that, (object[]) null);
        if (num1 < num2)
          property.SetValue((object) instance, (object) num2, (object[]) null);
      }
    }

    public static void AddAllValues(StatsCollection instance, StatsCollection that)
    {
      foreach (PropertyInfo property in StatsCollectionHelper.properties)
      {
        int num1 = (int) property.GetValue((object) instance, (object[]) null);
        int num2 = (int) property.GetValue((object) that, (object[]) null);
        property.SetValue((object) instance, (object) (num1 + num2), (object[]) null);
      }
    }
  }
}
