// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.ISynchronizable
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace UberStrike.Core.Models
{
  public interface ISynchronizable
  {
    SortedList<int, object> Changes { get; }
  }
}
