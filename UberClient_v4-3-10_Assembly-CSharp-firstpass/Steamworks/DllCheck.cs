// Decompiled with JetBrains decompiler
// Type: Steamworks.DllCheck
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Diagnostics;
using System.IO;

namespace Steamworks
{
  public class DllCheck
  {
    public static bool Test() => DllCheck.CheckSteamAPIDLL();

    private static bool CheckSteamAPIDLL()
    {
      string currentDirectory = Directory.GetCurrentDirectory();
      string str;
      int num;
      if (IntPtr.Size == 4)
      {
        str = Path.Combine(currentDirectory, "steam_api.dll");
        num = 187584;
      }
      else
      {
        str = Path.Combine(currentDirectory, "steam_api64.dll");
        num = 208296;
      }
      return !File.Exists(str) || new FileInfo(str).Length == (long) num && !(FileVersionInfo.GetVersionInfo(str).FileVersion != "02.59.51.43");
    }
  }
}
