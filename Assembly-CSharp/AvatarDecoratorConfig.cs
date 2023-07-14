// Decompiled with JetBrains decompiler
// Type: AvatarDecoratorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class AvatarDecoratorConfig : MonoBehaviour
{
  [SerializeField]
  private AvatarBone[] _avatarBones;
  private Color _skinColor;
  private List<Material> _materials;

  private void Awake()
  {
    this._materials = new List<Material>();
    foreach (AvatarBone avatarBone in this._avatarBones)
    {
      avatarBone.Collider = avatarBone.Transform.GetComponent<Collider>();
      avatarBone.Rigidbody = avatarBone.Transform.GetComponent<Rigidbody>();
      avatarBone.OriginalPosition = avatarBone.Transform.localPosition;
      avatarBone.OriginalRotation = avatarBone.Transform.localRotation;
    }
  }

  public List<Material> MaterialGroup => this._materials;

  public Transform GetBone(BoneIndex bone)
  {
    foreach (AvatarBone avatarBone in this._avatarBones)
    {
      if (avatarBone.Bone == bone)
        return avatarBone.Transform;
    }
    return this.transform;
  }

  public Color SkinColor
  {
    get => this._skinColor;
    set
    {
      this._skinColor = value;
      this.UpdateMaterials();
      foreach (Material material in this._materials)
      {
        if ((bool) (Object) material && material.name.Contains("Skin"))
          material.color = this._skinColor;
      }
    }
  }

  public IEnumerable<AvatarBone> Bones => (IEnumerable<AvatarBone>) this._avatarBones;

  public void SetBones(List<AvatarBone> bones) => this._avatarBones = bones.ToArray();

  public void UpdateMaterials()
  {
    SkinnedMeshRenderer componentInChildren = this.GetComponentInChildren<SkinnedMeshRenderer>();
    if (!(bool) (Object) componentInChildren)
      return;
    this._materials.Clear();
    foreach (Material material in componentInChildren.materials)
      this._materials.Add(material);
  }

  public static void CopyBones(AvatarDecoratorConfig srcAvatar, AvatarDecoratorConfig dstAvatar)
  {
    foreach (AvatarBone avatarBone in srcAvatar._avatarBones)
    {
      Transform bone = dstAvatar.GetBone(avatarBone.Bone);
      if ((bool) (Object) bone)
      {
        bone.position = avatarBone.Transform.position;
        bone.rotation = avatarBone.Transform.rotation;
      }
    }
  }
}
