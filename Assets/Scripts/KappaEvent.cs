using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KappaEvent : MonoBehaviour {

	private Vector3 _origin;
	private float offset;
	public float waveLength;
	private float totalY;
	public int speed;

	// Use this for initialization
	void Start () {
		_origin = transform.position;
		totalY = 0;
		offset = Random.Range(0f,100f);
	}

	// Update is called once per frame
	void Update () {
		Move();
	}

	void Move()
	{
		transform.position = _origin + new Vector3(Mathf.Sin(Time.time) * waveLength, totalY, 0f);
		totalY -= speed * Time.deltaTime;
	}

}
