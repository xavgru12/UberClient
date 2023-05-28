// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.UberstrikeItemQuickUseView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Types;

namespace UberStrike.DataCenter.Common.Entities
{
  public class UberstrikeItemQuickUseView : UberstrikeItemView
  {
    public ItemQuickUseConfigView Config { get; set; }

    public QuickItemLogic Logic { get; set; }

    public UberstrikeItemQuickUseView()
    {
    }

    public UberstrikeItemQuickUseView(ItemView item, int levelRequired)
      : base(item, levelRequired)
    {
    }

    public UberstrikeItemQuickUseView(
      ItemView item,
      int levelRequired,
      ItemQuickUseConfigView Config)
      : base(item, levelRequired)
    {
      this.Config = Config;
    }
  }
}
