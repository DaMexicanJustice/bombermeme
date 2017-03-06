using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {


	public GameObject unbreakable;
	public GameObject breakable;
	private Quaternion quat;
	private Vector3 spawnPosition = new Vector3(-6,1,-6f);
	private Vector3 addingToX = new Vector3(1,0,0);
	private Vector3 addingToZ = new Vector3(-13,0,1);
	private int tempInt = 0;
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
						grid [x, z] = (GameObject)Instantiate (breakable, spawnPosition, quat);
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
					grid [x, z] = (GameObject)Instantiate (unbreakable, spawnPosition, quat);
				}
				spawnPosition += addingToX;
			}
			spawnPosition += addingToZ;
		} 
	}

	void ResetSpawnPosition() {
		spawnPosition = new Vector3(-6,1,-6f);
	}

}
