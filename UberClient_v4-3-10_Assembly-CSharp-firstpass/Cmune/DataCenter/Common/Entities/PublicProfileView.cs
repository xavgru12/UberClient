// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.PublicProfileView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class PublicProfileView
  {
    public int Cmid { get; set; }

    public string Name { get; set; }

    public bool IsChatDisabled { get; set; }

    public MemberAccessLevel AccessLevel { get; set; }

    public string GroupTag { get; set; }

    public DateTime LastLoginDate { get; set; }

    public EmailAddressStatus EmailAddressStatus { get; set; }

    public string FacebookId { get; set; }

    public PublicProfileView()
    {
      this.Cmid = 0;
      this.Name = string.Empty;
      this.IsChatDisabled = false;
      this.AccessLevel = MemberAccessLevel.Default;
      this.GroupTag = string.Empty;
      this.LastLoginDate = DateTime.UtcNow;
      this.EmailAddressStatus = EmailAddressStatus.Unverified;
      this.FacebookId = string.Empty;
    }

    public PublicProfileView(
      int cmid,
      string name,
      MemberAccessLevel accesLevel,
      bool isChatDisabled,
      DateTime lastLoginDate,
      EmailAddressStatus emailAddressStatus,
      string facebookId)
    {
      this.SetPublicProfile(cmid, name, accesLevel, isChatDisabled, string.Empty, lastLoginDate, emailAddressStatus, facebookId);
    }

    public PublicProfileView(
      int cmid,
      string name,
      MemberAccessLevel accesLevel,
      bool isChatDisabled,
      string groupTag,
      DateTime lastLoginDate,
      EmailAddressStatus emailAddressStatus,
      string facebookId)
    {
      this.SetPublicProfile(cmid, name, accesLevel, isChatDisabled, groupTag, lastLoginDate, emailAddressStatus, facebookId);
    }

    private void SetPublicProfile(
      int cmid,
      string name,
      MemberAccessLevel accesLevel,
      bool isChatDisabled,
      string groupTag,
      DateTime lastLoginDate,
      EmailAddressStatus emailAddressStatus,
      string facebookId)
    {
      this.Cmid = cmid;
      this.Name = name;
      this.AccessLevel = accesLevel;
      this.IsChatDisabled = isChatDisabled;
      this.GroupTag = groupTag;
      this.LastLoginDate = lastLoginDate;
      this.EmailAddressStatus = emailAddressStatus;
      this.FacebookId = facebookId;
    }

    public override string ToString() => "[Public profile: " + "[Member name:" + this.Name + "][CMID:" + this.Cmid.ToString() + "][Is banned from chat: " + this.IsChatDisabled.ToString() + "]" + "[Access level:" + this.AccessLevel.ToString() + "][Group tag: " + this.GroupTag + "][Last login date: " + this.LastLoginDate.ToString() + "]]" + "[EmailAddressStatus:" + this.EmailAddressStatus.ToString() + "][FacebookId: " + this.FacebookId + "]";
  }
}
