// Decompiled with JetBrains decompiler
// Type: ObjectRecycler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectRecycler
{
  private List<GameObject> _objectList;
  private GameObject _objectToRecycle;
  private GameObject _parentObject;

  public ObjectRecycler(GameObject gameObject, int initialCapacity, GameObject parentObject = null)
  {
    this._objectList = new List<GameObject>(initialCapacity);
    this._objectToRecycle = gameObject;
    this._parentObject = parentObject;
    for (int index = 0; index < initialCapacity; ++index)
    {
      GameObject gameObject1 = UnityEngine.Object.Instantiate((UnityEngine.Object) this._objectToRecycle) as GameObject;
      gameObject1.gameObject.SetActive(false);
      if ((UnityEngine.Object) parentObject != (UnityEngine.Object) null)
        gameObject1.transform.parent = this._parentObject.transform;
      this._objectList.Add(gameObject1);
    }
  }

  public GameObject GetNextFree()
  {
    GameObject nextFree = this._objectList.Where<GameObject>((Func<GameObject, bool>) (item => !item.activeSelf)).FirstOrDefault<GameObject>();
    if ((UnityEngine.Object) nextFree == (UnityEngine.Object) null)
    {
      nextFree = UnityEngine.Object.Instantiate((UnityEngine.Object) this._objectToRecycle) as GameObject;
      if ((UnityEngine.Object) this._parentObject != (UnityEngine.Object) null)
        nextFree.transform.parent = this._parentObject.transform;
      this._objectList.Add(nextFree);
    }
    nextFree.SetActive(true);
    return nextFree;
  }

  public void FreeObject(GameObject objectToFree) => objectToFree.gameObject.SetActive(false);
}
