// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.Lite.EventCaching
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

namespace ExitGames.Client.Photon.Lite
{
  public enum EventCaching : byte
  {
    DoNotCache,
    MergeCache,
    ReplaceCache,
    RemoveCache,
    AddToRoomCache,
    AddToRoomCacheGlobal,
    RemoveFromRoomCache,
    RemoveFromRoomCacheForActorsLeft,
  }
}
