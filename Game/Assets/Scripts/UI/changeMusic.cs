using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class changeMusic : MonoBehaviour {

    public AudioClip level0Music;
    public AudioClip level1Music;
	public AudioClip level3Music;
	public AudioClip level5Music;
	public AudioClip IntroMusic;
	public AudioClip oneToTwoMusic;
	public AudioClip TwoToThreeMusic;
	public AudioClip WinMusic;

    private static AudioSource source;
    private bool active;

    /*
    void Awake () 
    {
        source = GetComponent<AudioSource>();
    }
    */
    private static changeMusic instance;
 
    public static changeMusic Instance
    {
        get { return instance; }
    }
 
    private void Awake()
    {
        // If no Player ever existed, we are it.
        if (instance == null){
            instance = this;
            source = GetComponent<AudioSource>();
        }
        // If one already exist, it's because it came from another level.
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
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
            case 1:
                source.clip = level1Music;
                source.Play ();
                break;
			case 2:
			     source.clip = WinMusic;
			     source.Play ();
			     break;
		   case 3:
			   source.clip = level3Music;
			   source.Play ();
			   break;
			case 4:
				source.clip = IntroMusic;
				source.Play ();
				break;
		   case 5:
			   source.clip = level5Music;
			   source.Play ();
			   break;
			case 6:
				source.clip = oneToTwoMusic;
				source.Play ();
				break;
			case 7:
				source.clip = TwoToThreeMusic;
				source.Play ();
				break;
           default:
                source.clip = level0Music;
                source.Play ();
                break;
        }
    }
}
