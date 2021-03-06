﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundController : MonoBehaviour {

	public Text roundText;
	private int round;
	public int totalPlayer;
	public int firstTo;
	private int playerCount;
	private string Winner;
	public int[] playerScores;
	public GridController gc;
	public AudioSource bgm;
	public AudioClip[] clips;

	public Image[] images;

	public Text p1Score;
	public Text p2Score;
	public Text p3Score;

	public Image goldenBomb;
	public Canvas canvas;


	// Use this for initialization
	void Start () {
		round = 1;
		SetupGame ();
		playerScores = new int[playerCount];
		playerScores [0] = 0;
		playerScores [1] = 0;
		playerScores [2] = 0;
		gc = gc.GetComponent<GridController>();
		CheckForRoundCountAndSet ();
	}

	void CheckForRoundCountAndSet() {
		GameObject nav = GameObject.Find("Navigator");
		if (nav != null) {
			firstTo = (int) nav.GetComponent<UIController> ().finalPoints;
		} else {
			firstTo = 3;
		}
		Debug.Log (nav);
	}

	public void RemovePlayer(){
		playerCount--;
		if (playerCount == 1) {
			for (int i = 0; i <= totalPlayer; i++) {
				if (gc.PlayerDead (1+i)) {
					playerScores[i]++;
					SpawnGoldenBomb (i);
					Debug.Log ("player: " + (i+1) + " gets a point and has: " + playerScores[i]);
					RoundEnd ();
					break;
				}
			}
		}
	}
	void RoundEnd(){

		if (WinnerCheck()) {
			Debug.Log("Highest Value is: "+ Mathf.Max(playerScores));
			//Go to POST-Game GUI which tells us who's the winner and how much each player got for points:
			images[DetermineWinner()].gameObject.SetActive(true);
		}

		else{	
			round++;
			UpdateScoreText ();
			Invoke ("SetupGame",3f);
		}
	}

	void SpawnGoldenBomb(int recipient) {
		Image b = Instantiate (goldenBomb, new Vector3(Screen.width / 2, Screen.height / 2, 0), Quaternion.identity);
		b.GetComponent<PointAnimation> ().SetRecipient (recipient);
		b.transform.SetParent (canvas.transform);
	}

	void UpdateScoreText() {
		p1Score.text = "" + playerScores [0];
		p2Score.text = "" + playerScores [1];
		p3Score.text = "" + playerScores [2];
	}

	public void ChooseTrack() {
		if (clips.Length > 0) {
			bgm.clip = clips [Random.Range (0, clips.Length - 1)];
			bgm.Play ();
		}
	}

	bool WinnerCheck(){

		for (int i = 0; i < playerScores.Length; i++) {

			if (playerScores [i] == firstTo) {
				return true;
			}
		} 
		return false;
	}

	void SetupGame() {
		roundText.enabled = true;
		roundText.text = "Round " + round;
		playerCount = totalPlayer;
		gc.Start ();
		ChooseTrack ();
		Invoke ("DisableRoundText", 2f);
	}

	void DisableRoundText() {
		roundText.enabled = false;
	}

	int DetermineWinner() {
		int winner = 0;
		int tmp = 0;
		for (int i = 0; i < playerScores.Length; i++) {
			if (playerScores [i] > tmp) {
				tmp = playerScores [i];
				winner = i;
			}
		}
		return winner;
	}
}
