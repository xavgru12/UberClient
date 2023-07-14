// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.CrossdomainPolicy
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UberStrike.Realtime.UnitySdk
{
  public static class CrossdomainPolicy
  {
    private static Dictionary<string, bool?> _dict = new Dictionary<string, bool?>(20);
    public static Func<string, IEnumerator> CheckPolicyRoutine = new Func<string, IEnumerator>(CrossdomainPolicy.Default);

    private static IEnumerator Default(string address)
    {
      CrossdomainPolicy.SetPolicyValue(address, true);
      yield break;
    }

    public static Coroutine CheckDomain(string address) => MonoInstance.Mono.StartCoroutine(CrossdomainPolicy.CheckPolicyRoutine(address));

    public static bool HasValidPolicy(string address)
    {
      bool? nullable;
      lock (CrossdomainPolicy._dict)
      {
        if (!CrossdomainPolicy._dict.TryGetValue(address, out nullable))
          return false;
      }
      return ((int) nullable ?? 0) != 0;
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
