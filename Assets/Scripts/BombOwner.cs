using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombOwner : MonoBehaviour {

	private GameObject owner;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetOwner(GameObject player) {
		owner = player;
	}

	public GameObject GetOwner() {
		return owner;
	}
}
