using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIController : MonoBehaviour {

	public Text textComponent;
	public Slider slider;
	public float finalPoints;
	float points = 1;

	void Start() {
		
	}

	void Update() {
		textComponent.text = slider.value.ToString();
		points = slider.value;
	}

	public void QuitGame(){

		Application.Quit();
	}

	public void StartGame(){
		finalPoints = points;
		SceneManager.LoadScene (1);
		DontDestroyOnLoad (gameObject);
	}



}
