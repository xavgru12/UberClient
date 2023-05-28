// Decompiled with JetBrains decompiler
// Type: CircularInteger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;

public class CircularInteger
{
  private int _lower;
  private int _length;
  private int _current;

  public CircularInteger(int lowerBound, int upperBound) => this.SetRange(lowerBound, upperBound);

  public void SetRange(int lowerBound, int upperBound)
  {
    if (lowerBound >= upperBound)
      throw new Exception("CircularInteger ctor failed because lowerBound greater than upperBound");
    this._current = 0;
    this._lower = lowerBound;
    this._length = upperBound - lowerBound + 1;
  }

  public void Reset() => this._current = 0;

  public int Current
  {
    get => this._current + this._lower;
    set
    {
      if (value >= this._lower + this._length && value < this._lower)
        throw new Exception("CircularInteger: Assigned value not in range!");
      this._current = value - this._lower;
    }
  }

  public int Next
  {
    get
    {
      this._current = (this._current + 1) % this._length;
      return this.Current;
    }
  }

  public int Prev
  {
    get
    {
      this._current = (this._current + this._length - 1) % this._length;
      return this.Current;
    }
  }

  public int First
  {
    get
    {
      this._current = 0;
      return this.Current;
    }
  }

  public int Last
  {
    get
    {
      this._current = this._length - 1;
      return this.Current;
    }
  }

  public int Range => this._length;
}
