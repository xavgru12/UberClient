// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.MemberView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.Collections.Generic;
using System.Text;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class MemberView
  {
    public PublicProfileView PublicProfile { get; set; }

    public MemberWalletView MemberWallet { get; set; }

    public List<int> MemberItems { get; set; }

    public MemberView()
    {
      this.PublicProfile = new PublicProfileView();
      this.MemberWallet = new MemberWalletView();
      this.MemberItems = new List<int>(0);
    }

    public MemberView(
      PublicProfileView publicProfile,
      MemberWalletView memberWallet,
      List<int> memberItems)
    {
      this.PublicProfile = publicProfile;
      this.MemberWallet = memberWallet;
      this.MemberItems = memberItems;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder("[Member view: ");
      if (this.PublicProfile != null && this.MemberWallet != null)
      {
        stringBuilder.Append((object) this.PublicProfile);
        stringBuilder.Append((object) this.MemberWallet);
        stringBuilder.Append("[items: ");
        if (this.MemberItems != null && this.MemberItems.Count > 0)
        {
          int count = this.MemberItems.Count;
          foreach (int memberItem in this.MemberItems)
          {
            stringBuilder.Append(memberItem);
            if (--count > 0)
              stringBuilder.Append(", ");
          }
        }
        else
          stringBuilder.Append("No items");
        stringBuilder.Append("]");
      }
      else
        stringBuilder.Append("No member");
      stringBuilder.Append("]");
      return stringBuilder.ToString();
    }
  }
}
