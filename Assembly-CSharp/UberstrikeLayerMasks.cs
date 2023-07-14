// Decompiled with JetBrains decompiler
// Type: UberstrikeLayerMasks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

public static class UberstrikeLayerMasks
{
  public static readonly int CrouchMask = ~LayerUtil.CreateLayerMask(UberstrikeLayer.LocalPlayer, UberstrikeLayer.Controller, UberstrikeLayer.IgnoreRaycast, UberstrikeLayer.Trigger, UberstrikeLayer.TransparentFX);
  public static readonly int ShootMask = ~LayerUtil.CreateLayerMask(UberstrikeLayer.Ragdoll, UberstrikeLayer.LocalPlayer, UberstrikeLayer.Controller, UberstrikeLayer.IgnoreRaycast, UberstrikeLayer.Trigger, UberstrikeLayer.TransparentFX, UberstrikeLayer.Raidbot);
  public static readonly int ShootMaskRemotePlayer = ~LayerUtil.CreateLayerMask(UberstrikeLayer.Controller, UberstrikeLayer.IgnoreRaycast, UberstrikeLayer.Trigger, UberstrikeLayer.TransparentFX, UberstrikeLayer.Raidbot);
  public static readonly int RemoteRocketMask = ~LayerUtil.CreateLayerMask(UberstrikeLayer.RemotePlayer, UberstrikeLayer.Controller, UberstrikeLayer.Teleporter, UberstrikeLayer.LocalProjectile, UberstrikeLayer.RemoteProjectile, UberstrikeLayer.IgnoreRaycast, UberstrikeLayer.Trigger, UberstrikeLayer.TransparentFX);
  public static readonly int LocalRocketMask = ~LayerUtil.CreateLayerMask(UberstrikeLayer.LocalPlayer, UberstrikeLayer.Controller, UberstrikeLayer.Teleporter, UberstrikeLayer.LocalProjectile, UberstrikeLayer.RemoteProjectile, UberstrikeLayer.IgnoreRaycast, UberstrikeLayer.Trigger, UberstrikeLayer.TransparentFX, UberstrikeLayer.Raidbot);
  public static readonly int ExplosionMask = LayerUtil.CreateLayerMask(UberstrikeLayer.LocalPlayer, UberstrikeLayer.RemotePlayer, UberstrikeLayer.Props, UberstrikeLayer.Raidbot);
  public static readonly int GrenadeCollisionMask = LayerUtil.CreateLayerMask(UberstrikeLayer.RemotePlayer);
  public static readonly int GrenadeMask = LayerUtil.CreateLayerMask(UberstrikeLayer.LocalPlayer, UberstrikeLayer.RemotePlayer, UberstrikeLayer.Props, UberstrikeLayer.Raidbot);
  public static readonly int ProtectionMask = ~LayerUtil.CreateLayerMask(UberstrikeLayer.LocalProjectile, UberstrikeLayer.RemoteProjectile, UberstrikeLayer.LocalPlayer, UberstrikeLayer.RemotePlayer, UberstrikeLayer.Controller, UberstrikeLayer.Props, UberstrikeLayer.Ragdoll, UberstrikeLayer.IgnoreRaycast, UberstrikeLayer.Trigger, UberstrikeLayer.TransparentFX);
  public static readonly int WaterMask = LayerUtil.CreateLayerMask(UberstrikeLayer.Water);
}
