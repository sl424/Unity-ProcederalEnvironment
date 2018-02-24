using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{

	public GameObject enemy;
	public List<GameObject> enemies = new List<GameObject>(); //create list of enemies to spawn for each wave
	public float spawnTime = 3f;
	public Transform[] spawnPoints;

	public int currentLevel;        // The current level.
	public int currentWave;        // The current wave.
	public static int foesLeft;       // Foes Left to defeat before wave ends.

	public Text infoText;         // Reference to the Text component for level, wave, foes left.
	public Text waveClearText;    // Reference to the Text component for wave cleared text.

	public static int[] waves = new int[] {0, 5, 45, 80};
	public int finalWave = waves.Length - 1;

	float timeRemaining = 10;
	        

	void Awake ()
	{
		infoText = GetComponentInChildren <Text> ();
		waveClearText.text = ""; //testing now.


		//when game starts, start on level 1 wave 1, create list of enemy game objects
		SetLevel(1);
		SetWave(1);
		CreateEnemyList();
		Spawn ();  
	}


	void Update ()
	{
		if (foesLeft == 0) {
			//waveClearText.text = "Wave Cleared!";

			if (timeRemaining >= 0) {
				timeRemaining -= Time.deltaTime;
				waveClearText.text = "Wave Cleared! New Waves Spawning In: " + timeRemaining.ToString ("f0");
				print (timeRemaining);
			}




		}

		//CheckFoesLeft();  
		infoText.text = "Level: " + currentLevel +  "\nWave: " + currentWave +  "\nFoes: " + foesLeft;
	}

	//init list of enemy objects for spawn on each wave
	void CreateEnemyList() {
		enemies.Clear();
		for (int i = 0; i < foesLeft; i++) {
			//Debug.Log(i);
			enemies.Add(enemy);
		}
	}


	void SetLevel(int level) {
		currentLevel = level;
	}

	void SetWave(int wave) {
		currentWave = wave;
		foesLeft = waves[currentWave];
	}

	void Spawn() 
	{



		int spawnPointIndex;

		for (int i = 0; i < foesLeft; i++) {
			spawnPointIndex = Random.Range(0, spawnPoints.Length);
			//this is a Transform, each Transform has a position and rotation
			Instantiate (enemies[i], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
			//Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
		}
	}
}
