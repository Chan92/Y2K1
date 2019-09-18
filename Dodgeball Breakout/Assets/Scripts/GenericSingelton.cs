﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSingelton<T> : MonoBehaviour
	where T : Component
{

	public static T Instance 
	{
		get;
		private set;
	}

	protected virtual void Awake() 
	{

		//print("singleton: " + GetInstanceID());
		if(Instance == null) 
		{
			Instance = this as T;
			DontDestroyOnLoad(this);
		} 
		else 
		{
			Destroy(gameObject);
		}
	}
}