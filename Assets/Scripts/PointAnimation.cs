using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointAnimation : MonoBehaviour {

	private Transform startMarker;
	private Transform endMarker;
	[Range(1,100)]
	public float speed;

	private float startTime;
	private float journeyLength;
	private bool doneEnlargening;

	// Use this for initialization
	void Start () {
		doneEnlargening = false;
		startTime = Time.time;
		startMarker = transform;
		journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
	}
	
	// Update is called once per frame
	void Update () {
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength ;
		transform.position = Vector3.Lerp (startMarker.position, endMarker.position, fracJourney);

		if (!doneEnlargening) {
			transform.localScale += new Vector3 (0.05f, 0.05f, 0.05f);
			if (transform.localScale.x >= 1f) {
				doneEnlargening = true;
			}
		} else {
			transform.localScale -= new Vector3 (0.01f, 0.01f, 0.01f);
			if (transform.localScale.x <= 0f) {
				Destroy (gameObject);
			}
		}
	}

	public void SetRecipient(int recipient) {
		// Handle 0, 1, 2 values
		int target = recipient + 1;
		// Travel towards:
		endMarker = GameObject.Find ("p"+target+" score").transform;
	}
}
