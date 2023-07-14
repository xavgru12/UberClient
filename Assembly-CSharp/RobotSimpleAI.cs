// Decompiled with JetBrains decompiler
// Type: RobotSimpleAI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (RobotAnimationController))]
public class RobotSimpleAI : BaseGameProp
{
  private AvatarHudInformation _hudInfo;
  private short _health;
  private List<Transform> _children;
  private List<Transform> _parents;
  private List<Vector3> _position;
  private List<Quaternion> _rotation;
  private List<Rigidbody> _rBody;
  private RobotSimpleAI.RobotStates _myRobotStates;
  private RobotAnimationController _animationController;
  private float _nextTimeToCheck;
  [SerializeField]
  private float _timeToDance = 2f;
  [SerializeField]
  private float _timeToHide = 3f;
  [SerializeField]
  private float _timeToExplode = 5f;
  [SerializeField]
  private float _timeToReborn = 5f;
  [SerializeField]
  private GameObject _damageFeedback;
  [SerializeField]
  private float _forceFactor = 0.2f;
  [SerializeField]
  private float _transparentTime;

  private void Awake()
  {
    this._animationController = this.GetComponent<RobotAnimationController>();
    this._children = new List<Transform>();
    this._parents = new List<Transform>();
    this._position = new List<Vector3>();
    this._rotation = new List<Quaternion>();
    this._rBody = new List<Rigidbody>();
    this.GetChildrenData(this.Transform, ref this._children, ref this._parents, ref this._position, ref this._rotation, ref this._rBody);
    this._health = (short) 100;
    if ((bool) (Object) this.transform.parent)
    {
      this._hudInfo = this.transform.parent.GetComponentInChildren<AvatarHudInformation>();
      if ((bool) (Object) this._hudInfo)
      {
        this._hudInfo.Target = this.transform;
        this._hudInfo.SetHealthBarValue((float) this._health / 100f);
      }
    }
    foreach (CharacterHitArea componentsInChild in this.GetComponentsInChildren<CharacterHitArea>(true))
      componentsInChild.Shootable = (IShootable) this;
  }

  private void Start()
  {
    this._animationController.PlayAnimationHard("Dance");
    Collider[] componentsInChildren = this.GetComponentsInChildren<Collider>(true);
    for (int index1 = 0; index1 < componentsInChildren.Length; ++index1)
    {
      for (int index2 = index1 + 1; index2 < componentsInChildren.Length; ++index2)
        Physics.IgnoreCollision(componentsInChildren[index1], componentsInChildren[index2]);
    }
  }

  private void Update()
  {
    if (this._myRobotStates == RobotSimpleAI.RobotStates.Show && !this._animationController.CheckIfActive("BallToBot"))
      this._myRobotStates = RobotSimpleAI.RobotStates.DoneShow;
    if ((double) Time.time <= (double) this._nextTimeToCheck)
      return;
    switch (this._myRobotStates)
    {
      case RobotSimpleAI.RobotStates.Dance:
        this._nextTimeToCheck = Time.time + this._timeToHide;
        this._animationController.PlayAnimationHard("BotToBall");
        this._myRobotStates = RobotSimpleAI.RobotStates.Hide;
        break;
      case RobotSimpleAI.RobotStates.Hide:
        this._animationController.PlayAnimationHard("BallToBot");
        this._myRobotStates = RobotSimpleAI.RobotStates.Show;
        break;
      case RobotSimpleAI.RobotStates.DoneShow:
        this._nextTimeToCheck = Time.time + this._timeToDance;
        this._animationController.PlayAnimationHard("Dance");
        this._myRobotStates = RobotSimpleAI.RobotStates.Dance;
        break;
      case RobotSimpleAI.RobotStates.Explode:
        this._nextTimeToCheck = Time.time + this._transparentTime * Time.deltaTime;
        this._myRobotStates = RobotSimpleAI.RobotStates.FadeOutParts;
        break;
      case RobotSimpleAI.RobotStates.FadeOutParts:
        this._nextTimeToCheck = Time.time + this._transparentTime * Time.deltaTime;
        if (!this.FadeOutRobot(0.1f))
          break;
        this._myRobotStates = RobotSimpleAI.RobotStates.Dead;
        this.HideRobot();
        this._nextTimeToCheck = Time.time + this._timeToReborn;
        break;
      case RobotSimpleAI.RobotStates.Dead:
        this.Reborn();
        break;
    }
  }

  public void Die(Vector3 force)
  {
    this.Explode(force);
    this._myRobotStates = RobotSimpleAI.RobotStates.Explode;
    this._nextTimeToCheck = Time.time + this._timeToExplode;
  }

  private void Explode(Vector3 force)
  {
    this._myRobotStates = RobotSimpleAI.RobotStates.Explode;
    this._animationController.AnimationStop();
    this._animationController.enabled = false;
    this.animation.enabled = false;
    if ((bool) (Object) this.collider)
      this.collider.isTrigger = true;
    this.gameObject.layer = 2;
    float magnitude = force.magnitude;
    for (int index = 0; index < this._rBody.Count; ++index)
    {
      this._rBody[index].isKinematic = false;
      this._rBody[index].AddForce((Random.onUnitSphere * magnitude + force) * this._forceFactor * 0.1f, ForceMode.Impulse);
      this._rBody[index].transform.parent = this.Transform;
    }
  }

  private void GetChildrenData(
    Transform root,
    ref List<Transform> go,
    ref List<Transform> parents,
    ref List<Vector3> pos,
    ref List<Quaternion> rot,
    ref List<Rigidbody> rb)
  {
    for (int index = 0; index < root.GetChildCount(); ++index)
    {
      if ((Object) root.GetChild(index).GetComponent<Rigidbody>() != (Object) null)
      {
        go.Add(root.GetChild(index));
        pos.Add(root.GetChild(index).localPosition);
        rot.Add(root.GetChild(index).localRotation);
        rb.Add(root.GetChild(index).GetComponent<Rigidbody>());
        parents.Add(root);
      }
      if (root.GetChild(index).GetChildCount() > 0)
        this.GetChildrenData(root.GetChild(index), ref go, ref parents, ref pos, ref rot, ref rb);
    }
  }

  private bool FadeOutRobot(float alpha)
  {
    bool flag = false;
    for (int index = 0; index < this._rBody.Count; ++index)
    {
      Vector4 color = (Vector4) this._children[index].renderer.material.color;
      if ((double) color.w < (double) alpha)
      {
        color.w = 0.0f;
        flag = true;
      }
      else
        color.w -= alpha;
      this._children[index].renderer.material.color = (Color) color;
    }
    return flag;
  }

  private void Reborn()
  {
    this.ClearProjectileDecorators();
    for (int index = 0; index < this._rBody.Count; ++index)
    {
      this._rBody[index].isKinematic = true;
      this._children[index].localPosition = this._position[index];
      this._children[index].localRotation = this._rotation[index];
      this._children[index].GetComponent<MeshRenderer>().enabled = true;
      this._children[index].collider.isTrigger = false;
      this._children[index].parent = this._parents[index];
      Vector4 color = (Vector4) this._children[index].renderer.material.color with
      {
        w = 1f
      };
      this._children[index].renderer.material.color = (Color) color;
    }
    this._animationController.enabled = true;
    this._myRobotStates = RobotSimpleAI.RobotStates.Show;
    this.animation.enabled = true;
    if ((bool) (Object) this.collider)
      this.collider.isTrigger = false;
    this._animationController.PlayAnimationHard("BallToBot");
    this.gameObject.layer = 21;
    this._health = (short) 100;
    if (!(bool) (Object) this._hudInfo)
      return;
    this._hudInfo.SetHealthBarValue((float) this._health / 100f);
  }

  private void HideRobot()
  {
    for (int index = 0; index < this._children.Count; ++index)
    {
      this._children[index].GetComponent<MeshRenderer>().enabled = false;
      this._children[index].collider.isTrigger = true;
    }
  }

  public override void ApplyDamage(DamageInfo d)
  {
    if (this._health <= (short) 0)
      return;
    this._health -= d.Damage;
    this.ShowDamageFeedback(d);
    if ((bool) (Object) this._hudInfo)
      this._hudInfo.SetHealthBarValue((float) this._health / 100f);
    if (this._health > (short) 0)
      return;
    this.Die(d.Force);
  }

  public override bool CanApplyDamage => this._health > (short) 0;

  private void ClearProjectileDecorators()
  {
    foreach (ArrowProjectile componentsInChild in this.GetComponentsInChildren<ArrowProjectile>(true))
      componentsInChild.Destroy();
  }

  private void ShowDamageFeedback(DamageInfo shot)
  {
    GameObject gameObject = Object.Instantiate((Object) this._damageFeedback, shot.Hitpoint, Quaternion.LookRotation(shot.Force)) as GameObject;
    if (!(bool) (Object) gameObject)
      return;
    gameObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
    PlayerDamageEffect component = gameObject.GetComponent<PlayerDamageEffect>();
    if (!(bool) (Object) component)
      return;
    component.Show(shot);
  }

  public int Health => (int) this._health;

  private enum RobotStates
  {
    Dance,
    Hide,
    Show,
    DoneShow,
    Explode,
    FadeOutParts,
    Dead,
  }
}
