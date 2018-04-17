using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {
	float changeTime;
	// Use this for initialization
	void Start () {
		changeTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Mouse0))
			changeTime = 0;
		changeTime += Time.deltaTime;
		if (changeTime > 20f) {
			SceneManager.LoadScene ("Title");
		}
	}
}
