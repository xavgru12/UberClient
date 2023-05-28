// Decompiled with JetBrains decompiler
// Type: UnityItemConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityItemConfiguration : MonoBehaviour
{
  public List<GearItem> UnityItemsDefaultGears;
  public List<WeaponItem> UnityItemsDefaultWeapons;
  public List<UnityItemConfiguration.FunctionalItemHolder> UnityItemsFunctional;

  public static UnityItemConfiguration Instance { get; private set; }

  public static bool Exists => (UnityEngine.Object) UnityItemConfiguration.Instance != (UnityEngine.Object) null;

  private void Awake()
  {
    UnityItemConfiguration.Instance = this;
    Singleton<ItemManager>.Instance.AddUnityItems((IUnityItem[]) this.UnityItemsDefaultGears.ToArray());
    Singleton<ItemManager>.Instance.AddUnityItems((IUnityItem[]) this.UnityItemsDefaultWeapons.ToArray());
    Singleton<ItemManager>.Instance.AddFunctionalItems((IUnityItem[]) this.UnityItemsFunctional.ConvertAll<FunctionalItem>((Converter<UnityItemConfiguration.FunctionalItemHolder, FunctionalItem>) (a =>
    {
      FunctionalItem functionalItem1 = new FunctionalItem();
      functionalItem1.Icon = a.Icon;
      FunctionalItem functionalItem2 = functionalItem1;
      FunctionalItemConfiguration itemConfiguration = new FunctionalItemConfiguration()
      {
        ID = a.ItemId,
        Name = a.Name
      };
      functionalItem2.Configuration = itemConfiguration;
      return functionalItem1;
    })).ToArray());
  }

  [Serializable]
  public class FunctionalItemHolder
  {
    public string Name;
    public Texture2D Icon;
    public int ItemId;
  }
}
