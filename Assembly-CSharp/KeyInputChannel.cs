// Decompiled with JetBrains decompiler
// Type: KeyInputChannel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class KeyInputChannel : IByteArray, IInputChannel
{
  private bool _wasDown;
  private bool _isDown;
  private KeyCode _key;
  private string _name = string.Empty;

  public KeyInputChannel(byte[] bytes, ref int idx)
  {
    idx = this.FromBytes(bytes, idx);
    this.InitKeyName();
  }

  public KeyInputChannel(KeyCode key)
  {
    this._key = key;
    this.InitKeyName();
  }

  private void InitKeyName()
  {
    KeyCode key = this._key;
    switch (key)
    {
      case KeyCode.Keypad0:
        this._name = "Keypad 0";
        break;
      case KeyCode.Keypad1:
        this._name = "Keypad 1";
        break;
      case KeyCode.Keypad2:
        this._name = "Keypad 2";
        break;
      case KeyCode.Keypad3:
        this._name = "Keypad 3";
        break;
      case KeyCode.Keypad4:
        this._name = "Keypad 4";
        break;
      case KeyCode.Keypad5:
        this._name = "Keypad 5";
        break;
      case KeyCode.Keypad6:
        this._name = "Keypad 6";
        break;
      case KeyCode.Keypad7:
        this._name = "Keypad 7";
        break;
      case KeyCode.Keypad8:
        this._name = "Keypad 8";
        break;
      case KeyCode.Keypad9:
        this._name = "Keypad 9";
        break;
      case KeyCode.KeypadPeriod:
        this._name = "Keypad Period";
        break;
      case KeyCode.KeypadDivide:
        this._name = "Keypad Divide";
        break;
      case KeyCode.KeypadMultiply:
        this._name = "Keypad Multiply";
        break;
      case KeyCode.KeypadMinus:
        this._name = "Keypad Minus";
        break;
      case KeyCode.KeypadPlus:
        this._name = "Keypad Plus";
        break;
      case KeyCode.KeypadEnter:
        this._name = "Keypad Enter";
        break;
      case KeyCode.KeypadEquals:
        this._name = "Keypad Equals";
        break;
      case KeyCode.UpArrow:
        this._name = "Up Arrow";
        break;
      case KeyCode.DownArrow:
        this._name = "Down Arrow";
        break;
      case KeyCode.RightArrow:
        this._name = "Right Arrow";
        break;
      case KeyCode.LeftArrow:
        this._name = "Left Arrow";
        break;
      case KeyCode.RightShift:
        this._name = "Right Shift";
        break;
      case KeyCode.LeftShift:
        this._name = "Left Shift";
        break;
      case KeyCode.RightControl:
        this._name = "Right Ctrl";
        break;
      case KeyCode.LeftControl:
        this._name = "Left Ctrl";
        break;
      case KeyCode.RightAlt:
        this._name = "Right Alt";
        break;
      case KeyCode.LeftAlt:
        this._name = "Left Alt";
        break;
      default:
        switch (key - 48)
        {
          case KeyCode.None:
            this._name = "0";
            return;
          case (KeyCode) 1:
            this._name = "1";
            return;
          case (KeyCode) 2:
            this._name = "2";
            return;
          case (KeyCode) 3:
            this._name = "3";
            return;
          case (KeyCode) 4:
            this._name = "4";
            return;
          case (KeyCode) 5:
            this._name = "5";
            return;
          case (KeyCode) 6:
            this._name = "6";
            return;
          case (KeyCode) 7:
            this._name = "7";
            return;
          case KeyCode.Backspace:
            this._name = "8";
            return;
          case KeyCode.Tab:
            this._name = "9";
            return;
          default:
            this._name = ((Enum) this._key).ToString();
            return;
        }
    }
  }

  public void Listen()
  {
    this._wasDown = this._isDown;
    this._isDown = Input.GetKey(this._key);
  }

  public void Reset()
  {
    this._wasDown = false;
    this._isDown = false;
  }

  public float RawValue() => !Input.GetKey(this._key) ? 0.0f : 1f;

  public override string ToString() => ((Enum) this._key).ToString();

  public override bool Equals(object obj) => obj is KeyInputChannel && (obj as KeyInputChannel).Key == this.Key;

  public override int GetHashCode() => base.GetHashCode();

  public KeyCode Key => this._key;

  public string Name => this._name;

  public InputChannelType ChannelType => InputChannelType.Key;

  public float Value
  {
    get => !this._isDown ? 0.0f : 1f;
    set => this._isDown = this._wasDown = (double) value != 0.0;
  }

  public bool IsChanged => this._isDown != this._wasDown;

  public int FromBytes(byte[] bytes, int idx)
  {
    this._key = (KeyCode) DefaultByteConverter.ToInt(bytes, ref idx);
    return idx;
  }

  public byte[] GetBytes()
  {
    List<byte> byteList = new List<byte>(4);
    DefaultByteConverter.FromInt((int) this._key, ref byteList);
    return byteList.ToArray();
  }
}
