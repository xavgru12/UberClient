// Decompiled with JetBrains decompiler
// Type: QuickItemConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Models.Views;

public class QuickItemConfiguration : UberStrikeItemQuickView
{
  [CustomProperty("Amount")]
  private int _totalAmount;
  [CustomProperty("RechargeTime")]
  private int _rechargeTime;
  [CustomProperty("SlowdownOnCharge")]
  private float _slowdownOnCharge = 2f;

  public int AmountRemaining
  {
    get => this._totalAmount;
    set => this._totalAmount = value;
  }

  public int RechargeTime => this._rechargeTime;

  public float SlowdownOnCharge => this._slowdownOnCharge;
}
