// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.OperationResponse
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System.Collections;
using System.Collections.Generic;

namespace ExitGames.Client.Photon
{
  public class OperationResponse
  {
    public byte OperationCode;
    public short ReturnCode;
    public string DebugMessage;
    public Dictionary<byte, object> Parameters;

    public object this[byte parameterCode]
    {
      get
      {
        object obj;
        this.Parameters.TryGetValue(parameterCode, out obj);
        return obj;
      }
      set => this.Parameters[parameterCode] = value;
    }

    public override string ToString() => string.Format("OperationResponse {0}: ReturnCode: {1}.", (object) this.OperationCode, (object) this.ReturnCode);

    public string ToStringFull() => string.Format("OperationResponse {0}: ReturnCode: {1} ({3}). Parameters: {2}", (object) this.OperationCode, (object) this.ReturnCode, (object) SupportClass.DictionaryToString((IDictionary) this.Parameters), (object) this.DebugMessage);
  }
}
