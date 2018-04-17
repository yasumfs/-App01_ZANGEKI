using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveEnemy : MonoBehaviour {
	[SerializeField]
	GameObject[] zangeki;
	[SerializeField]
	int waitingTime = 4;
	Vector3 e_Pos;
	[SerializeField]
	Vector3 move;
	[SerializeField]
	int moveTime;
	[SerializeField]
	Animator _animator;
	[SerializeField]
	AnimatorStateInfo animInfo;
	public static int stage = 0;
	float nextTime;

	public static int enemyHp;
	public static int maxEnemyHp;

	// 無敵時間
	Renderer renderer;

	// 動けるかどうか
	public static bool canPlay;

	[SerializeField]
	AudioSource se_huri;
	[SerializeField]
	GameObject se_huri_obj;
	[SerializeField]
	GameObject se_damage_obj;
	[SerializeField]
	AudioSource se_damage;
	[SerializeField]
	AudioSource[] attackVoice;
	[SerializeField]
	AudioSource[] damageVoice;

	int before_r;
	bool change;

	[SerializeField]
	int types;

	bool damage;

	GameObject voice_obj;
	AudioSource voice;
	[SerializeField]
	int enemyNum;

	void Start () {
		renderer = GetComponent<Renderer>();
		// エネミーの初期ライフ
		if (types == 1) {
			enemyHp = 4;
		} else if (types == 2) {
			enemyHp = 10;
		} else if (types == 3) {
			enemyHp = 20;
		} else if (types == 4) {
			enemyHp = 34;
		}
		if (enemyNum == 1 || enemyNum == 4) {
			voice_obj = GameObject.Find ("e_SE/se_damage01");
			voice = voice_obj.GetComponent<AudioSource> ();
		} else if (enemyNum == 2 || enemyNum == 5) {
			voice_obj = GameObject.Find ("e_SE/se_damage02");
			voice = voice_obj.GetComponent<AudioSource> ();
		} else if (enemyNum == 3 || enemyNum == 6) {
			voice_obj = GameObject.Find ("e_SE/se_damage03");
			voice = voice_obj.GetComponent<AudioSource> ();
		}
		maxEnemyHp = enemyHp;
		canPlay = true;

		se_huri_obj = GameObject.Find ("SE/se_huri");
		se_huri = se_huri_obj.GetComponent<AudioSource>();
		se_damage_obj = GameObject.Find ("SE/se_damage");
		se_damage = se_damage_obj.GetComponent<AudioSource>();
		change = false;
		before_r = -1;

		damage = false;
	}
		
	void Update () {
		_animator.SetBool ("damage", damage);
		if (damage) {
			damage = false;
		}
		e_Pos = this.transform.position;
		if (enemyHp <= 0) {
			damageVoice [0].Play ();
			Destroy (this.gameObject);
		}
		// 敵が抜刀1秒以上待ったら納刀
		animInfo = _animator.GetCurrentAnimatorStateInfo(0);
		if(animInfo.normalizedTime < 1.0f)
		{
			_animator.SetInteger ("state", 0);
		}

		//必殺中は敵は攻撃できない
		if (MovePlayer.state == 4 || MovePlayer.state == 5) {
			CancelInvoke ();
		} else if ( !(MovePlayer.state == 4 || MovePlayer.state == 5) && !canPlay) {
			Awake ();
			canPlay = true;
		}
			
	}

	// 敵の動き (敵が出たはじめに呼ばれる)
	void Awake () {
		// 出現や攻撃をランダムにする
		waitingTime = Random.Range (2, waitingTime);
		// InvokeRepeating("関数名",初回呼出までの遅延秒数,次回呼出までの遅延秒数)
		InvokeRepeating ("Move", moveTime, waitingTime);
		InvokeRepeating ("Create", waitingTime, waitingTime);
	}

	// 敵の攻撃
	void Create() {
		if (MovePlayer.state == 1 || MovePlayer.state == 3) {
			// ランダムに攻撃させる
			int r = Random.Range (0, zangeki.Length);
			attackVoice [0].Play ();
			r = RandomAttack (r);
			before_r = r;
			se_huri.Play ();
			if (MovePlayer.state == 1) {
				switch (r) {
				case 0:
					Instantiate (zangeki [0], e_Pos, Quaternion.identity);
					_animator.SetInteger ("state", 1);
					break;
				case 1:
					Instantiate (zangeki [1], e_Pos, Quaternion.identity);
					_animator.SetInteger ("state", 1);
					break;
				case 2:
					Instantiate (zangeki [2], e_Pos, Quaternion.identity);
					_animator.SetInteger ("state", 1);
					break;
				case 3:
					Instantiate (zangeki [3], e_Pos, Quaternion.identity);
					_animator.SetInteger ("state", 1);
					break;
				case 4:
					Instantiate (zangeki [4], e_Pos, Quaternion.identity);
					_animator.SetInteger ("state", 1);
					break;
				}
			} else {
				_animator.SetInteger ("state", 1);
			}
		}
	}

	// 2回同じ値にならないようにする
	int RandomAttack (int r) {
		if (change) {
			if (before_r == r) {
				r = Random.Range (0, zangeki.Length);
				RandomAttack (r);
				return r;
			} else {
				return r;
			}
		} else {
			change = true;
			return r;
		}
	}

	// 敵の動き
	void Move() {
		if (MovePlayer.state == 1 || MovePlayer.state == 3) {
			int r = Random.Range (0, 3);
			switch (r) {
			case 0:
				break;
			case 1:
				if (this.transform.position.x < move.x) {
					this.transform.position += move;
				}
				break;
			case 2:
				if (this.transform.position.x > -move.x) {
					this.transform.position -= move;
				}
				break;
			}
		}
	}

	// 斬撃当たった時
	void OnTriggerEnter (Collider c){
		if (MovePlayer.state == 1 || MovePlayer.state == 4 || MovePlayer.state == 5) {
			// 無敵時間のコルーチン呼び出し
			voice.Play ();
			damage = true;
			StartCoroutine ("Damage");
			se_damage.Play ();
		}
	}

	IEnumerator Damage ()
	{
		//レイヤーをPlayerDamageに変更
		gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
		//while文を10回ループ
		int count = 5;
		while (count > 0){
			//透明にする
			renderer.material.color = new Color (1,1,1,0);
			//0.05秒待つ
			yield return new WaitForSeconds(0.1f);
			//元に戻す
			renderer.material.color = new Color (1,1,1,1);
			//0.05秒待つ
			yield return new WaitForSeconds(0.1f);
			count--;
		}
		//レイヤーをPlayerに戻す
		gameObject.layer = LayerMask.NameToLayer("Player");
	}

}
