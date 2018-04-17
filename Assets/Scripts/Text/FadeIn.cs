using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour {
	[SerializeField]
	float fadeTime = 0;
	float currentRemainTime = 0;
	SpriteRenderer spRenderer;

	void Start () {
		currentRemainTime = 0;
		spRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		// 残り時間を更新
		currentRemainTime += Time.deltaTime;

		if (currentRemainTime >= fadeTime) {
			// 残り時間が無くなったら自分自身を消滅
			this.gameObject.SetActive (false);
			currentRemainTime = 0;
			float al = 0;
			var color1 = spRenderer.color;
			color1.a = al;
			spRenderer.color = color1;
			return;
		}

		// フェードアウト
		float alpha = currentRemainTime / fadeTime;
		var color = spRenderer.color;
		color.a = alpha;
		spRenderer.color = color;
	}
}
