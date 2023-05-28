// Decompiled with JetBrains decompiler
// Type: IInputChannel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;

public interface IInputChannel : IByteArray
{
  InputChannelType ChannelType { get; }

  string Name { get; }

  bool IsChanged { get; }

  float RawValue();

  float Value { get; }

  void Listen();

  void Reset();
}
