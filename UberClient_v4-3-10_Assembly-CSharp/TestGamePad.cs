// Decompiled with JetBrains decompiler
// Type: TestGamePad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

internal class TestGamePad : MonoBehaviour
{
  private UserInputMap _targetMap;

  private void Start()
  {
    AutoMonoBehaviour<InputManager>.Instance.IsGamepadEnabled = true;
    foreach (string joystickName in Input.GetJoystickNames())
      Debug.Log((object) ("Joystick " + joystickName));
  }

  private void OnGUI()
  {
    int num = 0;
    foreach (UserInputMap userInputMap in AutoMonoBehaviour<InputManager>.Instance.KeyMapping.Values)
    {
      bool flag = userInputMap == this._targetMap;
      GUI.Label(new Rect(20f, (float) (35 + num * 20), 140f, 20f), userInputMap.Description);
      if (userInputMap.IsConfigurable && GUI.Toggle(new Rect(180f, (float) (35 + num * 20), 20f, 20f), flag, string.Empty))
      {
        this._targetMap = userInputMap;
        Screen.lockCursor = true;
      }
      if (flag)
      {
        GUI.enabled = true;
        GUI.TextField(new Rect(220f, (float) (35 + num * 20), 100f, 20f), string.Empty);
        GUI.enabled = false;
      }
      else
      {
        GUI.contentColor = userInputMap.Channel == null ? Color.red : Color.white;
        GUI.Label(new Rect(220f, (float) (35 + num * 20), 150f, 20f), userInputMap.Assignment);
        GUI.contentColor = Color.white;
      }
      ++num;
    }
    if (this._targetMap != null && Event.current.type == UnityEngine.EventType.Layout && AutoMonoBehaviour<InputManager>.Instance.ListenForNewKeyAssignment(this._targetMap))
    {
      this._targetMap = (UserInputMap) null;
      Screen.lockCursor = false;
      Event.current.Use();
    }
    if (this._targetMap == null || Event.current.type != UnityEngine.EventType.Layout || !AutoMonoBehaviour<InputManager>.Instance.ListenForNewKeyAssignment(this._targetMap))
      return;
    this._targetMap = (UserInputMap) null;
    Screen.lockCursor = false;
    Event.current.Use();
  }
}
