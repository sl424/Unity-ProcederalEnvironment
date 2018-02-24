using UnityEngine;
using System.Collections;

public class dontDestroy : MonoBehaviour {

    // Use this for initialization
    void Awake () {

        DontDestroyOnLoad(gameObject);
    
    }
}
