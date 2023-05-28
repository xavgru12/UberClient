// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.ViewModel.PromotionContentElementViewModel
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
