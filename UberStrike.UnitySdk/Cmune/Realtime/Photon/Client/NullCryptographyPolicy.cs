﻿// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Photon.Client.NullCryptographyPolicy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

namespace Cmune.Realtime.Photon.Client
{
  public class NullCryptographyPolicy : ICryptographyPolicy
  {
    public string SHA256Encrypt(string inputString) => inputString;

    public byte[] RijndaelEncrypt(byte[] inputClearText, string passPhrase, string initVector) => inputClearText;

    public byte[] RijndaelDecrypt(byte[] inputCipherText, string passPhrase, string initVector) => inputCipherText;
  }
}
