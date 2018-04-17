using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gage : MonoBehaviour {
	[SerializeField]
	Image _slinder;
	public static float gage = 0;
	public static bool canClick;
	float gageTime;
	bool isHissatsu;
	[SerializeField]
	GameObject particle;

	[SerializeField]
	AudioSource se_charge_finish;
	[SerializeField]
	AudioSource se_hisstsu;
	[SerializeField]
	AudioSource se_hisstsumode;
	[SerializeField]
	AudioSource hissatsuVoice;

	[SerializeField]
	GameObject fadeIn;

	bool audioPlay;

	float a_color;
	bool flag_G;

	[SerializeField]
	int timeSpeed = 1;

	void Start () {
		gage = 0.8f;
		canClick = false;
		gageTime = 1f;
		isHissatsu = false;
		audioPlay = true;

		a_color = 0;
	}

	void Update () {
		if(gage > 1) {
			// 最大を超えないようにする
			gage = 1;
		}
		if (gage == 1) {
			fadeIn.SetActive (true);
			canClick = true;
			_slinder.color = new Color (1f, 1f, 1f, 1f);
			Tenmetsu ();
			if (audioPlay) {
				se_charge_finish.Play ();
				audioPlay = false;
			}
			particle.SetActive (true);
		} else {
			fadeIn.SetActive (false);
		}

		// HPゲージに値を設定
		_slinder.fillAmount = gage;

		if (MovePlayer.state == 4) {
			G_Time ();
		}

		if (isHissatsu && MovePlayer.state == 0) {
			gage = 0;
			_slinder.color = new Color(1f, 1f, 1f, 0.4f);
			isHissatsu = false;
			particle.SetActive (false);
		}
	}

	public void OnClick() {
		if(canClick && MovePlayer.state == 1) {
			hissatsuVoice.Play ();
			se_hisstsu.Play ();
			se_hisstsumode.Play();
			MovePlayer.state = 5;
			MoveEnemy.canPlay = false;
			canClick = false;
			isHissatsu = true;
			gageTime = 1f;
			fadeIn.SetActive (false);
		}
	}

	void G_Time () {
		gageTime -= Time.deltaTime / 4f;
		canClick = false;
		audioPlay = true;
		if (gageTime < 0f) {
			gage = 0;
			_slinder.color = new Color(1f, 1f, 1f, 0.4f);
			isHissatsu = false;
			gageTime = 1f;
			particle.SetActive (false);
		} else {
			gage = gageTime;
		}
	}

	void Tenmetsu() {
		this.GetComponent<Image>().color = new Color (1, 1, 1, a_color);
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
