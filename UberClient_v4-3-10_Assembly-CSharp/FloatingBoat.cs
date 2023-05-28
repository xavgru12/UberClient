// Decompiled with JetBrains decompiler
// Type: FloatingBoat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class FloatingBoat : MonoBehaviour
{
  public float Offset = 1f;
  public float Force = 400f;
  public Transform bug;
  public Transform heck1;
  public Transform heck2;
  private Rigidbody rb;
  private Transform tf;
  private float torque;

  private void Start()
  {
    this.rb = this.rigidbody;
    this.tf = this.transform;
  }

  private void OnEnable()
  {
    this.StopCoroutine("startKeepBoatUpright");
    this.StartCoroutine("startKeepBoatUpright");
  }

  [DebuggerHidden]
  private IEnumerator startKeepBoatUpright() => (IEnumerator) new FloatingBoat.\u003CstartKeepBoatUpright\u003Ec__Iterator37()
  {
    \u003C\u003Ef__this = this
  };
}
