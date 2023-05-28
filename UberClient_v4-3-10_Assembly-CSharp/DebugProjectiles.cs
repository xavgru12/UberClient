// Decompiled with JetBrains decompiler
// Type: DebugProjectiles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class DebugProjectiles : IDebugPage
{
  private Vector2 scroll1;
  private Vector2 scroll2;

  public string Title => "Projectiles";

  public void Draw()
  {
    this.scroll1 = GUILayout.BeginScrollView(this.scroll1);
    foreach (KeyValuePair<int, IProjectile> allProjectile in Singleton<ProjectileManager>.Instance.AllProjectiles)
      GUILayout.Label(allProjectile.Key.ToString() + " - " + (object) allProjectile.Value == null ? ProjectileManager.PrintID(allProjectile.Key) + " (exploded zombie)" : ProjectileManager.PrintID(allProjectile.Key));
    GUILayout.EndScrollView();
    GUILayout.Space(30f);
    this.scroll2 = GUILayout.BeginScrollView(this.scroll2);
    foreach (int limitedProjectile in Singleton<ProjectileManager>.Instance.LimitedProjectiles)
      GUILayout.Label("Limited " + ProjectileManager.PrintID(limitedProjectile));
    GUILayout.EndScrollView();
  }
}
