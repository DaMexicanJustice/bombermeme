﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour {

	private int roundCount;
	private int totalPlayer;
	private int playerCount;
	private string Winner;
	public int[] playerScores;
	public GridController gc;


	// Use this for initialization
	void Start () {
		totalPlayer = 3;
		playerCount = totalPlayer;
		playerScores = new int[playerCount];
		roundCount = 1;
	}

	// Update is called once per frame
	void Update () {
	}

	public void RemovePlayer(){

		playerCount--;
		if (playerCount == 1) {
			for (int i = 0; i <= totalPlayer; i++) {
				if (!gc.PlayerDead (1+i)) {
					playerScores[i]++;
					Debug.Log ("player: " + (i+1) + " gets a point and has: " + playerScores[i]);
					RoundEnd ();
					break;
				}
			}
		}
	}
	void RoundEnd(){

		roundCount--;
		if (roundCount == 0) {
			Mathf.Max (playerScores);
			Debug.Log("Highest Value is: "+ Mathf.Max (playerScores));
		}else{
			//Reset Game
		}


	}

}