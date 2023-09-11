// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.OperationFactory
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Realtime.Common.Utils;
using Cmune.Util;
using System;
using System.Collections.Generic;

namespace Cmune.Realtime.Common
{
  public static class OperationFactory
  {
    public static Dictionary<byte, object> Create(byte code, params object[] args)
    {
      Dictionary<byte, object> t = new Dictionary<byte, object>();
      try
      {
        switch (code)
        {
          case 66:
            OperationUtil.SetMethodId((IDictionary<byte, object>) t, args[0]);
            OperationUtil.SetArg<short>((IDictionary<byte, object>) t, (byte) 61, args[1]);
            OperationUtil.SetBytes((IDictionary<byte, object>) t, args[2]);
            break;
          case 80:
            OperationUtil.SetArg<int>((IDictionary<byte, object>) t, (byte) 102, args[0]);
            OperationUtil.SetInstanceID((IDictionary<byte, object>) t, args[1]);
            OperationUtil.SetMethodId((IDictionary<byte, object>) t, args[2]);
            OperationUtil.SetBytes((IDictionary<byte, object>) t, args[3]);
            break;
          case 81:
          case 83:
            OperationUtil.SetInstanceID((IDictionary<byte, object>) t, args[0]);
            OperationUtil.SetMethodId((IDictionary<byte, object>) t, args[1]);
            OperationUtil.SetBytes((IDictionary<byte, object>) t, args[2]);
            break;
          case 82:
            OperationUtil.SetInstanceID((IDictionary<byte, object>) t, args[0]);
            OperationUtil.SetMethodId((IDictionary<byte, object>) t, args[1]);
            OperationUtil.SetBytes((IDictionary<byte, object>) t, args[2]);
            break;
          case 88:
            OperationUtil.SetBytes((IDictionary<byte, object>) t, args[0]);
            OperationUtil.SetArg<int>((IDictionary<byte, object>) t, (byte) 206, args[1]);
            OperationUtil.SetArg<int>((IDictionary<byte, object>) t, (byte) 205, args[2]);
            break;
          case 89:
            OperationUtil.SetArg<byte[]>((IDictionary<byte, object>) t, (byte) 120, args[0]);
            break;
        }
      }
      catch (Exception ex)
      {
        t.Clear();
        CmuneDebug.Exception("Wrong number of Parameters while creating Operation of type {0}", (object) code);
      }
      return t;
    }
  }
}
