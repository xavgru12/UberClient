// Decompiled with JetBrains decompiler
// Type: Steamworks.CallbackIdentityAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
  internal class CallbackIdentityAttribute : Attribute
  {
    public int Identity { get; set; }

    public CallbackIdentityAttribute(int callbackNum) => this.Identity = callbackNum;
  }
}
