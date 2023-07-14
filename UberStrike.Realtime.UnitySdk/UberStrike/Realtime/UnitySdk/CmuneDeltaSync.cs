// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.CmuneDeltaSync
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
{
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
}
