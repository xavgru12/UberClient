// Decompiled with JetBrains decompiler
// Type: Steamworks.UTF8Marshaler
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
  public class UTF8Marshaler : ICustomMarshaler
  {
    public const string DoNotFree = "DoNotFree";
    private static UTF8Marshaler static_instance_free = new UTF8Marshaler(true);
    private static UTF8Marshaler static_instance = new UTF8Marshaler(false);
    private bool _freeNativeMemory;

    private UTF8Marshaler(bool freenativememory) => this._freeNativeMemory = freenativememory;

    public IntPtr MarshalManagedToNative(object managedObj)
    {
      if (managedObj == null)
        return IntPtr.Zero;
      byte[] numArray = managedObj is string s ? new byte[Encoding.UTF8.GetByteCount(s) + 1] : throw new Exception("UTF8Marshaler must be used on a string.");
      Encoding.UTF8.GetBytes(s, 0, s.Length, numArray, 0);
      IntPtr destination = Marshal.AllocHGlobal(numArray.Length);
      Marshal.Copy(numArray, 0, destination, numArray.Length);
      return destination;
    }

    public object MarshalNativeToManaged(IntPtr pNativeData)
    {
      int ofs = 0;
      while (Marshal.ReadByte(pNativeData, ofs) != (byte) 0)
        ++ofs;
      if (ofs == 0)
        return (object) string.Empty;
      byte[] numArray = new byte[ofs];
      Marshal.Copy(pNativeData, numArray, 0, numArray.Length);
      return (object) Encoding.UTF8.GetString(numArray);
    }

    public void CleanUpNativeData(IntPtr pNativeData)
    {
      if (!this._freeNativeMemory)
        return;
      Marshal.FreeHGlobal(pNativeData);
    }

    public void CleanUpManagedData(object managedObj)
    {
    }

    public int GetNativeDataSize() => -1;

    public static ICustomMarshaler GetInstance(string cookie)
    {
      switch (cookie)
      {
        case "DoNotFree":
          return (ICustomMarshaler) UTF8Marshaler.static_instance;
        default:
          return (ICustomMarshaler) UTF8Marshaler.static_instance_free;
      }
    }
  }
}
