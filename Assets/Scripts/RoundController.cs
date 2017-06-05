using System.Collections;
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


	// Use this for initialization
	void Start () {
		round = 1;
		SetupGame ();
		playerScores = new int[playerCount];
		playerScores [0] = 0;
		playerScores [1] = 0;
		playerScores [2] = 0;
		gc = gc.GetComponent<GridController>();
	}

	public void RemovePlayer(){
		playerCount--;
		if (playerCount == 1) {
			for (int i = 0; i <= totalPlayer; i++) {
				if (gc.PlayerDead (1+i)) {
					playerScores[i]++;
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
			Invoke ("SetupGame",3f);

		}
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
