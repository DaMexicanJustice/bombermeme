using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
	public float moveSpeed;
	public GameObject bombPrefab;
	public GameObject grid;
	public int placedBombs;
	public int fuse;
	public int bombCount;
	public int playerNumber;
    public GameObject breakPrefab;
    [Range(0,0.9f)]
    public float controllerDeadZone;

	void Start () {
		placedBombs = 0;
		rb = GetComponent<Rigidbody> ();
	}

	// Increment number of placed bombs, then calculate position for new bomb and place it, saving a reference to it.
	// then call method to arm the bomb
	void PlantBomb(){
		placedBombs++;
		Vector3 roundPos = rb.position;
		roundPos = new Vector3(Mathf.Round(roundPos.x),Mathf.Round(roundPos.y),Mathf.Round(roundPos.z));
		GameObject bomb = Instantiate (bombPrefab, roundPos, rb.rotation);
		ArmBomb (bomb);

		/*
		Måske et Array over dem alle?
		Vector3 north = new Vector3 (roundPos.x, roundPos.y, roundPos.z+1);
		Vector3 south = new Vector3 (roundPos.x, roundPos.y, roundPos.z-1);
		Vector3 west = new Vector3 (roundPos.x-1, roundPos.y, roundPos.z);
		Vector3 east = new Vector3 (roundPos.x+1, roundPos.y, roundPos.z);

		if(Physics.CheckSphere(north,1)){
			Destroy (,4.0f);

		}
		*/
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (playerNumber == 1) {
			float horizontal = Input.GetAxis ("P1_Horizontal");
			float vertical = Input.GetAxis ("P1_Vertical");
			Vector3 direction = new Vector3 (horizontal, 0f, vertical);
			rb.velocity = direction * moveSpeed * Time.deltaTime;
			transform.forward = rb.velocity;
			if (Input.GetButtonDown ("P1_Placebomb")) {
				if (placedBombs < bombCount) {
					PlantBomb ();
				}
			}
		} else if (playerNumber == 2) {
			float horizontal = Input.GetAxis ("P2_Horizontal");
			float vertical = Input.GetAxis ("P2_Vertical");
			Vector3 direction = new Vector3 (horizontal, 0f, vertical);
			rb.velocity = direction * moveSpeed * Time.deltaTime;
			transform.forward = rb.velocity;
			if (Input.GetButtonDown ("P2_Placebomb")) {
				if (placedBombs < bombCount) {
					PlantBomb ();
				}
			}
		} else if (playerNumber == 3) {
			float horizontal = Input.GetAxis ("P3_Horizontal");
			float vertical = Input.GetAxis ("P3_Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical);
            rb.velocity = direction * moveSpeed * Time.deltaTime;
			transform.forward = rb.velocity;
			if (Input.GetButtonDown ("P3_Placebomb")) {
				if (placedBombs < bombCount) {
					PlantBomb ();
				}
			}
		}

	}	
	// Detonate this bomb after fuse burns out and then reduce number of placed bombs
	// Invoke cannot take parameter so we split the activity into two methods
	void ArmBomb(GameObject bomb) {
		Destroy (bomb, fuse);
		Invoke ("ExpireBomb", fuse);
	}
	// Decrement number of placed bombs
	void ExpireBomb() {
		placedBombs--;
	}
	 void PlaceBox(){
        Vector3 boxPos = rb.transform.position + rb.transform.forward;
 
 
        GameObject box = Instantiate(breakPrefab, boxPos, rb.rotation);
 
 
    }
	
}
