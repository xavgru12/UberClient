// Decompiled with JetBrains decompiler
// Type: AnimationSynchronizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (Animation))]
public class AnimationSynchronizer : MonoBehaviour
{
  private UnityEngine.AnimationState animationState;

  private void Start() => this.animationState = this.animation[this.animation.clip.name];

  private void Update()
  {
    if (GameState.HasCurrentGame && GameState.CurrentGame.IsMatchRunning)
      this.animationState.time = GameState.CurrentGame.GameTime;
    else
      this.animationState.time += Time.deltaTime;
  }
}
