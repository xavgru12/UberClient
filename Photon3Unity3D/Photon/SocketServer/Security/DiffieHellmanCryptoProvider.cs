﻿// Decompiled with JetBrains decompiler
// Type: Photon.SocketServer.Security.DiffieHellmanCryptoProvider
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using Photon.SocketServer.Numeric;
using System;
using System.Security.Cryptography;

namespace Photon.SocketServer.Security
{
  public class DiffieHellmanCryptoProvider : IDisposable
  {
    private static readonly BigInteger primeRoot = new BigInteger((long) OakleyGroups.Generator);
    private readonly BigInteger prime;
    private readonly BigInteger secret;
    private readonly BigInteger publicKey;
    private Rijndael crypto;
    private byte[] sharedKey;

    public DiffieHellmanCryptoProvider()
    {
      this.prime = new BigInteger(OakleyGroups.OakleyPrime768);
      this.secret = this.GenerateRandomSecret(160);
      this.publicKey = this.CalculatePublicKey();
    }

    public bool IsInitialized => this.crypto != null;

    public byte[] PublicKey => this.publicKey.GetBytes();

    public byte[] SharedKey => this.sharedKey;

    public void DeriveSharedKey(byte[] otherPartyPublicKey)
    {
      this.sharedKey = this.CalculateSharedKey(new BigInteger(otherPartyPublicKey)).GetBytes();
      byte[] hash;
      using (SHA256 shA256 = (SHA256) new SHA256Managed())
        hash = shA256.ComputeHash(this.SharedKey);
      this.crypto = (Rijndael) new RijndaelManaged();
      this.crypto.Key = hash;
      this.crypto.IV = new byte[16];
      this.crypto.Padding = PaddingMode.PKCS7;
    }

    public byte[] Encrypt(byte[] data) => this.Encrypt(data, 0, data.Length);

    public byte[] Encrypt(byte[] data, int offset, int count)
    {
      using (ICryptoTransform encryptor = this.crypto.CreateEncryptor())
        return encryptor.TransformFinalBlock(data, offset, count);
    }

    public byte[] Decrypt(byte[] data) => this.Decrypt(data, 0, data.Length);

    public byte[] Decrypt(byte[] data, int offset, int count)
    {
      using (ICryptoTransform decryptor = this.crypto.CreateDecryptor())
        return decryptor.TransformFinalBlock(data, offset, count);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected void Dispose(bool disposing)
    {
      if (disposing)
        ;
    }

    private BigInteger CalculatePublicKey() => DiffieHellmanCryptoProvider.primeRoot.ModPow(this.secret, this.prime);

    private BigInteger CalculateSharedKey(BigInteger otherPartyPublicKey) => otherPartyPublicKey.ModPow(this.secret, this.prime);

    private BigInteger GenerateRandomSecret(int secretLength)
    {
      BigInteger random;
      do
      {
        random = BigInteger.GenerateRandom(secretLength);
      }
      while (random >= this.prime - (BigInteger) 1 || random == (BigInteger) 0);
      return random;
    }
  }
}
