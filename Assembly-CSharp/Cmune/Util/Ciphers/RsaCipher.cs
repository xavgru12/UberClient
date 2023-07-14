// Decompiled with JetBrains decompiler
// Type: Cmune.Util.Ciphers.RsaCipher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Security.Cryptography;
using System.Text;

namespace Cmune.Util.Ciphers
{
  public class RsaCipher
  {
    private const int PROVIDER_RSA_FULL = 1;
    private const string CONTAINER_NAME = "CmuneContainer";
    private const string PROVIDER_NAME = "Microsoft Strong Cryptographic Provider";
    private RSACryptoServiceProvider provider;

    public RsaCipher(string key, bool useMono)
    {
      this.provider = !useMono ? RsaCipher.GetNetProvider() : RsaCipher.GetMonoProvider();
      this.provider.FromXmlString(key);
    }

    private static RSACryptoServiceProvider GetNetProvider() => new RSACryptoServiceProvider(new CspParameters(1)
    {
      KeyContainerName = "CmuneContainer",
      Flags = CspProviderFlags.UseMachineKeyStore,
      ProviderName = "Microsoft Strong Cryptographic Provider"
    });

    private static RSACryptoServiceProvider GetMonoProvider() => new RSACryptoServiceProvider(new CspParameters(1)
    {
      KeyContainerName = "CmuneContainer",
      Flags = CspProviderFlags.NoFlags,
      ProviderName = "Microsoft Strong Cryptographic Provider"
    });

    public string EncryptString(string plainText) => Convert.ToBase64String(this.provider.Encrypt(Encoding.UTF8.GetBytes(plainText), false));

    public string DecryptString(string encryptedText) => Encoding.UTF8.GetString(this.provider.Decrypt(Convert.FromBase64String(encryptedText), false));

    public static void CreatePrivatePublicKeyPair(
      out string privateKey,
      out string publicKey,
      bool useMono)
    {
      RSACryptoServiceProvider cryptoServiceProvider = !useMono ? RsaCipher.GetNetProvider() : RsaCipher.GetMonoProvider();
      privateKey = cryptoServiceProvider.ToXmlString(true);
      publicKey = cryptoServiceProvider.ToXmlString(false);
    }
  }
}
