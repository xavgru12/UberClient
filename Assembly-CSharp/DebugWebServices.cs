// Decompiled with JetBrains decompiler
// Type: DebugWebServices
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using UberStrike.WebService.Unity;
using UnityEngine;

public class DebugWebServices : IDebugPage
{
  private StringBuilder _requestLog;
  private string _currentLog;
  private Vector2 scroller;

  public DebugWebServices()
  {
    this._requestLog = new StringBuilder();
    Configuration.RequestLogger = (Action<string>) Delegate.Combine((Delegate) Configuration.RequestLogger, (Delegate) new Action<string>(this.AddRequestLog));
  }

  private void AddRequestLog(string log)
  {
    this._requestLog.AppendLine(log);
    this._currentLog = this._requestLog.ToString();
  }

  public string Title => "WS";

  public void Draw()
  {
    this.scroller = GUILayout.BeginScrollView(this.scroller);
    GUILayout.Label("IN (" + (object) WebServiceStatistics.TotalBytesIn + ") -  OUT (" + (object) WebServiceStatistics.TotalBytesOut + ")");
    foreach (KeyValuePair<string, WebServiceStatistics.Statistics> keyValuePair in (Dictionary<string, WebServiceStatistics.Statistics>) WebServiceStatistics.Data)
      GUILayout.Label(keyValuePair.Key + ": " + (object) keyValuePair.Value);
    GUILayout.TextArea(this._currentLog);
    GUILayout.EndScrollView();
  }
}
