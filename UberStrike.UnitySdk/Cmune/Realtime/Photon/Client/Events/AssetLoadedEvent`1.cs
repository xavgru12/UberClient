
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
