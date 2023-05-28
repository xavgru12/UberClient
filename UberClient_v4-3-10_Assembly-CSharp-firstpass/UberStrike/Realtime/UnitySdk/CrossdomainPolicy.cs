// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.CrossdomainPolicy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
{
  public static class CrossdomainPolicy
  {
    private static Dictionary<string, bool?> _dict = new Dictionary<string, bool?>(20);
    public static Func<string, Action, IEnumerator> CheckPolicyRoutine = new Func<string, Action, IEnumerator>(CrossdomainPolicy.Default);

    private static IEnumerator Default(string address, Action callback)
    {
      CrossdomainPolicy.SetPolicyValue(address, true);
      callback();
      yield break;
    }

    public static bool HasValidPolicy(string address)
    {
      bool? nullable;
      lock (CrossdomainPolicy._dict)
      {
        if (!CrossdomainPolicy._dict.TryGetValue(address, out nullable))
          return false;
      }
      return nullable.HasValue && nullable.Value;
    }

    public static bool HasPolicyEntry(string address)
    {
      bool? nullable;
      lock (CrossdomainPolicy._dict)
        CrossdomainPolicy._dict.TryGetValue(address, out nullable);
      return nullable.HasValue;
    }

    public static void RemovePolicyEntry(string address)
    {
      lock (CrossdomainPolicy._dict)
        CrossdomainPolicy._dict.Remove(address);
    }

    public static void SetPolicyValue(string address, bool b)
    {
      lock (CrossdomainPolicy._dict)
        CrossdomainPolicy._dict[address] = new bool?(b);
    }
  }
}
