// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Client.GameRoomOperations
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models;
using UberStrike.Core.Serialization;
using UberStrike.Core.Types;
using UnityEngine;

namespace UberStrike.Realtime.Client
{
  public sealed class GameRoomOperations : IOperationSender
  {
    private byte __id;
    private RemoteProcedureCall sendOperation;

    public event RemoteProcedureCall SendOperation
    {
      add => this.sendOperation += value;
      remove => this.sendOperation -= value;
    }

    public GameRoomOperations(byte id = 0) => this.__id = id;

    public void SendJoinGame(TeamID team)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        EnumProxy<TeamID>.Serialize((Stream) bytes, team);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 1, customOpParameters) ? 1 : 0;
      }
    }

    public void SendJoinAsSpectator()
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) memoryStream.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 2, customOpParameters) ? 1 : 0;
      }
    }

    public void SendPowerUpRespawnTimes(List<ushort> respawnTimes)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        ListProxy<ushort>.Serialize((Stream) bytes, (ICollection<ushort>) respawnTimes, new ListProxy<ushort>.Serializer<ushort>(UInt16Proxy.Serialize));
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 3, customOpParameters) ? 1 : 0;
      }
    }

    public void SendPowerUpPicked(int powerupId, byte type, byte value)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, powerupId);
        ByteProxy.Serialize((Stream) bytes, type);
        ByteProxy.Serialize((Stream) bytes, value);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 4, customOpParameters) ? 1 : 0;
      }
    }

    public void SendIncreaseHealthAndArmor(byte health, byte armor)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        ByteProxy.Serialize((Stream) bytes, health);
        ByteProxy.Serialize((Stream) bytes, armor);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 5, customOpParameters) ? 1 : 0;
      }
    }

    public void SendOpenDoor(int doorId)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, doorId);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 6, customOpParameters) ? 1 : 0;
      }
    }

    public void SendSpawnPositions(TeamID team, List<Vector3> positions, List<byte> rotations)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        EnumProxy<TeamID>.Serialize((Stream) bytes, team);
        ListProxy<Vector3>.Serialize((Stream) bytes, (ICollection<Vector3>) positions, new ListProxy<Vector3>.Serializer<Vector3>(Vector3Proxy.Serialize));
        ListProxy<byte>.Serialize((Stream) bytes, (ICollection<byte>) rotations, new ListProxy<byte>.Serializer<byte>(ByteProxy.Serialize));
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 7, customOpParameters) ? 1 : 0;
      }
    }

    public void SendRespawnRequest()
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) memoryStream.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 8, customOpParameters) ? 1 : 0;
      }
    }

    public void SendDirectHitDamage(int target, byte bodyPart, byte bullets, byte weaponSlot)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, target);
        ByteProxy.Serialize((Stream) bytes, bodyPart);
        ByteProxy.Serialize((Stream) bytes, bullets);
        ByteProxy.Serialize((Stream) bytes, weaponSlot);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 9, customOpParameters) ? 1 : 0;
      }
    }

    public void SendExplosionDamage(int target, byte slot, byte distance, Vector3 force)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, target);
        ByteProxy.Serialize((Stream) bytes, slot);
        ByteProxy.Serialize((Stream) bytes, distance);
        Vector3Proxy.Serialize((Stream) bytes, force);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 10, customOpParameters) ? 1 : 0;
      }
    }

    public void SendDirectDamage(ushort damage)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        UInt16Proxy.Serialize((Stream) bytes, damage);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 11, customOpParameters) ? 1 : 0;
      }
    }

    public void SendDirectDeath()
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) memoryStream.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 12, customOpParameters) ? 1 : 0;
      }
    }

    public void SendJump(Vector3 position)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Vector3Proxy.Serialize((Stream) bytes, position);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 13, customOpParameters) ? 1 : 0;
      }
    }

    public void SendUpdatePositionAndRotation(
      ShortVector3 position,
      ShortVector3 velocity,
      byte hrot,
      byte vrot,
      byte moveState)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        ShortVector3Proxy.Serialize((Stream) bytes, position);
        ShortVector3Proxy.Serialize((Stream) bytes, velocity);
        ByteProxy.Serialize((Stream) bytes, hrot);
        ByteProxy.Serialize((Stream) bytes, vrot);
        ByteProxy.Serialize((Stream) bytes, moveState);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 14, customOpParameters, false) ? 1 : 0;
      }
    }

    public void SendKickPlayer(int cmid)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 15, customOpParameters) ? 1 : 0;
      }
    }

    public void SendIsFiring(bool on)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        BooleanProxy.Serialize((Stream) bytes, on);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 16, customOpParameters) ? 1 : 0;
      }
    }

    public void SendIsReadyForNextMatch(bool on)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        BooleanProxy.Serialize((Stream) bytes, on);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 17, customOpParameters) ? 1 : 0;
      }
    }

    public void SendIsPaused(bool on)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        BooleanProxy.Serialize((Stream) bytes, on);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 18, customOpParameters) ? 1 : 0;
      }
    }

    public void SendIsInSniperMode(bool on)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        BooleanProxy.Serialize((Stream) bytes, on);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 19, customOpParameters) ? 1 : 0;
      }
    }

    public void SendSingleBulletFire()
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) memoryStream.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 20, customOpParameters) ? 1 : 0;
      }
    }

    public void SendSwitchWeapon(byte weaponSlot)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        ByteProxy.Serialize((Stream) bytes, weaponSlot);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 21, customOpParameters) ? 1 : 0;
      }
    }

    public void SendSwitchTeam()
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) memoryStream.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 22, customOpParameters) ? 1 : 0;
      }
    }

    public void SendChangeGear(
      int head,
      int face,
      int upperBody,
      int lowerBody,
      int gloves,
      int boots,
      int holo)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, head);
        Int32Proxy.Serialize((Stream) bytes, face);
        Int32Proxy.Serialize((Stream) bytes, upperBody);
        Int32Proxy.Serialize((Stream) bytes, lowerBody);
        Int32Proxy.Serialize((Stream) bytes, gloves);
        Int32Proxy.Serialize((Stream) bytes, boots);
        Int32Proxy.Serialize((Stream) bytes, holo);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 23, customOpParameters) ? 1 : 0;
      }
    }

    public void SendEmitProjectile(
      Vector3 origin,
      Vector3 direction,
      byte slot,
      int projectileID,
      bool explode)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Vector3Proxy.Serialize((Stream) bytes, origin);
        Vector3Proxy.Serialize((Stream) bytes, direction);
        ByteProxy.Serialize((Stream) bytes, slot);
        Int32Proxy.Serialize((Stream) bytes, projectileID);
        BooleanProxy.Serialize((Stream) bytes, explode);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 24, customOpParameters) ? 1 : 0;
      }
    }

    public void SendEmitQuickItem(
      Vector3 origin,
      Vector3 direction,
      int itemId,
      byte playerNumber,
      int projectileID)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Vector3Proxy.Serialize((Stream) bytes, origin);
        Vector3Proxy.Serialize((Stream) bytes, direction);
        Int32Proxy.Serialize((Stream) bytes, itemId);
        ByteProxy.Serialize((Stream) bytes, playerNumber);
        Int32Proxy.Serialize((Stream) bytes, projectileID);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 25, customOpParameters) ? 1 : 0;
      }
    }

    public void SendRemoveProjectile(int projectileId, bool explode)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, projectileId);
        BooleanProxy.Serialize((Stream) bytes, explode);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 26, customOpParameters) ? 1 : 0;
      }
    }

    public void SendHitFeedback(int targetCmid, Vector3 force)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, targetCmid);
        Vector3Proxy.Serialize((Stream) bytes, force);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 27, customOpParameters) ? 1 : 0;
      }
    }

    public void SendActivateQuickItem(
      QuickItemLogic logic,
      int robotLifeTime,
      int scrapsLifeTime,
      bool isInstant)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        EnumProxy<QuickItemLogic>.Serialize((Stream) bytes, logic);
        Int32Proxy.Serialize((Stream) bytes, robotLifeTime);
        Int32Proxy.Serialize((Stream) bytes, scrapsLifeTime);
        BooleanProxy.Serialize((Stream) bytes, isInstant);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 28, customOpParameters) ? 1 : 0;
      }
    }

    public void SendChatMessage(string message, byte context)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, message);
        ByteProxy.Serialize((Stream) bytes, context);
        Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>()
        {
          {
            this.__id,
            (object) bytes.ToArray()
          }
        };
        if (this.sendOperation == null)
          return;
        int num = this.sendOperation((byte) 29, customOpParameters) ? 1 : 0;
      }
    }
  }
}
