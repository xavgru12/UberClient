// Decompiled with JetBrains decompiler
// Type: QuickItemRestriction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class QuickItemRestriction
{
  private QuickItemRestriction.RestrictedUsage[] _quickItemRestrictions;

  public QuickItemRestriction()
  {
    this._quickItemRestrictions = new QuickItemRestriction.RestrictedUsage[LoadoutManager.QuickSlots.Length];
    for (int index = 0; index < this._quickItemRestrictions.Length; ++index)
      this._quickItemRestrictions[index] = new QuickItemRestriction.RestrictedUsage();
  }

  public bool IsEnabled { get; set; }

  public void InitializeSlot(int index, QuickItem quickItem = null, int amountRemaining = 0)
  {
    if (index < 0 || index >= this._quickItemRestrictions.Length)
      return;
    if (this.IsEnabled && (Object) quickItem != (Object) null)
    {
      if (quickItem.Configuration.ID != this._quickItemRestrictions[index].ItemId)
        this._quickItemRestrictions[index].Init(quickItem);
    }
    else
      this._quickItemRestrictions[index].Init();
    this._quickItemRestrictions[index].RenewLifeUses();
    if (!((Object) quickItem != (Object) null))
      return;
    int num = Mathf.Min(amountRemaining, this._quickItemRestrictions[index].RemainingLifeUses);
    quickItem.Behaviour.CurrentAmount = num;
    quickItem.Configuration.AmountRemaining = num;
  }

  public void RenewGameUses()
  {
    foreach (QuickItemRestriction.RestrictedUsage quickItemRestriction in this._quickItemRestrictions)
      quickItemRestriction.RenewGameUses();
  }

  public void RenewRoundUses()
  {
    foreach (QuickItemRestriction.RestrictedUsage quickItemRestriction in this._quickItemRestrictions)
      quickItemRestriction.RenewRoundUses();
  }

  public void RenewLifeUses()
  {
    foreach (QuickItemRestriction.RestrictedUsage quickItemRestriction in this._quickItemRestrictions)
      quickItemRestriction.RenewLifeUses();
  }

  public void DecreaseUse(int index)
  {
    if (!this.IsEnabled || index >= this._quickItemRestrictions.Length || index < 0)
      return;
    this._quickItemRestrictions[index].DecreaseUse();
  }

  public int GetCurrentAvailableAmount(int index, int inventoryRemainingAmount)
  {
    if (!this.IsEnabled || index >= this._quickItemRestrictions.Length || index < 0)
      return inventoryRemainingAmount;
    QuickItemRestriction.RestrictedUsage quickItemRestriction = this._quickItemRestrictions[index];
    return inventoryRemainingAmount >= quickItemRestriction.RemainingLifeUses ? quickItemRestriction.RemainingLifeUses : inventoryRemainingAmount;
  }

  private class RestrictedUsage
  {
    private int _totalUsesPerGame;
    private int _totalUsesPerRound;
    private int _totalUsesPerLife;

    public int RemainingGameUses { get; private set; }

    public int RemainingRoundUses { get; private set; }

    public int RemainingLifeUses { get; private set; }

    public int ItemId { get; private set; }

    public void Init(QuickItem item = null)
    {
      if ((Object) item != (Object) null)
      {
        this.ItemId = item.Configuration.ID;
        this._totalUsesPerGame = item.Configuration.UsesPerGame <= 0 ? int.MaxValue : item.Configuration.UsesPerGame;
        this._totalUsesPerRound = item.Configuration.UsesPerRound <= 0 ? int.MaxValue : item.Configuration.UsesPerRound;
        this._totalUsesPerLife = item.Configuration.UsesPerLife <= 0 ? int.MaxValue : item.Configuration.UsesPerLife;
      }
      else
      {
        this.ItemId = 0;
        this._totalUsesPerGame = int.MaxValue;
        this._totalUsesPerRound = int.MaxValue;
        this._totalUsesPerLife = int.MaxValue;
      }
      this.RenewGameUses();
    }

    public void RenewGameUses()
    {
      this.RemainingGameUses = this._totalUsesPerGame;
      this.RemainingRoundUses = this._totalUsesPerRound;
      this.RemainingLifeUses = this._totalUsesPerLife;
    }

    public void RenewRoundUses()
    {
      this.RemainingRoundUses = Mathf.Min(this._totalUsesPerRound, this.RemainingGameUses);
      this.RenewLifeUses();
    }

    public void RenewLifeUses() => this.RemainingLifeUses = Mathf.Min(this._totalUsesPerLife, this.RemainingRoundUses);

    public void DecreaseUse()
    {
      this.RemainingGameUses = Mathf.Max(this.RemainingGameUses - 1, 0);
      this.RemainingRoundUses = Mathf.Max(this.RemainingRoundUses - 1, 0);
      this.RemainingLifeUses = Mathf.Max(this.RemainingLifeUses - 1, 0);
    }
  }
}
