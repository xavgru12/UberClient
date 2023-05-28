// Decompiled with JetBrains decompiler
// Type: Steamworks.HTML_ShowToolTip_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(4524)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct HTML_ShowToolTip_t
  {
    public const int k_iCallback = 4524;
    public HHTMLBrowser unBrowserHandle;
    public string pchMsg;
  }
}
