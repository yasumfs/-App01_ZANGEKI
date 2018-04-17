using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZangeki : MonoBehaviour {
	[SerializeField]
	Vector3 speed;
	[SerializeField]
	int downDestroyPos;
	[SerializeField]
	int upDestroyPos;
	[SerializeField]
	int moveSpeed;
	[SerializeField]
	int damage;

	void Start () {
		// 何もしない
	}
	
	void Update () {
		// 移動速度と生存距離
		this.transform.position += speed * Time.deltaTime * moveSpeed;
		if (this.transform.position.y < downDestroyPos || this.transform.position.y > upDestroyPos) {
			Destroy (this.gameObject);
		}
	}


	// 当てた時
	void OnTriggerEnter (Collider c){
		if (MovePlayer.state == 1 || MovePlayer.state == 4 || MovePlayer.state == 5) {
			// 敵ならダメージを与える
			if (c.gameObject.CompareTag ("enemy")) {
				Destroy (this.gameObject);
			}
		}
	}
}
