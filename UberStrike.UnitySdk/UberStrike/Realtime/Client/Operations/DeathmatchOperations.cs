﻿// Decompiled with JetBrains decompiler
// Type: Uberstrike.Realtime.Client.Operations.DeathmatchOperations
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Util;
using System;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Serialization;

namespace Uberstrike.Realtime.Client.Operations
{
  public sealed class DeathmatchOperations : IOperationDispatcher
  {
    private Func<byte, Dictionary<byte, object>, bool, bool> _onSendOperation;

    public DeathmatchOperations() => this.SetSender((Func<byte, Dictionary<byte, object>, bool, bool>) null);

    public void SetSender(
      Func<byte, Dictionary<byte, object>, bool, bool> sender)
    {
      if (sender != null)
        this._onSendOperation = sender;
      else
        this._onSendOperation = (Func<byte, Dictionary<byte, object>, bool, bool>) ((id, data, reliable) =>
        {
          CmuneDebug.LogWarning("DeathmatchOperations has no SenderFunction assigned. Lost command with id: " + (object) id, new object[0]);
          return false;
        });
    }

    public void SendKillPlayer(int killedPeerId)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, killedPeerId);
        int num = this._onSendOperation((byte) 1, new Dictionary<byte, object>()
        {
          {
            (byte) 0,
            (object) bytes.ToArray()
          }
        }, true) ? 1 : 0;
      }
    }

    public void SendShootPlayer(int shotPeerId)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, shotPeerId);
        int num = this._onSendOperation((byte) 2, new Dictionary<byte, object>()
        {
          {
            (byte) 0,
            (object) bytes.ToArray()
          }
        }, true) ? 1 : 0;
      }
    }
  }
}
