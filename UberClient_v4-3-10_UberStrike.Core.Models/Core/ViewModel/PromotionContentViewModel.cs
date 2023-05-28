// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.ViewModel.PromotionContentViewModel
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

using System;
using System.Collections.Generic;

namespace UberStrike.Core.ViewModel
{
  public class PromotionContentViewModel
  {
    public int PromotionContentId { get; set; }

    public string Name { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsPermanent { get; set; }

    public List<PromotionContentElementViewModel> PromotionContentElements { get; set; }
  }
}
