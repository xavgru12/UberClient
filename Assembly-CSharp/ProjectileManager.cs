// Decompiled with JetBrains decompiler
// Type: ProjectileManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager>
{
  private Dictionary<int, IProjectile> _projectiles;
  private List<int> _limitedProjectiles;

  private ProjectileManager()
  {
    this._projectiles = new Dictionary<int, IProjectile>();
    this._limitedProjectiles = new List<int>();
  }

  public static GameObject ProjectileContainer { get; set; }

  public static void CreateContainer()
  {
    if ((bool) (UnityEngine.Object) ProjectileManager.ProjectileContainer)
      ProjectileManager.DestroyContainer();
    ProjectileManager.ProjectileContainer = new GameObject();
    ProjectileManager.ProjectileContainer.name = "Projectiles";
  }

  public static void DestroyContainer() => UnityEngine.Object.Destroy((UnityEngine.Object) ProjectileManager.ProjectileContainer);

  public IEnumerable<KeyValuePair<int, IProjectile>> AllProjectiles => (IEnumerable<KeyValuePair<int, IProjectile>>) this._projectiles;

  public IEnumerable<int> LimitedProjectiles => (IEnumerable<int>) this._limitedProjectiles;

  public void AddProjectile(IProjectile p, int id)
  {
    if (p == null)
      return;
    p.ID = id;
    this._projectiles.Add(p.ID, p);
  }

  public void AddLimitedProjectile(IProjectile p, int id, int count)
  {
    if (p == null)
      return;
    p.ID = id;
    this._projectiles.Add(p.ID, p);
    this._limitedProjectiles.Add(p.ID);
    this.CheckLimitedProjectiles(count);
  }

  private void CheckLimitedProjectiles(int count)
  {
    int[] array = this._limitedProjectiles.ToArray();
    for (int index = 0; index < this._limitedProjectiles.Count - count; ++index)
    {
      this.RemoveProjectile(array[index]);
      GameState.CurrentGame.RemoveProjectile(array[index], true);
    }
  }

  public void RemoveAllLimitedProjectiles(bool explode = true)
  {
    int[] array = this._limitedProjectiles.ToArray();
    for (int index = 0; index < array.Length; ++index)
    {
      this.RemoveProjectile(array[index], explode);
      GameState.CurrentGame.RemoveProjectile(array[index], explode);
    }
  }

  public void RemoveProjectile(int id, bool explode = true)
  {
    try
    {
      IProjectile projectile;
      if (!this._projectiles.TryGetValue(id, out projectile))
        return;
      if (explode)
        projectile.Explode();
      else
        projectile.Destroy();
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex.Message);
    }
    finally
    {
      this._limitedProjectiles.RemoveAll((Predicate<int>) (i => i == id));
      this._projectiles.Remove(id);
    }
  }

  public void RemoveAllProjectilesFromPlayer(byte playerNumber)
  {
    foreach (int key in this._projectiles.KeyArray<int, IProjectile>())
    {
      if ((key & (int) byte.MaxValue) == (int) playerNumber)
        this.RemoveProjectile(key, false);
    }
  }

  public void ClearAll()
  {
    try
    {
      foreach (KeyValuePair<int, IProjectile> projectile in this._projectiles)
      {
        if (projectile.Value != null)
          projectile.Value.Destroy();
      }
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex.Message);
    }
    finally
    {
      this._projectiles.Clear();
    }
  }

  public static int CreateGlobalProjectileID(byte playerNumber, int localProjectileId) => (localProjectileId << 8) + (int) playerNumber;

  public static string PrintID(int id) => ProjectileManager.GetPlayerId(id).ToString() + "/" + (object) (id >> 8);

  private static int GetPlayerId(int projectileId) => projectileId & (int) byte.MaxValue;
}
