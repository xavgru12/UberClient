// Decompiled with JetBrains decompiler
// Type: UserInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public static class UserInput
{
  public static float ZoomSpeed = 1f;
  public static Vector2 TouchLookSensitivity = new Vector2(1f, 0.5f);
  public static Vector2 Mouse;
  public static Vector3 VerticalDirection;
  public static Vector3 HorizontalDirection;

  static UserInput() => UserInput.Reset();

  public static void Reset()
  {
    UserInput.Mouse = new Vector2(0.0f, 0.0f);
    UserInput.VerticalDirection = Vector3.zero;
    UserInput.HorizontalDirection = Vector3.zero;
    UserInput.Rotation = Quaternion.identity;
  }

  public static void UpdateDirections()
  {
    UserInput.ResetDirection();
    if ((GameState.LocalCharacter.Keys & KeyState.Left) != KeyState.Still)
      UserInput.HorizontalDirection.x -= (float) sbyte.MaxValue;
    if ((GameState.LocalCharacter.Keys & KeyState.Right) != KeyState.Still)
      UserInput.HorizontalDirection.x += (float) sbyte.MaxValue;
    if ((GameState.LocalCharacter.Keys & KeyState.Forward) != KeyState.Still)
      UserInput.HorizontalDirection.z += (float) sbyte.MaxValue;
    if ((GameState.LocalCharacter.Keys & KeyState.Backward) != KeyState.Still)
      UserInput.HorizontalDirection.z -= (float) sbyte.MaxValue;
    if ((GameState.LocalCharacter.Keys & KeyState.Jump) != KeyState.Still)
      UserInput.VerticalDirection.y += (float) sbyte.MaxValue;
    if ((GameState.LocalCharacter.Keys & KeyState.Crouch) != KeyState.Still)
      UserInput.VerticalDirection.y -= (float) sbyte.MaxValue;
    UserInput.HorizontalDirection.Normalize();
    UserInput.VerticalDirection.Normalize();
  }

  public static void ResetDirection()
  {
    UserInput.HorizontalDirection = Vector3.zero;
    UserInput.VerticalDirection = Vector3.zero;
  }

  public static KeyState GetkeyState(GameInputKey slot)
  {
    switch (slot)
    {
      case GameInputKey.Forward:
        return KeyState.Forward;
      case GameInputKey.Backward:
        return KeyState.Backward;
      case GameInputKey.Left:
        return KeyState.Left;
      case GameInputKey.Right:
        return KeyState.Right;
      case GameInputKey.Jump:
        return KeyState.Jump;
      case GameInputKey.Crouch:
        return KeyState.Crouch;
      default:
        return KeyState.Still;
    }
  }

  public static void SetRotation(float hAngle = 0, float vAngle = 0)
  {
    UserInput.Mouse = new Vector2(hAngle, -vAngle);
    UserInput.UpdateMouse();
    UserInput.UpdateDirections();
  }

  public static void UpdateMouse()
  {
    if ((Object) Camera.main != (Object) null)
    {
      float num1 = Mathf.Pow(Camera.main.fov / ApplicationDataManager.ApplicationOptions.CameraFovMax, 1.1f);
      UserInput.Mouse.x += AutoMonoBehaviour<InputManager>.Instance.RawValue(GameInputKey.HorizontalLook) * ApplicationDataManager.ApplicationOptions.InputXMouseSensitivity * num1;
      UserInput.Mouse.x = UserInput.ClampAngle(UserInput.Mouse.x, -360f, 360f);
      int num2 = !ApplicationDataManager.ApplicationOptions.InputInvertMouse ? 1 : -1;
      UserInput.Mouse.y += AutoMonoBehaviour<InputManager>.Instance.RawValue(GameInputKey.VerticalLook) * ApplicationDataManager.ApplicationOptions.InputXMouseSensitivity * (float) num2 * num1;
      UserInput.Mouse.y = UserInput.ClampAngle(UserInput.Mouse.y, -88f, 88f);
    }
    UserInput.Rotation = Quaternion.AngleAxis(UserInput.Mouse.x, Vector3.up) * Quaternion.AngleAxis(UserInput.Mouse.y, Vector3.left);
  }

  public static bool IsPressed(KeyState k) => (GameState.LocalCharacter.Keys & k) != KeyState.Still;

  public static bool IsWalking => (GameState.LocalCharacter.Keys & KeyState.Walking) != KeyState.Still && (GameState.LocalCharacter.Keys ^ KeyState.Horizontal) != KeyState.Still && (GameState.LocalCharacter.Keys ^ KeyState.Vertical) != KeyState.Still;

  public static bool IsMouseLooking => (double) AutoMonoBehaviour<InputManager>.Instance.RawValue(GameInputKey.HorizontalLook) != 0.0 || (double) AutoMonoBehaviour<InputManager>.Instance.RawValue(GameInputKey.VerticalLook) != 0.0;

  public static bool IsMovingVertically => (GameState.LocalCharacter.Keys & (KeyState.Jump | KeyState.Crouch)) != KeyState.Still;

  public static bool IsMovingUp => (GameState.LocalCharacter.Keys & KeyState.Jump) != KeyState.Still;

  public static bool IsMovingDown => (GameState.LocalCharacter.Keys & KeyState.Crouch) != KeyState.Still;

  public static Quaternion Rotation { get; private set; }

  private static float ClampAngle(float angle, float min, float max)
  {
    if ((double) angle < -360.0)
      angle += 360f;
    if ((double) angle > 360.0)
      angle -= 360f;
    return Mathf.Clamp(angle, min, max);
  }
}
