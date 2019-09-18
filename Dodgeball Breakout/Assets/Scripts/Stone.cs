using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour, IDamageable
{
	[SerializeField]
	private float maxHealthPoints = 1;
	public float pointValue = 100;
	public float HealthPoints 
	{
		get;
		set;
	}

	private void OnEnable() 
	{
		HealthPoints = maxHealthPoints;
	}

	//recieve damage when the ball hits the stone
	public void TakeDamage(float _damage) 
	{
		HealthPoints -= _damage;

		if (HealthPoints <= 0) 
		{
			Manager.Instance.AddPoints(pointValue, gameObject);
		}
	}
}
