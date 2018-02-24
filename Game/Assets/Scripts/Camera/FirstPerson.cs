using UnityEngine;
using System.Collections;

public class FirstPerson: MonoBehaviour
{

	Vector2 mouseLook;
	Vector2 smoothV;
	public float sensitivity = 1.0f;
	public float smoothing = 2.0f;
    public float speed = 5.0f;
	GameObject character;

	void Start(){
		//Cursor.lockState = CursorLockMode.Locked; //disables cursor showing up, implement later
		character = this.transform.parent.gameObject;
	}


	void Update() {
		if (Time.timeScale != 0 && !PlayerHealth.isDead) { //game is paused
			var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
			md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
			smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f/smoothing);
			smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f/smoothing);
			mouseLook += smoothV;
			//transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
			//character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
            Quaternion target = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
            character.transform.localRotation = Quaternion.Slerp(character.transform.localRotation, target, speed*Time.deltaTime);
        }


		/*
		   if (Input.GetKeyDown("T")) { //switching back to 3rd person show cursor
		   Cursor.lockState = CursorLockMode.None;
		   }
		 */

	}
}




