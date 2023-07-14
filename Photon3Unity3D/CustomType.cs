// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.CustomType
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System;

namespace ExitGames.Client.Photon
{
  internal class CustomType
  {
    public readonly byte Code;
    public readonly Type Type;
    public readonly SerializeMethod SerializeFunction;
    public readonly DeserializeMethod DeserializeFunction;

    public CustomType(
      Type type,
      byte code,
      SerializeMethod serializeFunction,
      DeserializeMethod deserializeFunction)
    {
      this.Type = type;
      this.Code = code;
      this.SerializeFunction = serializeFunction;
      this.DeserializeFunction = deserializeFunction;
    }
  }
}
