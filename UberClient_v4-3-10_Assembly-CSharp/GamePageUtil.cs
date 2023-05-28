// Decompiled with JetBrains decompiler
// Type: GamePageUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class GamePageUtil
{
  public static void SpawnLocalAvatar()
  {
    if (!(bool) (Object) GameState.LocalDecorator)
      return;
    Vector3 pos;
    Quaternion rot;
    GamePageUtil.GetAvatarSpawnPoint(out pos, out rot);
    AutoMonoBehaviour<AvatarAnimationManager>.Instance.ResetAnimationState(PageType.Shop);
    GameState.LocalDecorator.HideWeapons();
    GameState.LocalDecorator.SetPosition(pos, rot);
    GameState.LocalDecorator.MeshRenderer.enabled = true;
    GameState.LocalDecorator.SetLayers(UberstrikeLayer.RemotePlayer);
    GameState.LocalPlayer.transform.position = pos + Vector3.up * 2f;
  }

  private static void GetAvatarSpawnPoint(out Vector3 pos, out Quaternion rot)
  {
    Singleton<SpawnPointManager>.Instance.GetSpawnPointAt(GameState.CurrentSpace.DefaultSpawnPoint, GameMode.DeathMatch, TeamID.NONE, out pos, out rot);
    RaycastHit hitInfo;
    if (Physics.Raycast(pos, Vector3.down, out hitInfo, 5f))
      pos = hitInfo.point;
    rot = GamePageUtil.GetCollisionFreeRotation(pos);
  }

  private static Quaternion GetCollisionFreeRotation(Vector3 pos)
  {
    float num1 = Random.Range(0.0f, 45f);
    float num2 = 0.0f;
    Quaternion collisionFreeRotation = Quaternion.identity;
    for (int index = 0; index < 8; ++index)
    {
      Quaternion quaternion = Quaternion.Euler(0.0f, (float) (index * 45) + num1, 0.0f);
      RaycastHit hitInfo;
      if (Physics.Raycast(pos + Vector3.up * 1.5f, quaternion * LevelCamera.OverviewState.ViewDirection, out hitInfo, 7f, UberstrikeLayerMasks.ShootMask))
      {
        if ((double) hitInfo.distance > (double) num2)
        {
          num2 = hitInfo.distance;
          collisionFreeRotation = quaternion;
        }
      }
      else
      {
        collisionFreeRotation = quaternion;
        break;
      }
    }
    return collisionFreeRotation;
  }
}
