// Decompiled with JetBrains decompiler
// Type: DebugNetworkMessaging
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class DebugNetworkMessaging : IDebugPage
{
  private Vector2 v1;
  private Queue _outMessageQueue;
  private int _outMaxPackPerSec;
  private float _outAvgPackPerSec;
  private int _outTotalPackages;
  private Queue _inMessageQueue;
  private int _inMaxPackPerSec;
  private float _inAvgPackPerSec;
  private int _inTotalPackages;

  public DebugNetworkMessaging()
  {
    this._outMessageQueue = new Queue();
    this._inMessageQueue = new Queue();
  }

  public string Title => "Traffic";

  [DebuggerHidden]
  private IEnumerator countIncomingMessages() => (IEnumerator) new DebugNetworkMessaging.\u003CcountIncomingMessages\u003Ec__IteratorC()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator countOutgoingMessages() => (IEnumerator) new DebugNetworkMessaging.\u003CcountOutgoingMessages\u003Ec__IteratorD()
  {
    \u003C\u003Ef__this = this
  };

  public void Draw()
  {
    GUILayout.Label("OUT (tot):" + (object) this._outTotalPackages);
    GUILayout.Label("OUT (avg):" + (object) this._outAvgPackPerSec);
    GUILayout.Label("OUT (max):" + (object) this._outMaxPackPerSec);
    GUILayout.Label("IN (tot):" + (object) this._inTotalPackages);
    GUILayout.Label("IN (avg):" + (object) this._inAvgPackPerSec);
    GUILayout.Label("IN (max):" + (object) this._inMaxPackPerSec);
    if (GUILayout.Button("Dump To File"))
    {
      FileStream fileStream = new FileStream("NetworkMessages.txt", FileMode.OpenOrCreate, FileAccess.Write);
      StreamWriter streamWriter = new StreamWriter((Stream) fileStream);
      try
      {
        foreach (KeyValuePair<short, NetworkMessenger.NetworkClassInfo> callStatistic in (Dictionary<short, NetworkMessenger.NetworkClassInfo>) LobbyConnectionManager.Rmi.Messenger.CallStatistics)
        {
          foreach (KeyValuePair<byte, int> functionCall in (Dictionary<byte, int>) callStatistic.Value._functionCalls)
          {
            string str = string.Format("{0}\t{1}\t{2}\t{3}", (object) functionCall.Value, (object) LobbyConnectionManager.Rmi.GetAddress(callStatistic.Key, functionCall.Key), (object) callStatistic.Value.GetTotalExecutionTime(functionCall.Key), (object) callStatistic.Value.GetAvarageExecutionTime(functionCall.Key));
            streamWriter.WriteLine(str);
          }
        }
      }
      finally
      {
        streamWriter.Close();
        fileStream.Close();
      }
    }
    this.v1 = GUILayout.BeginScrollView(this.v1);
    foreach (KeyValuePair<short, NetworkMessenger.NetworkClassInfo> callStatistic in (Dictionary<short, NetworkMessenger.NetworkClassInfo>) LobbyConnectionManager.Rmi.Messenger.CallStatistics)
    {
      foreach (KeyValuePair<byte, int> functionCall in (Dictionary<byte, int>) callStatistic.Value._functionCalls)
        GUILayout.Label(string.Format("{0} {1}: [{2}ms /{3}ms]", (object) functionCall.Value, (object) LobbyConnectionManager.Rmi.GetAddress(callStatistic.Key, functionCall.Key), (object) callStatistic.Value.GetTotalExecutionTime(functionCall.Key), (object) callStatistic.Value.GetAvarageExecutionTime(functionCall.Key)));
    }
    GUILayout.EndScrollView();
  }

  private class MessageInfo
  {
    public DateTime timestamp;
    public int number;

    public MessageInfo(int num)
    {
      this.number = num;
      this.timestamp = DateTime.Now;
    }

    public override string ToString() => this.timestamp.ToLongTimeString() + " Messages " + this.number.ToString();
  }
}
