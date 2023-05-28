// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.SecureMemory`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.IO;
using UberStrike.Core.Serialization;

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
        if (!monitorMemory)
          return;
        SecureMemoryMonitor.Instance.AddToMonitor += new Action(this.ValidateData);
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
        this._encryptedData = Cryptography.RijndaelEncrypt(this.Serialize(value), "h&dk2Ks901HenM", "huSj39Dl)2kJ4nat");
      }
      catch (Exception ex)
      {
        throw new Exception("SecureMemory failed encrypting Data: " + ex.Message, ex.InnerException);
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
        return (T) ((object) this.Deserialize(Cryptography.RijndaelDecrypt(this._encryptedData, "h&dk2Ks901HenM", "huSj39Dl)2kJ4nat") ?? throw new Exception("SecureMemory failed decrypting Data becauase CmuneSecurity.Decrypt returned NULL")) ?? throw new Exception("SecureMemory failed decrypting Data becauase RealtimeSerialization.ToObject returned NULL"));
      }
      catch (Exception ex)
      {
        throw new Exception("SecureMemory failed decrypting Data: " + ex.Message, ex.InnerException);
      }
    }

    private byte[] Serialize(T obj)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Type type = typeof (T);
        if (type == typeof (int))
          Int32Proxy.Serialize((Stream) bytes, (int) (object) obj);
        else if (type == typeof (float))
          SingleProxy.Serialize((Stream) bytes, (float) (object) obj);
        else if (type == typeof (string))
          StringProxy.Serialize((Stream) bytes, (string) (object) obj);
        return bytes.ToArray();
      }
    }

    private T Deserialize(byte[] bytes)
    {
      using (MemoryStream bytes1 = new MemoryStream(bytes))
      {
        Type type = typeof (T);
        if (type == typeof (int))
          return (T) (ValueType) Int32Proxy.Deserialize((Stream) bytes1);
        if (type == typeof (float))
          return (T) (ValueType) SingleProxy.Deserialize((Stream) bytes1);
        return type == typeof (string) ? (T) StringProxy.Deserialize((Stream) bytes1) : default (T);
      }
    }
  }
}
