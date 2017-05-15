using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Animations;

public class RoundController : MonoBehaviour {

	public int roundCount;
	public int totalPlayer;
	private int playerCount;
	private string Winner;
	public int[] playerScores;
	public GridController gc;

	public AudioSource bgm;
	public AudioClip[] clips;

	public Image[] images;


	// Use this for initialization
	void Start () {
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

		roundCount--;
		if (roundCount == 0) {
			Debug.Log("Highest Value is: "+ Mathf.Max(playerScores));
			//Go to POST-Game GUI which tells us who's the winner and how much each player got for points:
			images[DetermineWinner()].gameObject.SetActive(true);
		}else{
			gc.Start ();
			ChooseTrack ();
			//Reset Game
			SetupGame();
		}
	}

	public void ChooseTrack() {
		if (clips.Length > 0) {
			bgm.clip = clips [Random.Range (0, clips.Length - 1)];
			bgm.Play ();
		}
	}

	void SetupGame() {
		playerCount = totalPlayer;
	}

	int DetermineWinner() {
		int p = 0;
		int tmp = 0;
		for (int i = 0; i < playerScores.Length; i++) {
			if (playerScores [i] > tmp) {
				tmp = playerScores [i];
				p = i;
			}
		}
		Debug.Log ("The winner is player " + (p+1));
		Debug.Log ("Player 1 has:" + playerScores[0] + " Player 2 has: " + playerScores[1] + " Player 3 has: " + playerScores[2]);
		return p;
	}
}
