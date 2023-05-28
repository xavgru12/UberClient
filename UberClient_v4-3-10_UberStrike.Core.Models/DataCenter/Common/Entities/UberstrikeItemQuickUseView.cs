// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.UberstrikeItemQuickUseView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

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
