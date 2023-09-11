// Decompiled with JetBrains decompiler
// Type: Cmune.Util.Cryptography
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Realtime.Photon.Client;

namespace Cmune.Util
{
  public static class Cryptography
  {
    public static ICryptographyPolicy Policy = (ICryptographyPolicy) new NullCryptographyPolicy();

    public static string SHA256Encrypt(string inputString) => Cryptography.Policy.SHA256Encrypt(inputString);

    public static byte[] RijndaelEncrypt(
      byte[] inputClearText,
      string passPhrase,
      string initVector)
    {
      return Cryptography.Policy.RijndaelEncrypt(inputClearText, passPhrase, initVector);
    }

    public static byte[] RijndaelDecrypt(
      byte[] inputCipherText,
      string passPhrase,
      string initVector)
    {
      return Cryptography.Policy.RijndaelDecrypt(inputCipherText, passPhrase, initVector);
    }
  }
}
