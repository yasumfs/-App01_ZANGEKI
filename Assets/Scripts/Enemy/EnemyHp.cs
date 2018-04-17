using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyHp : MonoBehaviour {
	[SerializeField]
	Slider _slider;

	void Start () {
		_slider.maxValue = MoveEnemy.enemyHp;
	}

	void Update () {
		_slider.maxValue = MoveEnemy.maxEnemyHp;
		if (MoveEnemy.enemyHp <= 0) {
			MoveEnemy.enemyHp = 0;
		}
		// HPゲージに値を設定
		_slider.value = MoveEnemy.enemyHp;
	}
}
