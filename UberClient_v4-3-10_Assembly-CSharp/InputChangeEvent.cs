// Decompiled with JetBrains decompiler
// Type: InputChangeEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

public class InputChangeEvent
{
  private GameInputKey _key;
  private float _value;

  public InputChangeEvent(GameInputKey key, float value)
  {
    this._key = key;
    this._value = value;
  }

  public GameInputKey Key => this._key;

  public bool IsDown => (double) this._value != 0.0;

  public float Value => this._value;
}
