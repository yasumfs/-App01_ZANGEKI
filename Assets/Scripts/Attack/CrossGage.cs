using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossGage : MonoBehaviour {

	public Slider crossCountBar;
	[SerializeField]
	GameObject crossText;

	void Start () {
		crossCountBar = GameObject.Find("Chara/Canvas/CrossLifeBar").GetComponent<Slider>();
		crossCountBar.maxValue = Cross.count;
	}

	void Update () {
		crossCountBar.value = Cross.count;
		if (Cross.count <= 0) {
			this.gameObject.SetActive (false);
			crossText.SetActive (false);
		}
		if (MovePlayer.state == 6) {
			crossText.SetActive (true);
		} else {
			Cross.count = 5;
			crossText.SetActive (false);
		}
	}
}
