// Decompiled with JetBrains decompiler
// Type: DebugConsoleManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UberStrike.WebService.Unity;
using UnityEngine;

public class DebugConsoleManager : MonoBehaviour
{
  private Vector2 _scrollDebug;
  private static bool _isExceptionSent = false;
  private List<string> _exceptions = new List<string>(10);
  private static IDebugPage[] _debugPages = new IDebugPage[0];
  private static string[] _debugPageDescriptors = new string[0];
  private static int _currentPageSelectedIdx = 0;
  private static IDebugPage _currentPageSelected;
  [SerializeField]
  private bool _isDebugConsoleEnabled;

  public bool IsDebugConsoleEnabled
  {
    get => this._isDebugConsoleEnabled;
    set => this._isDebugConsoleEnabled = value;
  }

  private void Awake()
  {
    if (Application.isEditor)
      this.UpdatePages(MemberAccessLevel.Admin);
    else
      CmuneEventHandler.AddListener<LoginEvent>((Action<LoginEvent>) (ev => this.UpdatePages(ev.AccessLevel)));
  }

  private void Start() => Application.RegisterLogCallback(new Application.LogCallback(this.OnUnityDebugCallback));

  private void Update()
  {
    if (!KeyInput.AltPressed || !KeyInput.CtrlPressed || !KeyInput.GetKeyDown(KeyCode.D))
      return;
    this._isDebugConsoleEnabled = !this._isDebugConsoleEnabled;
  }

  private void OnGUI()
  {
    if (ApplicationDataManager.BuildType != BuildType.Prod)
    {
      for (int index = 0; index < this._exceptions.Count; ++index)
        GUI.Label(new Rect(0.0f, (float) (GUITools.ScreenHeight - 40 - index * 25), (float) GUITools.ScreenWidth, 20f), this._exceptions[index]);
    }
    if (!this._isDebugConsoleEnabled || DebugConsoleManager._debugPageDescriptors.Length <= 0)
      return;
    GUI.skin = BlueStonez.Skin;
    Rect screenRect = new Rect(20f, (float) Screen.height * 0.2f, (float) (Screen.width - 40), (float) ((double) Screen.height * 0.800000011920929 - 20.0));
    GUILayout.BeginArea(screenRect, BlueStonez.window);
    GUI.Label(new Rect(0.0f, 0.0f, screenRect.width, 23f), "Debug Console", BlueStonez.tab_strip);
    GUILayout.BeginHorizontal();
    GUILayout.FlexibleSpace();
    if (GUILayout.Button("Close", BlueStonez.buttondark_small, GUILayout.Height(20f), GUILayout.Width(64f)))
      this._isDebugConsoleEnabled = false;
    GUILayout.EndHorizontal();
    GUILayout.Space(19f);
    this.DrawDebugMenuGrid();
    GUILayout.Space(2f);
    this.DrawDebugPage();
    GUILayout.EndArea();
  }

  private void DrawDebugMenuGrid()
  {
    int num = GUILayout.SelectionGrid(DebugConsoleManager._currentPageSelectedIdx, DebugConsoleManager._debugPageDescriptors, DebugConsoleManager._debugPageDescriptors.Length, BlueStonez.tab_medium);
    if (num == DebugConsoleManager._currentPageSelectedIdx)
      return;
    int index = Mathf.Clamp(num, 0, DebugConsoleManager._debugPages.Length - 1);
    DebugConsoleManager._currentPageSelectedIdx = index;
    DebugConsoleManager._currentPageSelected = DebugConsoleManager._debugPages[index];
  }

  private void DrawDebugPage()
  {
    this._scrollDebug = GUILayout.BeginScrollView(this._scrollDebug);
    if (DebugConsoleManager._currentPageSelected != null)
      DebugConsoleManager._currentPageSelected.Draw();
    GUILayout.EndScrollView();
  }

  private void UpdatePages(MemberAccessLevel level)
  {
    switch (level)
    {
      case MemberAccessLevel.JuniorModerator:
      case MemberAccessLevel.SeniorModerator:
        DebugConsoleManager._debugPages = new IDebugPage[7]
        {
          (IDebugPage) new DebugLogMessages(),
          (IDebugPage) new DebugApplication(),
          (IDebugPage) new DebugWebServices(),
          (IDebugPage) new DebugNetworkTraffic(),
          (IDebugPage) new DebugGraphics(),
          (IDebugPage) new DebugGameState(),
          (IDebugPage) new DebugProjectiles()
        };
        break;
      case MemberAccessLevel.Admin:
        DebugConsoleManager._debugPages = new IDebugPage[12]
        {
          (IDebugPage) new DebugLogMessages(),
          (IDebugPage) new DebugApplication(),
          (IDebugPage) new DebugWebServices(),
          (IDebugPage) new DebugNetworkTraffic(),
          (IDebugPage) new DebugMaps(),
          (IDebugPage) new DebugProjectiles(),
          (IDebugPage) new DebugConnection(),
          (IDebugPage) new DebugGameServerManager(),
          (IDebugPage) new DebugGameState(),
          (IDebugPage) new DebugPlayerManager(),
          (IDebugPage) new DebugFacebook(),
          (IDebugPage) new DebugShop()
        };
        break;
      default:
        DebugConsoleManager._debugPages = new IDebugPage[2]
        {
          (IDebugPage) new DebugLogMessages(),
          (IDebugPage) new DebugApplication()
        };
        break;
    }
    DebugConsoleManager._debugPageDescriptors = new string[DebugConsoleManager._debugPages.Length];
    for (int index = 0; index < DebugConsoleManager._debugPages.Length; ++index)
      DebugConsoleManager._debugPageDescriptors[index] = DebugConsoleManager._debugPages[index].Title;
    DebugConsoleManager._currentPageSelectedIdx = 0;
    DebugConsoleManager._currentPageSelected = DebugConsoleManager._debugPages[0];
  }

  private void OnUnityDebugCallback(string logString, string stackTrace, LogType logType)
  {
    switch (logType)
    {
      case LogType.Error:
        if (ApplicationDataManager.Config.DebugLevel < DebugLevel.Error)
          break;
        DebugLogMessages.Log(2, logString);
        break;
      case LogType.Assert:
        DebugLogMessages.Log(2, logString);
        break;
      case LogType.Warning:
        if (ApplicationDataManager.Config.DebugLevel < DebugLevel.Warning)
          break;
        DebugLogMessages.Log(1, logString);
        break;
      case LogType.Log:
        if (ApplicationDataManager.Config.DebugLevel < DebugLevel.Debug)
          break;
        DebugLogMessages.Log(0, logString);
        break;
      case LogType.Exception:
        if (logString.Contains("Could not resolve host") || logString.Contains("Failed downloading http://"))
        {
          DebugLogMessages.Console.Log(2, logString + "\n Info: It is likely you have lost connection to the internet, or the url is unreachable.");
          break;
        }
        DebugLogMessages.Console.Log(2, logString);
        DebugConsoleManager.SendExceptionReport(logString, stackTrace);
        if (this._exceptions.Count >= 10)
          break;
        this._exceptions.Add(logString + " " + stackTrace);
        break;
    }
  }

  public static void SendExceptionReport(string logString, string stackTrace, string popupMessage = null)
  {
    Debug.LogError((object) string.Format("Exception: {0}\n{1}", (object) logString, (object) stackTrace));
    if (ApplicationDataManager.IsOnline)
    {
      if (!DebugConsoleManager._isExceptionSent)
      {
        DebugConsoleManager._isExceptionSent = true;
        ApplicationWebServiceClient.RecordException(PlayerDataManager.CmidSecure, ApplicationDataManager.BuildType, ApplicationDataManager.Channel, "4.3.10." + ApplicationDataManager.BuildNumber, logString, stackTrace, DebugLogMessages.Console.ToHTML() + ApplicationDataManager.LocalSystemInfo.ToHTML(), (Action) (() => Debug.Log((object) "SendExceptionReport Called.")), (Action<Exception>) null);
      }
      if (string.IsNullOrEmpty(popupMessage))
        return;
      PopupSystem.ShowMessage("Sorry", popupMessage, PopupSystem.AlertType.OK);
    }
    else
      Debug.LogWarning((object) "SendExceptionReport: You can't send an exception report before the application is authenticated.");
  }
}
