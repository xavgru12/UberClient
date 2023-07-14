// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.Cryptography
// Assembly: UberStrike.DataCenter.UnitySdk, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 1925C691-C9DE-44B0-95F4-3171E7957DDD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.DataCenter.UnitySdk.dll

namespace UberStrike.WebService.Unity
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
