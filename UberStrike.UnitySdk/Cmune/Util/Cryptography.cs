
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
