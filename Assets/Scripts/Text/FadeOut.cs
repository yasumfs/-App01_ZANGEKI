using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour {
	[SerializeField]
	float fadeTime = 1f;
	float currentRemainTime;
	SpriteRenderer spRenderer;

	// Use this for initialization
	void Start () {
		// 初期化
		currentRemainTime = fadeTime;
		spRenderer = GetComponent<SpriteRenderer>();
	}
		
	void Update () {
		// 残り時間を更新
		currentRemainTime -= Time.deltaTime;

		if (currentRemainTime <= 0f) {
			// 残り時間が無くなったら自分自身を消滅
			this.gameObject.SetActive (false);
			currentRemainTime = fadeTime;
			return;
		}

		// フェードアウト
		float alpha = currentRemainTime / fadeTime;
		var color = spRenderer.color;
		color.a = alpha;
		spRenderer.color = color;
	}
}
