// Decompiled with JetBrains decompiler
// Type: WeaponDataManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Models.Views;
using UnityEngine;

public static class WeaponDataManager
{
  public static Vector3 ApplyDispersion(
    Vector3 shootingRay,
    WeaponItemConfiguration config,
    bool ironSight)
  {
    float accuracySpread = WeaponConfigurationHelper.GetAccuracySpread((UberStrikeItemWeaponView) config);
    if (WeaponFeedbackManager.Exists && WeaponFeedbackManager.Instance.IsIronSighted && ironSight)
      accuracySpread *= 0.5f;
    Vector2 vector2 = Random.insideUnitCircle * accuracySpread * 0.5f;
    return Quaternion.AngleAxis(vector2.x, GameState.WeaponCameraTransform.right) * Quaternion.AngleAxis(vector2.y, GameState.WeaponCameraTransform.up) * shootingRay;
  }
}
