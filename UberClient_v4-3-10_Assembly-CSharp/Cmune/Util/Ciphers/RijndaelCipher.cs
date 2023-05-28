// Decompiled with JetBrains decompiler
// Type: Cmune.Util.Ciphers.RijndaelCipher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Cmune.Util.Ciphers
{
  internal class RijndaelCipher
  {
    private static string DEFAULT_HASH_ALGORITHM = "SHA1";
    private static int DEFAULT_KEY_SIZE = 256;
    private static int MAX_ALLOWED_SALT_LEN = (int) byte.MaxValue;
    private static int MIN_ALLOWED_SALT_LEN = 4;
    private static int DEFAULT_MIN_SALT_LEN = RijndaelCipher.MIN_ALLOWED_SALT_LEN;
    private static int DEFAULT_MAX_SALT_LEN = 8;
    private int minSaltLen = -1;
    private int maxSaltLen = -1;
    private ICryptoTransform encryptor;
    private ICryptoTransform decryptor;

    public RijndaelCipher(string passPhrase)
      : this(passPhrase, (string) null)
    {
    }

    public RijndaelCipher(string passPhrase, string initVector)
      : this(passPhrase, initVector, -1)
    {
    }

    public RijndaelCipher(string passPhrase, string initVector, int minSaltLen)
      : this(passPhrase, initVector, minSaltLen, -1)
    {
    }

    public RijndaelCipher(string passPhrase, string initVector, int minSaltLen, int maxSaltLen)
      : this(passPhrase, initVector, minSaltLen, maxSaltLen, -1)
    {
    }

    public RijndaelCipher(
      string passPhrase,
      string initVector,
      int minSaltLen,
      int maxSaltLen,
      int keySize)
      : this(passPhrase, initVector, minSaltLen, maxSaltLen, keySize, (string) null)
    {
    }

    public RijndaelCipher(
      string passPhrase,
      string initVector,
      int minSaltLen,
      int maxSaltLen,
      int keySize,
      string hashAlgorithm)
      : this(passPhrase, initVector, minSaltLen, maxSaltLen, keySize, hashAlgorithm, (string) null)
    {
    }

    public RijndaelCipher(
      string passPhrase,
      string initVector,
      int minSaltLen,
      int maxSaltLen,
      int keySize,
      string hashAlgorithm,
      string saltValue)
      : this(passPhrase, initVector, minSaltLen, maxSaltLen, keySize, hashAlgorithm, saltValue, 1)
    {
    }

    public RijndaelCipher(
      string passPhrase,
      string initVector,
      int minSaltLen,
      int maxSaltLen,
      int keySize,
      string hashAlgorithm,
      string saltValue,
      int passwordIterations)
    {
      this.minSaltLen = minSaltLen >= RijndaelCipher.MIN_ALLOWED_SALT_LEN ? minSaltLen : RijndaelCipher.DEFAULT_MIN_SALT_LEN;
      this.maxSaltLen = maxSaltLen < 0 || maxSaltLen > RijndaelCipher.MAX_ALLOWED_SALT_LEN ? RijndaelCipher.DEFAULT_MAX_SALT_LEN : maxSaltLen;
      if (keySize <= 0)
        keySize = RijndaelCipher.DEFAULT_KEY_SIZE;
      hashAlgorithm = hashAlgorithm != null ? hashAlgorithm.ToUpper().Replace("-", string.Empty) : RijndaelCipher.DEFAULT_HASH_ALGORITHM;
      byte[] rgbIV = initVector != null ? Encoding.ASCII.GetBytes(initVector) : new byte[0];
      byte[] rgbSalt = saltValue != null ? Encoding.ASCII.GetBytes(saltValue) : new byte[0];
      byte[] bytes = new PasswordDeriveBytes(passPhrase, rgbSalt, hashAlgorithm, passwordIterations).GetBytes(keySize / 8);
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      rijndaelManaged.BlockSize = 128;
      if (rgbIV.Length == 0)
        rijndaelManaged.Mode = CipherMode.ECB;
      else
        rijndaelManaged.Mode = CipherMode.CBC;
      this.encryptor = rijndaelManaged.CreateEncryptor(bytes, rgbIV);
      this.decryptor = rijndaelManaged.CreateDecryptor(bytes, rgbIV);
    }

    public string Encrypt(string plainText) => this.Encrypt(Encoding.UTF8.GetBytes(plainText));

    public string Encrypt(byte[] plainTextBytes) => Convert.ToBase64String(this.EncryptToBytes(plainTextBytes));

    public byte[] EncryptToBytes(string plainText) => this.EncryptToBytes(Encoding.UTF8.GetBytes(plainText));

    public byte[] EncryptToBytes(byte[] plainTextBytes)
    {
      byte[] buffer = this.AddSalt(plainTextBytes);
      MemoryStream memoryStream = new MemoryStream();
      lock (this)
      {
        CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, this.encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(buffer, 0, buffer.Length);
        cryptoStream.FlushFinalBlock();
        byte[] array = memoryStream.ToArray();
        memoryStream.Close();
        cryptoStream.Close();
        return array;
      }
    }

    public string Decrypt(string cipherText) => this.Decrypt(Convert.FromBase64String(cipherText));

    public string Decrypt(byte[] cipherTextBytes) => Encoding.UTF8.GetString(this.DecryptToBytes(cipherTextBytes));

    public byte[] DecryptToBytes(string cipherText) => this.DecryptToBytes(Convert.FromBase64String(cipherText));

    public byte[] DecryptToBytes(byte[] cipherTextBytes)
    {
      int num = 0;
      int sourceIndex = 0;
      MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
      byte[] numArray = new byte[cipherTextBytes.Length];
      lock (this)
      {
        CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, this.decryptor, CryptoStreamMode.Read);
        num = cryptoStream.Read(numArray, 0, numArray.Length);
        memoryStream.Close();
        cryptoStream.Close();
      }
      if (this.maxSaltLen > 0 && this.maxSaltLen >= this.minSaltLen)
        sourceIndex = (int) numArray[0] & 3 | (int) numArray[1] & 12 | (int) numArray[2] & 48 | (int) numArray[3] & 192;
      byte[] destinationArray = new byte[num - sourceIndex];
      Array.Copy((Array) numArray, sourceIndex, (Array) destinationArray, 0, num - sourceIndex);
      return destinationArray;
    }

    private byte[] AddSalt(byte[] plainTextBytes)
    {
      if (this.maxSaltLen == 0 || this.maxSaltLen < this.minSaltLen)
        return plainTextBytes;
      byte[] salt = this.GenerateSalt();
      byte[] destinationArray = new byte[plainTextBytes.Length + salt.Length];
      Array.Copy((Array) salt, (Array) destinationArray, salt.Length);
      Array.Copy((Array) plainTextBytes, 0, (Array) destinationArray, salt.Length, plainTextBytes.Length);
      return destinationArray;
    }

    private byte[] GenerateSalt()
    {
      int length = this.minSaltLen != this.maxSaltLen ? this.GenerateRandomNumber(this.minSaltLen, this.maxSaltLen) : this.minSaltLen;
      byte[] data = new byte[length];
      new RNGCryptoServiceProvider().GetNonZeroBytes(data);
      data[0] = (byte) ((int) data[0] & 252 | length & 3);
      data[1] = (byte) ((int) data[1] & 243 | length & 12);
      data[2] = (byte) ((int) data[2] & 207 | length & 48);
      data[3] = (byte) ((int) data[3] & 63 | length & 192);
      return data;
    }

    private int GenerateRandomNumber(int minValue, int maxValue)
    {
      byte[] data = new byte[4];
      new RNGCryptoServiceProvider().GetBytes(data);
      return new Random(((int) data[0] & (int) sbyte.MaxValue) << 24 | (int) data[1] << 16 | (int) data[2] << 8 | (int) data[3]).Next(minValue, maxValue + 1);
    }
  }
}
