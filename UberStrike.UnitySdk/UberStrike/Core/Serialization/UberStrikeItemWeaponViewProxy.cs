
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
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.AccuracySpread);
          if (instance.CustomProperties != null)
            DictionaryProxy<string, string>.Serialize((Stream) bytes, instance.CustomProperties, new DictionaryProxy<string, string>.Serializer<string>(StringProxy.Serialize), new DictionaryProxy<string, string>.Serializer<string>(StringProxy.Serialize));
          else
            num |= 1;
          Int32Proxy.Serialize((Stream) bytes, instance.DamageKnockback);
          Int32Proxy.Serialize((Stream) bytes, instance.DamagePerProjectile);
          if (instance.Description != null)
            StringProxy.Serialize((Stream) bytes, instance.Description);
          else
            num |= 2;
          Int32Proxy.Serialize((Stream) bytes, instance.ID);
          BooleanProxy.Serialize((Stream) bytes, instance.IsConsumable);
          EnumProxy<UberstrikeItemClass>.Serialize((Stream) bytes, instance.ItemClass);
          Int32Proxy.Serialize((Stream) bytes, instance.LevelLock);
          Int32Proxy.Serialize((Stream) bytes, instance.MaxAmmo);
          Int32Proxy.Serialize((Stream) bytes, instance.MissileBounciness);
          Int32Proxy.Serialize((Stream) bytes, instance.MissileForceImpulse);
          Int32Proxy.Serialize((Stream) bytes, instance.MissileTimeToDetonate);
          if (instance.Name != null)
            StringProxy.Serialize((Stream) bytes, instance.Name);
          else
            num |= 4;
          if (instance.Prices != null)
            ListProxy<ItemPrice>.Serialize((Stream) bytes, instance.Prices, new ListProxy<ItemPrice>.Serializer<ItemPrice>(ItemPriceProxy.Serialize));
          else
            num |= 8;
          Int32Proxy.Serialize((Stream) bytes, instance.ProjectileSpeed);
          Int32Proxy.Serialize((Stream) bytes, instance.ProjectilesPerShot);
          Int32Proxy.Serialize((Stream) bytes, instance.RateOfFire);
          Int32Proxy.Serialize((Stream) bytes, instance.RecoilKickback);
          Int32Proxy.Serialize((Stream) bytes, instance.RecoilMovement);
          EnumProxy<ItemShopHighlightType>.Serialize((Stream) bytes, instance.ShopHighlightType);
          Int32Proxy.Serialize((Stream) bytes, instance.SplashRadius);
          Int32Proxy.Serialize((Stream) bytes, instance.StartAmmo);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static UberStrikeItemWeaponView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      UberStrikeItemWeaponView strikeItemWeaponView = (UberStrikeItemWeaponView) null;
      if (num != 0)
      {
        strikeItemWeaponView = new UberStrikeItemWeaponView();
        strikeItemWeaponView.AccuracySpread = Int32Proxy.Deserialize(bytes);
        if ((num & 1) != 0)
          strikeItemWeaponView.CustomProperties = DictionaryProxy<string, string>.Deserialize(bytes, new DictionaryProxy<string, string>.Deserializer<string>(StringProxy.Deserialize), new DictionaryProxy<string, string>.Deserializer<string>(StringProxy.Deserialize));
        strikeItemWeaponView.DamageKnockback = Int32Proxy.Deserialize(bytes);
        strikeItemWeaponView.DamagePerProjectile = Int32Proxy.Deserialize(bytes);
        if ((num & 2) != 0)
          strikeItemWeaponView.Description = StringProxy.Deserialize(bytes);
        strikeItemWeaponView.ID = Int32Proxy.Deserialize(bytes);
        strikeItemWeaponView.IsConsumable = BooleanProxy.Deserialize(bytes);
        strikeItemWeaponView.ItemClass = EnumProxy<UberstrikeItemClass>.Deserialize(bytes);
        strikeItemWeaponView.LevelLock = Int32Proxy.Deserialize(bytes);
        strikeItemWeaponView.MaxAmmo = Int32Proxy.Deserialize(bytes);
        strikeItemWeaponView.MissileBounciness = Int32Proxy.Deserialize(bytes);
        strikeItemWeaponView.MissileForceImpulse = Int32Proxy.Deserialize(bytes);
        strikeItemWeaponView.MissileTimeToDetonate = Int32Proxy.Deserialize(bytes);
        if ((num & 4) != 0)
          strikeItemWeaponView.Name = StringProxy.Deserialize(bytes);
        if ((num & 8) != 0)
          strikeItemWeaponView.Prices = (ICollection<ItemPrice>) ListProxy<ItemPrice>.Deserialize(bytes, new ListProxy<ItemPrice>.Deserializer<ItemPrice>(ItemPriceProxy.Deserialize));
        strikeItemWeaponView.ProjectileSpeed = Int32Proxy.Deserialize(bytes);
        strikeItemWeaponView.ProjectilesPerShot = Int32Proxy.Deserialize(bytes);
        strikeItemWeaponView.RateOfFire = Int32Proxy.Deserialize(bytes);
        strikeItemWeaponView.RecoilKickback = Int32Proxy.Deserialize(bytes);
        strikeItemWeaponView.RecoilMovement = Int32Proxy.Deserialize(bytes);
        strikeItemWeaponView.ShopHighlightType = EnumProxy<ItemShopHighlightType>.Deserialize(bytes);
        strikeItemWeaponView.SplashRadius = Int32Proxy.Deserialize(bytes);
        strikeItemWeaponView.StartAmmo = Int32Proxy.Deserialize(bytes);
      }
      return strikeItemWeaponView;
    }
  }
}
