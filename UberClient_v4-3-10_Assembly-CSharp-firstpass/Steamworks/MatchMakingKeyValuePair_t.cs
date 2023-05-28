// Decompiled with JetBrains decompiler
// Type: Steamworks.MatchMakingKeyValuePair_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  public struct MatchMakingKeyValuePair_t
  {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string m_szKey;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string m_szValue;

    private MatchMakingKeyValuePair_t(string strKey, string strValue)
    {
      this.m_szKey = strKey;
      this.m_szValue = strValue;
    }
  }
}
