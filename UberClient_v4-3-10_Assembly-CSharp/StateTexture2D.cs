// Decompiled with JetBrains decompiler
// Type: StateTexture2D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class StateTexture2D
{
  private Texture[] _list;
  private int _index;
  private static readonly Texture _default = (Texture) new Texture2D(1, 1);

  public StateTexture2D(params Texture[] states) => this.SetStates(states);

  public void SetStates(params Texture[] states)
  {
    this._list = states == null ? (Texture[]) new Texture2D[0] : states;
    this._index = Mathf.Clamp(this._index, 0, this._list.Length - 1);
  }

  public Texture ChangeState(int stateId)
  {
    this._index = Mathf.Clamp(stateId, 0, this._list.Length - 1);
    return this.Current;
  }

  public Texture Current => this._list.Length > 0 ? this._list[this._index] : StateTexture2D._default;
}
