// Decompiled with JetBrains decompiler
// Type: GuiLockController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class GuiLockController : AutoMonoBehaviour<GuiLockController>
{
  private void Awake()
  {
    this.enabled = false;
    GuiLockController.Alpha = 0.6f;
  }

  public static void LockApplication()
  {
    GuiLockController.IsApplicationLocked = true;
    GuiLockController.LockingDepth = GuiDepth.Popup;
    AutoMonoBehaviour<GuiLockController>.Instance.enabled = true;
  }

  public static bool IsApplicationLocked { get; private set; }

  public static float Alpha { get; private set; }

  public static GuiDepth LockingDepth { get; private set; }

  public static bool IsEnabled { get; private set; }

  public static bool IsLocked(params GuiDepth[] levels) => GuiLockController.IsEnabled && Array.Exists<GuiDepth>(levels, (Predicate<GuiDepth>) (l => l == GuiLockController.LockingDepth));

  public static void EnableLock(GuiDepth depth)
  {
    if (GuiLockController.IsApplicationLocked || GuiLockController.IsEnabled && GuiLockController.LockingDepth <= depth)
      return;
    GuiLockController.LockingDepth = depth;
    GuiLockController.IsEnabled = true;
    AutoMonoBehaviour<GuiLockController>.Instance.enabled = GuiLockController.IsEnabled;
  }

  public static void ReleaseLock(GuiDepth depth)
  {
    if (GuiLockController.IsApplicationLocked || !GuiLockController.IsEnabled || GuiLockController.LockingDepth != depth)
      return;
    GuiLockController.IsEnabled = false;
    AutoMonoBehaviour<GuiLockController>.Instance.enabled = GuiLockController.IsEnabled;
  }

  private void OnGUI()
  {
    GUI.depth = (int) (GuiLockController.LockingDepth + 1);
    if (Event.current.type == UnityEngine.EventType.MouseDown || Event.current.type == UnityEngine.EventType.MouseUp)
      Event.current.Use();
    GUI.color = new Color(1f, 1f, 1f, GuiLockController.Alpha);
    GUI.Button(new Rect(0.0f, 0.0f, (float) (Screen.width + 5), (float) (Screen.height + 5)), string.Empty, BlueStonez.box_grey31);
    GUI.color = Color.white;
  }
}
