// Decompiled with JetBrains decompiler
// Type: CryptographyPolicy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.Util.Ciphers;
using System;
using System.Security.Cryptography;
using System.Text;

public class CryptographyPolicy : UberStrike.WebService.Unity.ICryptographyPolicy, UberStrike.Realtime.UnitySdk.ICryptographyPolicy
{
  public string SHA256Encrypt(string inputString)
  {
    byte[] hash = new SHA256Managed().ComputeHash(new UTF8Encoding().GetBytes(inputString));
    string empty = string.Empty;
    for (int index = 0; index < hash.Length; ++index)
      empty += Convert.ToString(hash[index], 16).PadLeft(2, '0');
    return empty.PadLeft(32, '0');
  }

  public byte[] RijndaelEncrypt(byte[] inputClearText, string passPhrase, string initVector) => new RijndaelCipher(passPhrase, initVector).EncryptToBytes(inputClearText);

  public byte[] RijndaelDecrypt(byte[] inputCipherText, string passPhrase, string initVector) => new RijndaelCipher(passPhrase, initVector).DecryptToBytes(inputCipherText);
}
