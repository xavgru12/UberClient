// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.SecureMemory`1
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;
using System.Diagnostics;

namespace UberStrike.Realtime.UnitySdk
{
  public class SecureMemory<T>
  {
    private const string pp = "h&dk2Ks901HenM";
    private const string iv = "huSj39Dl)2kJ4nat";
    private byte[] _encryptedData;
    private T _cachedValue;
    private bool _useAOTCompatibleMode;

    public SecureMemory(T value, bool monitorMemory = true, bool useAOTCompatibleMode = false)
    {
      this._useAOTCompatibleMode = useAOTCompatibleMode;
      if (this._useAOTCompatibleMode)
      {
        this._cachedValue = value;
      }
      else
      {
        this.WriteData(value);
        if (monitorMemory)
        {
          SecureMemoryMonitor.Instance.AddToMonitor += new Action(this.ValidateData);
          if (CmuneDebug.IsDebugEnabled)
          {
            StackTrace stackTrace = new StackTrace(1);
          }
        }
      }
    }

    public void ReleaseData(SecureMemory<T> instance)
    {
      if (this._useAOTCompatibleMode || instance == null)
        return;
      SecureMemoryMonitor.Instance.AddToMonitor -= new Action(instance.ValidateData);
    }

    public void SimulateMemoryHack(T value) => this._cachedValue = value;

    public void WriteData(T value)
    {
      try
      {
        this._cachedValue = value;
        if (this._useAOTCompatibleMode)
          return;
        this._encryptedData = Cryptography.RijndaelEncrypt(RealtimeSerialization.ToBytes((object) value).ToArray(), "h&dk2Ks901HenM", "huSj39Dl)2kJ4nat");
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("SecureMemory failed encrypting Data: {0}", (object) ex.Message), ex.InnerException);
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
      if (this._useAOTCompatibleMode)
        return this._cachedValue;
      try
      {
        return (T) (RealtimeSerialization.ToObject(Cryptography.RijndaelDecrypt(this._encryptedData, "h&dk2Ks901HenM", "huSj39Dl)2kJ4nat") ?? throw new Exception("SecureMemory failed decrypting Data becauase CmuneSecurity.Decrypt returned NULL")) ?? throw new Exception("SecureMemory failed decrypting Data becauase RealtimeSerialization.ToObject returned NULL"));
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("SecureMemory failed decrypting Data: {0}", (object) ex.Message), ex.InnerException);
      }
    }
  }
}
