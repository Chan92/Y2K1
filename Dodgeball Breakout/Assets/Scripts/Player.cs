using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	[SerializeField]
	private float moveSpeed = 25f;
	private float lives;
	private Vector2 positionClamp = new Vector2(-9f, 9f);
	private Rigidbody rb;

	void Start() 
	{
		ResetGame();
		rb = gameObject.GetComponent<Rigidbody>();		
		Manager.Instance.resetGameDelegate += ResetGame;
	}

	void Update() 
	{	
		Movement();
		StartGame();
	}

	private void StartGame() 
	{
		if(Input.GetButtonDown("Jump"))
		{
			//if the game is gameover or cleared, reset the game
			if (Manager.gameOver) 
			{
				Manager.Instance.RestartGame();
			} 
			//else start playeing
			else 
			{
				Manager.Instance.startUI.SetActive(false);

				if(Manager.Instance.startGameDelegate != null) 
				{
					Manager.gameStarted = true;
					Manager.Instance.startGameDelegate();
				}
			}
		}
	}

	private void ResetGame() 
	{
		foreach (Transform _child in transform) 
		{
			_child.gameObject.SetActive(true);
		}

		lives = transform.childCount;
	}

	//reduces health if a rock hits the player
	public void Health() 
	{
		lives--;

		if(lives <= 0) 
		{
			Manager.Instance.GameOver(true);
		}
	}

	//moves the player based on input and 
	//limits the player to stay within the gamefield
	private void Movement() 
	{
		float _xAxis = Input.GetAxis("Horizontal");
		Vector3 _movement = new Vector3(_xAxis, 0f, 0f) * moveSpeed * Time.deltaTime;
		Vector3 _newPosition = transform.position + _movement;
		float _newPosX = Mathf.Clamp(_newPosition.x, positionClamp.x, positionClamp.y);
		_newPosition.x = _newPosX;
		
		rb.MovePosition(_newPosition);
	}

	private void OnDisable() 
	{
		Manager.Instance.resetGameDelegate -= ResetGame;
	}
}
