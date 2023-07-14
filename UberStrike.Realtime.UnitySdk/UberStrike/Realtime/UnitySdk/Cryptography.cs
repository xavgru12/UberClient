// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.Cryptography
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

namespace UberStrike.Realtime.UnitySdk
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
