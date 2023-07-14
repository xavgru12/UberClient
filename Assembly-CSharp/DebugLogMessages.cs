// Decompiled with JetBrains decompiler
// Type: DebugLogMessages
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Text;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class DebugLogMessages : IDebugPage
{
  public static readonly DebugLogMessages.ConsoleDebug Console = new DebugLogMessages.ConsoleDebug();

  public string Title => "Logs";

  public void Draw() => GUILayout.TextArea(DebugLogMessages.Console.DebugOut);

  public static void Log(int type, string msg) => DebugLogMessages.Console.Log(type, msg);

  public class ConsoleDebug
  {
    private LimitedQueue<string> _queue = new LimitedQueue<string>(100);
    private string _debugOut = string.Empty;

    public void Log(int level, string s)
    {
      this._queue.Enqueue(s);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string str in this._queue)
        stringBuilder.AppendLine(str);
      this._debugOut = stringBuilder.ToString();
    }

    public string DebugOut => this._debugOut;

    public string ToHTML()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("<h3>DEBUG LOG</h3>");
      foreach (string str in this._queue)
        stringBuilder.AppendLine(str + "<br/>");
      return stringBuilder.ToString();
    }
  }
}
