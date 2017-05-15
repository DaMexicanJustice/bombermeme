using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {

	public GameObject roundMaster;
	private RoundController rc;
	public GameObject explosion;
	public GameObject unbreakable;
	public GameObject breakable;
	private Vector3 spawnPosition = new Vector3(0,1,0);
	private Vector3 addingToX = new Vector3(1,0,0);
	private Vector3 addingToZ = new Vector3(-13,0,1);


	public GameObject bContainer;
	public GameObject ubContainer;

	public GameObject playerOne;
	public GameObject playerTwo;
	public GameObject playerThree;

	public Vector3 playerOneStart;
	public Vector3 playerTwoStart;
	public Vector3 playerThreeStart;

	bool safeLeft = true;
	bool safeRight = true;
	bool safeUp = true;
	bool safeDown = true;

	[Range(2,8)]
	public int boxLimiter;

	//Disallowed spawn positions:
	//0,0    1,0    1,1     
	//0,12   0,13   1,13
	//12,0   13,0   13,1
	//12,13  13,13  13,12
	GameObject[,] grid = new GameObject[13,13];

	void Awake(){
		playerOneStart = playerOne.transform.position;
		playerTwoStart = playerTwo.transform.position;
		playerThreeStart = playerThree.transform.position;
	}
	public void Start() {

		rc = roundMaster.GetComponent<RoundController> ();
		rc.ChooseTrack ();
		DestroyAllObjects ();
		SetUpUnBreakables ();
		SetUpBreakables ();

		playerOne.SetActive (true);
		playerTwo.SetActive (true);
		playerThree.SetActive (true);
		playerOne.GetComponent<PlayerController> ().ResetPowerUps();
		playerTwo.GetComponent<PlayerController> ().ResetPowerUps();
		playerThree.GetComponent<PlayerController> ().ResetPowerUps();

		playerOne.transform.position = playerOneStart;
		playerTwo.transform.position = playerTwoStart;
		playerThree.transform.position = playerThreeStart;
	}

	// Prevent blocks from spawning in player corners
	bool IsPosAllowed(int x, int z) {
		if (x == 0 && z  == 0) return false;
		else if (x == 1 && z  == 0) return false;
		else if (x == 0 && z  == 1) return false;
		else if (x == 0 && z  == 11) return false;
		else if (x == 0 && z  == 12) return false;
		else if (x == 1 && z  == 12) return false;
		else if (x == 11 && z  == 0) return false;
		else if (x == 12 && z  == 0) return false;
		else if (x == 12 && z  == 1) return false;
		else if (x == 11 && z  == 12) return false;
		else if (x == 12 && z  == 12) return false;
		else if (x == 12 && z == 11) return false;
		else
			return true;
	}
	// randomly build a map with breakable crates	
	void SetUpBreakables() {
		ResetSpawnPosition ();
		for (int z = 0; z < grid.GetLength (1); z++) {
			for (int x = 0; x < grid.GetLength (0); x++) {
				if (grid [x, z] == null && Random.Range(0,boxLimiter) == 1) {
					if (IsPosAllowed (x, z)) {

						GameObject b = (GameObject)Instantiate (breakable, spawnPosition, Quaternion.identity);
						grid [x, z] = b;
						b.transform.parent = bContainer.transform;
					}
				}
				spawnPosition += addingToX;
			}
			spawnPosition += addingToZ;
		} 
	}
	// place unbreakable cubes in the same pattern everytime
	void SetUpUnBreakables() {
		ResetSpawnPosition ();
		for (int z = 0; z < grid.GetLength (1); z++) {
			for (int x = 0; x < grid.GetLength (0); x++) {
				if (x % 2 == 1 && z % 2 == 1) {

					GameObject b = (GameObject)Instantiate (unbreakable, spawnPosition, Quaternion.identity);
					grid [x, z] = b;
					b.transform.parent = ubContainer.transform;
				}
				spawnPosition += addingToX;
			}
			spawnPosition += addingToZ;
		} 
	}
	// resetter used in setup map routines
	void ResetSpawnPosition() {
		spawnPosition = new Vector3(0,1,0f);
	}
	// help determine if a specific player has been killed
	public bool PlayerDead(int player) {
		switch (player) {
		case 1:
			return playerOne.activeSelf;
			break;
		case 2:
			return playerTwo.activeSelf;
			break;
		case 3:
			return playerThree.activeSelf;
			break;
		default:
			return false;
		}
	}
	// did a bomb hit a player
	int PlayerHit(int x, int z) {
		// add 0.1f to keep players in expected range 1-12, so a player cannot stand on a tile but be counted as standing 1 below it
		if (PlayerDead(1)) {
			int p1X = (int)(playerOne.transform.position.x + 0.1f);
			int p1Z = (int)(playerOne.transform.position.z + 0.1f);
			if (p1X == x && p1Z == z) {
				return 1;
			}
		}
		if (PlayerDead(2)) {
			int p2X = (int) (playerTwo.transform.position.x+0.1f);
			int p2Z = (int) (playerTwo.transform.position.z+0.1f);
			if (p2X == x && p2Z == z) {
				return 2;
			}
		}
		if (PlayerDead(3)) {
			int p3X = (int) (playerThree.transform.position.x+0.1f);
			int p3Z = (int) (playerThree.transform.position.z+0.1f);
			if (p3X == x && p3Z == z) {
				return 3;
			}
		}
		return 0;
	}
	// destroy the player we hit
	void KillPlayer(int player) {
		switch (player) {
		case 1:
			playerOne.SetActive (false);
			rc.RemovePlayer ();
			break;
		case 2:
			playerTwo.SetActive (false);
			rc.RemovePlayer ();
			break;
		case 3:
			playerThree.SetActive (false);
			rc.RemovePlayer ();
			break;
		default:
			break;
		}
	}
	// detonate a bomb and check what we hit in the grid. Kills breakables, bombs and players
	public void ExplodeBreakablesAtPos(GameObject bomb, float firePower, bool breakthrough)
	{
		int x = (int) bomb.transform.position.x;
		int z = (int) bomb.transform.position.z;
		for (int i = 0; i <= firePower; i++)
		{
			if (x > 0 && safeLeft == true)
			{
				if (x - i >= 0) {

					CreateExplosionAt (x - i, z);
					if (grid [x - i, z] != null && grid [x - i, z].gameObject.tag == "Breakable") {
						Destroy (grid [x - i, z]);
						if (breakthrough == false) {
							safeLeft = false;
						}
					}
					if (grid [x - i, z] != null && grid [x - i, z].gameObject.tag == "Unbreakable") {
						safeLeft = false;
					}
					if (PlayerHit (x - i, z) != 0) {
						KillPlayer (PlayerHit (x - 1, z));
					}

					if (grid [x - i, z] != null && grid [x - i, z].gameObject.tag.Equals ("Bomb")) {
						GameObject b = grid [x - i, z];
						GameObject owner = GetBombOwner (b);
						owner.GetComponent<PlayerController>().StartChainReaction (b);
					} 
				}

			}

			if (x < 12 && safeRight == true)
			{
				if (x + i <= 12) {

					CreateExplosionAt (x + i, z);
					if (grid [x + i, z] != null && grid [x + i, z].gameObject.tag == "Breakable") {

						Destroy (grid [x + i, z]);
						if (breakthrough == false) {
							safeRight = false;
						}
					}
					if (grid [x + i, z] != null && grid [x + i, z].gameObject.tag == "Unbreakable") {
						safeLeft = false;
					}

					if (PlayerHit (x + i, z) != 0) {
						KillPlayer (PlayerHit (x + i, z));
					}

					if (grid [x + i, z] != null && grid [x + i, z].gameObject.tag.Equals ("Bomb")) {
						GameObject b = grid [x + i, z];
						GameObject owner = GetBombOwner (b);
						owner.GetComponent<PlayerController>().StartChainReaction (b);
					} 
				}

			}

			if (z > 0 && safeUp == true)
			{
				if (z - i >= 0) {

					CreateExplosionAt (x, z - i);
					if (grid [x, z - i] != null && grid [x, z - i].gameObject.tag == "Breakable") {

						Destroy (grid [x, z - i]);
						if (breakthrough == false) {
							safeUp = false;
						}
					}
					if (grid [x, z - i] != null && grid [x, z - i].gameObject.tag == "Unbreakable") {
						safeUp = false;
					}
					if (PlayerHit (x, z - 1) != 0) {
						KillPlayer (PlayerHit (x, z - i));
					}

					if (grid [x, z - i] != null && grid [x, z - i].gameObject.tag.Equals ("Bomb")) {
						GameObject b = grid [x, z - i];
						GameObject owner = GetBombOwner (b);
						owner.GetComponent<PlayerController>().StartChainReaction (b);
					} 
				}
			}

			if (z < 12 && safeDown == true)
			{
				if (z + i <= 12) {

					CreateExplosionAt (x, z + i);
					if (grid [x, z + i] != null && grid [x, z + i].gameObject.tag == "Breakable") {
						Destroy (grid [x, z + i]);
						if (breakthrough == false) {
							safeDown = false;
						}
					}
					if (grid [x, z + i] != null && grid [x, z + i].gameObject.tag == "Unbreakable") {
						safeDown = false;
					}
					if (PlayerHit (x, z + i) != 0) {
						KillPlayer (PlayerHit (x, z + i));
					}

					if (grid [x, z + i] != null && grid [x, z + i].gameObject.tag.Equals ("Bomb")) {
						GameObject b = grid [x, z + i];
						GameObject owner = GetBombOwner (b);
						owner.GetComponent<PlayerController>().StartChainReaction (b);
					} 
				}
			}
		}
		safeDown = true;
		safeLeft = true;
		safeRight = true;
		safeUp = true;
	}
	// We use this to prevent positions outside the map
	public bool IsAllowedPosition(float x, float z) {
		// Edge detection
		return (x > 0 && x < 12) && (z > 0 && z < 12);
	}

	public void PlaceBoxAtCoords(GameObject box) {
		int x = (int) box.transform.position.x;
		int z = (int)box.transform.position.z;
		if (grid [x, z] == null) {
			grid [x, z] = box;
			box.transform.parent = bContainer.transform;
		}
	}
	// We use this to create graphics (bomb explosion) at a certain position
	public void CreateExplosionAt(float x, float z) {
		Vector3 pos = new Vector3 (x, 0, z);
		Instantiate (explosion, pos, Quaternion.identity);
	}
	// pass the grid to other scripts
	public GameObject[,] GetGrid() {
		return grid;
	}
	// place a bomb as a gameobject into the grid
	public void PlaceBombInGrid(GameObject bomb) {
		int x = (int) bomb.transform.position.x;
		int z = (int) bomb.transform.position.z;
		grid [x, z] = bomb;
	}
	// when a bomb explodes remove it from the grid
	public void RemoveBombFromGrid(float xCord, float zCord) {
		int x = (int) xCord;
		int z = (int) zCord;
		grid [x, z] = null;
	}
	// find out who placed a bomb
	GameObject GetBombOwner(GameObject b) {
		GameObject owner = b.GetComponent<BombOwner> ().GetOwner ();
		int player = owner.GetComponent<PlayerController> ().playerNumber;
		switch (player) {
		case 1:
			return playerOne;
			break;
		case 2:
			return playerTwo;
			break;
		case 3:
			return playerThree;
			break;
		default:
			return null;
			break;
		}
	}

	void DestroyAllObjects()
	{
		GameObject[] breakables = GameObject.FindGameObjectsWithTag ("Breakable");
		GameObject[] bombs = GameObject.FindGameObjectsWithTag ("Bomb");
		GameObject[] powerUps = GameObject.FindGameObjectsWithTag ("Power Up");

		for (int i = 0; i < breakables.Length; i++) {
			GameObject br = breakables [i];
			br.GetComponent<BreakableScript> ().ChangeSpawnable (false);
			Destroy (br);
		}
		for( int i = 0; i < bombs.Length; i++){
			Destroy (bombs [i]);
		}
		for (int i = 0; i < powerUps.Length; i++) {
			Destroy (powerUps[i]);
		}
	}
}