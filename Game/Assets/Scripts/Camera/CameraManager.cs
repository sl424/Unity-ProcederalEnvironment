using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public GameObject FirstPersonCamera; 
    public GameObject ThirdPersonCamera; 
    public GameObject crosshair; 
    public bool viewIsThird;

    void Start ()
    {
        FirstPersonCamera.gameObject.active = false;
        ThirdPersonCamera.gameObject.active = true;
        crosshair.gameObject.active = false;
        viewIsThird = true;
            if (viewIsThird) {
                FirstPersonCamera.gameObject.active = true;
                ThirdPersonCamera.gameObject.active = false;
                crosshair.gameObject.active = true;
                Cursor.visible = false; 
            }
            else {
                FirstPersonCamera.gameObject.active = false;
                ThirdPersonCamera.gameObject.active = true;
                crosshair.gameObject.active = false;
            }
    }

    void Update ()
    {
        if (PlayerHealth.isDead) {return;}
        if (viewIsThird) { Cursor.visible = true; }

        if (Input.GetKeyDown(KeyCode.T)) {
            if (viewIsThird) {
                FirstPersonCamera.gameObject.active = true;
                ThirdPersonCamera.gameObject.active = false;
                crosshair.gameObject.active = true;
                Cursor.visible = false; 
            }
            else {
                FirstPersonCamera.gameObject.active = false;
                ThirdPersonCamera.gameObject.active = true;
                crosshair.gameObject.active = false;
            }
            viewIsThird = !viewIsThird;
        }

    }
}

