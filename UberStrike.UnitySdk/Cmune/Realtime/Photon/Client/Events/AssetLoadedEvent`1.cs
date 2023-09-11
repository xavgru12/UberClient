// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Photon.Client.Events.AssetLoadedEvent`1
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

namespace Cmune.Realtime.Photon.Client.Events
{
  public class AssetLoadedEvent<Type>
  {
    protected Type _asset;
    protected int _instanceID;

    public Type Asset => this._asset;

    public int InstanceID => this._instanceID;

    public AssetLoadedEvent(int instanceID, Type asset)
    {
      this._instanceID = instanceID;
      this._asset = asset;
    }
  }
}
