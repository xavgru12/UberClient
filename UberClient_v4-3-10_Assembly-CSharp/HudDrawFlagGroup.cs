// Decompiled with JetBrains decompiler
// Type: HudDrawFlagGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

internal class HudDrawFlagGroup : Singleton<HudDrawFlagGroup>
{
  private const HudDrawFlags screenshotDrawFlagTuning = HudDrawFlags.None;
  private const HudDrawFlags tabScreenDrawFlagsTuning = HudDrawFlags.Reticle;
  private HudDrawFlags _baseDrawFlag;
  private HashSet<HudDrawFlags> _drawFlagTunings;
  private bool _isScreenShotMode;

  private HudDrawFlagGroup() => this._drawFlagTunings = new HashSet<HudDrawFlags>();

  public HudDrawFlags BaseDrawFlag
  {
    get => this._baseDrawFlag;
    set
    {
      this._baseDrawFlag = value;
      this.UpdateDrawFlags();
    }
  }

  public bool IsScreenshotMode
  {
    get => this._isScreenShotMode;
    set
    {
      this._isScreenShotMode = value;
      if (value)
        this._drawFlagTunings.Add(HudDrawFlags.None);
      else
        this._drawFlagTunings.Remove(HudDrawFlags.None);
      this.UpdateDrawFlags();
    }
  }

  public bool TuningTabScreen
  {
    set
    {
      if (value)
        this._drawFlagTunings.Add(HudDrawFlags.Reticle);
      else
        this._drawFlagTunings.Remove(HudDrawFlags.Reticle);
      this.UpdateDrawFlags();
    }
  }

  public void AddFlag(HudDrawFlags drawFlag)
  {
    this._drawFlagTunings.Add(drawFlag);
    this.UpdateDrawFlags();
  }

  public void RemoveFlag(HudDrawFlags drawFlag)
  {
    if (!this._drawFlagTunings.Contains(drawFlag))
      return;
    this._drawFlagTunings.Remove(drawFlag);
    this.UpdateDrawFlags();
  }

  public void ClearFlags()
  {
    this._drawFlagTunings.Clear();
    this.UpdateDrawFlags();
  }

  public HudDrawFlags GetConsolidatedFlag()
  {
    HudDrawFlags consolidatedFlag = ~HudDrawFlags.None & this.BaseDrawFlag;
    foreach (HudDrawFlags drawFlagTuning in this._drawFlagTunings)
      consolidatedFlag &= drawFlagTuning;
    return consolidatedFlag;
  }

  private void UpdateDrawFlags() => HudController.Instance.DrawFlags = this.GetConsolidatedFlag();
}
