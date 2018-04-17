using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	[SerializeField]
	GameObject[] enemy;
	public static int stage;
	[SerializeField]
	Vector3 pos;
	public static bool putout;

	float changeTime;
	[SerializeField]
	GameObject ClearText;
	[SerializeField]
	int clearStage;

	// Use this for initialization
	void Start () {
		stage = 0;
		putout = true;
		changeTime = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		// ステージごとに出す
		if (stage == 0 && putout) {
			Instantiate (enemy [stage], pos, Quaternion.identity);
			putout = false;
		} else if (stage == 1 && putout) {
			Instantiate (enemy [stage], pos, Quaternion.identity);
			putout = false;
		} else if (stage == 2 && putout) {
			Instantiate (enemy [stage], pos, Quaternion.identity);
			putout = false;
		} else if (stage >= 3) {
			stage = 3;
		}
		if (stage == clearStage) {
			GameClear ();
		}
	}

	void GameClear () {
		changeTime += Time.deltaTime;
		ClearText.SetActive (true);
		if (changeTime > 5) {
			SceneManager.LoadScene ("Clear");
		}
	}
}
