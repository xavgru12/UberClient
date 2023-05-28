// Decompiled with JetBrains decompiler
// Type: BaseWeaponEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class BaseWeaponEffect : MonoBehaviour
{
  protected BaseWeaponDecorator _decorator;

  public void SetDecorator(BaseWeaponDecorator decorator) => this._decorator = decorator;

  public abstract void OnShoot();

  public abstract void OnPostShoot();

  public abstract void Hide();
}
