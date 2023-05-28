// Decompiled with JetBrains decompiler
// Type: GameServerView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.Core.Models.Views;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class GameServerView
{
  public ServerLoadData Data = ServerLoadData.Empty;
  private ConnectionAddress _address = ConnectionAddress.Empty;
  private PhotonView _view;

  private GameServerView()
  {
    this._view = new PhotonView();
    this._address.ConnectionString = "0.0.0.0:0";
    this.Region = "Default";
    this.Flag = new DynamicTexture(string.Empty);
  }

  public GameServerView(string address, PhotonUsageType type)
  {
    this._address.ConnectionString = address;
    if (this._address.IsValid)
      this._view = new PhotonView()
      {
        Name = "No Name",
        IP = this._address.ServerIP,
        Port = int.Parse(this._address.ServerPort),
        UsageType = type
      };
    else
      this._view = new PhotonView() { Name = "No Name" };
    this.Region = "Default";
    this.Flag = new DynamicTexture(ApplicationDataManager.BaseImageURL + "Flags/" + this.Region + ".png");
  }

  public GameServerView(PhotonView view)
  {
    this._address.ConnectionString = string.Format("{0}:{1}", (object) view.IP, (object) view.Port);
    this._view = view;
    int num1 = this._view.Name.IndexOf('[');
    int num2 = this._view.Name.IndexOf(']');
    this.Region = num1 < 0 || num2 <= 1 || num2 <= num1 ? "Default" : this._view.Name.Substring(num1 + 1, num2 - num1 - 1);
    this.Flag = new DynamicTexture(ApplicationDataManager.BaseImageURL + "Flags/" + this.Region + ".png");
  }

  public DynamicTexture Flag { get; set; }

  public static GameServerView Empty => new GameServerView();

  public int Id => this._view.PhotonId;

  public string ConnectionString => this._address.ConnectionString;

  public float ServerLoad => (float) Mathf.Min(this.Data.PlayersConnected + this.Data.RoomsCreated, 100) / 100f;

  public int Latency => this.Data.Latency;

  public int MinLatency => this._view.MinLatency;

  public bool IsValid => this.UsageType != PhotonUsageType.None;

  public PhotonUsageType UsageType => this._view.UsageType;

  public string Name => this._view.Name;

  public string Region { get; private set; }

  public override string ToString() => string.Format("Address: {0}\nLatency: {1}\nType: {2}\n{3}", (object) this._address.ConnectionString, (object) this.Latency, (object) this.UsageType, (object) this.Data.ToString());

  internal bool CheckLatency() => this.MinLatency <= 0 || this.MinLatency > this.Latency;
}
