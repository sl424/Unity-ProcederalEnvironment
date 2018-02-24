using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour {

    public int level;
    public Button startButton;
    public GameObject loadingImage;
    //public Slider loadingBar;

    //private AsyncOperation async;

    void Awake()
    {
        startButton.onClick.AddListener( 
                delegate() {LoadByIndex();}
                );
    }

    void LoadByIndex()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);
        asyncLoad.allowSceneActivation = false;
        loadingImage.SetActive(true);
        //SceneManager.LoadScene (index);

        //Wait until the last operation fully loads to return anything
        while (asyncLoad.progress < 0.9f)
            //while (!asyncLoad.isDone)
        {
            //loadingBar.value = asyncLoad.progress;
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
    }
}

