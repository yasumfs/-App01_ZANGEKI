using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToGame : MonoBehaviour {
	float changeTime;
	[SerializeField]
	Image _slinder;

	[SerializeField]
	GameObject tutorial01;
	[SerializeField]
	GameObject tutorial02;

	bool change;

	GameObject BGM_title;
	AudioSource main;


	void Start () {
		changeTime = 0;
		change = true;
		BGM_title = GameObject.Find ("bgm_Title");
		if (BGM_title != null) { 
			main = BGM_title.GetComponent<AudioSource> ();
			main.volume = 0.5f;
		}
		if (ToTutorial.stage == 1 || ToTutorial.stage == 2) {

		} else {
			ToTutorial.stage = 1;
		}
	}


	void Update () {
		changeTime += Time.deltaTime * 1f;
		if (changeTime > 15f) {
			SceneManager.LoadScene ("Main" + ToTutorial.stage);
		} else {
			_slinder.fillAmount = changeTime / 15f;
		}
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			if (!change) {
				tutorial01.SetActive (true);
				tutorial02.SetActive (false);
				change = true;
			} else {
				tutorial01.SetActive (false);
				tutorial02.SetActive (true);
				change = false;
			}
		}
	}
}
