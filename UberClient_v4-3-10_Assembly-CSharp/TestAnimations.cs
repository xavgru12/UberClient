// Decompiled with JetBrains decompiler
// Type: TestAnimations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class TestAnimations : MonoBehaviour
{
  private Vector2 scroll;
  private Vector2 itemSize = new Vector2(220f, 30f);
  public bool stopall;

  private void OnGUI()
  {
    if (!((UnityEngine.Object) GameState.LocalDecorator != (UnityEngine.Object) null))
      return;
    this.scroll = GUITools.BeginScrollView(new Rect(1f, 100f, this.itemSize.x + 20f, (float) (Screen.height - 20)), this.scroll, new Rect(0.0f, 0.0f, this.itemSize.x, (float) GameState.LocalDecorator.Animation.GetClipCount() * this.itemSize.y));
    int num = 0;
    foreach (UnityEngine.AnimationState animationState in GameState.LocalDecorator.Animation)
    {
      if ((bool) (TrackedReference) animationState)
      {
        if (GUI.Button(new Rect(0.0f, (float) num * this.itemSize.y, this.itemSize.x, this.itemSize.y), "Play " + animationState.name))
          GameState.LocalDecorator.AnimationController.TriggerAnimation((AnimationIndex) Enum.Parse(typeof (AnimationIndex), animationState.name, true), this.stopall);
      }
      else
        GUI.Label(new Rect(0.0f, (float) num * this.itemSize.y, this.itemSize.x, this.itemSize.y), "Missing clip");
      ++num;
    }
    GUITools.EndScrollView();
  }
}
