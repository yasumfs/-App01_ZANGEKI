using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToTitle : MonoBehaviour {
	[SerializeField]
	AudioSource touch;
	[SerializeField]
	AudioSource main;
	bool canAudio;
	float fadeTime;

	void Start () {
		canAudio = true;
		fadeTime = 0.5f;
	}

	void Update () {
		main.volume = fadeTime;
		if (Input.GetKeyUp (KeyCode.Mouse0)) {
			StartCoroutine ("coFlick");
			fadeTime = 0.1f;
		}
	}

	IEnumerator coFlick (){
		if (canAudio) {
			touch.Play ();
			canAudio = false;
		}
		yield return new WaitForSeconds (1);
		SceneManager.LoadScene ("Title");
	}
}
