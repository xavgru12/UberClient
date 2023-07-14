// Decompiled with JetBrains decompiler
// Type: AvatarBone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class AvatarBone
{
  public BoneIndex Bone;
  public Transform Transform;

  public Vector3 OriginalPosition { get; set; }

  public Quaternion OriginalRotation { get; set; }

  public Collider Collider { get; set; }

  public Rigidbody Rigidbody { get; set; }
}
