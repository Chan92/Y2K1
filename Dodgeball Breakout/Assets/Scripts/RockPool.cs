using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPool :GenericSingelton<RockPool> {
	private Queue<GameObject> poolList = new Queue<GameObject>();

	//spawn an amount of objects and add them to the pool
	public void SpawnToPool(GameObject _poolablePrefab, int _amount) 
	{
		for(int i = 0; i < _amount; i++) 
		{
			GameObject _newObject = Instantiate(_poolablePrefab);
			_newObject.transform.parent = transform;
			_newObject.SetActive(false);
			poolList.Enqueue(_newObject);
		}
	}

	//gets an object from the pool
	public Transform GetFromPool() 
	{
		if(poolList.Count <= 0) 
		{
			return null;
		}

		Transform _object = poolList.Dequeue().transform;
		_object.gameObject.SetActive(true);
		return _object;
	}

	//returns the object to the pool
	public void ReturnToPool(GameObject _object) 
	{
		if(!poolList.Contains(_object)) 
		{
			poolList.Enqueue(_object);
			_object.SetActive(false);

		} 
	}

	//makes the list size public while keeping the list itself private
	public int PoolSize()
	{
		if(poolList == null) 
		{
			return 0;
		}

		return poolList.Count;
	}
}