// Decompiled with JetBrains decompiler
// Type: InventoryItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class InventoryItem
{
  private IUnityItem _item;

  public InventoryItem(IUnityItem item) => this._item = item;

  public IUnityItem Item => this._item;

  public int DaysRemaining => !this.IsPermanent && this.ExpirationDate.HasValue ? Mathf.CeilToInt((float) this.ExpirationDate.Value.Subtract(ApplicationDataManager.ServerDateTime).TotalHours / 24f) : 0;

  public int AmountRemaining { get; set; }

  public bool IsPermanent { get; set; }

  public DateTime? ExpirationDate { get; set; }

  public bool IsHighlighted { get; set; }

  public bool IsValid => this.IsPermanent || this.DaysRemaining > 0;
}
