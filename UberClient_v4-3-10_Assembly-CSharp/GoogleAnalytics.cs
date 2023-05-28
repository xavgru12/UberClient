// Decompiled with JetBrains decompiler
// Type: GoogleAnalytics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class GoogleAnalytics : Singleton<GoogleAnalytics>
{
  private List<string> GAEventList = new List<string>();

  private GoogleAnalytics()
  {
  }

  public void LogEvent(string category, string action, bool unique = false) => this.LogEvent(category, action, (float) (int) Time.time, unique);

  public void LogEvent(string category, string action, float time, bool unique)
  {
    if (unique && this.GAEventList.Contains(category + "-" + action))
      return;
    AutoMonoBehaviour<Google>.Instance.logEvent(this.GetUserContext(), category, action, ((Enum) ApplicationDataManager.Channel).ToString(), (int) time);
    if (!unique)
      return;
    this.GAEventList.Add(category + "-" + action);
  }

  public void LogPageView(string page) => AutoMonoBehaviour<Google>.Instance.logPageView(page, ((Enum) ApplicationDataManager.Channel).ToString() + "-" + this.GetUserContext());

  private string GetUserContext() => GameState.HasCurrentGame ? GameState.CurrentGameMode.ToString() : MenuPageManager.Instance.GetCurrentPage().ToString();
}
