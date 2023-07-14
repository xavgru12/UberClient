// Decompiled with JetBrains decompiler
// Type: KeyInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public static class KeyInput
{
  private static Dictionary<KeyCode, bool> keys = new Dictionary<KeyCode, bool>();
  private static KeyCode LastKey;

  public static bool AltPressed { get; private set; }

  public static bool CtrlPressed { get; private set; }

  public static KeyCode KeyPressed { get; private set; }

  public static bool GetKeyDown(KeyCode key) => KeyInput.KeyPressed == key;

  public static void Update()
  {
    if (KeyInput.keys.ContainsKey(KeyInput.LastKey) && KeyInput.keys[KeyInput.LastKey])
    {
      KeyInput.keys[KeyInput.LastKey] = false;
      KeyInput.KeyPressed = KeyInput.LastKey;
    }
    else
      KeyInput.KeyPressed = KeyCode.None;
    KeyInput.LastKey = KeyCode.None;
  }

  public static void OnGUI()
  {
    KeyInput.AltPressed = Event.current.alt;
    KeyInput.CtrlPressed = Event.current.control;
    if (Event.current.type == UnityEngine.EventType.KeyDown && Event.current.keyCode != KeyCode.None && !KeyInput.keys.ContainsKey(Event.current.keyCode))
    {
      KeyInput.keys[Event.current.keyCode] = true;
      KeyInput.LastKey = Event.current.keyCode;
    }
    else
    {
      if (Event.current.type != UnityEngine.EventType.KeyUp)
        return;
      KeyInput.keys.Remove(Event.current.keyCode);
      KeyInput.LastKey = KeyCode.None;
    }
  }
}
