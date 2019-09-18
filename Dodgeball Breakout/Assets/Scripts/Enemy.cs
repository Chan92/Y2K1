using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed = 5f, throwTime = 3f;

	private Vector2 positionClamp = new Vector2(-11f, 11f);
	private Transform player;

	private float randomTarget;
	private const float minThrowDelay = 0f, maxThrowDelay = 5f;	

	private void Start()
    {
		player = GameObject.FindWithTag("Player").transform;
		Manager.Instance.startGameDelegate += StartThrow;
		StartCoroutine(Movement());
	}

	//start throwing when the game starts
	private void StartThrow() 
	{
		StartCoroutine(Throw());
	}
	
	private IEnumerator Movement() 
	{
		while (true) 
		{
			randomTarget = Random.Range(positionClamp.x, positionClamp.y);
			Vector3 _target = new Vector3(randomTarget, transform.position.y, transform.position.z);

			while(transform.position != _target) 
			{
				transform.position = Vector3.MoveTowards(transform.position, _target, moveSpeed * Time.deltaTime);
				yield return new WaitForSeconds(0);
			}
		}		
	}

	//throws a rock towards the player
	private IEnumerator Throw() 
	{
		while (Manager.gameStarted) 
		{
			float _throwDelay = Random.Range(minThrowDelay, maxThrowDelay);
			yield return new WaitForSeconds(_throwDelay);

			Transform _rock = RockPool.Instance.GetFromPool();
			if(_rock != null) 
			{
				Vector3 _rockSpawn = transform.position;
				Vector3 _destination = player.position;
				_rock.position = _rockSpawn;	
				float time = 0;
			
				while(time < throwTime) 
				{
					_rock.position = Vector3.Lerp(_rockSpawn, _destination, (time / throwTime));
					time += Time.deltaTime;

					yield return null;
				}

				RockPool.Instance.ReturnToPool(_rock.gameObject);
			}
		}
	}

	private void OnDestroy() 
	{
		Manager.Instance.startGameDelegate -= StartThrow;
	}
}
