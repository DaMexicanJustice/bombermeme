using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {


	public GameObject unbreakable;
	public GameObject breakable;
	private Vector3 spawnPosition = new Vector3(0,1,0);
	private Vector3 addingToX = new Vector3(1,0,0);
	private Vector3 addingToZ = new Vector3(-13,0,1);

    [Range(2,8)]
	public int boxLimiter;

	//Disallowed spawn positions:
	// 0,0   1,0    1,1     
	//0,12   0,13   1,13
	//12,0   13,0   13,1
	//12,13  13,13  13,12
	GameObject[,] grid = new GameObject[13,13];

	void Start() {
		SetUpUnBreakables ();
		SetUpBreakables ();
	}

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
		
	void SetUpBreakables() {
		ResetSpawnPosition ();
		for (int z = 0; z < grid.GetLength (1); z++) {
			for (int x = 0; x < grid.GetLength (0); x++) {
				if (grid [x, z] == null && Random.Range(0,boxLimiter) == 1) {
					if (IsPosAllowed (x, z)) {
						
						grid [x, z] = (GameObject)Instantiate (breakable, spawnPosition, Quaternion.identity);
					}
				}
				spawnPosition += addingToX;
			}
			spawnPosition += addingToZ;
		} 
	}

	void SetUpUnBreakables() {
		ResetSpawnPosition ();
		for (int z = 0; z < grid.GetLength (1); z++) {
			for (int x = 0; x < grid.GetLength (0); x++) {
				if (x % 2 == 1 && z % 2 == 1) {

					grid [x, z] = (GameObject)Instantiate (unbreakable, spawnPosition, Quaternion.identity);
				}
				spawnPosition += addingToX;
			}
			spawnPosition += addingToZ;
		} 
	}

	void ResetSpawnPosition() {
		spawnPosition = new Vector3(0,1,0f);
	}

    public void ExplodeBreakablesAtPos(GameObject bomb) {
        int x = (int)bomb.transform.position.x;
        int z = (int)bomb.transform.position.z;

		if (x > 0) {
			if (grid[x - 1, z] != null && grid [x - 1, z].gameObject.tag == "Breakable") {
				Debug.Log ("Blowing up cube at pos: " + (x-1) + ", " + z);
				Destroy (grid [x - 1, z]);
			}
		}
		if (x < 12) {
			if (grid[x + 1, z] != null && grid [x + 1, z].gameObject.tag == "Breakable") {
				Debug.Log ("Blowing up cube at pos: " + (x+1) + ", " + z);
				Destroy (grid [x + 1, z]);
			}
		}
		if (z > 0) {
			if (grid[x, z - 1] != null && grid [x, z - 1].gameObject.tag == "Breakable") {
				Debug.Log ("Blowing up cube at pos: " + x + ", " + (z-1));
				Destroy (grid [x, z-1]);
			}
		}
		if (z < 12) {
			if (grid[x, z + 1] != null && grid [x, z + 1].gameObject.tag == "Breakable") {
				Debug.Log ("Blowing up cube at pos: " + x + ", " + (z+1));
				Destroy (grid [x, z+1]);
			}
		}
    }

}
