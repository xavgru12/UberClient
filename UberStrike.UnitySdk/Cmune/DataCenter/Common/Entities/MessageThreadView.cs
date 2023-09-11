// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.MessageThreadView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
