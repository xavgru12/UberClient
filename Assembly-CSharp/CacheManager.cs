// Decompiled with JetBrains decompiler
// Type: CacheManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class CacheManager
{
  static CacheManager() => CacheManager.IsAuthorized = false;

  public static bool IsAuthorized { get; private set; }

  public static bool RunAuthorization()
  {
    TextAsset textAsset = Resources.Load("cmune_perm") as TextAsset;
    CacheManager.IsAuthorized = false;
    if (!string.IsNullOrEmpty(textAsset.text))
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      long size = -1;
      int expiration = -1;
      string empty3 = string.Empty;
      string[] strArray = textAsset.text.Split(' ');
      if (strArray.Length >= 4)
      {
        empty1 = strArray[0];
        empty2 = strArray[1];
        size = (long) int.Parse(strArray[2]);
        empty3 = strArray[3];
      }
      if (strArray.Length == 5)
        expiration = int.Parse(strArray[4]);
      CacheManager.IsAuthorized = (expiration >= 0 || Caching.Authorize(empty1, empty2, size, empty3)) && Caching.Authorize(empty1, empty2, size, expiration, empty3);
    }
    if (!CacheManager.IsAuthorized)
      Debug.LogWarning((object) ("Cache Autorization failed with license: " + textAsset.text));
    return CacheManager.IsAuthorized;
  }
}
