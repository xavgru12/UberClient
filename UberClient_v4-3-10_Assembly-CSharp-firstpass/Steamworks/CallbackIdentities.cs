// Decompiled with JetBrains decompiler
// Type: Steamworks.CallbackIdentities
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  internal class CallbackIdentities
  {
    public static int GetCallbackIdentity(Type callbackStruct)
    {
      object[] customAttributes = callbackStruct.GetCustomAttributes(typeof (CallbackIdentityAttribute), false);
      int index = 0;
      if (index < customAttributes.Length)
        return ((CallbackIdentityAttribute) customAttributes[index]).Identity;
      throw new Exception("Callback number not found for struct " + callbackStruct?.ToString());
    }
  }
}
