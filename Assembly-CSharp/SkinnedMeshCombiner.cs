// Decompiled with JetBrains decompiler
// Type: SkinnedMeshCombiner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshCombiner
{
  public static void Combine(GameObject target, List<GameObject> objects)
  {
    if (!(bool) (Object) target || objects == null)
      return;
    List<SkinnedMeshRenderer> otherGear = new List<SkinnedMeshRenderer>();
    foreach (GameObject gameObject in objects)
    {
      if ((Object) gameObject != (Object) null)
        otherGear.AddRange((IEnumerable<SkinnedMeshRenderer>) gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true));
    }
    SkinnedMeshCombiner.SuperCombineCreate(target, otherGear);
  }

  public static void Update(GameObject target, List<GameObject> objects)
  {
    List<SkinnedMeshRenderer> otherGear = new List<SkinnedMeshRenderer>();
    if ((bool) (Object) target && objects != null)
    {
      foreach (GameObject gameObject in objects)
      {
        if ((bool) (Object) gameObject)
          otherGear.AddRange((IEnumerable<SkinnedMeshRenderer>) gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true));
      }
    }
    SkinnedMeshCombiner.SuperCombineUpdate(target, otherGear);
  }

  private static GameObject SuperCombineCreate(
    GameObject sourceGameObject,
    List<SkinnedMeshRenderer> otherGear)
  {
    foreach (SkinnedMeshRenderer skinnedMeshRenderer in otherGear)
    {
      if ((Object) skinnedMeshRenderer.sharedMesh == (Object) null)
        Debug.LogError((object) (skinnedMeshRenderer.name + "'s sharedMesh is null!"));
    }
    List<CombineInstance> combineInstanceList = new List<CombineInstance>();
    List<Material> materialList = new List<Material>();
    List<Transform> transformList = new List<Transform>();
    Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>();
    foreach (Transform componentsInChild in sourceGameObject.GetComponentsInChildren<Transform>(true))
      dictionary.Add(componentsInChild.name, componentsInChild.transform);
    foreach (SkinnedMeshRenderer componentsInChild in sourceGameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true))
    {
      materialList.AddRange((IEnumerable<Material>) componentsInChild.sharedMaterials);
      for (int index = 0; index < componentsInChild.sharedMesh.subMeshCount; ++index)
      {
        combineInstanceList.Add(new CombineInstance()
        {
          mesh = componentsInChild.sharedMesh,
          subMeshIndex = index
        });
        transformList.AddRange((IEnumerable<Transform>) componentsInChild.bones);
      }
      Object.Destroy((Object) componentsInChild);
    }
    if (otherGear != null && otherGear.Count > 0)
    {
      foreach (SkinnedMeshRenderer skinnedMeshRenderer in otherGear)
      {
        materialList.AddRange((IEnumerable<Material>) skinnedMeshRenderer.sharedMaterials);
        if (!((Object) skinnedMeshRenderer.sharedMesh == (Object) null))
        {
          for (int index = 0; index < skinnedMeshRenderer.sharedMesh.subMeshCount; ++index)
          {
            combineInstanceList.Add(new CombineInstance()
            {
              mesh = skinnedMeshRenderer.sharedMesh,
              subMeshIndex = index
            });
            foreach (Transform bone in skinnedMeshRenderer.bones)
            {
              if (dictionary.ContainsKey(bone.name))
                transformList.Add(dictionary[bone.name]);
              else
                Debug.LogError((object) ("I couldn't find a matching bone transform in the gameobject you're trying to add this skinned mesh to! " + bone.name));
            }
          }
        }
      }
    }
    SkinnedMeshRenderer skinnedMeshRenderer1 = sourceGameObject.AddComponent<SkinnedMeshRenderer>();
    if ((Object) skinnedMeshRenderer1.sharedMesh == (Object) null)
      skinnedMeshRenderer1.sharedMesh = new Mesh();
    skinnedMeshRenderer1.sharedMesh.Clear();
    skinnedMeshRenderer1.sharedMesh.name = "CombinedMesh";
    skinnedMeshRenderer1.sharedMesh.CombineMeshes(combineInstanceList.ToArray(), false, false);
    skinnedMeshRenderer1.bones = transformList.ToArray();
    foreach (Object material in skinnedMeshRenderer1.materials)
      Object.Destroy(material);
    skinnedMeshRenderer1.materials = materialList.ToArray();
    Animation component = sourceGameObject.GetComponent<Animation>();
    if ((bool) (Object) component)
      component.cullingType = AnimationCullingType.AlwaysAnimate;
    return sourceGameObject;
  }

  private static GameObject SuperCombineUpdate(
    GameObject sourceGameObject,
    List<SkinnedMeshRenderer> otherGear)
  {
    List<CombineInstance> combineInstanceList = new List<CombineInstance>();
    List<Material> materialList = new List<Material>();
    List<Transform> transformList = new List<Transform>();
    Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>();
    foreach (Transform componentsInChild in sourceGameObject.GetComponentsInChildren<Transform>(true))
    {
      if (!dictionary.ContainsKey(componentsInChild.name))
        dictionary.Add(componentsInChild.name, componentsInChild.transform);
    }
    if (otherGear != null && otherGear.Count > 0)
    {
      foreach (SkinnedMeshRenderer skinnedMeshRenderer in otherGear)
      {
        materialList.AddRange((IEnumerable<Material>) skinnedMeshRenderer.sharedMaterials);
        if (!((Object) skinnedMeshRenderer.sharedMesh == (Object) null))
        {
          for (int index = 0; index < skinnedMeshRenderer.sharedMesh.subMeshCount; ++index)
          {
            combineInstanceList.Add(new CombineInstance()
            {
              mesh = skinnedMeshRenderer.sharedMesh,
              subMeshIndex = index
            });
            foreach (Transform bone in skinnedMeshRenderer.bones)
            {
              if (dictionary.ContainsKey(bone.name))
                transformList.Add(dictionary[bone.name]);
              else
                Debug.LogError((object) ("I couldn't find a matching bone transform in the gameobject you're trying to add this skinned mesh to! " + bone.name));
            }
          }
        }
      }
    }
    else
      Debug.LogError((object) "Gear array contains no Skinned Meshes! Trying to go naked?");
    SkinnedMeshRenderer component1 = sourceGameObject.GetComponent<SkinnedMeshRenderer>();
    if ((bool) (Object) component1)
    {
      if ((Object) component1.sharedMesh == (Object) null)
        component1.sharedMesh = new Mesh();
      component1.sharedMesh.Clear();
      component1.sharedMesh.name = "CombinedMesh";
      component1.sharedMesh.CombineMeshes(combineInstanceList.ToArray(), false, false);
      component1.bones = transformList.ToArray();
      foreach (Object material in component1.materials)
        Object.Destroy(material);
      component1.materials = materialList.ToArray();
    }
    else
      Debug.LogError((object) ("There is no SkinnedMeshRenderer on " + sourceGameObject.name));
    Animation component2 = sourceGameObject.GetComponent<Animation>();
    if ((bool) (Object) component2)
      component2.cullingType = AnimationCullingType.AlwaysAnimate;
    return sourceGameObject;
  }
}
