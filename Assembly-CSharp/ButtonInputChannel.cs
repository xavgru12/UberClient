// Decompiled with JetBrains decompiler
// Type: ButtonInputChannel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class ButtonInputChannel : IByteArray, IInputChannel
{
  private bool _isDown;
  private bool _wasDown;
  private string _button = string.Empty;

  public ButtonInputChannel(byte[] bytes, ref int idx) => idx = this.FromBytes(bytes, idx);

  public ButtonInputChannel(string button) => this._button = button;

  public void Listen()
  {
    this._wasDown = this._isDown;
    this._isDown = Input.GetButton(this._button);
  }

  public void Reset()
  {
    this._wasDown = false;
    this._isDown = false;
  }

  public float RawValue() => !Input.GetButton(this._button) ? 0.0f : 1f;

  public override string ToString() => this._button;

  public override bool Equals(object obj) => obj is ButtonInputChannel && (obj as ButtonInputChannel).Button == this.Button;

  public override int GetHashCode() => base.GetHashCode();

  public string Button => this._button;

  public string Name => this._button;

  public float Value => !this._isDown ? 0.0f : 1f;

  public InputChannelType ChannelType => InputChannelType.Axis;

  public bool IsPressed => this._isDown;

  public bool IsChanged => this._wasDown != this._isDown;

  public int FromBytes(byte[] bytes, int idx)
  {
    this._button = DefaultByteConverter.ToString(bytes, ref idx);
    return idx;
  }

  public byte[] GetBytes()
  {
    List<byte> byteList = new List<byte>();
    DefaultByteConverter.FromString(this._button, ref byteList);
    return byteList.ToArray();
  }
}
