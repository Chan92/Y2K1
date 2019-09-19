using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball :MonoBehaviour 
{
	[SerializeField]
	private float moveSpeed = 500f;
	private const float spawnOfset = 1f, strength = 1f;
	private const int maxLives = 5;	
	private int lives;
	private Transform player;
	private Rigidbody rb;

	private void Awake() 
	{	
		player = GameObject.FindWithTag("Player").transform;
		rb = gameObject.GetComponent<Rigidbody>();
	}

	private void Start() 
	{
		ResetLives();
		Respawn();

		Manager.Instance.startGameDelegate += ShootBall;
		Manager.Instance.resetGameDelegate += ResetLives;
	}

	private void ResetLives() 
	{
		lives = maxLives;
		Manager.Instance.ballsText.text = "Balls: " + lives;
		Respawn();
	}

	//Shooting the ball up when the game starts
	private void ShootBall() 
	{
		transform.parent = null;
		rb.isKinematic = false;
		rb.AddForce(new Vector3(0f, moveSpeed, 0f));
	}

	//reduces live when the ball gets out the gamefield
	private void Health() 
	{
		lives--;

		if(lives <= 0) 
		{
			Manager.Instance.GameOver(true);
		} 
		else 
		{
			Respawn();
		}
	}

	//break stones
	private void OnCollisionEnter(Collision _collision) 
	{
		IDamageable _takeDmg = _collision.gameObject.GetComponent<IDamageable>();
		if(_takeDmg != null) 
		{
			_takeDmg.TakeDamage(strength);
		}
	}

	//respawn the ball after dying
	private void Respawn() 
	{
		rb.Sleep();
		transform.position = player.position + new Vector3(0f, spawnOfset, 0f);
		transform.parent = player;
		rb.isKinematic = true;
		rb.WakeUp();
	}

	//if the ball gets out of the game, check for the health
	private void OnTriggerExit(Collider _other) 
	{
		if(_other.GetComponent<Manager>() != null) 
		{
			Health();
			Manager.gameStarted = false;
			Manager.Instance.ballsText.text = "Balls: " + lives;
		}
	}

	private void OnDestroy() 
	{
		Manager.Instance.startGameDelegate -= ShootBall;
		Manager.Instance.resetGameDelegate -= ResetLives;
	}
}
