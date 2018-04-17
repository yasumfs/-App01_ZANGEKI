using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHp : MonoBehaviour {
	public static int playerHp;
	[SerializeField]
	Slider _slider;
	float gameoverTime;

	void Start () {
		playerHp = 20;
		_slider.maxValue = playerHp;
		gameoverTime = 0f;
	}

	void Update () {
		if (playerHp <= 0) {
			playerHp = 0;
			GameOver ();
		}
		// HPゲージに値を設定
		_slider.value = playerHp;
	}

	void GameOver() {
		gameoverTime += Time.deltaTime;
		if (gameoverTime > 5) {
			SceneManager.LoadScene ("Title");
		}
	}
}
