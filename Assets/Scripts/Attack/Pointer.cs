using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour {
	[SerializeField]
	int point;
	[SerializeField]
	GameObject zangeki_1;
	[SerializeField]
	GameObject zangeki_1_1;
	[SerializeField]
	GameObject zangeki_1_2;
	[SerializeField]
	GameObject boom_1;
	[SerializeField]
	GameObject boom_1_1;
	[SerializeField]
	GameObject boom_1_2;
	[SerializeField]
	GameObject Player;
	Vector3 pos;

	[SerializeField]
	GameObject slowText;
	[SerializeField]
	GameObject justText;
	[SerializeField]
	GameObject fastText;
	[SerializeField]
	GameObject hikari;
	//bool set = false;
	//float time;


	void Start () {
		//set = false;
		//time = 0f;

	}

	void Update () {
		pos = Player.transform.position + new Vector3 (0, 1, 1);
		/*if (set) {
			time += Time.deltaTime;
		}

		if (time > 1f) {
			hikari.SetActive (false);
			time = 0f;
		}*/
	}

	void OnTriggerStay (Collider c) {
		MovePlayer mp = Player.GetComponent<MovePlayer> ();
		// pointer_1の時の処理
		if (point == 1) {
			if (mp.touch && c.gameObject.tag == "e_zangeki") {
				Destroy (c.gameObject);
				slowText.SetActive (true);
				//set = true;
				Instantiate(zangeki_1, pos, Quaternion.identity);
			} else if (mp.touch && c.gameObject.tag == "e_boom") {
				Destroy (c.gameObject);
				slowText.SetActive (true);
				//set = true;
				Instantiate(boom_1, pos, Quaternion.identity);
			}
		// pointer_2の時の処理
		} else if (point == 2) {
			if (mp.touch && c.gameObject.tag == "e_zangeki") {
				Destroy (c.gameObject);
				hikari.SetActive (true);
				justText.SetActive (true);
				Gage.gage += 0.2f;
				//set = true;
				Instantiate (zangeki_1_1, pos, Quaternion.identity);
			} else if (mp.touch && c.gameObject.tag == "e_boom") {
				Destroy (c.gameObject);
				hikari.SetActive (true);
				justText.SetActive (true);
				Gage.gage += 0.2f;
				//set = true;
				Instantiate (boom_1_1, pos, Quaternion.identity);
			}
		// pointer_3の時の処理
		} else if (point == 3) {
			if (mp.touch && c.gameObject.tag == "e_zangeki") {
				Destroy (c.gameObject);
				fastText.SetActive (true);
				//set = true;
				Instantiate(zangeki_1_2, pos, Quaternion.identity);
			} else if (mp.touch && c.gameObject.tag == "e_boom") {
				Destroy (c.gameObject);
				fastText.SetActive (true);
				//set = true;
				Instantiate(boom_1_2, pos, Quaternion.identity);
			}
		}
	}
}
