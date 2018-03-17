using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float speed = 7f;
	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidbody;
	int floorMask;
	float camRayLength = 100f;
    public Camera firstPerson;
    public Camera thirdPerson;

	// Use this for initialization
	void Awake () {
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();

        transform.Translate(Vector3.up*10);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 100f)){
            //Debug.Log(hit.point);
            transform.position = hit.point;
            //transform.Translate(hit.point);
            //transform.Translate(Vector3.down*(10-hit.point.y));
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");
		// Move the player around the scene.

        if(thirdPerson.gameObject.activeSelf) {
		    // Turn the player to face the mouse cursor.
		    Move (h, v);
		    Turning ();
        }
        else { //first person control different
            h *= speed;
            v *= speed;
            h *= Time.deltaTime;
            v *= Time.deltaTime;
            transform.Translate(h, 0, v);
        }
		    Animating (h, v);

	}


    public void changeMovementSpeed(float s) {
	    speed = s;
    }

	void Move(float h, float v) {
		movement.Set (h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidbody.MovePosition (transform.position + movement);
	}

	void Turning() {
		// Create a ray from the mouse cursor on screen in the direction of the camera.
		Ray camRay = thirdPerson.ScreenPointToRay (Input.mousePosition);

		// Create a RaycastHit variable to store information about what was hit by the ray.
		RaycastHit floorHit;

		// Perform the raycast and if it hits something on the floor layer...
		if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
		{
			// Create a vector from the player to the point on the floor the raycast from the mouse hit.
			Vector3 playerToMouse = floorHit.point - transform.position;

			// Ensure the vector is entirely along the floor plane.
			playerToMouse.y = 0f;

			// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);

			// Set the player's rotation to this new rotation.
			playerRigidbody.MoveRotation (newRotation);
		}
	}

	void Animating(float h, float v) {
		// Create a boolean that is true if either of the input axes is non-zero.
		bool walking = h != 0f || v != 0f;

		// Tell the animator whether or not the player is walking.
		anim.SetBool ("IsWalking", walking);
	}

}
