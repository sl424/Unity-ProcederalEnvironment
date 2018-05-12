using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int nextPerk;
    int nextPerkInTime;
    bool playerHasPerk = false;
    //public GameObject locator;

    //used to call a perk function
    public static Dictionary<int, System.Action> perks = new Dictionary<int, System.Action>();
        // add perk functions to dict

    public AudioClip regBullet;
    public AudioClip fastBullet;
    public AudioClip cannonBullet;

    //public AudioSource youWinSX;

    public bool isFirstPerson = false;
    public PlayerHealth playerHealth;
    public PlayerShooting playerShooting;
    public PlayerMovement playerMovement;

    public GameObject enemyFast; 
    public GameObject enemyTank; 
    public GameObject enemyReg; 
    public GameObject enemyRanged; 

    public GameObject perk3D;
    public static GameObject clone = null;

    public List<GameObject> enemies = new List<GameObject>(); //create list of enemies to spawn for each wave

    public float spawnTime = 3f; 
    public Transform[] spawnPoints; 

    public int currentLevel;        // The current level.
    public int currentWave;        // The current wave.
    public static int foesLeft;       // Foes Left to defeat before wave ends.

    public Text infoText;         // Reference to the Text component for level, wave, foes left.
    public Text waveClearText;    // Reference to the Text component for wave cleared text.
    public Text perkText;    // Reference to the Text component for perk text. 
    /*
       total enemies for wave 0 through N 
     */
    //public static int[] waves = new int[] {0, 15, 30, 50};
    // wave #      {0, 1, 2, 3, 4,  5,  6,}

    public static int[] fastEnemyWaves = new int[]      {0, 0,  0,  5,  0,  3};
    public static int[] tankEnemyWaves = new int[]      {0, 0,  2,  0,  0,  1};
    public static int[] regEnemyWaves = new int[]       {0, 3,  3,  0,  4,  3};
    public static int[] rangedEnemyWaves = new int[]    {0, 0,  0,  0,  2,  1};

    /*  For testing purposes
    public static int[] fastEnemyWaves = new int[]      {0, 0};
    public static int[] tankEnemyWaves = new int[]      {0, 0};
    public static int[] regEnemyWaves = new int[]       {0, 1};
    public static int[] rangedEnemyWaves = new int[]    {0, 0};
    */


    public int finalWave;

    float timeRemaining = -1;
    int perkTime = 10; // duration of perk

    void Start(){
               timeRemaining = -1;
    }


    public void beginGame ()
    {
        finalWave = regEnemyWaves.Length - 1;
        //finalWave = 1; 

        //youWinSX = GetComponent <AudioSource> ();
        playerHealth = GetComponentInChildren <PlayerHealth> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();
        playerMovement = GetComponentInChildren <PlayerMovement> ();

        // Set up the reference.
        //infoText = GetComponentInChildren <Text> ();
        infoText.text = "";

        waveClearText.text = ""; 

        perkText.text = ""; 
        perks.Clear();
        perks.Add(0, FullHealthPerk);
        perks.Add(1, FastMovementPerk);
        perks.Add(2, RapidFirePerk);
        perks.Add(3, InstantKillPerk);



        //when game starts, start on level 1 wave 1, create list of enemy game objects
        timeRemaining = 5;
        SetLevel(1);
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "level3") {
            SetLevel(2); 
        }
        else if (sceneName == "level5") {
            SetLevel(3); 
        }

        SetWave(1);
        CreateEnemyList();
        Spawn ();  


    }


    // picks a random perk to spawn at a random time at a random spawn point
    void RandomizePerk(){
        nextPerk = Random.Range(0, 4); //perk type
        nextPerkInTime = Random.Range(10, 60); //how often perk spawns 
        Invoke("SpawnPerk", nextPerkInTime); //spawn perk after time
    }


    //init list of enemy objects for spawn on each wave
    void CreateEnemyList() {
        Debug.Log(currentLevel);
        Debug.Log(regEnemyWaves[currentWave] * currentLevel);

        enemies.Clear();
        for (int i = 0; i < fastEnemyWaves[currentWave] * currentLevel; i++) {
            enemies.Add(enemyFast);
        }
        for (int i = 0; i < regEnemyWaves[currentWave] * currentLevel; i++) {
            Debug.Log(i);
            enemies.Add(enemyReg);
        }
        for (int i = 0; i < tankEnemyWaves[currentWave] * currentLevel; i++) {
            enemies.Add(enemyTank);
        }
        for (int i = 0; i < rangedEnemyWaves[currentWave] * currentLevel; i++) {
            enemies.Add(enemyRanged);
        }

    }

    void SetLevel(int level) {
        currentLevel = level;
    }

    void SetWave(int wave) {
        currentWave = wave;
        foesLeft = fastEnemyWaves[currentWave] * currentLevel + regEnemyWaves[currentWave] * currentLevel + tankEnemyWaves[currentWave] * currentLevel + rangedEnemyWaves[currentWave] * currentLevel;

        RandomizePerk(); // picks a random perk to spawn at a random time at a random spawn point
    }

    public static void SetPerk() {
        perks[nextPerk]();
        //RemovePerk();
    }

    void DisableFastMovementPerk() {
        playerHasPerk = false;
        perkText.text = ""; 
        playerMovement.changeMovementSpeed(7f); 
    } 

    // call this when player picks up fast movement perk
    void FastMovementPerk() {
        playerHasPerk = true;
        Debug.Log("Fast Speed");
        perkText.text = "Increased Speed"; 

        playerMovement.changeMovementSpeed(20f); 
        Invoke("DisableFastMovementPerk", perkTime);
    } 
//
    // call this when player picks up full health perk
    void FullHealthPerk() {
        playerHasPerk = true;
        Debug.Log("Full Health");
        perkText.text = "Full Health"; 

        playerHealth.fullHealth();  
        Invoke("RemoveFullHealthText", 4);
    }

    void RemoveFullHealthText() {
        playerHasPerk = false;
        perkText.text = ""; 
    }

    // call this when resetting fire rate
    void DisableRapidFirePerk() {
        playerHasPerk = false;
        perkText.text = ""; 

        playerShooting.updateShootSpeed(.15f); 
        playerShooting.switchGunAudio(regBullet);
    }

    // call this when player picks up rapid fire perk
    void RapidFirePerk() {
        playerHasPerk = true;
        Debug.Log("Rapid Fire");
        perkText.text = "Rapid Fire"; 

        playerShooting.updateShootSpeed(.07f); 
        playerShooting.switchGunAudio(fastBullet);
        Invoke("DisableRapidFirePerk", perkTime);
    } 

    // disables instant kill perk
    void DisableInstantKillPerk(){
        playerHasPerk = false;
        perkText.text = ""; 

        playerShooting.updateDamage(20); 
        playerShooting.updateShootSpeed(.15f); 
        playerShooting.switchGunAudio(regBullet);
    }

    // call this when player picks up instant kill perk
    void InstantKillPerk() {
        playerHasPerk = true;
        Debug.Log("Instant Kill");
        perkText.text = "Instant Kill"; 

        playerShooting.updateDamage(1000); 
        playerShooting.updateShootSpeed(.50f); 
        playerShooting.switchGunAudio(cannonBullet);
        Invoke("DisableInstantKillPerk", perkTime);
    } 


    void LoadEndGame(){
        Cursor.visible = true;
        SceneManager.LoadScene ("WinScreen"); //testing now
    }


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

        infoText.text = "Level: " + currentLevel +  "\nWave: " + currentWave +  "\nFoes: " + foesLeft;

        if (currentWave == finalWave && foesLeft < 1) {
            if (currentLevel == 3) {
                LoadEndGame(); 
            }
            else if (currentLevel == 2) {
                Cursor.visible = true;
                SceneManager.LoadScene ("TwoToThreeScreen"); //testing now
                //currentLevel = 3;
            }
            else {
                Cursor.visible = true;
                SceneManager.LoadScene ("OneToTwoScreen"); //testing now
                //currentLevel = 2;
            }
        }

    }

    static int maxl = 8;
    int[,] spawnzone = new int[maxl,maxl];

    Vector3 returnSpawnZone(){
        int i = Random.Range(0,maxl);
        int j = Random.Range(0,maxl);
        while (spawnzone[i,j]==1) {
            i = Random.Range(0,maxl);
            j = Random.Range(0,maxl);
        }
        spawnzone[i,j] = 1;
        Vector3 r = new Vector3(i-maxl/2, 0, j-maxl/2);
        return r;
    }

    Vector3 returnHeight(Vector3 pos){
        //locator.transform.position = pos+returnSpawnZone();
        Vector3 tmp = pos+returnSpawnZone();
        RaycastHit hit;
        if (Physics.Raycast(tmp+Vector3.up*10, Vector3.down, out hit, 100f)){
            //Debug.Log(hit.point);
            return hit.point;
        }
        return  tmp;
    }

    void Spawn() 
    {

        int spawnPointIndex;

        for (int i = 0; i < foesLeft; i++) {
            spawnPointIndex = Random.Range(0, spawnPoints.Length);

            //this is a Transform, each Transform has a position and rotation
            Instantiate (enemies[i], returnHeight(spawnPoints[spawnPointIndex].position), spawnPoints[spawnPointIndex].rotation);
        }
        spawnzone = new int[maxl,maxl];
    }

    void SpawnPerk() {
        //Debug.Log("attempting to spawn PERK");

        //make sure there isn't already a perk out and player doesn't have a perk currently
        if (clone == null && !playerHasPerk) { 

            int spawnPointIndex;
            spawnPointIndex = Random.Range(0, spawnPoints.Length);

            clone = Instantiate (perk3D, spawnPoints[spawnPointIndex].position, perk3D.transform.rotation);
            //Debug.Log("SPAWNING PERK");
            Invoke("RemovePerk", 15); // destroy perk after 15 s
        }
    }

    
    // removes game object for perk
    void RemovePerk() {
        //Debug.Log("REMOVING PERK");

        if (clone != null) {
        
            Destroy(clone, 0f);
            clone = null;
        
        }
    }



}

