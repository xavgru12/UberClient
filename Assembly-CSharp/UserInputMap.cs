// Decompiled with JetBrains decompiler
// Type: UserInputMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UserInputMap
{
  private KeyCode _prefix;

  public UserInputMap(
    string description,
    GameInputKey s,
    IInputChannel channel = null,
    bool isConfigurable = true,
    bool isEventSender = true,
    KeyCode prefix = KeyCode.None)
  {
    this._prefix = prefix;
    this.IsConfigurable = isConfigurable;
    this.IsEventSender = isEventSender;
    this.Channel = channel;
    this.Slot = s;
    this.Description = description;
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder(this.Description);
    stringBuilder.AppendFormat(": {0}", (object) this.Channel);
    return stringBuilder.ToString();
  }

  public int SetChannel(byte[] bytes, int idx)
  {
    switch (bytes[idx++])
    {
      case 0:
        this.Channel = (IInputChannel) new KeyInputChannel(bytes, ref idx);
        break;
      case 1:
        this.Channel = (IInputChannel) new MouseInputChannel(bytes, ref idx);
        break;
      case 2:
        this.Channel = (IInputChannel) new AxisInputChannel(bytes, ref idx);
        break;
      case 3:
        this.Channel = (IInputChannel) new ButtonInputChannel(bytes, ref idx);
        break;
      default:
        Debug.LogError((object) "KeyMap deserialization failed");
        break;
    }
    return idx;
  }

  public byte[] GetChannel()
  {
    List<byte> byteList = new List<byte>();
    if (this.Channel is KeyInputChannel)
      byteList.Add((byte) 0);
    else if (this.Channel is MouseInputChannel)
      byteList.Add((byte) 1);
    else if (this.Channel is AxisInputChannel)
      byteList.Add((byte) 2);
    else if (this.Channel is ButtonInputChannel)
      byteList.Add((byte) 3);
    else
      byteList.Add(byte.MaxValue);
    if (this.Channel != null)
      byteList.AddRange((IEnumerable<byte>) this.Channel.GetBytes());
    return byteList.ToArray();
  }

  public string GetPrefString() => WWW.EscapeURL(Encoding.ASCII.GetString(this.GetChannel()), Encoding.ASCII);

  public void FromPrefString(string pref) => this.SetChannel(Encoding.ASCII.GetBytes(WWW.UnEscapeURL(pref, Encoding.ASCII)), 0);

  public GameInputKey Slot { get; private set; }

  public string Description { get; private set; }

  public string Assignment
  {
    get
    {
      if (this.Channel == null)
        return "None";
      return this._prefix != KeyCode.None ? string.Format("{0} + {1}", (object) this.PrintKeyCode(this._prefix), (object) this.Channel.Name) : this.Channel.Name;
    }
  }

  private string PrintKeyCode(KeyCode keyCode)
  {
    switch (keyCode)
    {
      case KeyCode.RightShift:
        return "Right Shift";
      case KeyCode.LeftShift:
        return "Left Shift";
      case KeyCode.RightControl:
        return "Right Ctrl";
      case KeyCode.LeftControl:
        return "Left Ctrl";
      case KeyCode.RightAlt:
        return "Right Alt";
      case KeyCode.LeftAlt:
        return "Left Alt";
      default:
        return ((Enum) keyCode).ToString();
    }
  }

  public IInputChannel Channel { get; set; }

  public bool IsConfigurable { get; set; }

  public float Value => this.Channel != null && (this._prefix == KeyCode.None || Input.GetKey(this._prefix)) ? this.Channel.Value : 0.0f;

  public float RawValue() => this.Channel != null && (this._prefix == KeyCode.None || Input.GetKey(this._prefix)) ? this.Channel.RawValue() : 0.0f;

  public bool IsEventSender { get; private set; }
}
