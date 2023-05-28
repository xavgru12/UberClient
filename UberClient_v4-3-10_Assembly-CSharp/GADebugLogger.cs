// Decompiled with JetBrains decompiler
// Type: GADebugLogger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.IO;
using UnityEngine;

public class GADebugLogger : IGALogger
{
  private string _pathToLogFile;

  public GADebugLogger()
  {
    string str = !Application.isEditor ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : Application.dataPath;
    if (!str.EndsWith("Documents"))
      str = Path.Combine(str, "Documents");
    if (Application.isEditor && !Directory.Exists(str))
      Directory.CreateDirectory(str);
    this._pathToLogFile = Path.Combine(str, "GALog.txt");
  }

  public void log(string data)
  {
    using (StreamWriter streamWriter = new StreamWriter(this._pathToLogFile, true))
      streamWriter.Write(string.Format("{0} {1}\r\n", (object) DateTime.Now, (object) data));
  }

  public void logStartRequest(string url) => this.log(string.Format("[started] {0}", (object) url));

  public void logSuccessfulRequest(string url) => this.log(string.Format("[succeeded] {0}", (object) url));

  public void logFailedRequest(string url, string error) => this.log(string.Format("[failed] {0}, {1}", (object) url, (object) error));
}
