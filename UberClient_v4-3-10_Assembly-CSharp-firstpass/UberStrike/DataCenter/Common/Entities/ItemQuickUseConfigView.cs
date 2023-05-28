// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.ItemQuickUseConfigView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UberStrike.Core.Types;

namespace UberStrike.DataCenter.Common.Entities
{
  [Serializable]
  public class ItemQuickUseConfigView
  {
    public int ItemId { get; set; }

    public int LevelRequired { get; set; }

    public int UsesPerLife { get; set; }

    public int UsesPerRound { get; set; }

    public int UsesPerGame { get; set; }

    public int CoolDownTime { get; set; }

    public int WarmUpTime { get; set; }

    public QuickItemLogic BehaviourType { get; set; }
  }
}
