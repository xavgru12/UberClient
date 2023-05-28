// Decompiled with JetBrains decompiler
// Type: WebPlayerSrcValues
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WebPlayerSrcValues
{
  public WebPlayerSrcValues(string srcValue)
  {
    this.Expiration = DateTime.MinValue;
    if (string.IsNullOrEmpty(srcValue))
      return;
    Dictionary<string, string> queryString = WebPlayerSrcValues.ParseQueryString(WWW.UnEscapeURL(srcValue, Encoding.UTF8));
    this.Cmid = WebPlayerSrcValues.ParseKey<int>(queryString, "cmid");
    this.ChannelType = WebPlayerSrcValues.ParseKey<ChannelType>(queryString, "channeltype");
    this.EmbedType = WebPlayerSrcValues.ParseKey<EmbedType>(queryString, "embedtype");
    this.Expiration = WebPlayerSrcValues.ParseKey<DateTime>(queryString, "time");
    this.Content = WebPlayerSrcValues.ParseKey<string>(queryString, "content");
    this.Hash = WebPlayerSrcValues.ParseKey<string>(queryString, "hash");
    this.EsnsId = WebPlayerSrcValues.ParseKey<string>(queryString, "esnsmemberid");
    this.Locale = WebPlayerSrcValues.ParseKey<string>(queryString, "lang");
  }

  public int Cmid { get; private set; }

  public DateTime Expiration { get; private set; }

  public string Content { get; private set; }

  public string Hash { get; private set; }

  public string EsnsId { get; private set; }

  public ChannelType ChannelType { get; private set; }

  public EmbedType EmbedType { get; private set; }

  public string Locale { get; private set; }

  public bool IsValid => this.Cmid > 0 && !string.IsNullOrEmpty(this.Hash);

  private static T ParseKey<T>(Dictionary<string, string> dict, string key)
  {
    T key1 = default (T);
    string str;
    if (dict.TryGetValue(key, out str))
      key1 = StringUtils.ParseValue<T>(str);
    else
      Debug.LogWarning((object) ("ParseKey didn't find value for key '" + key + "'"));
    return key1;
  }

  public static Dictionary<string, string> ParseQueryString(string queryString)
  {
    Dictionary<string, string> queryString1 = new Dictionary<string, string>();
    if (string.IsNullOrEmpty(queryString) || !queryString.Contains("=") || queryString.Length < 3)
    {
      Debug.LogWarning((object) ("Invalid Querystring: " + queryString));
    }
    else
    {
      string str1 = queryString.Substring(queryString.IndexOf("?") + 1, queryString.Length - queryString.IndexOf("?") - 1);
      char[] chArray = new char[1]{ '&' };
      foreach (string str2 in str1.Split(chArray))
        queryString1.Add(str2.Substring(0, str2.IndexOf("=")).ToLower(), str2.Substring(str2.IndexOf("=") + 1, str2.Length - str2.IndexOf("=") - 1));
    }
    return queryString1;
  }
}
