
using Cmune.Core.Types;
using Cmune.Realtime.Common;
using Cmune.Realtime.Common.Synchronization;
using System.Collections.Generic;

public abstract class CmuneDeltaSync
{
  public readonly Dictionary<int, int> Cache = new Dictionary<int, int>();

  protected CmuneDeltaSync() => this.VersionID = 1U;

  public void ReadSyncData(SyncObject obj) => SyncObjectBuilder.ReadSyncData(obj, true, -1, this);

  public void ReadSyncData(SyncObject obj, int syncMask) => SyncObjectBuilder.ReadSyncData(obj, true, syncMask, this);

  public void ReadSyncData(SyncObject obj, bool updateCache) => SyncObjectBuilder.ReadSyncData(obj, updateCache, -1, this);

  public void IncrementVersion() => ++this.VersionID;

  public uint VersionID { get; internal set; }

  public int InstanceId { get; internal set; }

  public class FieldTag : ExtendableEnum<int>
  {
  }
}
