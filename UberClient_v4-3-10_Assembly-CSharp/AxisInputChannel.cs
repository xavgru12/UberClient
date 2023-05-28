// Decompiled with JetBrains decompiler
// Type: AxisInputChannel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class AxisInputChannel : IByteArray, IInputChannel
{
  private string _axis = string.Empty;
  private string _axisName = string.Empty;
  private float _value;
  private float _lastValue;
  private float _deadRange = 0.1f;
  private AxisInputChannel.AxisReadingMethod _axisReading;

  public AxisInputChannel(byte[] bytes, ref int idx) => idx = this.FromBytes(bytes, idx);

  public AxisInputChannel(string axis)
  {
    this._axis = axis;
    this._axisName = this._axis;
  }

  public AxisInputChannel(string axis, float deadRange)
    : this(axis)
  {
    this._deadRange = deadRange;
  }

  public AxisInputChannel(string axis, float deadRange, AxisInputChannel.AxisReadingMethod method)
    : this(axis, deadRange)
  {
    this._axisReading = method;
    switch (method)
    {
      case AxisInputChannel.AxisReadingMethod.PositiveOnly:
        this._axisName += " Down";
        break;
      case AxisInputChannel.AxisReadingMethod.NegativeOnly:
        this._axisName += " Up";
        break;
    }
  }

  public void Listen()
  {
    this._lastValue = this._value;
    this._value = this.RawValue();
    switch (this._axisReading)
    {
      case AxisInputChannel.AxisReadingMethod.PositiveOnly:
        if ((double) this._value < 0.0)
        {
          this._value = 0.0f;
          break;
        }
        break;
      case AxisInputChannel.AxisReadingMethod.NegativeOnly:
        if ((double) this._value > 0.0)
        {
          this._value = 0.0f;
          break;
        }
        break;
    }
    if ((double) Mathf.Abs(this._value) >= (double) this._deadRange)
      return;
    this._value = 0.0f;
  }

  public void Reset()
  {
    this._value = 0.0f;
    this._lastValue = 0.0f;
  }

  public float RawValue() => Input.GetAxis(this._axis);

  public override string ToString() => this._axis;

  public override bool Equals(object obj) => obj is AxisInputChannel && (obj as AxisInputChannel).Axis == this.Axis;

  public override int GetHashCode() => base.GetHashCode();

  public string Axis => this._axis;

  public string Name => this._axisName;

  public float Value
  {
    get => this._value;
    set => this._value = this._lastValue = value;
  }

  public InputChannelType ChannelType => InputChannelType.Axis;

  public bool IsPressed => (double) this._value != 0.0;

  public bool IsChanged => (double) this._lastValue != (double) this._value;

  public int FromBytes(byte[] bytes, int idx)
  {
    this._axis = DefaultByteConverter.ToString(bytes, ref idx);
    this._deadRange = DefaultByteConverter.ToFloat(bytes, ref idx);
    this._axisReading = (AxisInputChannel.AxisReadingMethod) DefaultByteConverter.ToInt(bytes, ref idx);
    switch (this._axisReading)
    {
      case AxisInputChannel.AxisReadingMethod.PositiveOnly:
        this._axisName = this._axis + " Down";
        break;
      case AxisInputChannel.AxisReadingMethod.NegativeOnly:
        this._axisName = this._axis + " Up";
        break;
      default:
        this._axisName = this._axis;
        break;
    }
    return idx;
  }

  public byte[] GetBytes()
  {
    List<byte> byteList = new List<byte>();
    DefaultByteConverter.FromString(this._axis, ref byteList);
    DefaultByteConverter.FromFloat(this._deadRange, ref byteList);
    DefaultByteConverter.FromInt((int) this._axisReading, ref byteList);
    return byteList.ToArray();
  }

  public enum AxisReadingMethod
  {
    All,
    PositiveOnly,
    NegativeOnly,
  }
}
