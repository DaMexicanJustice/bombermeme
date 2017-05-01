using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour {

    public GridController gc;
	private Rigidbody rb;
	public AudioSource sfx;
	public AudioClip explosion;
	public AudioClip pickup;
	public AudioClip walk;

    public bool breakthrough;
    public float firePower;
    public float blockTimer;
    private Vector3 boxDirection = new Vector3(0, 0, 0);
	private float boxCooldown = 10f;
	private float nextPlacement; 

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
	private float timeStamp = 0;

	void Start () {
        placedBombs = 0;
        firePower = 1;
        moveSpeed = 200;
        bombCount = 1;
        blockTimer = 0;
        breakthrough = false;
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

        if (blockTimer > 0)
        {
            blockTimer--;
        }

        if (playerNumber == 1) {
			float horizontal = Input.GetAxis ("P1_Horizontal");
			float vertical = Input.GetAxis ("P1_Vertical");
			Vector3 direction = new Vector3 (horizontal, 0f, vertical);

			if (Input.GetKeyDown (KeyCode.W)) {
				boxDirection = new Vector3(0, 0, 1);
			} else if (Input.GetKeyDown (KeyCode.S)) {
				boxDirection = new Vector3 (0, 0, -1);
			} else if (Input.GetKeyDown (KeyCode.A)) {
				boxDirection = new Vector3 (-1, 0, 0);
			} else if (Input.GetKeyDown (KeyCode.D)) {
			    boxDirection = new Vector3 (1, 0, 0);
			}
			if (Mathf.Abs (horizontal) > 0f || Mathf.Abs (vertical) > 0f) {
				PlayWalkSound ();
			} 
			rb.velocity = direction * moveSpeed * Time.deltaTime;
			//transform.forward = rb.velocity;
			if (Input.GetButtonDown ("P1_Placebomb")) {
				if (placedBombs < bombCount) {
					PlantBomb ();
				}
			}
			if (Input.GetButtonDown ("P1_Placeblock") & Time.time > nextPlacement) {
				PlaceBox (boxDirection);
			}
		} else if (playerNumber == 2) {
			float horizontal = Input.GetAxis ("P2_Horizontal");
			float vertical = Input.GetAxis ("P2_Vertical");
			Vector3 direction = new Vector3 (horizontal, 0f, vertical);
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				boxDirection = new Vector3(0, 0, 1);
			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				boxDirection = new Vector3 (0, 0, -1);
			} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				boxDirection = new Vector3 (-1, 0, 0);
			} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
				boxDirection = new Vector3 (1, 0, 0);
			}
			rb.velocity = direction * moveSpeed * Time.deltaTime;
			//transform.forward = rb.velocity;
            if (Input.GetButtonDown("P2_Placebomb")) {
				if (placedBombs < bombCount) {
					PlantBomb ();
				}
			}
			if (Input.GetButtonDown ("P2_Placeblock") & Time.time > nextPlacement) {
				PlaceBox (boxDirection);
			}
		} else if (playerNumber == 3) {
			float horizontal = Input.GetAxis ("P3_Horizontal");
			float vertical = Input.GetAxis ("P3_Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical);
			Vector3 boxDirection = new Vector3 (0, 0, 1);
            rb.velocity = direction * moveSpeed * Time.deltaTime;
			//transform.forward = rb.velocity;
			if (Input.GetButtonDown ("P3_Placebomb")) {
				if (placedBombs < bombCount) {
					PlantBomb ();
				}
			}
			if (Input.GetButtonDown ("P3_Placeblock") & Time.time > nextPlacement) {
				PlaceBox (boxDirection);
			}
		}

	}	
	// Detonate this bomb after fuse burns out and then reduce number of placed bombs
	// Coroutine is used to schedule task
	// we add 0.1 to the fuse, so the gameobject exists when accessed by grid controller. Else accesing removed element
	void ArmBomb(GameObject bomb) {
		Destroy (bomb, fuse+0.06f);
		StartCoroutine(ExpireBombAfter(bomb, fuse));
	}

	IEnumerator ExpireBombAfter(GameObject bomb, float fuse) {
		yield return new WaitForSeconds (fuse);
		placedBombs--;
		sfx.clip = explosion;
		sfx.pitch = Random.Range (0.8f, 1.2f);
		sfx.Play ();
		gc.ExplodeBreakablesAtPos (bomb, firePower, breakthrough);
	}

	 // Places a box prefab based off of the player object's position and rotation
	 void PlaceBox(Vector3 boxDirection){
		Vector3 boxPos = rb.transform.position + boxDirection;
		boxPos = new Vector3 (Mathf.Round (boxPos.x), Mathf.Round (boxPos.y), Mathf.Round (boxPos.z));
		if (gc.IsAllowedPosition (boxPos.x, boxPos.y)) {
			GameObject box = Instantiate (breakPrefab, boxPos, Quaternion.Euler (0, 0, 0));
			gc.PlaceBoxAtCoords (box);
			nextPlacement = Time.time + boxCooldown; 
		}
    }

	void BoxCooldown(){
		timeStamp = Time.time + 5;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Speed Up"))
        {
            moveSpeed += 50;
            other.gameObject.SetActive(false);
            PlayPickupSound();
        }
        if (other.gameObject.CompareTag("Bomb Up"))
        {
            bombCount += 1;
            other.gameObject.SetActive(false);
            PlayPickupSound();
        }
        if (other.gameObject.CompareTag("Fire Up"))
        {
            if (firePower < 8)
            {
                firePower++;
            }
            other.gameObject.SetActive(false);
            PlayPickupSound();
        }
        if (other.gameObject.CompareTag("Max Fire"))
        {
            if (firePower < 8)
            {
                firePower = 8;
            }
            other.gameObject.SetActive(false);
            PlayPickupSound();
        }
        if (other.gameObject.CompareTag("Block Fill"))
        {
            blockTimer = 0;
            other.gameObject.SetActive(false);
            PlayPickupSound();
        }
        if (other.gameObject.CompareTag("Breakthrough"))
        {
            breakthrough = true;
            other.gameObject.SetActive(false);
            PlayPickupSound();
        }
    }

    void PlayPickupSound() {
		sfx.clip = pickup;
		sfx.Play ();
	}

	void PlayWalkSound() {
		if (sfx.isPlaying == false) {
			sfx.clip = walk;
			sfx.pitch = Random.Range (0.8f, 1.2f);
			sfx.volume = 1f;
			sfx.Play ();
		}
	}

}
