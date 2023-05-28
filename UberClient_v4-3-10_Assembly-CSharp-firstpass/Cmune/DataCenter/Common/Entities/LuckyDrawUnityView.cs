// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.LuckyDrawUnityView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class LuckyDrawUnityView
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Price { get; set; }

    public UberStrikeCurrencyType UberStrikeCurrencyType { get; set; }

    public string IconUrl { get; set; }

    public BundleCategoryType Category { get; set; }

    public bool IsAvailableInShop { get; set; }

    public List<LuckyDrawSetUnityView> LuckyDrawSets { get; set; }
  }
}
