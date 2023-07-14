// Decompiled with JetBrains decompiler
// Type: ClientConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using UnityEngine;

public class ClientConfiguration
{
  public ClientConfiguration()
  {
    this.BuildType = BuildType.Dev;
    this.DebugLevel = DebugLevel.Debug;
    this.WebServiceBaseUrl = string.Empty;
    this.ContentBaseUrl = string.Empty;
    this.ChannelType = ChannelType.WebPortal;
  }

  public BuildType BuildType { get; set; }

  public DebugLevel DebugLevel { get; set; }

  public string WebServiceBaseUrl { get; set; }

  public string ContentBaseUrl { get; set; }

  public ChannelType ChannelType { get; set; }

  public void SetBuildType(string value)
  {
    try
    {
      this.BuildType = (BuildType) Enum.Parse(typeof (BuildType), value);
    }
    catch
    {
      Debug.LogError((object) "Unsupported BuildType!");
    }
  }

  public void SetChannelType(string value)
  {
    try
    {
      this.ChannelType = (ChannelType) Enum.Parse(typeof (ChannelType), value);
    }
    catch
    {
      Debug.LogError((object) "Unsupported ChannelType!");
    }
  }

  public void SetDebugLevel(string value)
  {
    try
    {
      this.DebugLevel = (DebugLevel) Enum.Parse(typeof (DebugLevel), value);
    }
    catch
    {
      Debug.LogError((object) "Unsupported DebugLevel!");
    }
  }

  public bool IsValid() => (string.IsNullOrEmpty(this.WebServiceBaseUrl) ? 1 : (string.IsNullOrEmpty(this.ContentBaseUrl) ? 1 : 0)) == 0;
}
