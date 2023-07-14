// Decompiled with JetBrains decompiler
// Type: AvatarBuilder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class AvatarBuilder : Singleton<AvatarBuilder>
{
  private AvatarBuilder()
  {
  }

  public static void Destroy(GameObject obj)
  {
    foreach (Renderer componentsInChild in obj.GetComponentsInChildren<Renderer>())
    {
      foreach (Object material in componentsInChild.materials)
        Object.Destroy(material);
    }
    SkinnedMeshRenderer componentInChildren = obj.GetComponentInChildren<SkinnedMeshRenderer>();
    if ((bool) (Object) componentInChildren)
      Object.Destroy((Object) componentInChildren.sharedMesh);
    Object.Destroy((Object) obj);
  }

  public AvatarDecorator CreateLocalAvatar() => this.CreateLocalAvatar(Singleton<LoadoutManager>.Instance.GearLoadout);

  public AvatarDecorator CreateLocalAvatar(GearLoadout gearLoadout)
  {
    AvatarDecorator avatar = this.CreateAvatar(gearLoadout);
    avatar.gameObject.AddComponent<DontDestroyOnLoad>();
    this.SetupLocalAvatar(avatar);
    return avatar;
  }

  public AvatarDecorator CreateRemoteAvatar(GearLoadout gearLoadout, Color skinColor)
  {
    AvatarDecorator avatar = this.CreateAvatar(gearLoadout);
    this.SetupRemoteAvatar(avatar, skinColor);
    return avatar;
  }

  public void UpdateLocalAvatar(GearLoadout gearLoadout)
  {
    if ((bool) (Object) GameState.LocalDecorator)
    {
      this.UpdateAvatar(GameState.LocalDecorator, gearLoadout);
      GameState.LocalDecorator.SetSkinColor(PlayerDataManager.SkinColor);
      GameState.LocalDecorator.UpdateLayers();
      GameState.LocalDecorator.ShowWeapon(GameState.LocalDecorator.CurrentWeaponSlot);
      GameState.LocalDecorator.HudInformation.SetAvatarLabel(!PlayerDataManager.IsPlayerInClan ? PlayerDataManager.Name : string.Format("[{0}] {1}", (object) PlayerDataManager.ClanTag, (object) PlayerDataManager.Name));
      GameState.LocalDecorator.HudInformation.enabled = true;
      SkinnedMeshRenderer componentInChildren = GameState.LocalDecorator.GetComponentInChildren<SkinnedMeshRenderer>();
      if (!(bool) (Object) componentInChildren)
        return;
      componentInChildren.castShadows = true;
      componentInChildren.receiveShadows = false;
    }
    else
      Debug.LogError((object) "No local Player created yet! Call 'CreateLocalPlayerAvatar' first!");
  }

  public void UpdateRemoteAvatar(AvatarDecorator decorator, int[] gearItems, Color skinColor)
  {
    this.UpdateAvatar(decorator, new GearLoadout(new List<int>((IEnumerable<int>) gearItems)));
    this.SetupRemoteAvatar(decorator, skinColor);
  }

  private void UpdateAvatar(AvatarDecorator avatar, GearLoadout gearLoadout)
  {
    List<GameObject> objects = new List<GameObject>();
    if (gearLoadout.HoloItemId > 0)
    {
      HoloGearItem itemInShop = Singleton<ItemManager>.Instance.GetItemInShop(gearLoadout.HoloItemId) as HoloGearItem;
      if ((bool) (Object) itemInShop && (bool) (Object) itemInShop.Configuration.Avatar)
        objects.Add(Object.Instantiate((Object) itemInShop.Configuration.Avatar.gameObject) as GameObject);
    }
    else
    {
      foreach (GameObject gearPrefab in gearLoadout.GetGearPrefabs())
      {
        if ((bool) (Object) gearPrefab)
          objects.Add(Object.Instantiate((Object) gearPrefab) as GameObject);
      }
      objects.Add((Object.Instantiate((Object) PrefabManager.Instance.DefaultAvatar) as AvatarDecorator).gameObject);
    }
    SkinnedMeshCombiner.Update(avatar.gameObject, objects);
    avatar.Gear = gearLoadout;
    avatar.MeshRenderer.receiveShadows = false;
    foreach (Object @object in objects)
      Object.Destroy(@object);
  }

  private AvatarDecorator CreateAvatar(GearLoadout gearLoadout) => gearLoadout.HoloItemId == 0 ? this.CreateLutzRavinoff(gearLoadout) : this.CreateHolo(gearLoadout.HoloItemId);

  private AvatarDecorator CreateLutzRavinoff(GearLoadout gearLoadout)
  {
    List<GameObject> objects = new List<GameObject>();
    foreach (GameObject gearPrefab in gearLoadout.GetGearPrefabs())
    {
      if ((bool) (Object) gearPrefab)
        objects.Add(Object.Instantiate((Object) gearPrefab) as GameObject);
    }
    AvatarDecorator lutzRavinoff = Object.Instantiate((Object) PrefabManager.Instance.DefaultAvatar) as AvatarDecorator;
    lutzRavinoff.Gear = gearLoadout;
    lutzRavinoff.SetLayers(UberstrikeLayer.LocalPlayer);
    SkinnedMeshCombiner.Combine(lutzRavinoff.gameObject, objects);
    foreach (GameObject gameObject in objects)
    {
      ParticleSystem componentInChildren = gameObject.GetComponentInChildren<ParticleSystem>();
      if ((bool) (Object) componentInChildren)
      {
        componentInChildren.transform.parent = lutzRavinoff.transform;
        componentInChildren.transform.localPosition = Vector3.zero;
      }
      Object.Destroy((Object) gameObject);
    }
    return lutzRavinoff;
  }

  private AvatarDecorator CreateHolo(int holoItemId)
  {
    AvatarDecorator holo = (AvatarDecorator) null;
    HoloGearItem itemInShop = Singleton<ItemManager>.Instance.GetItemInShop(holoItemId) as HoloGearItem;
    if ((bool) (Object) itemInShop && (bool) (Object) itemInShop.Configuration.Avatar)
    {
      holo = Object.Instantiate((Object) itemInShop.Configuration.Avatar) as AvatarDecorator;
      SkinnedMeshCombiner.Combine(holo.gameObject, new List<GameObject>());
    }
    else
      Debug.LogError((object) ("Failed to create holo! ItemId = " + (object) holoItemId));
    holo.SetLayers(UberstrikeLayer.LocalPlayer);
    return holo;
  }

  private void SetupLocalAvatar(AvatarDecorator avatar)
  {
    if (!(bool) (Object) avatar)
      return;
    foreach (LoadoutSlotType weaponSlot in LoadoutManager.WeaponSlots)
    {
      InventoryItem inventoryItem;
      if (Singleton<LoadoutManager>.Instance.TryGetItemInSlot(weaponSlot, out inventoryItem))
      {
        BaseWeaponDecorator component = Singleton<ItemManager>.Instance.Instantiate(inventoryItem.Item.ItemId).GetComponent<BaseWeaponDecorator>();
        component.EnableShootAnimation = false;
        avatar.AssignWeapon(weaponSlot, component);
      }
    }
    avatar.UpdateLayers();
    avatar.HudInformation.DistanceCap = 100f;
    avatar.SetSkinColor(PlayerDataManager.SkinColor);
    avatar.HudInformation.SetAvatarLabel(PlayerDataManager.NameSecure);
    SkinnedMeshRenderer componentInChildren = avatar.GetComponentInChildren<SkinnedMeshRenderer>();
    if (!(bool) (Object) componentInChildren)
      return;
    componentInChildren.castShadows = true;
    componentInChildren.receiveShadows = false;
  }

  private void SetupRemoteAvatar(AvatarDecorator avatar, Color skinColor)
  {
    if (!(bool) (Object) avatar)
      return;
    avatar.SetLayers(UberstrikeLayer.RemotePlayer);
    avatar.SetSkinColor(skinColor);
    avatar.ShowWeapon(avatar.CurrentWeaponSlot);
  }
}
