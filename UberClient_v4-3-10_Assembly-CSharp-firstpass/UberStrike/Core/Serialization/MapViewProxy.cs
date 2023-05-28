// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MapViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;

namespace UberStrike.Core.Serialization
{
  public static class MapViewProxy
  {
    public static void Serialize(Stream stream, MapView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        if (instance.Description != null)
          StringProxy.Serialize((Stream) bytes, instance.Description);
        else
          num |= 1;
        if (instance.DisplayName != null)
          StringProxy.Serialize((Stream) bytes, instance.DisplayName);
        else
          num |= 2;
        EnumProxy<GameBoxType>.Serialize((Stream) bytes, instance.BoxType);
        Int32Proxy.Serialize((Stream) bytes, instance.MapId);
        Int32Proxy.Serialize((Stream) bytes, instance.MaxPlayers);
        Int32Proxy.Serialize((Stream) bytes, instance.RecommendedItemId);
        if (instance.SceneName != null)
          StringProxy.Serialize((Stream) bytes, instance.SceneName);
        else
          num |= 4;
        if (instance.Settings != null)
          DictionaryProxy<GameModeType, MapSettings>.Serialize((Stream) bytes, instance.Settings, new DictionaryProxy<GameModeType, MapSettings>.Serializer<GameModeType>(EnumProxy<GameModeType>.Serialize), new DictionaryProxy<GameModeType, MapSettings>.Serializer<MapSettings>(MapSettingsProxy.Serialize));
        else
          num |= 8;
        Int32Proxy.Serialize((Stream) bytes, instance.SupportedGameModes);
        Int32Proxy.Serialize((Stream) bytes, instance.SupportedItemClass);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static MapView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MapView mapView = new MapView();
      if ((num & 1) != 0)
        mapView.Description = StringProxy.Deserialize(bytes);
      if ((num & 2) != 0)
        mapView.DisplayName = StringProxy.Deserialize(bytes);
      mapView.BoxType = EnumProxy<GameBoxType>.Deserialize(bytes);
      mapView.MapId = Int32Proxy.Deserialize(bytes);
      mapView.MaxPlayers = Int32Proxy.Deserialize(bytes);
      mapView.RecommendedItemId = Int32Proxy.Deserialize(bytes);
      if ((num & 4) != 0)
        mapView.SceneName = StringProxy.Deserialize(bytes);
      if ((num & 8) != 0)
        mapView.Settings = DictionaryProxy<GameModeType, MapSettings>.Deserialize(bytes, new DictionaryProxy<GameModeType, MapSettings>.Deserializer<GameModeType>(EnumProxy<GameModeType>.Deserialize), new DictionaryProxy<GameModeType, MapSettings>.Deserializer<MapSettings>(MapSettingsProxy.Deserialize));
      mapView.SupportedGameModes = Int32Proxy.Deserialize(bytes);
      mapView.SupportedItemClass = Int32Proxy.Deserialize(bytes);
      return mapView;
    }
  }
}
