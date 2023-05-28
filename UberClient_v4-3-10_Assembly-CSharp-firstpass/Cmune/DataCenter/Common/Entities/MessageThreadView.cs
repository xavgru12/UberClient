// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.MessageThreadView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class MessageThreadView
  {
    public int ThreadId { get; set; }

    public string ThreadName { get; set; }

    public bool HasNewMessages { get; set; }

    public int MessageCount { get; set; }

    public string LastMessagePreview { get; set; }

    public DateTime LastUpdate { get; set; }
  }
}
