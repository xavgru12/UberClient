// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.LoadoutViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.Core.Types;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class LoadoutViewProxy
  {
    public static void Serialize(Stream stream, LoadoutView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.Backpack);
        Int32Proxy.Serialize((Stream) bytes, instance.Boots);
        Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
        Int32Proxy.Serialize((Stream) bytes, instance.Face);
        Int32Proxy.Serialize((Stream) bytes, instance.FunctionalItem1);
        Int32Proxy.Serialize((Stream) bytes, instance.FunctionalItem2);
        Int32Proxy.Serialize((Stream) bytes, instance.FunctionalItem3);
        Int32Proxy.Serialize((Stream) bytes, instance.Gloves);
        Int32Proxy.Serialize((Stream) bytes, instance.Head);
        Int32Proxy.Serialize((Stream) bytes, instance.LoadoutId);
        Int32Proxy.Serialize((Stream) bytes, instance.LowerBody);
        Int32Proxy.Serialize((Stream) bytes, instance.MeleeWeapon);
        Int32Proxy.Serialize((Stream) bytes, instance.QuickItem1);
        Int32Proxy.Serialize((Stream) bytes, instance.QuickItem2);
        Int32Proxy.Serialize((Stream) bytes, instance.QuickItem3);
        if (instance.SkinColor != null)
          StringProxy.Serialize((Stream) bytes, instance.SkinColor);
        else
          num |= 1;
        EnumProxy<AvatarType>.Serialize((Stream) bytes, instance.Type);
        Int32Proxy.Serialize((Stream) bytes, instance.UpperBody);
        Int32Proxy.Serialize((Stream) bytes, instance.Weapon1);
        Int32Proxy.Serialize((Stream) bytes, instance.Weapon1Mod1);
        Int32Proxy.Serialize((Stream) bytes, instance.Weapon1Mod2);
        Int32Proxy.Serialize((Stream) bytes, instance.Weapon1Mod3);
        Int32Proxy.Serialize((Stream) bytes, instance.Weapon2);
        Int32Proxy.Serialize((Stream) bytes, instance.Weapon2Mod1);
        Int32Proxy.Serialize((Stream) bytes, instance.Weapon2Mod2);
        Int32Proxy.Serialize((Stream) bytes, instance.Weapon2Mod3);
        Int32Proxy.Serialize((Stream) bytes, instance.Weapon3);
        Int32Proxy.Serialize((Stream) bytes, instance.Weapon3Mod1);
        Int32Proxy.Serialize((Stream) bytes, instance.Weapon3Mod2);
        Int32Proxy.Serialize((Stream) bytes, instance.Weapon3Mod3);
        Int32Proxy.Serialize((Stream) bytes, instance.Webbing);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static LoadoutView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      LoadoutView loadoutView = new LoadoutView();
      loadoutView.Backpack = Int32Proxy.Deserialize(bytes);
      loadoutView.Boots = Int32Proxy.Deserialize(bytes);
      loadoutView.Cmid = Int32Proxy.Deserialize(bytes);
      loadoutView.Face = Int32Proxy.Deserialize(bytes);
      loadoutView.FunctionalItem1 = Int32Proxy.Deserialize(bytes);
      loadoutView.FunctionalItem2 = Int32Proxy.Deserialize(bytes);
      loadoutView.FunctionalItem3 = Int32Proxy.Deserialize(bytes);
      loadoutView.Gloves = Int32Proxy.Deserialize(bytes);
      loadoutView.Head = Int32Proxy.Deserialize(bytes);
      loadoutView.LoadoutId = Int32Proxy.Deserialize(bytes);
      loadoutView.LowerBody = Int32Proxy.Deserialize(bytes);
      loadoutView.MeleeWeapon = Int32Proxy.Deserialize(bytes);
      loadoutView.QuickItem1 = Int32Proxy.Deserialize(bytes);
      loadoutView.QuickItem2 = Int32Proxy.Deserialize(bytes);
      loadoutView.QuickItem3 = Int32Proxy.Deserialize(bytes);
      if ((num & 1) != 0)
        loadoutView.SkinColor = StringProxy.Deserialize(bytes);
      loadoutView.Type = EnumProxy<AvatarType>.Deserialize(bytes);
      loadoutView.UpperBody = Int32Proxy.Deserialize(bytes);
      loadoutView.Weapon1 = Int32Proxy.Deserialize(bytes);
      loadoutView.Weapon1Mod1 = Int32Proxy.Deserialize(bytes);
      loadoutView.Weapon1Mod2 = Int32Proxy.Deserialize(bytes);
      loadoutView.Weapon1Mod3 = Int32Proxy.Deserialize(bytes);
      loadoutView.Weapon2 = Int32Proxy.Deserialize(bytes);
      loadoutView.Weapon2Mod1 = Int32Proxy.Deserialize(bytes);
      loadoutView.Weapon2Mod2 = Int32Proxy.Deserialize(bytes);
      loadoutView.Weapon2Mod3 = Int32Proxy.Deserialize(bytes);
      loadoutView.Weapon3 = Int32Proxy.Deserialize(bytes);
      loadoutView.Weapon3Mod1 = Int32Proxy.Deserialize(bytes);
      loadoutView.Weapon3Mod2 = Int32Proxy.Deserialize(bytes);
      loadoutView.Weapon3Mod3 = Int32Proxy.Deserialize(bytes);
      loadoutView.Webbing = Int32Proxy.Deserialize(bytes);
      return loadoutView;
    }
  }
}
