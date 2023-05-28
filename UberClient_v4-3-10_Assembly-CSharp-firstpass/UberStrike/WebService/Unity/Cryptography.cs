// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.Cryptography
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
