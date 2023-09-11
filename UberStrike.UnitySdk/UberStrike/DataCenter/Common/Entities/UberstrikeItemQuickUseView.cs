// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.UberstrikeItemQuickUseView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
