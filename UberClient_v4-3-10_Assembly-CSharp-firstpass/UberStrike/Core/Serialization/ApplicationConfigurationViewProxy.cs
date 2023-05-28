// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ApplicationConfigurationViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.Core.Models.Views;

namespace UberStrike.Core.Serialization
{
  public static class ApplicationConfigurationViewProxy
  {
    public static void Serialize(Stream stream, ApplicationConfigurationView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.MaxLevel);
        Int32Proxy.Serialize((Stream) bytes, instance.MaxXp);
        Int32Proxy.Serialize((Stream) bytes, instance.PointsBaseLoser);
        Int32Proxy.Serialize((Stream) bytes, instance.PointsBaseWinner);
        Int32Proxy.Serialize((Stream) bytes, instance.PointsHeadshot);
        Int32Proxy.Serialize((Stream) bytes, instance.PointsKill);
        Int32Proxy.Serialize((Stream) bytes, instance.PointsNutshot);
        Int32Proxy.Serialize((Stream) bytes, instance.PointsPerMinuteLoser);
        Int32Proxy.Serialize((Stream) bytes, instance.PointsPerMinuteWinner);
        Int32Proxy.Serialize((Stream) bytes, instance.PointsSmackdown);
        Int32Proxy.Serialize((Stream) bytes, instance.XpBaseLoser);
        Int32Proxy.Serialize((Stream) bytes, instance.XpBaseWinner);
        Int32Proxy.Serialize((Stream) bytes, instance.XpHeadshot);
        Int32Proxy.Serialize((Stream) bytes, instance.XpKill);
        Int32Proxy.Serialize((Stream) bytes, instance.XpNutshot);
        Int32Proxy.Serialize((Stream) bytes, instance.XpPerMinuteLoser);
        Int32Proxy.Serialize((Stream) bytes, instance.XpPerMinuteWinner);
        if (instance.XpRequiredPerLevel != null)
          DictionaryProxy<int, int>.Serialize((Stream) bytes, instance.XpRequiredPerLevel, new DictionaryProxy<int, int>.Serializer<int>(Int32Proxy.Serialize), new DictionaryProxy<int, int>.Serializer<int>(Int32Proxy.Serialize));
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.XpSmackdown);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static ApplicationConfigurationView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ApplicationConfigurationView configurationView = new ApplicationConfigurationView();
      configurationView.MaxLevel = Int32Proxy.Deserialize(bytes);
      configurationView.MaxXp = Int32Proxy.Deserialize(bytes);
      configurationView.PointsBaseLoser = Int32Proxy.Deserialize(bytes);
      configurationView.PointsBaseWinner = Int32Proxy.Deserialize(bytes);
      configurationView.PointsHeadshot = Int32Proxy.Deserialize(bytes);
      configurationView.PointsKill = Int32Proxy.Deserialize(bytes);
      configurationView.PointsNutshot = Int32Proxy.Deserialize(bytes);
      configurationView.PointsPerMinuteLoser = Int32Proxy.Deserialize(bytes);
      configurationView.PointsPerMinuteWinner = Int32Proxy.Deserialize(bytes);
      configurationView.PointsSmackdown = Int32Proxy.Deserialize(bytes);
      configurationView.XpBaseLoser = Int32Proxy.Deserialize(bytes);
      configurationView.XpBaseWinner = Int32Proxy.Deserialize(bytes);
      configurationView.XpHeadshot = Int32Proxy.Deserialize(bytes);
      configurationView.XpKill = Int32Proxy.Deserialize(bytes);
      configurationView.XpNutshot = Int32Proxy.Deserialize(bytes);
      configurationView.XpPerMinuteLoser = Int32Proxy.Deserialize(bytes);
      configurationView.XpPerMinuteWinner = Int32Proxy.Deserialize(bytes);
      if ((num & 1) != 0)
        configurationView.XpRequiredPerLevel = DictionaryProxy<int, int>.Deserialize(bytes, new DictionaryProxy<int, int>.Deserializer<int>(Int32Proxy.Deserialize), new DictionaryProxy<int, int>.Deserializer<int>(Int32Proxy.Deserialize));
      configurationView.XpSmackdown = Int32Proxy.Deserialize(bytes);
      return configurationView;
    }
  }
}
