
namespace Cmune.Realtime.Common
{
  public interface IRoomMetaData
  {
    CmuneRoomID RoomID { get; }

    string RoomName { get; }

    string Password { get; }

    bool IsPublic { get; }

    int ConnectedPlayers { get; set; }

    int MaxPlayers { get; }

    string ServerConnection { get; }
  }
}
