using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Judge : MonoBehaviour {
	Vector3 pos;
	[SerializeField]
	Vector3[] checkPos;

	[SerializeField]
	GameObject[] zangeki;

	GameObject judge;

	GameObject slow;
	GameObject just;
	GameObject fast;
	GameObject slowText;
	GameObject justText;
	GameObject fastText;
	GameObject hikari;

	GameObject Player;

	GameObject se_longMetal_obj;
	AudioSource se_longMetal;
	GameObject se_charge_obj;
	AudioSource se_charge;
	GameObject se_cross_obj;
	AudioSource se_cross;
	GameObject crossText;
	GameObject crossCount;
	bool seCross;
	[SerializeField]
	GameObject crossSpark;
	bool isSpark;

	[SerializeField]
	GameObject returnFlash;

	[SerializeField]
	int type;

	int check = 0;

	void Start () {
		slow = GameObject.Find("Chara/JudgeText/slow");
		just = GameObject.Find("Chara/JudgeText/just");
		fast = GameObject.Find("Chara/JudgeText/fast");
		slowText = GameObject.Find("Chara/JudgeText/slowText");
		justText = GameObject.Find("Chara/JudgeText/justText");
		fastText = GameObject.Find("Chara/JudgeText/fastText");
		hikari = GameObject.Find("Chara/JudgeText/hikari");
		Player = GameObject.Find("Chara");
		se_longMetal_obj = GameObject.Find("SE/se_kaeshi");
		se_longMetal = se_longMetal_obj.GetComponent<AudioSource>();
		se_charge_obj = GameObject.Find("SE/se_charge");
		se_charge = se_charge_obj.GetComponent<AudioSource>();
		se_cross_obj = GameObject.Find("SE/se_cross");
		se_cross = se_cross_obj.GetComponent<AudioSource>();

		judge = GameObject.Find ("Chara/Pointer/e_zangeki01");

		crossCount = GameObject.Find ("Chara/Canvas/CrossLifeBar");
		crossText = GameObject.Find ("Chara/Canvas/Text");
		seCross = true;
		isSpark = false;

	}

	void Update () {

		JudgeColor ();
		Check ();

		if (MovePlayer.state == 7) {
			se_cross.Stop ();
		}

		// 必殺などのカットシーンの時にZangekiを破壊
		if (MovePlayer.state == 5) {
			Destroy (this.gameObject);
		}
	}

	// スワイプした時の返す方向
	void Check() {
		MovePlayer mp = Player.GetComponent<MovePlayer> ();
		pos = this.transform.position;
		Vector3 p_Pos = Player.gameObject.transform.position;
		// タッチした時
		if (mp.touch) {
			// 手前の判定
			if (pos.y >= checkPos [0].y && pos.y <= checkPos [1].y && p_Pos.x == pos.x) {
				check = 1;
				if (type == 1) {
					Zangeki (check);
				} else if (type == 2) {
					CrossZangeki (check);
				} else if (type == 3) {
					Zangeki (check);
				}
				// 真ん中判定
			} else if (pos.y > checkPos [1].y && pos.y < checkPos [2].y && p_Pos.x == pos.x) {
				check = 2;
				if (type == 1) {
					Zangeki (check);
				} else if (type == 2) {
					CrossZangeki (check);
				} else if (type == 3) {
					Zangeki (check);
				}
				// 奥判定
			} else if (pos.y >= checkPos [2].y && pos.y <= checkPos [3].y && p_Pos.x == pos.x) {
				check = 3;
				if (type == 1) {
					Zangeki (check);
				} else if (type == 2) {
					CrossZangeki (check);
				} else if (type == 3) {
					Zangeki (check);
				}
			} else {
				mp._animator.SetInteger ("P_state", 1);
			}
			judge.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0.3f);
			mp.chargeTime = 0f;
		}
	}

	// 普通の斬撃
	void Zangeki(int z_direction) {
		MovePlayer mp = Player.GetComponent<MovePlayer> ();
		mp._animator.SetInteger ("P_state", 1);
		switch (z_direction) {
		case 1:
			Instantiate (returnFlash, pos + new Vector3 (-0.5f, 0, 0), Quaternion.identity);
			if (mp.chargeTime > 1f) {
				Instantiate (zangeki [3], pos, Quaternion.identity);
			} else {
				Instantiate (zangeki [0], pos, Quaternion.identity);
			}
			se_longMetal.Play ();
			Destroy (this.gameObject);
			slowText.SetActive (true);
			slow.SetActive (true);
			break;
		case 2:
			Instantiate (returnFlash, pos, Quaternion.identity);
			if (mp.chargeTime > 1f) {
				Instantiate (zangeki [4], pos, Quaternion.identity);
			} else {
				Instantiate (zangeki [1], pos, Quaternion.identity);
			}
			se_longMetal.Play ();
			Destroy (this.gameObject);
			hikari.SetActive (true);
			justText.SetActive (true);
			just.SetActive (true);
			Gage.gage += 0.2f;
			if (Gage.gage != 1) {
				se_charge.Play ();
			}
			break;
		case 3:
			Instantiate (returnFlash, pos + new Vector3 (0.5f, 0, 0), Quaternion.identity);
			if (mp.chargeTime > 1f) {
				Instantiate (zangeki [5], pos, Quaternion.identity);
			} else {
				Instantiate (zangeki [2], pos, Quaternion.identity);
			}
			se_longMetal.Play ();
			Destroy (this.gameObject);
			fastText.SetActive (true);
			fast.SetActive (true);
			break;
		}
	}

	// クロス斬撃(タッチ連打が必要な斬撃)
	void CrossZangeki (int z_direction) {
		MovePlayer mp = Player.GetComponent<MovePlayer> ();
		mp._animator.SetInteger ("P_state", 6);
		crossText.SetActive (true);
		crossCount.SetActive (true);
		if (seCross) {
			se_cross.Play ();
			seCross = false;
		}
		MovePlayer.state = 6;
		if (MovePlayer.state == 6) {
			if (!isSpark) {
				Instantiate (crossSpark, pos + new Vector3 (0,0,0), Quaternion.identity);
				isSpark = true;
			}
		} else {
			crossSpark.SetActive (false);
			isSpark = false;
		}
		if (Cross.count <= 0) {
			mp._animator.SetInteger ("P_state", 7);
			se_cross.Stop ();
			MovePlayer.state = 1;
			switch (z_direction) {
			case 1:
				Instantiate (returnFlash, pos + new Vector3 (-0.5f,0,0), Quaternion.identity);
				Instantiate (zangeki [6], pos, Quaternion.identity);
				se_longMetal.Play ();
				Destroy (this.gameObject);
				slowText.SetActive (true);
				slow.SetActive (true);
				break;
			case 2:
				Instantiate (returnFlash, pos + new Vector3 (0,0,0), Quaternion.identity);
				Instantiate (zangeki [7], pos, Quaternion.identity);
				se_longMetal.Play ();
				Destroy (this.gameObject);
				hikari.SetActive (true);
				justText.SetActive (true);
				just.SetActive (true);
				Gage.gage += 0.2f;
				if (Gage.gage != 1) {
					se_charge.Play ();
				}
				break;
			case 3:
				Instantiate (returnFlash, pos + new Vector3 (0.5f,0,0), Quaternion.identity);
				Instantiate (zangeki [8], pos, Quaternion.identity);
				se_longMetal.Play ();
				Destroy (this.gameObject);
				fastText.SetActive (true);
				fast.SetActive (true);
				break;
			}
		}
	}

	void JudgeColor () {
		pos = this.transform.position;
		Vector3 p_Pos = Player.gameObject.transform.position;
		if (pos.y >= checkPos [0].y && pos.y <= checkPos [3].y && p_Pos.x == pos.x) {
			judge.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 0, 1);
		} else {
			judge.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0.3f);
		}
	}

	void OnTriggerEnter (Collider c) {
		MovePlayer.state = 1;
	}
}
