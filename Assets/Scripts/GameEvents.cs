using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEvents : MonoBehaviour {

	public GameObject canvas;
	public GameObject kappa;
	public GameObject swiftrage;
	public GameObject bomb;
	public GridController gc;

	public AudioSource sfx;
	public AudioSource bgm;
	public AudioClip remix;
	public AudioClip explosion;

	[Range(5,20)]
	public int eventDuration;

	public Camera camera;
	private bool shouldRotate;
	private float rotationSpeed = 36f;

	private float fuse = 3;

	public GameObject directionalLight;
	private Light light;

	// Use this for initialization
	void Start () {
		light = directionalLight.GetComponent<Light> ();
	}

	// Update is called once per frame
	void Update () {
		if (shouldRotate) {
			// Time to travel 360 degrees = 360.0f * Time.deltaTime / eventDuration. Rotate around z-axis
			camera.transform.Rotate(0, 0, 360.0f * Time.deltaTime / eventDuration);
		} 
	}

	public void StartKappaEvent() {
		Vector3 origin = new Vector3 (Screen.width / 2, Screen.height + 100 ,0);
		GameObject k = Instantiate (kappa, origin, Quaternion.identity);
		k.transform.parent = canvas.transform;
	}

	public void StartBombEvent() {

		int bombs = Random.Range (1, 3);

		for (int i = 0; i < bombs; i++) {

		GameObject[,] grid = gc.GetGrid ();

		int randX = Random.Range (0, 12);
		int randZ = Random.Range (0, 12);

		while ( grid[randX, randZ] != null )   {
			if (grid [randX, randZ].gameObject.tag != "Breakable" || grid [randX, randZ].gameObject.tag != "Unbreakable") {
				randX = Random.Range (0, 12);
				randZ = Random.Range (0, 12);
			}
			Debug.Log ("Finding new position");
		}
			
		Vector3 randPos = new Vector3 (randX, 10, randZ);

		GameObject b = Instantiate (bomb, randPos, Quaternion.identity);

		ArmBomb (b);
		
		}

	}

	void ArmBomb(GameObject bomb) {
		Destroy (bomb, fuse+0.06f);
		StartCoroutine(ExpireBombAfter(bomb, fuse));
	}

	IEnumerator ExpireBombAfter(GameObject bomb, float fuse) {
		yield return new WaitForSeconds (fuse);
		sfx.clip = explosion;
		sfx.pitch = Random.Range (0.8f, 1.2f);
		sfx.Play ();
		gc.ExplodeBreakablesAtPos (bomb, 1, false);
	}
		
	public void UpgradeMusic() {
		if (bgm.clip != remix) {
			bgm.Stop ();
			bgm.clip = remix;
			bgm.Play ();
		}
	}

	public void StartSwiftRageEvent() {
		InvokeRepeating ("SpawnSwiftRage", 0, 0.3f);
		Invoke ("CancelEvents", eventDuration);
	}

	void SpawnSwiftRage() {
		int r = Random.Range (0, Screen.width / 2);
		Vector3 origin = new Vector3 ( (Screen.width / 4) + r , Screen.height + 100 ,0);
		GameObject sr = Instantiate (swiftrage, origin, Quaternion.identity);
		sr.transform.parent = canvas.transform;
	}

	void CancelEvents() {
		CancelInvoke ();
		shouldRotate = false;
	}

	public void StartLightEvent() {
		light.color = Color.magenta;
		InvokeRepeating ("ChangeColor", 0, 0.3f);
		Invoke ("CancelEvents", eventDuration);
	}

	void ChangeColor() {
		if (light.color == Color.magenta)
			light.color = Color.red;
		else if (light.color == Color.red)
			light.color = Color.blue;
		else if (light.color == Color.blue)
			light.color = Color.green;
		else if (light.color == Color.green)
			light.color = Color.yellow;
		else if (light.color == Color.yellow)
			light.color = Color.magenta;
	}

	public void RotateEvent() {
		InvokeRepeating ("RotateCamera", 0, 0.1f);
		Invoke ("CancelEvents", eventDuration);
	}

	void RotateCamera() {
		shouldRotate = true;
	}

	IEnumerator Rotate(float duration)
	{
		float startRotation = transform.eulerAngles.y;
		float endRotation = startRotation + 360.0f;
		float t = 0.0f;
		while ( t  < duration )
		{
			t += Time.deltaTime;
			float yRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
			camera.transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
			yield return null;
		}
	}

}
