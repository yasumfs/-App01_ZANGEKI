using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSpark : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (MovePlayer.state != 6) {
			Destroy (this.gameObject);
		}
	}
}
