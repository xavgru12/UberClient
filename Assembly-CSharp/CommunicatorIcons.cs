// Decompiled with JetBrains decompiler
// Type: CommunicatorIcons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class CommunicatorIcons
{
  static CommunicatorIcons()
  {
    Texture2DConfigurator component;
    try
    {
      component = GameObject.Find(nameof (CommunicatorIcons)).GetComponent<Texture2DConfigurator>();
    }
    catch
    {
      Debug.LogError((object) "Missing instance of the prefab with name: CommunicatorIcons!");
      return;
    }
    CommunicatorIcons.ChannelPortal16x16 = component.Assets[0];
    CommunicatorIcons.ChannelFacebook16x16 = component.Assets[1];
    CommunicatorIcons.ChannelWindows16x16 = component.Assets[2];
    CommunicatorIcons.ChannelApple16x16 = component.Assets[3];
    CommunicatorIcons.ChannelKongregate16x16 = component.Assets[4];
    CommunicatorIcons.ChannelAndroid16x16 = component.Assets[5];
    CommunicatorIcons.ChannelIos16x16 = component.Assets[6];
    CommunicatorIcons.PresenceOffline = component.Assets[7];
    CommunicatorIcons.PresenceOnline = component.Assets[8];
    CommunicatorIcons.PresencePlaying = component.Assets[9];
    CommunicatorIcons.NewInboxMessage = component.Assets[10];
  }

  public static Texture2D ChannelPortal16x16 { get; private set; }

  public static Texture2D ChannelFacebook16x16 { get; private set; }

  public static Texture2D ChannelWindows16x16 { get; private set; }

  public static Texture2D ChannelApple16x16 { get; private set; }

  public static Texture2D ChannelKongregate16x16 { get; private set; }

  public static Texture2D ChannelAndroid16x16 { get; private set; }

  public static Texture2D ChannelIos16x16 { get; private set; }

  public static Texture2D PresenceOffline { get; private set; }

  public static Texture2D PresenceOnline { get; private set; }

  public static Texture2D PresencePlaying { get; private set; }

  public static Texture2D NewInboxMessage { get; private set; }
}
