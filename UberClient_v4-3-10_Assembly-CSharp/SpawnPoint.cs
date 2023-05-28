// Decompiled with JetBrains decompiler
// Type: SpawnPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
  [SerializeField]
  private bool DrawGizmos = true;
  [SerializeField]
  private float Radius = 1f;
  [SerializeField]
  public TeamID TeamPoint;
  [SerializeField]
  public GameMode GameMode;

  public Vector3 Position => this.transform.position;

  public Vector2 Rotation => new Vector2(this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.x);

  public TeamID TeamId => this.TeamPoint;

  public float SpawnRadius => this.Radius;

  private void OnDrawGizmos()
  {
    if (!this.DrawGizmos)
      return;
    switch (this.TeamPoint)
    {
      case TeamID.NONE:
        Gizmos.color = Color.green;
        break;
      case TeamID.BLUE:
        Gizmos.color = Color.blue;
        break;
      case TeamID.RED:
        Gizmos.color = Color.red;
        break;
    }
    Gizmos.matrix = Matrix4x4.TRS(this.transform.position, Quaternion.identity, new Vector3(1f, 0.1f, 1f));
    Gizmos.DrawSphere(Vector3.zero, this.Radius);
    switch (this.GameMode)
    {
      case GameMode.TeamDeathMatch:
        Gizmos.color = Color.white;
        break;
      case GameMode.DeathMatch:
        Gizmos.color = Color.yellow;
        break;
      case GameMode.TeamElimination:
        Gizmos.color = Color.magenta;
        break;
    }
    Gizmos.matrix = Matrix4x4.identity;
    Gizmos.DrawLine(this.transform.position + this.transform.forward * this.Radius, this.transform.position + this.transform.forward * 2f * this.Radius);
  }
}
