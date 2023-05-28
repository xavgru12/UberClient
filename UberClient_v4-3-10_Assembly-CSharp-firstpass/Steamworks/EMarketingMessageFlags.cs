// Decompiled with JetBrains decompiler
// Type: Steamworks.EMarketingMessageFlags
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum EMarketingMessageFlags
  {
    k_EMarketingMessageFlagsNone = 0,
    k_EMarketingMessageFlagsHighPriority = 1,
    k_EMarketingMessageFlagsPlatformWindows = 2,
    k_EMarketingMessageFlagsPlatformMac = 4,
    k_EMarketingMessageFlagsPlatformLinux = 8,
    k_EMarketingMessageFlagsPlatformRestrictions = k_EMarketingMessageFlagsPlatformLinux | k_EMarketingMessageFlagsPlatformMac | k_EMarketingMessageFlagsPlatformWindows, // 0x0000000E
  }
}
