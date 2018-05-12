using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {
    GameObject pauseObjects;
    GameObject confirmRestartObjects;
    GameObject confirmQuitObjects;
    GameObject gameOverObjects;

    // Use this for initialization
    void Start () {
        Time.timeScale = 1;
        //pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        pauseObjects = GameObject.FindWithTag("ShowOnPause");
        confirmRestartObjects = GameObject.FindWithTag("ConfirmRestart");
        confirmQuitObjects = GameObject.FindWithTag("ConfirmQuit");
        gameOverObjects = GameObject.FindWithTag("ShowOnGameOver");
        hideAll();
    }

    // Update is called once per frame
    void Update () {

        //uses the p button to pause and unpause the game
        if(Input.GetKeyDown(KeyCode.P))
        {
            pauseControl();		
        }
    }

    //controls the pausing of the scene
    public void pauseControl(){
        if (PlayerHealth.isDead) {return;}

        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            showPaused();
        } else if (Time.timeScale == 0){
            Time.timeScale = 1;
            Cursor.visible = false;
            hideAll();
        }
    }



    //hides all possible pause objects 
    public void hideAll(){
        pauseObjects.SetActive(false);
        confirmRestartObjects .SetActive(false);
        confirmQuitObjects.SetActive(false);
        gameOverObjects.SetActive(false);
    }


    //shows objects with ShowOnPause tag
    public void showPaused(){
        hideAll();
        pauseObjects.SetActive(true);
    }

    //shows confirm restart menu 
    public void showConfirmRestart(){
        hideAll();
        confirmRestartObjects.SetActive(true);
    }

    //shows confirm quit menu 
    public void showConfirmQuit(){
        hideAll();
        confirmQuitObjects.SetActive(true);
    }

    //shows game over menu 
    public void showGameOver(){
        hideAll();
        Cursor.visible = true;
        gameOverObjects.SetActive(true);
    }

    //Restarts the current Level
    public void Restart(){

		Scene currentScene = SceneManager.GetActiveScene();
		string sceneName = currentScene.name;

		if (sceneName == "level3") {
			SceneManager.LoadScene (3);
		} else if (sceneName == "level5") {
			SceneManager.LoadScene (5);
		} else {
			SceneManager.LoadScene (1);
		}
    }

    // go to main menu
    public void ExitToMain(){
        SceneManager.LoadScene(0);
    }



}
