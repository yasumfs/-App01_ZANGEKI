using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveZangeki : MonoBehaviour {
	[SerializeField]
	Vector3 Speed;
	Vector3 Pos;
	GameObject enemy;
	[SerializeField]
	int downDestroyPos_y;
	[SerializeField]
	int upDestroyPos_y;
	[SerializeField]
	int speed = 1;


	void Start () {
		// 何もしない
	}

	void Update () {

		// 斬撃移動
		if (MovePlayer.state != 6) {
			this.transform.position += Speed * speed * Time.deltaTime;
		}

		if (this.transform.position.y < downDestroyPos_y || this.transform.position.y > upDestroyPos_y) {
			Destroy (this.gameObject);
		}
			
		// clear,gameover,
		if (MovePlayer.state == 2 || MovePlayer.state == 3 || MovePlayer.state == 0) {
			Destroy (this.gameObject);
		}

//		// boomの場合
//		if (type == 1 && this.transform.position.y < downDestroyPos_y) {
//			PlayerHp.playerHp -= 2;
//			Destroy (this.gameObject);
//		} else if (type == 1 && this.transform.position.y > upDestroyPos_y) {
//			MoveEnemy.enemyHp -= 2;
//			Destroy (this.gameObject);
//		}
	}

}
