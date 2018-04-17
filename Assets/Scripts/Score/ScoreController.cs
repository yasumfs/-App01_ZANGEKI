using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {
	[SerializeField]
	GameObject[] rank;
	[SerializeField]
	GameObject[] chara_rank;
	int rankNum;
	/*[SerializeField]
	GameObject[] damageNum;*/
	[SerializeField]
	Sprite[] m_sprite = new Sprite[10];
	// Use this for initialization
	[SerializeField]
	GameObject[] zangeki;

	void Start () {
		rankNum = 0;
	}
	
	// Update is called once per frame
	void Update () {
		rankNum = Score_damage.z_damage * 2 + Score_damage.cz_damage * 4 + Score_damage.kz_damage * 4;
		if (rankNum == 0) {
			rank [0].SetActive (true);
			chara_rank [0].SetActive (true);
		} else if (rankNum < 5) {
			rank [1].SetActive (true);
			chara_rank [1].SetActive (true);
		} else if (rankNum < 12) {
			rank [2].SetActive (true);
			chara_rank [2].SetActive (true);
		} else {
			rank [3].SetActive (true);
			chara_rank [3].SetActive (true);
		} 
		SetNumber (Score_damage.z_damage);
		SetNumber_cross (Score_damage.cz_damage);
		SetNumber_kyo (Score_damage.kz_damage);
		/*damageNum[0].GetComponent<Text>().text = Score_damage.z_damage.ToString();
		damageNum[1].GetComponent<Text>().text =Score_damage.kz_damage.ToString();
		damageNum[2].GetComponent<Text>().text = Score_damage.cz_damage.ToString();*/
	}

	public void SetNumber(int num)
	{
		SpriteRenderer sr = zangeki[0].gameObject.GetComponent<SpriteRenderer>();
		sr.sprite = m_sprite[num];
	}

	public void SetNumber_kyo(int num)
	{
		SpriteRenderer sr = zangeki[1].gameObject.GetComponent<SpriteRenderer>();
		sr.sprite = m_sprite[num];
	}

	public void SetNumber_cross(int num)
	{
		SpriteRenderer sr = zangeki[2].gameObject.GetComponent<SpriteRenderer>();
		sr.sprite = m_sprite[num];
	}
}
