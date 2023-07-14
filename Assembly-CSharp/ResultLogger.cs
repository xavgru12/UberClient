// Decompiled with JetBrains decompiler
// Type: ResultLogger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Text;
using UnityEngine;

public class ResultLogger : Object
{
  public static void logObject(object result)
  {
    if (result == null)
      Debug.Log((object) "attempting to log a null object");
    else if (result.GetType() == typeof (ArrayList))
      ResultLogger.logArraylist((ArrayList) result);
    else if (result.GetType() == typeof (Hashtable))
      ResultLogger.logHashtable((Hashtable) result);
    else
      Debug.Log((object) "result is not a hashtable or arraylist");
  }

  public static void logArraylist(ArrayList result)
  {
    StringBuilder builder = new StringBuilder();
    foreach (Hashtable hashtable in result)
    {
      ResultLogger.addHashtableToString(builder, hashtable);
      builder.Append("\n--------------------\n");
    }
    Debug.Log((object) builder.ToString());
  }

  public static void logHashtable(Hashtable result)
  {
    StringBuilder builder = new StringBuilder();
    ResultLogger.addHashtableToString(builder, result);
    Debug.Log((object) builder.ToString());
  }

  public static void addHashtableToString(StringBuilder builder, Hashtable item)
  {
    foreach (DictionaryEntry dictionaryEntry in item)
    {
      if (dictionaryEntry.Value is Hashtable)
      {
        builder.AppendFormat("{0}: ", dictionaryEntry.Key);
        ResultLogger.addHashtableToString(builder, (Hashtable) dictionaryEntry.Value);
      }
      else if (dictionaryEntry.Value is ArrayList)
      {
        builder.AppendFormat("{0}: ", dictionaryEntry.Key);
        ResultLogger.addArraylistToString(builder, (ArrayList) dictionaryEntry.Value);
      }
      else
        builder.AppendFormat("{0}: {1}\n", dictionaryEntry.Key, dictionaryEntry.Value);
    }
  }

  public static void addArraylistToString(StringBuilder builder, ArrayList result)
  {
    foreach (object result1 in result)
    {
      switch (result1)
      {
        case Hashtable _:
          ResultLogger.addHashtableToString(builder, (Hashtable) result1);
          break;
        case ArrayList _:
          ResultLogger.addArraylistToString(builder, (ArrayList) result1);
          break;
      }
      builder.Append("\n--------------------\n");
    }
  }
}
