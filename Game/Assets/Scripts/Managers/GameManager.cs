using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public bool isFirstPerson = false;
    public PlayerHealth playerHealth;
    //public GameObject enemy;
    public GameObject enemyFast; 
    public GameObject enemyTank; 
    public GameObject enemyReg; 

    public List<GameObject> enemies = new List<GameObject>(); //create list of enemies to spawn for each wave
    //public List<GameObject> fastEnemies = new List<GameObject>(); //create list of enemies to spawn for each wave
    //public List<GameObject> tankEnemies = new List<GameObject>(); //create list of enemies to spawn for each wave
    //public List<GameObject> regEnemies = new List<GameObject>(); //create list of enemies to spawn for each wave

    public float spawnTime = 3f; public Transform[] spawnPoints; 

    public int currentLevel;        // The current level.
    public int currentWave;        // The current wave.
    public static int foesLeft;       // Foes Left to defeat before wave ends.

    public Text infoText;         // Reference to the Text component for level, wave, foes left.
    public Text waveClearText;    // Reference to the Text component for wave cleared text.
    /*
       total enemies for wave 0 through N 
       */
    //public static int[] waves = new int[] {0, 15, 30, 50};
                                         // wave # {0, 1, 2, 3, 4,  5,  6,}
    public static int[] fastEnemyWaves = new int[] {0, 0, 0, 6, 0,  0,  10};
    public static int[] tankEnemyWaves = new int[] {0, 0, 0, 0, 2,  4,  0 };
    public static int[] regEnemyWaves = new int[]  {0, 4, 8, 0, 12, 16, 0};
    public int finalWave;

    float timeRemaining = -1;

    void start(){
     timeRemaining = -1;
    }

    public void beginGame ()
    {
        finalWave = regEnemyWaves.Length - 1;
        playerHealth = GetComponentInChildren <PlayerHealth> ();

        //spawn enemies
        //InvokeRepeating("Spawn", spawnTime, spawnTime);

        // Set up the reference.
        infoText = GetComponentInChildren <Text> ();
        waveClearText.text = ""; //testing now.


        //when game starts, start on level 1 wave 1, create list of enemy game objects
        timeRemaining = 5;
        SetLevel(1);
        SetWave(1);
        CreateEnemyList();
        Spawn ();  



        //text.text = "Level: " + currentLevel +  "\nWave: " + currentWave +  "\nFoes: " + foesLeft;
    }

    //init list of enemy objects for spawn on each wave
    void CreateEnemyList() {
        //fastEnemies.Clear();
        //tankEnemies.Clear();
        //regEnemies.Clear();
        enemies.Clear();
        for (int i = 0; i < fastEnemyWaves[currentWave]; i++) {
            enemies.Add(enemyFast);
        }
        for (int i = 0; i < regEnemyWaves[currentWave]; i++) {
            enemies.Add(enemyReg);
        }
        for (int i = 0; i < tankEnemyWaves[currentWave]; i++) {
            enemies.Add(enemyTank);
        }

    }

    void SetLevel(int level) {
        currentLevel = level;
    }

    void SetWave(int wave) {
        currentWave = wave;
        foesLeft = fastEnemyWaves[currentWave] + regEnemyWaves[currentWave] + tankEnemyWaves[currentWave];
    }

    //Commented out by Alejandro For testing.
    /*
       void CheckFoesLeft() {
       int remaining = 0; 
       for (int i = 0; i < enemies.Count; i++) {
       if (enemies[i] != null) {  //if still alive add to remaining count
       remaining++;
       }
       }
       foesLeft = remaining;
       }
       */
    //Testing countdown





    void Update ()
    {

		if (foesLeft < 1 && currentWave != finalWave) {

            if (timeRemaining >= 0) { //counts down from 10 to 0
                timeRemaining -= Time.deltaTime;
                waveClearText.text = "Wave Cleared! New Waves Spawning In: " + timeRemaining.ToString ("f0");

            }
            else { 
                waveClearText.text = "";
                timeRemaining = 5;
                SetWave(currentWave + 1);
                CreateEnemyList();
                Spawn ();  
            }




        }

        //CheckFoesLeft();  
        infoText.text = "Level: " + currentLevel +  "\nWave: " + currentWave +  "\nFoes: " + foesLeft;

		//testing now -Alex
		if (currentWave == finalWave && foesLeft < 1) {

			waveClearText.text = "You win!";
		}

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

