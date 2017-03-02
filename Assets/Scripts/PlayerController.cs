using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour {

	// Use this for initialization

	private Rigidbody rb;
	public float moveSpeed;
	public GameObject bombPrefab;
	public GameObject grid;
	public GameObject other;

	void Start () {

		rb = GetComponent<Rigidbody> ();

	}

	void PlantBomb(){

		Vector3 roundPos = rb.position;
		roundPos = new Vector3(Mathf.Round(roundPos.x),Mathf.Round(roundPos.y),Mathf.Round(roundPos.z));


		var bomb = (GameObject)Instantiate (bombPrefab, roundPos, rb.rotation);
			
		Destroy (bomb, 4.0f);

//		Måske et Array over dem alle?
		Vector3 north = new Vector3 (roundPos.x, roundPos.y, roundPos.z+1);
		Vector3 south = new Vector3 (roundPos.x, roundPos.y, roundPos.z-1);
		Vector3 west = new Vector3 (roundPos.x-1, roundPos.y, roundPos.z);
		Vector3 east = new Vector3 (roundPos.x+1, roundPos.y, roundPos.z);

		if(Physics.CheckSphere(north,1)){
		//	Destroy (,4.0f);
		}	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        rb.velocity = direction * moveSpeed * Time.deltaTime;

		if(Input.GetKeyDown(KeyCode.Space)){
			PlantBomb();
		}

        /*
		if (horizontal != 0f) {
			direction = new Vector3 (horizontal, 0f, 0f);
			rb.velocity = direction * moveSpeed * Time.deltaTime;
		} else if (vertical != 0f) {
			direction = new Vector3 (0f, 0f, vertical);
			rb.velocity = direction * moveSpeed * Time.deltaTime;
		}
        */

	}	
}
