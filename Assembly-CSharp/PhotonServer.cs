using Cmune.Core.Models;
using Cmune.Core.Models.Views;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models;
using UnityEngine;

public class PhotonServer
{
	public PhotonServerLoad Data = new PhotonServerLoad();

	private ConnectionAddress _address = new ConnectionAddress();

	private PhotonView _view;

	public DynamicTexture Flag
	{
		get;
		set;
	}

	public static PhotonServer Empty => new PhotonServer();

	public int Id => _view.PhotonId;

	public string ConnectionString
	{
		get
		{
			return _address.ConnectionString;
		}
		set
		{
			_address = new ConnectionAddress(value);
		}
	}

	public float ServerLoad => (float)Mathf.Min(Data.PlayersConnected + Data.RoomsCreated, 100) / 100f;

	public int Latency => Data.Latency;

	public int MinLatency => _view.MinLatency;

	public bool IsValid => UsageType != PhotonUsageType.None;

	public PhotonUsageType UsageType => _view.UsageType;

	public string Name => _view.Name;

	public string Region
	{
		get;
		private set;
	}

	private PhotonServer()
	{
		_view = new PhotonView();
		Region = "Default";
		Flag = new DynamicTexture(string.Empty);
	}

	public PhotonServer(string address, PhotonUsageType type)
	{
		_address = new ConnectionAddress(address);
		_view = new PhotonView
		{
			Name = "No Name",
			IP = _address.IpAddress,
			Port = _address.Port,
			UsageType = type
		};
		Region = "Default";
		Flag = new DynamicTexture(ApplicationDataManager.ImagePath + "flags/" + Region + ".png", Region != "Default");
	}

	public PhotonServer(PhotonView view)
	{
		_address.Ipv4 = ConnectionAddress.ToInteger(view.IP);
		_address.Port = (ushort)view.Port;
		_view = view;
		int num = (!string.IsNullOrEmpty(_view.Name)) ? _view.Name.IndexOf('[') : 0;
		int num2 = (!string.IsNullOrEmpty(_view.Name)) ? _view.Name.IndexOf(']') : 0;
		if (num >= 0 && num2 > 1 && num2 > num)
		{
			Region = _view.Name.Substring(num + 1, num2 - num - 1);
		}
		else
		{
			Region = "Default";
		}
		Flag = new DynamicTexture(ApplicationDataManager.ImagePath + "flags/" + Region + ".png", Region != "Default");
	}

	public override string ToString()
	{
		return $"Address: {_address.ConnectionString}\nLatency: {Latency}\nType: {UsageType}\n{Data.ToString()}";
	}

	internal bool CheckLatency()
	{
		if (MinLatency > 0)
		{
			return MinLatency > Latency;
		}
		return true;
	}
}
