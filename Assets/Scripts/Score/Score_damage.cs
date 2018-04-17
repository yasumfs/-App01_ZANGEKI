using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score_damage : MonoBehaviour {
	public static int z_damage;
	public static int kz_damage;
	public static int cz_damage;

	void Start () {
		z_damage = 0;
		kz_damage = 0;
		cz_damage = 0;
	}

	void Update () {
		//何もしない
	}

	void OnTriggerEnter (Collider c) {
		if (c.gameObject.CompareTag ("e_zangeki")) {
			z_damage++;
		}
		if (c.gameObject.CompareTag ("e_crossZangeki")) {
			cz_damage++;
		}
		if (c.gameObject.CompareTag ("e_strongZangeki")) {
			kz_damage++;
		}
	}
}
