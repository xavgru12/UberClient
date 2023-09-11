
using System.Collections.Generic;

namespace Cmune.Realtime.Common.Utils
{
  public class PlayerIdCache
  {
    private List<int> _allPlayerIDs = new List<int>();
    private Dictionary<int, int[]> _playerIdCache = new Dictionary<int, int[]>();

    public void Add(int playerId)
    {
      if (this._allPlayerIDs.Contains(playerId))
        return;
      this._allPlayerIDs.Add(playerId);
      this.updateCache();
    }

    public void Remove(int playerId)
    {
      if (!this._allPlayerIDs.Remove(playerId))
        return;
      this.updateCache();
    }

    public int[] GetAll()
    {
      int[] numArray;
      return this._playerIdCache.TryGetValue(0, out numArray) ? numArray : new int[0];
    }

    public int[] GetOthers(int playerId)
    {
      int[] numArray;
      return this._playerIdCache.TryGetValue(playerId, out numArray) ? numArray : new int[0];
    }

    private void updateCache()
    {
      this._playerIdCache.Clear();
      int[] array = this._allPlayerIDs.ToArray();
      this._playerIdCache[0] = array;
      foreach (int key in array)
      {
        this._allPlayerIDs.Remove(key);
        this._playerIdCache[key] = this._allPlayerIDs.ToArray();
        this._allPlayerIDs.Add(key);
      }
    }
  }
}
