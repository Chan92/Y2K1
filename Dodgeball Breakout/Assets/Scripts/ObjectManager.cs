using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : GenericSingelton<ObjectManager>
{
	private Dictionary<string, List<GameObject>> objectList = new Dictionary<string, List<GameObject>>();
	private const int wallRows = 3;

	public void SpawnToList(GameObject _object, Transform _objectParent, int _amount) 
	{
		//creates a new list if the list doesnt exist yet
		List<GameObject> _newList;
		if (!objectList.TryGetValue(_object.name, out _newList)) 
		{
			_newList = new List<GameObject>(_amount);
		}

		for(int i = 0; i < _amount; i++) 
		{
			GameObject _newObject = Instantiate(_object);
			_newObject.transform.parent = _objectParent;
			_newObject.transform.localPosition = Vector3.zero;
			_newObject.name = _object.name + " " + i;

			_newList.Add(_newObject);
		}

		//if the new objectlist are stones, positioning them
		if(_object.GetComponent<Stone>() != null) 
		{
			StonePositioning(_newList);
		}

		objectList[_object.name] = _newList;
	}

	//repositions the stones to create a wall and set stone points based on position
	private void StonePositioning(List<GameObject> _objects) 
	{
		Vector3 _positioning = _objects[0].transform.localPosition;
		Vector2 _size = new Vector2(_objects[0].transform.localScale.x, _objects[0].transform.localScale.y);
		
		int _column = 0;
		int _newnum = _objects.Count / wallRows;

		for(int _row = 1; _row <= wallRows; _row++) 
		{
			for(; _column < _newnum * _row; _column++) 
			{
				_objects[_column].transform.localPosition = _positioning;
				_positioning.x += _size.x;

				//multiplies the value of the stones to make the higher stones give more points
				_objects[_column].GetComponent<Stone>().pointValue *= (wallRows - _row + 1);
			}

			_positioning.x = _objects[0].transform.localPosition.x;
			_positioning.y += -_size.y;
		}
	}

	//counts howmany objects in the list are active
	public int EnabledInList(string _listName) 
	{
		List<GameObject> _list;
		if(objectList.TryGetValue(_listName, out _list)) 
		{
			int _counter = 0;
			foreach (GameObject _obj in _list) 
			{
				if(_obj.activeInHierarchy) 
				{
					_counter++;
				}
			}

			return _counter;
		} 
		else 
		{
			Debug.LogWarning("This list doesnt exist.");
			return 0;
		}
	}

	//Enables the objects in the list
	public void EnableListObjects(string _listName) 
	{
		List<GameObject> _list;
		if(objectList.TryGetValue(_listName, out _list)) 
		{
			foreach(GameObject _obj in _list) 
			{
				_obj.SetActive(true);
			}
		} 
		else 
		{
			Debug.LogWarning("This list doesnt exist.");
		}
	}
}
