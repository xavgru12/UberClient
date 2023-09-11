// Decompiled with JetBrains decompiler
// Type: Cmune.Util.ConvertBytes
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;

namespace Cmune.Util
{
  public static class ConvertBytes
  {
    private const double toKilo = 0.0009765625;

    public static float ToKiloBytes(ulong bytes) => Convert.ToSingle((double) bytes * 0.0009765625);

    public static float ToKiloBytes(int bytes) => Convert.ToSingle((double) bytes * 0.0009765625);

    public static float ToKiloBytes(long bytes) => Convert.ToSingle((double) bytes * 0.0009765625);

    public static float ToMegaBytes(ulong bytes) => Convert.ToSingle((double) bytes * 0.0009765625 * 0.0009765625);

    public static float ToMegaBytes(long bytes) => Convert.ToSingle((double) bytes * 0.0009765625 * 0.0009765625);

    public static float ToMegaBytes(int bytes) => Convert.ToSingle((double) bytes * 0.0009765625 * 0.0009765625);

    public static float ToGigaBytes(ulong bytes) => Convert.ToSingle((double) bytes * 0.0009765625 * 0.0009765625 * 0.0009765625);

    public static float ToGigaBytes(long bytes) => Convert.ToSingle((double) bytes * 0.0009765625 * 0.0009765625 * 0.0009765625);

    public static float ToGigaBytes(int bytes) => Convert.ToSingle((double) bytes * 0.0009765625 * 0.0009765625 * 0.0009765625);
  }
}
