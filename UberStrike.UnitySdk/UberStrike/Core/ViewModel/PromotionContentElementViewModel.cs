// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.ViewModel.PromotionContentElementViewModel
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Types;

namespace UberStrike.Core.ViewModel
{
  public class PromotionContentElementViewModel
  {
    public int PromotionContentElementId { get; set; }

    public ChannelType ChannelType { get; set; }

    public ChannelElement ChannelElement { get; set; }

    public string Filename { get; set; }

    public string FilenameTitle { get; set; }

    public int PromotionContentId { get; set; }

    public string AnchorLink { get; set; }
  }
}
