// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.PlayerTagFlag
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;

namespace Cmune.Realtime.Common
{
  [Flags]
  public enum PlayerTagFlag
  {
    None = 0,
    Speedhacker = 1,
    Ammohacker = 2,
    ReportedCheater = 4,
    ReportedAbuser = 8,
  }
}
