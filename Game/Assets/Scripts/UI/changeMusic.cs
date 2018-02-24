using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class changeMusic : MonoBehaviour {

    public AudioClip level0Music;
    public AudioClip level1Music;

    private AudioSource source;
    private bool active;

    void Awake () 
    {
        source = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    public void toggleMusic(){
        if (active){
            source.Pause();
            active = false;
        }
        else {
            source.Play();
            active = true;
        }
    }

    /*
     * Load music depending on the game level.
     */
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        switch((int)scene.buildIndex){
           case 0:
                source.clip = level0Music;
                source.Play ();
                break;
            case 1:
                source.clip = level1Music;
                source.Play ();
                break;
        }
    }
}
