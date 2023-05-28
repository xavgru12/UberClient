// Decompiled with JetBrains decompiler
// Type: Google
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public sealed class Google : AutoMonoBehaviour<Google>
{
  public const string trackingCode = "UA-5221837-14";
  public const string domain = "ga-events.uberstrike.com";
  public const float wwwTimeout = 15f;
  private const string _baseURL = "http://www.google-analytics.com/__utm.gif?";
  private const string _archivedURLSeperator = "||~||";
  private const string _unfinishedRequestsPrefsKey = "_GAUnfinishedRequests";
  private const string _persistancePrefsKey = "_GAPlayerMemory";
  private const string _visitCountPrefsKey = "_GAVisitCount";
  public bool rememberPlayerBetweenSessions = true;
  public bool debug;
  private IGALogger _logger;
  private Queue<string> _requestQueue = new Queue<string>();
  private bool _queueIsProcessing;
  private int _domainHash;

  private double _epochTime => (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

  private void Awake()
  {
    if ("UA-5221837-14".Length == 0 || "ga-events.uberstrike.com".Length == 0)
    {
      UnityEngine.Debug.LogError((object) "Please enter your tracking code and domain in the prefab!");
    }
    else
    {
      this._logger = Application.isWebPlayer || !this.debug ? (IGALogger) new GAEmptyLogger() : (IGALogger) new GADebugLogger();
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
      this.calculateDomainHash();
      this.resurrectQueue();
    }
  }

  private void OnApplicationPause(bool didPause)
  {
    if (didPause)
      this.persistQueue();
    else
      this.resurrectQueue();
  }

  public void OnApplicationExit() => this.persistQueue();

  public void logPageView() => this.logPageView(Application.loadedLevelName);

  public void logPageView(string page) => this.logPageView(page, page);

  public void logPageView(string page, string pageTitle) => this.sendRequest(page, pageTitle, (string) null, (string) null, (string) null, new int?());

  public void logEvent(string page, string category, string action) => this.logEvent(page, page, category, action);

  public void logEvent(string page, string pageTitle, string category, string action) => this.sendRequest(page, pageTitle, category, action, (string) null, new int?());

  public void logEvent(string page, string category, string action, string label, int value) => this.logEvent(page, page, category, action, label, value);

  public void logEvent(
    string page,
    string pageTitle,
    string category,
    string action,
    string label,
    int value)
  {
    this.sendRequest(page, pageTitle, category, action, label, new int?(value));
  }

  private void sendRequest(
    string page,
    string pageTitle,
    string category,
    string action,
    string label,
    int? value)
  {
    if (!page.StartsWith("/"))
      page = "/" + page;
    System.Random random = new System.Random();
    int epochTime = (int) this._epochTime;
    int num = PlayerPrefs.GetInt("_GAVisitCount", 0) + 1;
    PlayerPrefs.SetInt("_GAVisitCount", num);
    string empty = string.Empty;
    string str1;
    if (this.rememberPlayerBetweenSessions)
    {
      string str2 = PlayerPrefs.GetString("_GAPlayerMemory");
      if (str2.Length == 0)
      {
        str2 = string.Format("{0}.{1}.{2}.{3}.{4}.", (object) this._domainHash, (object) random.Next(1000000000), (object) epochTime, (object) epochTime, (object) epochTime);
        PlayerPrefs.SetString("_GAPlayerMemory", str2);
      }
      str1 = str2 + num.ToString();
    }
    else
      str1 = string.Format("{0}.{1}.{2}.{3}.{4}.{5}", (object) this._domainHash, (object) random.Next(1000000000), (object) epochTime, (object) epochTime, (object) epochTime, (object) num);
    string str3 = string.Format("{0}.{1}.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", (object) this._domainHash, (object) epochTime);
    string str4 = WWW.EscapeURL(string.Format("__utma={0};+__utmz={1};", (object) str1, (object) str3)).Replace("|", "%7C");
    Dictionary<string, string> dictionary = new Dictionary<string, string>()
    {
      {
        "utmwv",
        "4.8.8"
      },
      {
        "utmn",
        random.Next(1000000000).ToString()
      },
      {
        "utmhn",
        WWW.EscapeURL("ga-events.uberstrike.com")
      },
      {
        "utmcs",
        "UTF-8"
      },
      {
        "utmsr",
        string.Format("{0}x{1}", (object) Screen.currentResolution.width, (object) Screen.currentResolution.height)
      },
      {
        "utmsc",
        "24-bit"
      },
      {
        "utmul",
        "en-us"
      },
      {
        "utmje",
        "0"
      },
      {
        "utmfl",
        "-"
      },
      {
        "utmdt",
        WWW.EscapeURL(pageTitle)
      },
      {
        "utmhid",
        random.Next(1000000000).ToString()
      },
      {
        "utmr",
        "-"
      },
      {
        "utmp",
        WWW.EscapeURL(page)
      },
      {
        "utmac",
        "UA-5221837-14"
      },
      {
        "utmcc",
        str4
      }
    };
    if (category != null && action != null && category.Length > 0 && action.Length > 0)
    {
      string str5 = string.Format("5({0}*{1}", (object) category, (object) action);
      string s = label == null || !value.HasValue || label.Length <= 0 ? str5 + ")" : str5 + string.Format("*{0})({1})", (object) label, (object) value.ToString());
      dictionary.Add("utme", WWW.EscapeURL(s));
      dictionary.Add("utmt", "event");
    }
    StringBuilder stringBuilder = new StringBuilder();
    foreach (string key in dictionary.Keys)
      stringBuilder.AppendFormat("{0}={1}&", (object) key, (object) dictionary[key]);
    stringBuilder.Remove(stringBuilder.Length - 1, 1);
    this._requestQueue.Enqueue("http://www.google-analytics.com/__utm.gif?" + stringBuilder.ToString());
    this.StartCoroutine(this.processRequestQueue());
  }

  [DebuggerHidden]
  private IEnumerator waitForRequest(WWW www) => (IEnumerator) new Google.\u003CwaitForRequest\u003Ec__Iterator43()
  {
    www = www,
    \u003C\u0024\u003Ewww = www
  };

  [DebuggerHidden]
  private IEnumerator processRequestQueue() => (IEnumerator) new Google.\u003CprocessRequestQueue\u003Ec__Iterator44()
  {
    \u003C\u003Ef__this = this
  };

  private void persistQueue()
  {
    if (this._requestQueue.Count <= 0)
      return;
    this._logger.log(string.Format("[Saving {0} unsent requests]", (object) this._requestQueue.Count.ToString()));
    string[] array = new string[this._requestQueue.Count];
    this._requestQueue.CopyTo(array, 0);
    PlayerPrefs.SetString("_GAUnfinishedRequests", string.Join("||~||", array));
    this._requestQueue.Clear();
  }

  private void resurrectQueue()
  {
    string str1 = PlayerPrefs.GetString("_GAUnfinishedRequests");
    if (!(str1 != string.Empty))
      return;
    string str2 = str1;
    string[] separator = new string[1]{ "||~||" };
    foreach (string str3 in str2.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      this._requestQueue.Enqueue(str3);
    PlayerPrefs.SetString("_GAUnfinishedRequests", string.Empty);
    this._logger.log(string.Format("[Resurrected {0} unsent requests]", (object) this._requestQueue.Count.ToString()));
    this.StartCoroutine(this.processRequestQueue());
  }

  private void calculateDomainHash()
  {
    int num1 = 0;
    for (int startIndex = "ga-events.uberstrike.com".Length - 1; startIndex >= 0; --startIndex)
    {
      int num2 = (int) char.Parse("ga-events.uberstrike.com".Substring(startIndex, 1));
      int num3 = (num1 << 6 & 268435455) + num2 + (num2 << 14);
      int num4 = num3 & 266338304;
      num1 = num4 == 0 ? num3 : num3 ^ num4 >> 21;
    }
    this._domainHash = num1;
  }
}
