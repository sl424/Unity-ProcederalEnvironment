using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float speed = 10.0f;
	public int damage = 5;

	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (0, 0, speed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other) {
		PlayerHealth player = other.GetComponent<PlayerHealth> ();
		if (player != null) {
			player.TakeDamage (damage);
		}
		//Destroy (this.gameObject);

	}
}
