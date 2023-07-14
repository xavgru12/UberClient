// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.TrafficStatsGameLevel
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System;

namespace ExitGames.Client.Photon
{
  public class TrafficStatsGameLevel
  {
    private int timeOfLastDispatchCall;
    private int timeOfLastSendCall;

    public int OperationByteCount { get; internal set; }

    public int OperationCount { get; internal set; }

    public int ResultByteCount { get; internal set; }

    public int ResultCount { get; internal set; }

    public int EventByteCount { get; internal set; }

    public int EventCount { get; internal set; }

    public int LongestOpResponseCallback { get; internal set; }

    public byte LongestOpResponseCallbackOpCode { get; internal set; }

    public int LongestEventCallback { get; internal set; }

    public byte LongestEventCallbackCode { get; internal set; }

    public int LongestDeltaBetweenDispatching { get; internal set; }

    public int LongestDeltaBetweenSending { get; internal set; }

    public int DispatchCalls { get; internal set; }

    public int SendOutgoingCommandsCalls { get; internal set; }

    public int TotalByteCount => this.OperationByteCount + this.ResultByteCount + this.EventByteCount;

    public int TotalMessageCount => this.OperationCount + this.ResultCount + this.EventCount;

    public int TotalIncomingByteCount => this.ResultByteCount + this.EventByteCount;

    public int TotalIncomingMessageCount => this.ResultCount + this.EventCount;

    public int TotalOutgoingByteCount => this.OperationByteCount;

    public int TotalOutgoingMessageCount => this.OperationCount;

    internal void CountOperation(int operationBytes)
    {
      this.OperationByteCount += operationBytes;
      ++this.OperationCount;
    }

    internal void CountResult(int resultBytes)
    {
      this.ResultByteCount += resultBytes;
      ++this.ResultCount;
    }

    internal void CountEvent(int eventBytes)
    {
      this.EventByteCount += eventBytes;
      ++this.EventCount;
    }

    internal void TimeForResponseCallback(byte code, int time)
    {
      if (time <= this.LongestOpResponseCallback)
        return;
      this.LongestOpResponseCallback = time;
      this.LongestOpResponseCallbackOpCode = code;
    }

    internal void TimeForEventCallback(byte code, int time)
    {
      if (time <= this.LongestOpResponseCallback)
        return;
      this.LongestEventCallback = time;
      this.LongestEventCallbackCode = code;
    }

    internal void DispatchIncomingCommandsCalled()
    {
      if (this.timeOfLastDispatchCall != 0)
      {
        int num = Environment.TickCount - this.timeOfLastDispatchCall;
        if (num > this.LongestDeltaBetweenDispatching)
          this.LongestDeltaBetweenDispatching = num;
      }
      ++this.DispatchCalls;
      this.timeOfLastDispatchCall = Environment.TickCount;
    }

    internal void SendOutgoingCommandsCalled()
    {
      if (this.timeOfLastSendCall != 0)
      {
        int num = Environment.TickCount - this.timeOfLastSendCall;
        if (num > this.LongestDeltaBetweenSending)
          this.LongestDeltaBetweenSending = num;
      }
      ++this.SendOutgoingCommandsCalls;
      this.timeOfLastSendCall = Environment.TickCount;
    }

    public override string ToString() => string.Format("OperationByteCount: {0} ResultByteCount: {1} EventByteCount: {2}", (object) this.OperationByteCount, (object) this.ResultByteCount, (object) this.EventByteCount);

    public string ToStringVitalStats() => string.Format("Longest delta between Send: {0}ms Dispatch: {1}ms. Longest callback OnEv: {3}={2}ms OnResp: {5}={4}ms. Calls of Send: {6} Dispatch: {7}.", (object) this.LongestDeltaBetweenSending, (object) this.LongestDeltaBetweenDispatching, (object) this.LongestEventCallback, (object) this.LongestEventCallbackCode, (object) this.LongestOpResponseCallback, (object) this.LongestOpResponseCallbackOpCode, (object) this.SendOutgoingCommandsCalls, (object) this.DispatchCalls);
  }
}
