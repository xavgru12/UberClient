
namespace UberStrike.Realtime.Common
{
  public interface IGameMetaData
  {
    int MapID { get; }

    short GameMode { get; }

    int RoundTime { get; }
  }
}
