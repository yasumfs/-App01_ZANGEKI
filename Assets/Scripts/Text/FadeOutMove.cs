using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutMove : MonoBehaviour {
	[SerializeField]
	float fadeTime = 1f;
	float currentRemainTime;
	SpriteRenderer spRenderer;

	[SerializeField]
	int types;

	Vector3 pos;

	// Use this for initialization
	void Start () {
		// 初期化
		currentRemainTime = fadeTime;
		spRenderer = GetComponent<SpriteRenderer>();
		pos = new Vector3 (0, 0, 0);
	}

	void Update () {
		// 残り時間を更新
		currentRemainTime -= Time.deltaTime;

		if (currentRemainTime <= 0f) {
			// 残り時間が無くなったら自分自身を消滅
			this.gameObject.transform.position -= pos;
			pos = new Vector3 (0, 0, 0);
			this.gameObject.SetActive (false);
			currentRemainTime = fadeTime;

		} else {
			if (types == 1) {
				this.gameObject.transform.position += new Vector3 (-0.01f, 0.01f, 0);
				pos += new Vector3 (-0.01f, 0.01f, 0);
			} else if (types == 2) {
				this.gameObject.transform.position += new Vector3 (0, 0.01f, 0);
				pos += new Vector3 (0, 0.01f, 0);
			} else if (types == 3) {
				this.gameObject.transform.position += new Vector3 (0.01f, 0.01f, 0);
				pos += new Vector3 (0.01f, 0.01f, 0);
			} else if (types == 4) {
				this.gameObject.transform.position += new Vector3 (0, 0.001f, 0);
				pos += new Vector3 (0, 0.001f, 0);
			}
		}
		if (types != 4) {
			// フェードアウト
			float alpha = currentRemainTime / fadeTime;
			var color = spRenderer.color;
			color.a = alpha;
			spRenderer.color = color;
		}
	}
}