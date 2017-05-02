using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEvents : MonoBehaviour {

	public GameObject canvas;
	public GameObject kappa;

	// Use this for initialization
	void Start () {
		StartKappaEvent ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartKappaEvent() {
		Vector3 origin = new Vector3 (Screen.width / 2, Screen.height + 100 ,0);
		GameObject k = Instantiate (kappa, origin, Quaternion.identity);
		k.transform.parent = canvas.transform;
	}
}
