using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MovePlayer : MonoBehaviour {
	[SerializeField]
	int stage;
	[SerializeField]
	GameObject zangeki;

	// タッチ判定、スワイプ判定のための変数
	Vector3 touchPos;
	Vector3 touchStartPos;
	Vector3 touchEndPos;
	Vector3 startPos;
	Vector3 screenToWorldPointPosition;
	Vector3 screenToWorldPointPosition1;

	// どっち方向のスワイプか
	string direction;

	// 動けるか、攻撃できるかの判定bool
	public bool touch = false;
	// 攻撃してからの数秒間動けない
	float delayTime = 0f;

	bool delay = false;
	bool canMove = false;

	// 移動距離に使う変数
	Vector3 p_Pos;
	[SerializeField]
	Vector3 xMove = new Vector3 (2f,0f,0f);

	// Animation
	public Animator _animator;

	// ゲームオーバー
	[SerializeField]
	GameObject gameover;

	float nextTime;
	[SerializeField]
	float appearanceTime;

	bool canRun = false;

	[SerializeField]
	GameObject hissatsuText;
	float h_time;

	[SerializeField]
	GameObject partical;

	// ステージを進めることができるか
	bool canNext;

	// 無敵時間
	Renderer renderer;

	public static int state;
	// -1 = チュートリアル
	//	0 = play前
	//	1 = play
	//	2 = stageClear
	//	3 = gameOver
	//	4 = hissatsu
	//	5 = カットシーン
	//	6 = クロス斬撃中
	//	7 = クロス斬撃返せなかった後

	[SerializeField]
	GameObject[] stageText;
	float stageTextTime;

	[SerializeField]
	GameObject[] tree;
	float m_time;
	[SerializeField]
	GameObject rock;

	[SerializeField]
	float delayT;

	enum AnimState {
		None = 1,
		attack = 3,
		attack_2 = 4,
	}

	AnimState _animState = AnimState.None;

	int animState;
	float aniChangeTime;

	[SerializeField]
	AudioSource BGM_main;
	[SerializeField]
	AudioSource BGM_boss;
	bool seChange;
	[SerializeField]
	AudioSource se_swing;
	[SerializeField]
	AudioSource se_huri;
	[SerializeField]
	GameObject se_clear;
	[SerializeField]
	GameObject se_gameover;
	[SerializeField]
	AudioSource se_dash;
	bool sePlay;
	[SerializeField]
	AudioSource se_damage;
	[SerializeField]
	AudioSource[] AttackVoice;
	[SerializeField]
	AudioSource[] damageVoice;

	float h_AttackTime;
	bool canAttack;

	public float chargeTime;

	[SerializeField]
	GameObject crossGage;

	float cleartime = 0f;

	GameObject BGM_title;

	bool damage;

	void Start () {
		renderer = GetComponent<Renderer>();
		nextTime = 0;
		state = 0;
		h_time = -9;
		stageTextTime = -9;
		canNext = false;
		m_time = 0;
		animState = 0;
		h_AttackTime = 0;
		canAttack = true;
		sePlay = true;
		chargeTime = 0;
		seChange = false;

		BGM_title = GameObject.Find ("bgm_Title");
		if (BGM_title != null) {
			Destroy (BGM_title);
		}
		damage = false;
	}

	void Update () {
		// クリア時の音
		if (state == 2) {
			cleartime += Time.deltaTime;
			BGM_main.volume = 0.1f;
			se_clear.SetActive (true);
			if (cleartime > 1f) {
				_animator.SetInteger ("P_state", 10);
				_animator.SetBool ("damage", false);
			}
		}
		_animator.SetBool ("damage", damage);
		if (damage) {
			damage = false;
		}
		// ボス戦の音
		if (GameController.stage == 2) {
			if (BGM_main.volume > 0) {
				BGM_main.volume -= Time.deltaTime;
			} else {
				if (!seChange) {
					BGM_main.Stop ();
					BGM_boss.Play ();
					seChange = true;
				}
			}
		}

		if (state == 0 && canNext && GameController.stage <= 4) {
			GameController.stage += 1;
			if (GameController.stage != 3) {
				_animator.SetInteger ("P_state", 5);
				_animator.SetBool ("damage", false);
			}
			canNext = false;
		}

		if (state == 0 && GameController.stage <= 3) {
			StartCoroutine ("coStageText");
		} else {
			stageText [GameController.stage].SetActive (false);
		}

		if (state == 0 && GameController.stage != 0 && GameController.stage != 3 && sePlay) {
			se_dash.Play ();
			sePlay = false;
		}

		if (state == 1) {
			canNext = true;
			m_time = 0;
		}

		// 必殺
		if (state == 5) {
			Hissatsu ();
		}

		// 必殺中
		if (state == 4) {
			hissatsuText.SetActive (false);
			touch = true;
			if (Gage.gage == 0) {
				h_time = -9;
				state = 1;
				partical.SetActive (false);
			}
		} else if (state == 1){
			touch = false;
		}

		// フリック処理
		if (state == 1 || state == 4) {
			sePlay = true;
			se_dash.Stop ();
			Flick ();
			//Swing();
		}

		// ゲームオーバー
		if (PlayerHp.playerHp == 0) {
			GameOver ();
		}

		// 敵のライフが0になったら時
		if ((state == 1 || state == 4 || state == 0) && GameController.stage <= 2) {
			DefeatEnemy ();
		}

		if (GameController.stage == 3) {
			state = 2;
		}
		// 移動距離制限
		Clamp ();

		// 攻撃のdelay時
		Delay ();

		if (!canAttack) {
			h_AttackTime += Time.deltaTime * 1f;
		}
		if (h_AttackTime > 0.5f) {
			canAttack = true;
		}

	}

	// 戦闘開始前テキスト出す
	IEnumerator coStageText () {
		stageText [GameController.stage].SetActive (true);
		stageTextTime += Time.deltaTime * 15f;
		if (stageTextTime >= 0) {
			stageTextTime = 0;
			stageText [GameController.stage].transform.position = new Vector3 (stageTextTime, 2f, 0f);
			// 動ききったら数秒間止める
			yield return new WaitForSeconds (2);
			if (state != 4 && state != 5) {
				state = 1;
			}
			stageText [GameController.stage].SetActive (false);
			stageTextTime = -9f;
		} else {
			stageText [GameController.stage].transform.position = new Vector3 (stageTextTime, 2f, 0f);
		}
	}

	// 移動制限
	void Clamp() {
		Vector3 min = -xMove;
		Vector3 max = xMove;
		this.transform.position =  new Vector3 (Mathf.Clamp (this.transform.position.x, min.x, max.x),transform.position.y,transform.position.z);
	}

	public void MoveLeft () {
		if (state == 0 || state == 1 || state == 4) {
			canMove = true;
			p_Pos = this.transform.position;
			// 左右移動
			if (canMove) {
				p_Pos -= xMove;
				this.transform.position = p_Pos;
			}
			canMove = false;
		}
	}

	public void MoveRight () {
		if (state == 0 || state == 1 || state == 4) {
			canMove = true;
			p_Pos = this.transform.position;
			// 左右移動
			if (canMove) {
				p_Pos += xMove;
				this.transform.position = p_Pos;
			}
			canMove = false;
		}
	}

	// 画面をタッチした瞬間に攻撃(チャージ攻撃なくなる)
	/*void Swing() {
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			Vector3 _scrPos = (Input.mousePosition);
			if (!IsUGUIHit (_scrPos) ){
				Attack ();
			}
		}
	}*/

	// UI以外をタッチしたら
	bool IsUGUIHit(Vector3 _scrPos){ // Input.mousePosition
		PointerEventData pointer = new PointerEventData (EventSystem.current);
		pointer.position = _scrPos;
		List<RaycastResult> result = new List<RaycastResult> ();
		EventSystem.current.RaycastAll (pointer, result);
		return (result.Count > 0);
	}

	// ダメージくらった時の無敵時間
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

	void GameOver() {
		state = 3;
		se_gameover.SetActive (true);
		gameover.SetActive (true);
		Destroy (this.gameObject);
	}

	void DefeatEnemy () {
		if (MoveEnemy.enemyHp <= 0) {
			state = 0;
			// プレーヤー走る
			canRun = true;
			nextTime += Time.deltaTime * 1f;
			h_time = -9;
			partical.SetActive (false);
			MoveObj ();
			// 次の敵のリスポン
			if (nextTime > appearanceTime) {
				if (state != 4 && state != 5 && state != 2)
					state = 1;
				GameController.putout = true;
				canRun = false;
				// 納刀状態に戻す
				_animator.SetInteger ("P_state", 0);
				nextTime = 0;
			}
		} else {
			if (!delay) {
				_animator.SetInteger ("P_state", 0);
			}
			nextTime = 0;
			canRun = false;
		}
	}

	// 攻撃からdelay時間
	void Delay() {
		if (delayTime > delayT && !canRun && state != 4) {
			delay = false;
			// 納刀状態に戻す
			_animator.SetInteger ("P_state", 0);
		}
		// delay時間
		if (delay) {
			delayTime += Time.deltaTime * 1f;
			if (animState == 1) {
				aniChangeTime += Time.deltaTime;
				if (aniChangeTime > 0.5f) {
					_animator.SetInteger ("P_state", 2);
					animState = 2;
					aniChangeTime = 0f;
				}
			}
		}
	}

	void MoveObj() {
		// 木や岩を動かす
		m_time += Time.deltaTime * 1;
		if (stage == 1) {
			tree [0].transform.position -= new Vector3 (0, m_time / 10f, m_time / 10f);
			tree [1].transform.position -= new Vector3 (0, m_time / 10f, m_time / 10f);
			rock.transform.position -= new Vector3 (0, m_time / 10f, m_time / 10f);
			if (tree [0].transform.position.y < 6f) {
				tree [0].transform.position = new Vector3 (-4f, 11f, 9f);
			}
			if (tree [1].transform.position.y < 2f) {
				tree [1].transform.position = new Vector3 (4.2f, 9f, 9f);
			}
			if (rock.transform.position.y < -4) {
				rock.transform.position = new Vector3 (0f, 8f, 16f);
			}
		} else if (stage == 2) {
			tree [0].transform.position -= new Vector3 (0, m_time / 10f, (m_time / 10f) * 1.2f);
			tree [1].transform.position -= new Vector3 (0, m_time / 10f, (m_time / 10f) * 1.2f);
			tree [2].transform.position -= new Vector3 (0, m_time / 7.8f, (m_time / 10f) * 1.2f);
			if (tree [0].transform.position.y < -3.5f) {
				tree [0].transform.position = new Vector3 (0f, 10f, 12f);
			}
			if (tree [1].transform.position.y < -3.5f) {
				tree [1].transform.position = new Vector3 (0f, 10f, 12f);
			}
			if (tree [2].transform.position.y < -5f) {
				tree [2].transform.position = new Vector3 (0f, 7.8f, 20f);
			}
		}
	}

	// 必殺中
	void Hissatsu () {
		hissatsuText.SetActive (true);
		StartCoroutine("coFlowTime");
		partical.SetActive (true);
		Gage.canClick = false;
	}

	// 必殺打った時のカットシーン
	IEnumerator coFlowTime () {
		// h_time = -9なのでそこから移動さす
		h_time += Time.deltaTime * 20f;
		// カットシーンの動き
		if (h_time >= 0) {
			h_time = 0;
			hissatsuText.transform.position = new Vector3 (h_time,1.38f,0f);
			// 動ききったら数秒間止める
			yield return new WaitForSeconds( 2 );
			// 必殺中にする
			state = 4;
		} else {
			hissatsuText.transform.position = new Vector3 (h_time,1.38f,0f);
		}
	} 

	// 必殺中と攻撃
	void Attack () {
		Vector3 pos = this.gameObject.transform.position + new Vector3 (0, 1.5f, 1.5f);
		int rnd = Random.Range (0, AttackVoice.Length);
		if (!delay) {
			se_swing.Play ();
			AttackVoice [rnd].Play ();
			touch = true;
			delay = true;
			delayTime = 0f;
			_animator.SetInteger ("P_state", 1);
			animState = 1;
		}
		if (state == 4) {
			if (canAttack) {
				Instantiate (zangeki, pos, Quaternion.identity);
				se_huri.Play ();
				AttackVoice [rnd].Play ();
				int setStateValue = 0;
				switch (_animState) {
				case AnimState.None:
					setStateValue = 1;
					_animState = AnimState.attack;
					break;
				case AnimState.attack:
					setStateValue = 3;
					_animState = AnimState.attack_2;
					break;
				case AnimState.attack_2:
					setStateValue = 4;
					_animState = AnimState.attack;
					break;
				}
				_animator.SetInteger ("P_state", setStateValue);
				delayTime = 0f;
				h_AttackTime = 0f;
				canAttack = false;
			}
		}
	}

	// 斬撃に当たるとダメージ
	void OnTriggerEnter (Collider c) {
		damage = true;
		_animator.SetBool ("damage", damage);
		int rnd = Random.Range (0, damageVoice.Length);
		damageVoice [rnd].Play ();
		Destroy (c.gameObject);
		se_damage.Play ();
		// 無敵時間のコルーチン呼び出し
		StartCoroutine ("Damage");
		if (c.gameObject.CompareTag ("e_crossZangeki")) {
			crossGage.SetActive (false);
		}
	}


	// フリックした時
	void Flick(){
		if (Input.GetKeyDown(KeyCode.Mouse0)){
			touchStartPos = new Vector3(Input.mousePosition.x,
				Input.mousePosition.y,
				Input.mousePosition.z);

		}

		if (Input.GetKeyUp(KeyCode.Mouse0)){
			touchEndPos = new Vector3(Input.mousePosition.x,
				Input.mousePosition.y,
				Input.mousePosition.z);
			GetDirection();
		}
		/*if (Input.GetKey (KeyCode.Mouse0)) {
			chargeTime += Time.deltaTime * 1;
			_animator.SetInteger ("P_state", 6);
		}*/
	}

	// フリック時の方向
	void GetDirection(){
		float directionX = touchEndPos.x - touchStartPos.x;
		float directionY = touchEndPos.y - touchStartPos.y;

		if (Mathf.Abs (directionY) < Mathf.Abs (directionX)) {
			if (50 < directionX) {
				//右向きにフリック
				direction = "right";
			} else if (-50 > directionX) {
				//左向きにフリック
				direction = "left";
			} else {
				//タッチを検出
				direction = "touch";
			}
		} else if (Mathf.Abs (directionX) < Mathf.Abs (directionY)) {
			if (50 < directionY) {
				//上向きにフリック
				direction = "up";
			} else if (-50 > directionY) {
				//下向きのフリック
				direction = "down";
			} else {
				//タッチを検出
				direction = "touch";
			}
		} else {
			direction = "touch";
		}
		switch (direction) {
		case "up":
			//上フリックされた時の処理
			Attack ();
			break;

		case "down":
			//下フリックされた時の処理
			break;

		case "right":
			//右フリックされた時の処理
			MoveRight();
			canMove = true;
			break;

		case "left":
			//左フリックされた時の処理
			MoveLeft();
			canMove = true;
			break;

		case "touch":
			Attack ();
			//タッチされた時の処理
			break;
		}
	}

	/*void Attack () {
	Vector3 pos = this.gameObject.transform.position + new Vector3 (0, 1.5f, 1.5f);
	if (!delay) {
		se_swing.Play ();
		swipe = true;
		delay = true;
		delayTime = 0f;
		_animator.SetInteger ("P_state", 1);
		animState = 1;
	}
	if (state == 4) {
		if (canAttack) {
			Instantiate (zangeki, pos, Quaternion.identity);
			if (animState != 3 && animState != 4) {
				_animator.SetInteger ("P_state", 1);
				animState = 3;
				delayTime = 0f;
			} else if (animState == 3) {
				_animator.SetInteger ("P_state", 3);
				se_huri.Play ();
				animState = 4;
				delayTime = 0f;
			} else if (animState == 4) {
				se_huri.Play ();
				_animator.SetInteger ("P_state", 4);
				animState = 3;
				delayTime = 0f;
			}
			h_AttackTime = 0f;
			canAttack = false;
		}
	}
}*/

}