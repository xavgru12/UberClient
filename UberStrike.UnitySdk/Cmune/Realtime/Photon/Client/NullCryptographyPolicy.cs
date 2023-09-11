
namespace Cmune.Realtime.Photon.Client
{
  public class NullCryptographyPolicy : ICryptographyPolicy
  {
    public string SHA256Encrypt(string inputString) => inputString;

    public byte[] RijndaelEncrypt(byte[] inputClearText, string passPhrase, string initVector) => inputClearText;

    public byte[] RijndaelDecrypt(byte[] inputCipherText, string passPhrase, string initVector) => inputCipherText;
  }
}
