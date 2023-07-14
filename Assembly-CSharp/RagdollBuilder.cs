// Decompiled with JetBrains decompiler
// Type: RagdollBuilder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class RagdollBuilder : Singleton<RagdollBuilder>
{
  private RagdollBuilder()
  {
  }

  public AvatarDecoratorConfig CreateRagdoll(GearLoadout gearLoadout) => gearLoadout.HoloItemId == 0 ? this.CreateLutzRavinoff(gearLoadout) : this.CreateHolo(gearLoadout.HoloItemId);

  private AvatarDecoratorConfig CreateLutzRavinoff(GearLoadout gearLoadout)
  {
    AvatarDecoratorConfig lutzRavinoff = Object.Instantiate((Object) PrefabManager.Instance.DefaultRagdoll) as AvatarDecoratorConfig;
    List<GameObject> objects = new List<GameObject>();
    foreach (GameObject gearPrefab in gearLoadout.GetGearPrefabs())
    {
      if ((bool) (Object) gearPrefab)
        objects.Add(Object.Instantiate((Object) gearPrefab) as GameObject);
    }
    SkinnedMeshCombiner.Combine(lutzRavinoff.gameObject, objects);
    foreach (Object @object in objects)
      Object.Destroy(@object);
    return lutzRavinoff;
  }

  private AvatarDecoratorConfig CreateHolo(int holoItemId)
  {
    AvatarDecoratorConfig holo = (AvatarDecoratorConfig) null;
    if (holoItemId > 0)
    {
      HoloGearItem itemInShop = Singleton<ItemManager>.Instance.GetItemInShop(holoItemId) as HoloGearItem;
      if ((bool) (Object) itemInShop && (bool) (Object) itemInShop.Configuration.Ragdoll)
      {
        holo = Object.Instantiate((Object) itemInShop.Configuration.Ragdoll) as AvatarDecoratorConfig;
        LayerUtil.SetLayerRecursively(holo.transform, UberstrikeLayer.Ragdoll);
        SkinnedMeshCombiner.Combine(holo.gameObject, new List<GameObject>());
      }
    }
    return holo;
  }
}
