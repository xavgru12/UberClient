// Decompiled with JetBrains decompiler
// Type: MouseInputChannel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class MouseInputChannel : IByteArray, IInputChannel
{
  private bool _wasDown;
  private bool _isDown;
  private int _button;

  public MouseInputChannel(byte[] bytes, ref int idx) => idx = this.FromBytes(bytes, idx);

  public MouseInputChannel(int button) => this._button = button;

  public void Listen()
  {
    this._wasDown = this._isDown;
    this._isDown = Input.GetMouseButton(this._button);
  }

  public float RawValue() => !Input.GetMouseButton(this._button) ? 0.0f : 1f;

  public void Reset()
  {
    this._wasDown = false;
    this._isDown = false;
  }

  public override string ToString() => string.Format("Mouse {0}", (object) this._button);

  public override bool Equals(object obj) => obj is MouseInputChannel && (obj as MouseInputChannel).Button == this.Button;

  public override int GetHashCode() => base.GetHashCode();

  private string ConvertMouseButtonName(int _button)
  {
    switch (_button)
    {
      case 0:
        return "Left Mousebutton";
      case 1:
        return "Right Mousebutton";
      default:
        return string.Format("Mouse {0}", (object) _button);
    }
  }

  public int Button => this._button;

  public string Name => this.ConvertMouseButtonName(this._button);

  public InputChannelType ChannelType => InputChannelType.Mouse;

  public float Value
  {
    get => !this._isDown ? 0.0f : 1f;
    set => this._isDown = this._wasDown = (double) value != 0.0;
  }

  public bool IsChanged => this._isDown != this._wasDown;

  public int FromBytes(byte[] bytes, int idx)
  {
    this._button = DefaultByteConverter.ToInt(bytes, ref idx);
    return idx;
  }

  public byte[] GetBytes()
  {
    List<byte> byteList = new List<byte>(4);
    DefaultByteConverter.FromInt(this._button, ref byteList);
    return byteList.ToArray();
  }
}
