// Decompiled with JetBrains decompiler
// Type: ReticleRepository
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Core.Types;
using UnityEngine;

internal class ReticleRepository : Singleton<ReticleRepository>
{
  private Dictionary<UberstrikeItemClass, Reticle> _reticles;

  private ReticleRepository() => this.InitReticles();

  public Reticle GetReticle(UberstrikeItemClass type) => this._reticles.ContainsKey(type) ? this._reticles[type] : (Reticle) null;

  public void UpdateAllReticles()
  {
    foreach (Reticle reticle in this._reticles.Values)
      reticle.Update();
  }

  private void InitReticles()
  {
    this._reticles = new Dictionary<UberstrikeItemClass, Reticle>(8);
    Reticle reticle1 = new Reticle();
    reticle1.SetInnerScale((Texture) HudTextures.MGScale, 1.2f);
    reticle1.SetTranslate((Texture) HudTextures.MGTranslate, 5f);
    Reticle reticle2 = new Reticle();
    reticle2.SetInnerScale((Texture) HudTextures.SRScale, 1.2f);
    reticle2.SetTranslate((Texture) HudTextures.SRTranslate, 5f);
    Reticle reticle3 = new Reticle();
    reticle3.SetInnerScale((Texture) HudTextures.SGScaleInside, 1.2f);
    reticle3.SetOutterScale((Texture) HudTextures.SGScaleOutside, 2f);
    Reticle reticle4 = new Reticle();
    reticle4.SetRotate((Texture) HudTextures.CNRotate, 60f);
    reticle4.SetInnerScale((Texture) HudTextures.CNScale, 1.5f);
    Reticle reticle5 = new Reticle();
    reticle5.SetTranslate((Texture) HudTextures.HGTraslate, 5f);
    Reticle reticle6 = new Reticle();
    reticle6.SetInnerScale((Texture) HudTextures.SPScale, 1.2f);
    reticle6.SetTranslate((Texture) HudTextures.SPTranslate, 5f);
    Reticle reticle7 = new Reticle();
    reticle7.SetInnerScale((Texture) HudTextures.LRScale, 1.2f);
    reticle7.SetTranslate((Texture) HudTextures.LRTranslate, 5f);
    Reticle reticle8 = new Reticle();
    reticle8.SetTranslate((Texture) HudTextures.MWTranslate, 5f);
    this._reticles.Add(UberstrikeItemClass.WeaponMachinegun, reticle1);
    this._reticles.Add(UberstrikeItemClass.WeaponSniperRifle, reticle2);
    this._reticles.Add(UberstrikeItemClass.WeaponShotgun, reticle3);
    this._reticles.Add(UberstrikeItemClass.WeaponCannon, reticle4);
    this._reticles.Add(UberstrikeItemClass.WeaponHandgun, reticle5);
    this._reticles.Add(UberstrikeItemClass.WeaponSplattergun, reticle6);
    this._reticles.Add(UberstrikeItemClass.WeaponLauncher, reticle7);
    this._reticles.Add(UberstrikeItemClass.WeaponMelee, reticle8);
  }
}
