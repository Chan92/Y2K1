using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
	float HealthPoints 
	{
		get;
		set;
	}

	void TakeDamage(float _damage);
}
