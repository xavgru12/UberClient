// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ApplicationView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Core.Models.Views;
using System;
using System.Collections.Generic;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class ApplicationView
  {
    public int ApplicationVersionId { get; set; }

    public string Version { get; set; }

    public BuildType Build { get; set; }

    public ChannelType Channel { get; set; }

    public string FileName { get; set; }

    public DateTime ReleaseDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public int RemainingTime { get; set; }

    public bool IsCurrent { get; set; }

    public List<PhotonView> Servers { get; set; }

    public string SupportUrl { get; set; }

    public int PhotonGroupId { get; set; }

    public string PhotonGroupName { get; set; }

    public ApplicationView() => this.Servers = new List<PhotonView>();

    public ApplicationView(string version, BuildType build, ChannelType channel)
    {
      this.Version = version;
      this.Build = build;
      this.Channel = channel;
      this.Servers = new List<PhotonView>();
    }

    public ApplicationView(
      int applicationVersionId,
      string version,
      BuildType build,
      ChannelType channel,
      string fileName,
      DateTime releaseDate,
      DateTime? expirationDate,
      bool isCurrent,
      string supportUrl,
      int photonGroupId,
      List<PhotonView> servers)
    {
      int remainingTime = -1;
      if (expirationDate.HasValue && expirationDate.HasValue)
      {
        DateTime dateTime = expirationDate.Value;
        remainingTime = dateTime.CompareTo(DateTime.Now) > 0 ? (int) Math.Floor(DateTime.Now.Subtract(dateTime).TotalMinutes) : 0;
      }
      this.SetApplication(applicationVersionId, version, build, channel, fileName, releaseDate, expirationDate, remainingTime, isCurrent, supportUrl, photonGroupId, servers);
    }

    private void SetApplication(
      int applicationVersionID,
      string version,
      BuildType build,
      ChannelType channel,
      string fileName,
      DateTime releaseDate,
      DateTime? expirationDate,
      int remainingTime,
      bool isCurrent,
      string supportUrl,
      int photonGroupId,
      List<PhotonView> servers)
    {
      this.ApplicationVersionId = applicationVersionID;
      this.Version = version;
      this.Build = build;
      this.Channel = channel;
      this.FileName = fileName;
      this.ReleaseDate = releaseDate;
      this.ExpirationDate = expirationDate;
      this.RemainingTime = remainingTime;
      this.IsCurrent = isCurrent;
      this.SupportUrl = supportUrl;
      this.PhotonGroupId = photonGroupId;
      if (servers != null)
        this.Servers = servers;
      else
        this.Servers = new List<PhotonView>();
    }

    public override string ToString()
    {
      string str = "[Application: " + "[ID: " + (object) this.ApplicationVersionId + "][version: " + this.Version + "][Build: " + (object) this.Build + "][Channel: " + (object) this.Channel + "][File name: " + this.FileName + "][Release date: " + (object) this.ReleaseDate + "][Expiration date: " + (object) this.ExpirationDate + "][Remaining time: " + (object) this.RemainingTime + "][Is current: " + (object) this.IsCurrent + "][Support URL: " + this.SupportUrl + "]" + "[Servers]";
      foreach (PhotonView server in this.Servers)
        str += server.ToString();
      return str + "[/Servers]]" + "]";
    }
  }
}
