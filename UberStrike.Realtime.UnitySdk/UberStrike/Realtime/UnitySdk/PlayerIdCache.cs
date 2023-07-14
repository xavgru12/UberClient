// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.PlayerIdCache
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
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
