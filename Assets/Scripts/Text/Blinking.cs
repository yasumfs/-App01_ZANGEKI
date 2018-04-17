using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinking : MonoBehaviour {

	[SerializeField]
	GameObject Qtext;
	float a_color;
	bool flag_G;

	[SerializeField]
	int timeSpeed = 1;

	void Start () {
		a_color = 0;
	}

	void Update () {
		//テキストの透明度を変更する
		Qtext.GetComponent<SpriteRenderer>().color = new Color (1, 1, 1, a_color);
		if (flag_G) {
			a_color -= Time.deltaTime * timeSpeed;
		} else {
			a_color += Time.deltaTime * timeSpeed;
		}
		if (a_color < 0) {
			a_color = 0;
			flag_G = false;
		} else if (a_color > 1){
			a_color = 1;
			flag_G = true;
		}
	}
}
