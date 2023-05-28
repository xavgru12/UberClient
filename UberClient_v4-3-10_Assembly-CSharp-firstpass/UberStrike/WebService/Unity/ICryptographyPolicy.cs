// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.ICryptographyPolicy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace UberStrike.WebService.Unity
{
  public interface ICryptographyPolicy
  {
    string SHA256Encrypt(string inputString);

    byte[] RijndaelEncrypt(byte[] inputClearText, string passPhrase, string initVector);

    byte[] RijndaelDecrypt(byte[] inputCipherText, string passPhrase, string initVector);
  }
}
