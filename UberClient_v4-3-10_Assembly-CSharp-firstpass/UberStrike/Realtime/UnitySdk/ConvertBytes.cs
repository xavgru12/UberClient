// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.ConvertBytes
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
