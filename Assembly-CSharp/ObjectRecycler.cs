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
		_objectList = new List<GameObject>(initialCapacity);
		_objectToRecycle = gameObject;
		_parentObject = parentObject;
		for (int i = 0; i < initialCapacity; i++)
		{
			GameObject gameObject2 = Object.Instantiate(_objectToRecycle) as GameObject;
			gameObject2.gameObject.SetActive(value: false);
			if (parentObject != null)
			{
				gameObject2.transform.parent = _parentObject.transform;
			}
			_objectList.Add(gameObject2);
		}
	}

	public GameObject GetNextFree()
	{
		GameObject gameObject = _objectList.Where((GameObject item) => !item.activeSelf).FirstOrDefault();
		if (gameObject == null)
		{
			gameObject = (Object.Instantiate(_objectToRecycle) as GameObject);
			if (_parentObject != null)
			{
				gameObject.transform.parent = _parentObject.transform;
			}
			_objectList.Add(gameObject);
		}
		gameObject.SetActive(value: true);
		return gameObject;
	}

	public void FreeObject(GameObject objectToFree)
	{
		objectToFree.gameObject.SetActive(value: false);
	}
}
