using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableScript : MonoBehaviour {

	public bool isDestroyed;
	private bool isQuitting;
	private bool isSpawnable;

	public GameObject[] powerUps;
	private GameObject powerup;
	public GameObject puContainerPrefab;
	public GameObject puContainerContainer;

	private void Start()
	{
		isQuitting = false;
		isSpawnable = true;
		puContainerContainer = GameObject.FindGameObjectWithTag ("PuContainerContainer");
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
				GameObject pu = Instantiate(powerUps[Random.Range(0, powerUps.Length)], transform.position, Quaternion.identity);
				GameObject puContainer = Instantiate (puContainerPrefab, transform.position, Quaternion.identity);
				puContainer.transform.parent = puContainerContainer.transform;
				pu.transform.parent = puContainer.transform;
			}
		}

	}

	public void ChangeSpawnable(bool b){

		isSpawnable = b;

	}
}