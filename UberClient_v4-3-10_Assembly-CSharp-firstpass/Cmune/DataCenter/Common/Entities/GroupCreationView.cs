// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.GroupCreationView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class GroupCreationView
  {
    public string Name { get; set; }

    public string Description { get; set; }

    public string Motto { get; set; }

    public string Address { get; set; }

    public bool HasPicture { get; set; }

    public int ApplicationId { get; set; }

    public string AuthToken { get; set; }

    public string Tag { get; set; }

    public string Locale { get; set; }

    public GroupCreationView()
    {
    }

    public GroupCreationView(
      string name,
      string description,
      string motto,
      string address,
      bool hasPicture,
      int applicationId,
      string authToken,
      string tag,
      string locale)
    {
      this.Name = name;
      this.Description = description;
      this.Motto = motto;
      this.Address = address;
      this.HasPicture = hasPicture;
      this.ApplicationId = applicationId;
      this.AuthToken = authToken;
      this.Tag = tag;
      this.Locale = locale;
    }

    public GroupCreationView(
      string name,
      string motto,
      int applicationId,
      string authToken,
      string tag,
      string locale)
    {
      this.Name = name;
      this.Description = string.Empty;
      this.Motto = motto;
      this.Address = string.Empty;
      this.HasPicture = false;
      this.ApplicationId = applicationId;
      this.AuthToken = authToken;
      this.Tag = tag;
      this.Locale = locale;
    }

    public override string ToString() => "[GroupCreationView: [name:" + this.Name + "][description:" + this.Description + "][Motto:" + this.Motto + "]" + "[Address:" + this.Address + "][Has picture:" + this.HasPicture.ToString() + "][Application Id:" + this.ApplicationId.ToString() + "][AuthToken:" + this.AuthToken + "]" + "[Tag:" + this.Tag + "][Locale:" + this.Locale + "]";
  }
}
