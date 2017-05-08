using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableScript : MonoBehaviour {

	public bool isDestroyed;
	private bool isQuitting;
	private bool isSpawnable;

	public GameObject[] powerUps;
	private GameObject powerup;

	private void Start()
	{
		isQuitting = false;
		isSpawnable = true;
	}

	private void Update()
	{
		if (isDestroyed)
		{
			Destroy(gameObject);
		}
	}
	private void OnApplicationQuit()
	{
		isQuitting = true;
	}
	void OnDestroy()
	{
		if (!isQuitting & isSpawnable)
		{
			if (Random.Range(0, 2) == 1)
			{
				//powerup =  powerUps[Random.Range(0, powerUps.Length)];
				Instantiate(powerUps[Random.Range(0, powerUps.Length)], transform.position, Quaternion.identity);
			}
		}

	}

	public void ChangeSpawnable(bool b){

		isSpawnable = b;

	}
}