using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {


	public GameObject box;
	private Quaternion quat;
	private Vector3 spawnPosition = new Vector3(14,1,-6.5f);
	private Vector3 addingToX = new Vector3(1,0,0);
	private Vector3 addingToZ = new Vector3(-9,0,1);
	private int tempInt = 0;


	GameObject[,] grid = new GameObject[10,13];

	void Start(){
		
		for (int i = 0; i <= 10; i++) {

			if (Random.Range (0, 2) == 1) {
				grid [i, i] = (GameObject)Instantiate (box, spawnPosition, quat);
			}

			spawnPosition = spawnPosition + addingToX;
			if (i == 9) {
				tempInt++;
				i = 0;
				spawnPosition = spawnPosition + addingToZ;
				if (tempInt > 13) {
					return;
				}
			}
	}
}

}
