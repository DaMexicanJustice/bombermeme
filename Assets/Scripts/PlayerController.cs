using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Use this for initialization

	private Rigidbody rb;
	public float moveSpeed;


	void Start () {

		rb = GetComponent<Rigidbody> ();

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		Vector3 direction;
		if (horizontal != 0f) {
			direction = new Vector3 (horizontal, 0f, 0f);
			rb.velocity = direction * moveSpeed * Time.deltaTime;
		} else if (vertical != 0f) {
			direction = new Vector3 (0f, 0f, vertical);
			rb.velocity = direction * moveSpeed * Time.deltaTime;
		}

	}	
}
