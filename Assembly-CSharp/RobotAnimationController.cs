// Decompiled with JetBrains decompiler
// Type: RobotAnimationController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class RobotAnimationController : MonoBehaviour
{
  private List<string> _animationNames = new List<string>();
  private Transform _transform;
  private Animation _animation;

  private void Awake()
  {
    this._transform = this.transform;
    this._animation = this._transform.GetComponent<Animation>();
    this._animationNames.Add("BotToBall");
    this._animationNames.Add("BallToBot");
    this._animationNames.Add("Dance");
    this._animationNames.Add("StandToShoot");
    this._animationNames.Add("Shoot");
  }

  public void PlayAnimationHard(int animationNr)
  {
    this._animation.Stop();
    this._animation.Play(this._animationNames[animationNr]);
  }

  public void PlayAnimationHard(string animationName)
  {
    this._animation.Stop();
    this._animation.Play(animationName);
  }

  public bool CheckIfActive(int animationNr) => this._animation.IsPlaying(this._animationNames[animationNr]);

  public bool CheckIfActive(string animationName) => this._animation.IsPlaying(animationName);

  public void AnimationStop() => this._animation.Stop();
}
