
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
