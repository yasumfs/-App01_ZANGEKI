using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zangeki_paramater : MonoBehaviour {
	
	[SerializeField]
	int damage;

	void Start () {
		// 何もしない
	}
	
	// Update is called once per frame
	void Update () {
		// 何もしない
	}

	void OnTriggerEnter (Collider c) {
		if (c.gameObject.CompareTag ("e_zangeki")) {
			PlayerHp.playerHp -= damage;
		}
		if (c.gameObject.CompareTag ("c_zangeki")) {
			MoveEnemy.enemyHp -= damage;
		}
		if (c.gameObject.CompareTag ("e_crossZangeki") || c.gameObject.CompareTag ("e_strongZangeki")) {
			PlayerHp.playerHp -= damage * 2;
		}
		if (c.gameObject.CompareTag ("c_crossZangeki") || c.gameObject.CompareTag ("c_strongZangeki")) {
			MoveEnemy.enemyHp -= damage * 2;
		}
	}
}
