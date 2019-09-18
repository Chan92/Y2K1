using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void StartGameDelegate();
public delegate void ResetGameDelegate();

public class Manager : GenericSingelton<Manager> 
{
	public float CurrentPoints 
	{
		get;
		set;
	}

	public Text pointsText, ballsText;
	public GameObject gameOverUI, victoryUI, startUI;

	public static bool gameStarted = false, gameOver = false;
	public StartGameDelegate startGameDelegate;
	public ResetGameDelegate resetGameDelegate;

	[SerializeField]
	private GameObject rockPrefab, enemyPrefab, stonePrefab;
	private Transform enemyParent, stoneParent;	
	private const int startEnemyAmount = 2, startStoneAmount = 39, rocksPerEnemy = 2;

	protected override void Awake() 
	{
		base.Awake();

		enemyParent = GameObject.Find("EnemySpawn").transform;
		stoneParent = GameObject.Find("Wall").transform;	
	}

	private void Start() 
	{
		startUI.SetActive(true);
		victoryUI.SetActive(false);
		gameOverUI.SetActive(false);

		FillLists();
		FillPools();
	}

	//adds stones to the rock pool
	private void FillPools() 
	{
		int _enemyCount = ObjectManager.Instance.EnabledInList(enemyPrefab.name);
		int _rocksToSpawn = (_enemyCount * rocksPerEnemy) - RockPool.Instance.PoolSize();
		RockPool.Instance.SpawnToPool(rockPrefab, _rocksToSpawn);		
	}

	private void FillLists() 
	{
		//spawn enemies
		ObjectManager.Instance.SpawnToList(enemyPrefab, enemyParent, startEnemyAmount);
		
		//spawn blocks and position them
		ObjectManager.Instance.SpawnToList(stonePrefab, stoneParent, startStoneAmount);
	}

	public void AddPoints(float _obtainedPoints, GameObject _stone)
	{
		CurrentPoints += _obtainedPoints;
		pointsText.text = "Score: " + CurrentPoints;
		RemoveStone(_stone);
	}

	//When a stone breaks, remove it from the list and check if its the last
	public void RemoveStone(GameObject _stone) 
	{
		_stone.SetActive(false);

		if(ObjectManager.Instance.EnabledInList(stonePrefab.name) == 0) 
		{
			GameOver(false);
		}
	}

	//Gameover if the player broke or is out of balls
	//Victory if all stones broke
	public void GameOver(bool _levelFailed) 
	{
		gameStarted = false;
		gameOver = true;
		if(_levelFailed) 
		{
			gameOverUI.SetActive(true);
		} 
		else 
		{
			victoryUI.SetActive(true);
		}
	}

	//start a new game after gameover or victory
	public void RestartGame() 
	{
		ObjectManager.Instance.EnableListObjects(stonePrefab.name);

		resetGameDelegate?.Invoke();

		CurrentPoints = 0;
		pointsText.text = "Score: 0";
		victoryUI.SetActive(false);
		gameOverUI.SetActive(false);
		gameOver = false;

		startUI.SetActive(true);
	}
}