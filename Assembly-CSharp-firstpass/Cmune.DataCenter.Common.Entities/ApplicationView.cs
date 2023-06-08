using Cmune.Core.Models.Views;
using System;
using System.Collections.Generic;

namespace Cmune.DataCenter.Common.Entities
{
	[Serializable]
	public class ApplicationView
	{
		public int ApplicationVersionId
		{
			get;
			set;
		}

		public string Version
		{
			get;
			set;
		}

		public BuildType Build
		{
			get;
			set;
		}

		public ChannelType Channel
		{
			get;
			set;
		}

		public string FileName
		{
			get;
			set;
		}

		public DateTime ReleaseDate
		{
			get;
			set;
		}

		public DateTime? ExpirationDate
		{
			get;
			set;
		}

		public int RemainingTime
		{
			get;
			set;
		}

		public bool IsCurrent
		{
			get;
			set;
		}

		public List<PhotonView> Servers
		{
			get;
			set;
		}

		public string SupportUrl
		{
			get;
			set;
		}

		public int PhotonGroupId
		{
			get;
			set;
		}

		public string PhotonGroupName
		{
			get;
			set;
		}

		public ApplicationView()
		{
			Servers = new List<PhotonView>();
		}

		public ApplicationView(string version, BuildType build, ChannelType channel)
		{
			Version = version;
			Build = build;
			Channel = channel;
			Servers = new List<PhotonView>();
		}

		public ApplicationView(int applicationVersionId, string version, BuildType build, ChannelType channel, string fileName, DateTime releaseDate, DateTime? expirationDate, bool isCurrent, string supportUrl, int photonGroupId, List<PhotonView> servers)
		{
			int remainingTime = -1;
			if (expirationDate.HasValue && expirationDate.HasValue)
			{
				DateTime value = expirationDate.Value;
				remainingTime = ((value.CompareTo(DateTime.UtcNow) > 0) ? ((int)Math.Floor(DateTime.UtcNow.Subtract(value).TotalMinutes)) : 0);
			}
			SetApplication(applicationVersionId, version, build, channel, fileName, releaseDate, expirationDate, remainingTime, isCurrent, supportUrl, photonGroupId, servers);
		}

		private void SetApplication(int applicationVersionID, string version, BuildType build, ChannelType channel, string fileName, DateTime releaseDate, DateTime? expirationDate, int remainingTime, bool isCurrent, string supportUrl, int photonGroupId, List<PhotonView> servers)
		{
			ApplicationVersionId = applicationVersionID;
			Version = version;
			Build = build;
			Channel = channel;
			FileName = fileName;
			ReleaseDate = releaseDate;
			ExpirationDate = expirationDate;
			RemainingTime = remainingTime;
			IsCurrent = isCurrent;
			SupportUrl = supportUrl;
			PhotonGroupId = photonGroupId;
			if (servers != null)
			{
				Servers = servers;
			}
			else
			{
				Servers = new List<PhotonView>();
			}
		}

		public override string ToString()
		{
			string text = "[Application: ";
			string text2 = text;
			text = text2 + "[ID: " + ApplicationVersionId.ToString() + "][version: " + Version + "][Build: " + Build.ToString() + "][Channel: " + Channel.ToString() + "][File name: " + FileName + "][Release date: " + ReleaseDate.ToString() + "][Expiration date: " + ExpirationDate.ToString() + "][Remaining time: " + RemainingTime.ToString() + "][Is current: " + IsCurrent.ToString() + "][Support URL: " + SupportUrl + "]";
			text += "[Servers]";
			foreach (PhotonView server in Servers)
			{
				text += server.ToString();
			}
			text += "[/Servers]]";
			return text + "]";
		}
	}
}
