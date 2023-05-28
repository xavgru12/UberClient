// Decompiled with JetBrains decompiler
// Type: QuickItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public abstract class QuickItem : BaseUnityItem
{
  [SerializeField]
  private QuickItemLogic _logic;
  [SerializeField]
  private QuickItemSfx _sfx;

  public QuickItemLogic Logic => this._logic;

  public QuickItemSfx Sfx => this._sfx;

  public abstract QuickItemConfiguration Configuration { get; set; }

  public override BaseUberStrikeItemView ItemView => (BaseUberStrikeItemView) this.Configuration;

  public QuickItemBehaviour Behaviour { get; set; }

  protected abstract void OnActivated();

  private void Awake() => this.Behaviour = new QuickItemBehaviour(this, new Action(this.OnActivated));
}
