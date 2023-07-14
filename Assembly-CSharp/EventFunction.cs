// Decompiled with JetBrains decompiler
// Type: EventFunction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

public class EventFunction
{
  private EventDelegate _eventDelegate;
  private object[] _args;

  public EventFunction(EventDelegate eventDelegate, params object[] args)
  {
    this._eventDelegate = eventDelegate;
    this._args = args;
  }

  public void Execute() => this._eventDelegate(this._args);

  private void DefaultEventDelegate(params object[] args)
  {
  }
}
