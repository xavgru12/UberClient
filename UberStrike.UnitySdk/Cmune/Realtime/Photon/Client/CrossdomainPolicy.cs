// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Photon.Client.CrossdomainPolicy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Cmune.Realtime.Photon.Client
{
  public static class CrossdomainPolicy
  {
    private static Dictionary<string, bool?> _dict = new Dictionary<string, bool?>(20);

    public static IEnumerator CheckDomain(string address)
    {
      if (Application.isWebPlayer)
      {
        CrossdomainPolicy.RemovePolicyEntry(address);
        try
        {
          ThreadPool.QueueUserWorkItem((WaitCallback) (_param1 => CrossdomainPolicy.SetPolicyValue(address, Security.PrefetchSocketPolicy(address, 843))));
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Failed to queue work item: " + ex.Message));
        }
        while (!CrossdomainPolicy.HasPolicyEntry(address))
          yield return (object) new WaitForEndOfFrame();
      }
      else
        CrossdomainPolicy.SetPolicyValue(address, true);
    }

    public static bool HasValidPolicy(string address)
    {
      bool? nullable;
      lock (CrossdomainPolicy._dict)
      {
        if (!CrossdomainPolicy._dict.TryGetValue(address, out nullable))
          return false;
      }
      return nullable ?? false;
    }

    private static bool HasPolicyEntry(string address)
    {
      bool? nullable;
      lock (CrossdomainPolicy._dict)
        CrossdomainPolicy._dict.TryGetValue(address, out nullable);
      return nullable.HasValue;
    }

    private static void RemovePolicyEntry(string address)
    {
      lock (CrossdomainPolicy._dict)
        CrossdomainPolicy._dict.Remove(address);
    }

    private static void SetPolicyValue(string address, bool b)
    {
      lock (CrossdomainPolicy._dict)
        CrossdomainPolicy._dict[address] = new bool?(b);
    }
  }
}
