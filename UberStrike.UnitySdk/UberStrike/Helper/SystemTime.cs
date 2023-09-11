// Decompiled with JetBrains decompiler
// Type: UberStrike.Helper.SystemTime
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;

namespace UberStrike.Helper
{
  public static class SystemTime
  {
    public static int Running => Environment.TickCount & int.MaxValue;
  }
}
