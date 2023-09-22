
using Cmune.Realtime.Common.IO;
using Cmune.Util;
using System;
using System.Diagnostics;

namespace Cmune.Realtime.Common.Security
{
  public class SecureMemory<T> : ISecureMemory
  {
    private const string pp = "h&dk2Ks901HenM";
    private const string iv = "huSj39Dl)2kJ4nat";
    private byte[] _encryptedData;
    private T _cachedValue;

    public SecureMemory(T value, bool monitorMemory = true)
    {
      this.WriteData(value);
      if (!monitorMemory)
        return;
      SecureMemoryMonitor.Instance.AddToMonitor += new Action(this.ValidateData);
      if (!CmuneDebug.IsDebugEnabled)
        return;
      StackTrace stackTrace = new StackTrace(1);
    }

    public static void ReleaseData(SecureMemory<T> instance)
    {
      if (instance == null)
        return;
      SecureMemoryMonitor.Instance.AddToMonitor -= new Action(instance.ValidateData);
    }

    public void SimulateMemoryHack(T value) => this._cachedValue = value;

    public void WriteData(T value)
    {
      try
      {
        this._cachedValue = value;
        this._encryptedData = Cryptography.RijndaelEncrypt(RealtimeSerialization.ToBytes((object) value).ToArray(), "h&dk2Ks901HenM", "huSj39Dl)2kJ4nat");
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("CmuneSecureVariable failed encrypting Data: {0}", (object) ex.Message), ex.InnerException);
      }
    }

    public void ValidateData()
    {
      if (!Comparison.IsEqual((object) this._cachedValue, (object) this.DecryptValue()))
        throw new Exception("Failed to validate data due to a corrupted memory");
    }

    public object ReadObject(bool secure) => (object) this.ReadData(secure);

    public T ReadData(bool secure)
    {
      if (secure)
        this._cachedValue = this.DecryptValue();
      return this._cachedValue;
    }

    private T DecryptValue()
    {
      try
      {
        return (T) (RealtimeSerialization.ToObject(Cryptography.RijndaelDecrypt(this._encryptedData, "h&dk2Ks901HenM", "huSj39Dl)2kJ4nat") ?? throw new Exception("CmuneSecureVariable failed decrypting Data becauase CmuneSecurity.Decrypt returned NULL")) ?? throw new Exception("CmuneSecureVariable failed decrypting Data becauase RealtimeSerialization.ToObject returned NULL"));
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("CmuneSecureVariable failed decrypting Data: {0}", (object) ex.Message), ex.InnerException);
      }
    }
  }
}
