using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDetect : MonoBehaviour {

	private SphereCollider sc;

	// Use this for initialization
	void Start () {
		sc = GetComponent<SphereCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerExit(Collider other) {
		sc.isTrigger = false;
	}
}
