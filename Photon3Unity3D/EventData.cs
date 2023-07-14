﻿// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.EventData
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System.Collections;
using System.Collections.Generic;

namespace ExitGames.Client.Photon
{
  public class EventData
  {
    public byte Code;
    public Dictionary<byte, object> Parameters;

    public object this[byte key]
    {
      get
      {
        object obj;
        this.Parameters.TryGetValue(key, out obj);
        return obj;
      }
      set => this.Parameters[key] = value;
    }

    public override string ToString() => string.Format("Event {0}.", (object) this.Code.ToString());

    public string ToStringFull() => string.Format("Event {0}: {1}", (object) this.Code, (object) SupportClass.DictionaryToString((IDictionary) this.Parameters));
  }
}
