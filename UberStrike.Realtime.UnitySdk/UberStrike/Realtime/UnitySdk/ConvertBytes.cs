// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.ConvertBytes
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;

namespace UberStrike.Realtime.UnitySdk
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
