using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cross : MonoBehaviour {
	[SerializeField]
	float crossTime;
	float c_time;

	public static int count = 5;

	void Start () {
		count = 5;
		c_time = crossTime;
	}

	void Update () {
		if (MovePlayer.state == 6) {
			c_time -= Time.deltaTime;
			if (Input.GetKeyDown (KeyCode.Mouse0)) {
				count -= 1;
			}
		} else {
			this.gameObject.SetActive (false);
			c_time = crossTime;
		}
		GetComponent<Text>().text = c_time.ToString("F1");
		if (c_time <= 0) {
			MovePlayer.state = 7;
			c_time = 0;
			Cross.count = 5;
		}
	}
}
