// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.ViewModel.PromotionContentElementViewModel
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

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
