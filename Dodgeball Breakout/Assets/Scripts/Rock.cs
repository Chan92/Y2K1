using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
	private void OnTriggerEnter(Collider _other) 
	{
		if(_other.tag == "Player") 
		{
			_other.gameObject.SetActive(false);
			_other.GetComponentInParent<Player>().Health();
			RockPool.Instance.ReturnToPool(gameObject);
		}
	}
}
