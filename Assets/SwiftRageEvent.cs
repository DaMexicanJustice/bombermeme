using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwiftRageEvent : MonoBehaviour {

	private float y;
	private float x;

	// Use this for initialization
	void Start () {
		y = transform.position.y;
		x = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (x, y, transform.position.z);
		y -= 5;
		x += 1;
	}
}
