// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.UberStrikeItemWeaponViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;

namespace UberStrike.Core.Serialization
{
  public static class UberStrikeItemWeaponViewProxy
  {
    public static void Serialize(Stream stream, UberStrikeItemWeaponView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.AccuracySpread);
        Int32Proxy.Serialize((Stream) bytes, instance.CombatRange);
        Int32Proxy.Serialize((Stream) bytes, instance.CriticalStrikeBonus);
        if (instance.CustomProperties != null)
          DictionaryProxy<string, string>.Serialize((Stream) bytes, instance.CustomProperties, new DictionaryProxy<string, string>.Serializer<string>(StringProxy.Serialize), new DictionaryProxy<string, string>.Serializer<string>(StringProxy.Serialize));
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.DamageKnockback);
        Int32Proxy.Serialize((Stream) bytes, instance.DamagePerProjectile);
        Int32Proxy.Serialize((Stream) bytes, instance.DefaultZoomMultiplier);
        if (instance.Description != null)
          StringProxy.Serialize((Stream) bytes, instance.Description);
        else
          num |= 2;
        BooleanProxy.Serialize((Stream) bytes, instance.HasAutomaticFire);
        Int32Proxy.Serialize((Stream) bytes, instance.ID);
        BooleanProxy.Serialize((Stream) bytes, instance.IsConsumable);
        EnumProxy<UberstrikeItemClass>.Serialize((Stream) bytes, instance.ItemClass);
        if (instance.ItemProperties != null)
          DictionaryProxy<ItemPropertyType, int>.Serialize((Stream) bytes, instance.ItemProperties, new DictionaryProxy<ItemPropertyType, int>.Serializer<ItemPropertyType>(EnumProxy<ItemPropertyType>.Serialize), new DictionaryProxy<ItemPropertyType, int>.Serializer<int>(Int32Proxy.Serialize));
        else
          num |= 4;
        Int32Proxy.Serialize((Stream) bytes, instance.LevelLock);
        Int32Proxy.Serialize((Stream) bytes, instance.MaxAmmo);
        Int32Proxy.Serialize((Stream) bytes, instance.MaxDurationDays);
        Int32Proxy.Serialize((Stream) bytes, instance.MaxZoomMultiplier);
        Int32Proxy.Serialize((Stream) bytes, instance.MinZoomMultiplier);
        Int32Proxy.Serialize((Stream) bytes, instance.MissileBounciness);
        Int32Proxy.Serialize((Stream) bytes, instance.MissileForceImpulse);
        Int32Proxy.Serialize((Stream) bytes, instance.MissileTimeToDetonate);
        if (instance.Name != null)
          StringProxy.Serialize((Stream) bytes, instance.Name);
        else
          num |= 8;
        if (instance.PrefabName != null)
          StringProxy.Serialize((Stream) bytes, instance.PrefabName);
        else
          num |= 16;
        if (instance.Prices != null)
          ListProxy<ItemPrice>.Serialize((Stream) bytes, instance.Prices, new ListProxy<ItemPrice>.Serializer<ItemPrice>(ItemPriceProxy.Serialize));
        else
          num |= 32;
        Int32Proxy.Serialize((Stream) bytes, instance.ProjectileSpeed);
        Int32Proxy.Serialize((Stream) bytes, instance.ProjectilesPerShot);
        Int32Proxy.Serialize((Stream) bytes, instance.RateOfFire);
        Int32Proxy.Serialize((Stream) bytes, instance.RecoilKickback);
        Int32Proxy.Serialize((Stream) bytes, instance.RecoilMovement);
        Int32Proxy.Serialize((Stream) bytes, instance.SecondaryActionReticle);
        EnumProxy<ItemShopHighlightType>.Serialize((Stream) bytes, instance.ShopHighlightType);
        Int32Proxy.Serialize((Stream) bytes, instance.SplashRadius);
        Int32Proxy.Serialize((Stream) bytes, instance.StartAmmo);
        Int32Proxy.Serialize((Stream) bytes, instance.Tier);
        Int32Proxy.Serialize((Stream) bytes, instance.WeaponSecondaryAction);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static UberStrikeItemWeaponView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      UberStrikeItemWeaponView strikeItemWeaponView = new UberStrikeItemWeaponView();
      strikeItemWeaponView.AccuracySpread = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.CombatRange = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.CriticalStrikeBonus = Int32Proxy.Deserialize(bytes);
      if ((num & 1) != 0)
        strikeItemWeaponView.CustomProperties = DictionaryProxy<string, string>.Deserialize(bytes, new DictionaryProxy<string, string>.Deserializer<string>(StringProxy.Deserialize), new DictionaryProxy<string, string>.Deserializer<string>(StringProxy.Deserialize));
      strikeItemWeaponView.DamageKnockback = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.DamagePerProjectile = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.DefaultZoomMultiplier = Int32Proxy.Deserialize(bytes);
      if ((num & 2) != 0)
        strikeItemWeaponView.Description = StringProxy.Deserialize(bytes);
      strikeItemWeaponView.HasAutomaticFire = BooleanProxy.Deserialize(bytes);
      strikeItemWeaponView.ID = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.IsConsumable = BooleanProxy.Deserialize(bytes);
      strikeItemWeaponView.ItemClass = EnumProxy<UberstrikeItemClass>.Deserialize(bytes);
      if ((num & 4) != 0)
        strikeItemWeaponView.ItemProperties = DictionaryProxy<ItemPropertyType, int>.Deserialize(bytes, new DictionaryProxy<ItemPropertyType, int>.Deserializer<ItemPropertyType>(EnumProxy<ItemPropertyType>.Deserialize), new DictionaryProxy<ItemPropertyType, int>.Deserializer<int>(Int32Proxy.Deserialize));
      strikeItemWeaponView.LevelLock = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.MaxAmmo = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.MaxDurationDays = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.MaxZoomMultiplier = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.MinZoomMultiplier = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.MissileBounciness = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.MissileForceImpulse = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.MissileTimeToDetonate = Int32Proxy.Deserialize(bytes);
      if ((num & 8) != 0)
        strikeItemWeaponView.Name = StringProxy.Deserialize(bytes);
      if ((num & 16) != 0)
        strikeItemWeaponView.PrefabName = StringProxy.Deserialize(bytes);
      if ((num & 32) != 0)
        strikeItemWeaponView.Prices = (ICollection<ItemPrice>) ListProxy<ItemPrice>.Deserialize(bytes, new ListProxy<ItemPrice>.Deserializer<ItemPrice>(ItemPriceProxy.Deserialize));
      strikeItemWeaponView.ProjectileSpeed = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.ProjectilesPerShot = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.RateOfFire = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.RecoilKickback = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.RecoilMovement = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.SecondaryActionReticle = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.ShopHighlightType = EnumProxy<ItemShopHighlightType>.Deserialize(bytes);
      strikeItemWeaponView.SplashRadius = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.StartAmmo = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.Tier = Int32Proxy.Deserialize(bytes);
      strikeItemWeaponView.WeaponSecondaryAction = Int32Proxy.Deserialize(bytes);
      return strikeItemWeaponView;
    }
  }
}
