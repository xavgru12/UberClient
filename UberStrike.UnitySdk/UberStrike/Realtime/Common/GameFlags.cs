// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Common.GameFlags
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;

namespace UberStrike.Realtime.Common
{
  public class GameFlags
  {
    private GameFlags.GAME_FLAGS gameFlags = GameFlags.GAME_FLAGS.None;

    public bool LowGravity
    {
      get => this.IsFlagSet(GameFlags.GAME_FLAGS.LowGravity);
      set => this.SetFlag(GameFlags.GAME_FLAGS.LowGravity, value);
    }

    public bool Instakill
    {
      get => this.IsFlagSet(GameFlags.GAME_FLAGS.Instakill);
      set => this.SetFlag(GameFlags.GAME_FLAGS.Instakill, value);
    }

    public bool NinjaArena
    {
      get => this.IsFlagSet(GameFlags.GAME_FLAGS.NinjaArena);
      set => this.SetFlag(GameFlags.GAME_FLAGS.NinjaArena, value);
    }

    public bool SniperArena
    {
      get => this.IsFlagSet(GameFlags.GAME_FLAGS.SniperArena);
      set => this.SetFlag(GameFlags.GAME_FLAGS.SniperArena, value);
    }

    public bool CannonArena
    {
      get => this.IsFlagSet(GameFlags.GAME_FLAGS.CannonArena);
      set => this.SetFlag(GameFlags.GAME_FLAGS.CannonArena, value);
    }

    public int ToInt() => (int) this.gameFlags;

    public static bool IsFlagSet(GameFlags.GAME_FLAGS flag, int state) => ((GameFlags.GAME_FLAGS) state & flag) != GameFlags.GAME_FLAGS.None;

    private bool IsFlagSet(GameFlags.GAME_FLAGS f) => (this.gameFlags & f) == f;

    private void SetFlag(GameFlags.GAME_FLAGS f, bool b) => this.gameFlags = b ? this.gameFlags | f : this.gameFlags & ~f;

    public void SetFlags(int flags) => this.gameFlags = (GameFlags.GAME_FLAGS) flags;

    public void ResetFlags() => this.gameFlags = GameFlags.GAME_FLAGS.None;

    [Flags]
    public enum GAME_FLAGS
    {
      None = 0,
      LowGravity = 1,
      Instakill = 2,
      NinjaArena = 4,
      SniperArena = 8,
      CannonArena = 16, // 0x00000010
    }
  }
}
