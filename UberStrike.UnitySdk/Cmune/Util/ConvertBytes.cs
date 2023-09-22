
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
