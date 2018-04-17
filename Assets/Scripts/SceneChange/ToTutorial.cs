using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToTutorial : MonoBehaviour {
	[SerializeField]
	AudioSource touch;
	AudioSource main;
	bool canAudio;
	float fadeTime;

	[SerializeField]
	Vector3 stagePos;
	[SerializeField]
	GameObject frame;

	public static int stage;
	static bool isSelect;

	GameObject BGM_title;
	// Use this for initialization
	void Start () {
		isSelect = false;
		canAudio = true;
		fadeTime = 0.5f;
		BGM_title = GameObject.Find ("bgm_Title");
		if (BGM_title != null) {
			main = BGM_title.GetComponent<AudioSource> ();
			main.volume = 0.5f;
		}
	}

	// Update is called once per frame
	void Update () {
		if (BGM_title != null)
			main.volume = fadeTime;
	}

	public void Stage1 () {
		if (!isSelect) {
			StartCoroutine ("coFlick");
			fadeTime = 0.1f;
			frame.transform.position = stagePos;
			frame.SetActive (true);
			isSelect = true;
		}

	}

	public void Stage2 () {
		if (!isSelect) {
			StartCoroutine ("coFlick2");
			fadeTime = 0.1f;
			frame.transform.position = stagePos;
			frame.SetActive (true);
			isSelect = true;
		}
	}

	IEnumerator coFlick (){
		if (canAudio) {
			touch.Play ();
			canAudio = false;
		}
		stage = 1;
		yield return new WaitForSeconds (1);
		SceneManager.LoadScene ("Tutorial");
	}

	IEnumerator coFlick2 (){
		if (canAudio) {
			touch.Play ();
			canAudio = false;
		}
		stage = 2;
		yield return new WaitForSeconds (1);
		SceneManager.LoadScene ("Tutorial");
	}
}
