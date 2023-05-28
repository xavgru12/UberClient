// Decompiled with JetBrains decompiler
// Type: DoorOpenedEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

public class DoorOpenedEvent
{
  private int _doorID;

  public DoorOpenedEvent(int doorID) => this.DoorID = doorID;

  public int DoorID
  {
    get => this._doorID;
    protected set => this._doorID = value;
  }
}
