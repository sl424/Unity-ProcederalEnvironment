using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class quitScene : MonoBehaviour {

    public Button quitButton;

    void Start()
    {
        //Button btn = quitButton.GetComponent<Button>();
        quitButton.onClick.AddListener( delegate() {Quit();});
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

}
