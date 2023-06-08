using System;
using System.Collections.Generic;
using System.Text;
using UberStrike.WebService.Unity;
using UnityEngine;

public class DebugWebServices : IDebugPage
{
	private StringBuilder _requestLog;

	private string _currentLog = string.Empty;

	private Vector2 scroller;

	public string Title => "WS";

	public DebugWebServices()
	{
		_requestLog = new StringBuilder();
		Configuration.RequestLogger = (Action<string>)Delegate.Combine(Configuration.RequestLogger, new Action<string>(AddRequestLog));
	}

	private void AddRequestLog(string log)
	{
		_requestLog.AppendLine(log);
		_currentLog = _requestLog.ToString();
	}

	public void Draw()
	{
		scroller = GUILayout.BeginScrollView(scroller);
		GUILayout.Label("IN (" + WebServiceStatistics.TotalBytesIn.ToString() + ") -  OUT (" + WebServiceStatistics.TotalBytesOut.ToString() + ")");
		foreach (KeyValuePair<string, WebServiceStatistics.Statistics> datum in WebServiceStatistics.Data)
		{
			GUILayout.Label(datum.Key + ": " + datum.Value?.ToString());
		}
		GUILayout.TextArea(_currentLog);
		GUILayout.EndScrollView();
	}
}
