using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public GameObject FirstPersonCamera; 
    public GameObject ThirdPersonCamera; 
    public GameObject crosshair; 
    public bool viewIsThird = true;

    void Start ()
    {
        //FirstPersonCamera.gameObject.SetActive(false);
        FirstPersonCamera.gameObject.SetActive(true);
        ThirdPersonCamera.gameObject.SetActive(true);
        crosshair.gameObject.SetActive(false);
        //viewIsThird = true;
            if (!viewIsThird) {
                FirstPersonCamera.gameObject.SetActive(true);
                ThirdPersonCamera.gameObject.SetActive(false);
                crosshair.gameObject.SetActive(true);
                Cursor.visible = false; 
            }
            else {
                FirstPersonCamera.gameObject.SetActive(false);
                ThirdPersonCamera.gameObject.SetActive(true);
                crosshair.gameObject.SetActive(false);
            }
    }

    void Update ()
    {
        if (PlayerHealth.isDead) {return;}
        if (viewIsThird) { Cursor.visible = true; }

        if (Input.GetKeyDown(KeyCode.T)) {
            if (viewIsThird) {
                FirstPersonCamera.gameObject.SetActive(true);
                ThirdPersonCamera.gameObject.SetActive(false);
                crosshair.gameObject.SetActive(true);
                Cursor.visible = false; 
            }
            else {
                FirstPersonCamera.gameObject.SetActive(false);
                ThirdPersonCamera.gameObject.SetActive(true);
                crosshair.gameObject.SetActive(false);
            }
            viewIsThird = !viewIsThird;
        }

    }
}

