// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.GameFlags
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace UberStrike.Realtime.UnitySdk
{
  public class GameFlags
  {
    private GameFlags.GAME_FLAGS gameFlags;

    public bool LowGravity
    {
      get => this.IsFlagSet(GameFlags.GAME_FLAGS.LowGravity);
      set => this.SetFlag(GameFlags.GAME_FLAGS.LowGravity, value);
    }

    public bool KnockBack
    {
      get => this.IsFlagSet(GameFlags.GAME_FLAGS.KnockBack);
      set => this.SetFlag(GameFlags.GAME_FLAGS.KnockBack, value);
    }

    public bool NoArmor
    {
      get => this.IsFlagSet(GameFlags.GAME_FLAGS.NoArmor);
      set => this.SetFlag(GameFlags.GAME_FLAGS.NoArmor, value);
    }

    public bool QuickSwitch
    {
      get => this.IsFlagSet(GameFlags.GAME_FLAGS.QuickSwitch);
      set => this.SetFlag(GameFlags.GAME_FLAGS.QuickSwitch, value);
    }

    public bool MeleeOnly
    {
      get => this.IsFlagSet(GameFlags.GAME_FLAGS.MeleeOnly);
      set => this.SetFlag(GameFlags.GAME_FLAGS.MeleeOnly, value);
    }

    public int ToInt() => (int) this.gameFlags;

    public static bool IsFlagSet(GameFlags.GAME_FLAGS flag, int state) => ((GameFlags.GAME_FLAGS) state & flag) != 0;

    private bool IsFlagSet(GameFlags.GAME_FLAGS f) => (this.gameFlags & f) == f;

    private void SetFlag(GameFlags.GAME_FLAGS f, bool b) => this.gameFlags = !b ? this.gameFlags & ~f : this.gameFlags | f;

    public void SetFlags(int flags) => this.gameFlags = (GameFlags.GAME_FLAGS) flags;

    public void ResetFlags() => this.gameFlags = GameFlags.GAME_FLAGS.None;

    [Flags]
    public enum GAME_FLAGS
    {
      None = 0,
      LowGravity = 1,
      NoArmor = 2,
      QuickSwitch = 4,
      MeleeOnly = 8,
      KnockBack = 16, // 0x00000010
    }
  }
}
